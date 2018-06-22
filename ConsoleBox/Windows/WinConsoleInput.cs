using System;
using System.Threading;

namespace ConsoleBox.Windows
{
    /// <summary>
    /// Uses the Windows API to grab console input events
    /// </summary>
    public class WinConsoleInput : InputEventEmitter, IConsoleInput, IDisposable
    {
        public int Width => Console.WindowWidth;
        public int Height => Console.WindowHeight;

        private IntPtr input;
        private bool mouseEnabled;
        private WinConsoleMode mode;

        public WinConsoleInput()
        {
            input = Kernel32.GetStdHandle(Constants.STD_INPUT_HANDLE);
            mode = new WinConsoleMode(input);
            mode.Initialize();
        }

        public void PollEvents(CancellationToken cancel = default(CancellationToken))
        {
            ReadInput(input, cancel);
        }

        public void EnableMouse()
        {
            if (!mouseEnabled)
            {
                mode.Add(Constants.ENABLE_MOUSE_INPUT);
                mouseEnabled = true;
            }
        }

        public void DisableMouse()
        {
            if (mouseEnabled)
            {
                mode.Remove(Constants.ENABLE_MOUSE_INPUT);
                mouseEnabled = false;
            }
        }

        public void Dispose()
        {
            // disable mouse input if enabled
            DisableMouse();
        }

        private Kernel32.INPUT_RECORD ReadConsoleInput(IntPtr inputHandle)
        {
            int numEventsRead = -1;
            Kernel32.INPUT_RECORD ir;
            var r = Kernel32.ReadConsoleInputW(inputHandle, out ir, 1, out numEventsRead);
            if (!r || numEventsRead == 0)
            {
                throw new InvalidOperationException("Redirected input");
            }

            return ir;
        }

        private void ReadInput(IntPtr inputHandle, CancellationToken cancel = default(CancellationToken))
        {
            while (!cancel.IsCancellationRequested)
            {
                var ir = ReadConsoleInput(inputHandle);

                switch (ir.EventType)
                {
                    case Constants.key_event:
                        EmitKeyEvents(ir.KeyEvent);
                        break;
                    case Constants.mouse_event:
                        EmitMouseEvents(ir.MouseEvent);
                        break;
                    case Constants.window_buffer_size_event:
                        EmitResizeEvent(ir.WindowBufferSizeEvent);
                        break;
                    default: break;
                }
            }
        }

        [Flags]
        internal enum ControlKeyState
        {
            RightAltPressed = 0x0001,
            LeftAltPressed = 0x0002,
            RightCtrlPressed = 0x0004,
            LeftCtrlPressed = 0x0008,
            ShiftPressed = 0x0010,
            NumLockOn = 0x0020,
            ScrollLockOn = 0x0040,
            CapsLockOn = 0x0080,
            EnhancedKey = 0x0100
        }

        private void EmitKeyEvents(Kernel32.KEY_EVENT_RECORD kr)
        {
            var keyCode = kr.virtualKeyCode;

            if (kr.keyDown == 0 && keyCode != Constants.vk_alt)
            {
                // Unicode IME input comes through as KeyUp event with no accompanying KeyDown.
                return;
            }

            ControlKeyState state = (ControlKeyState)kr.controlKeyState;
            bool shift = (state & ControlKeyState.ShiftPressed) != 0;
            bool alt = (state & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;
            bool control = (state & (ControlKeyState.LeftCtrlPressed | ControlKeyState.RightCtrlPressed)) != 0;

            ConsoleKeyInfo info = new ConsoleKeyInfo((char)kr.uChar, (ConsoleKey)kr.virtualKeyCode, shift, alt, control);

            // alt codes unicode entry

            // deal with virtual keys?

            // deal with mod keys held down/repeated keys? (maybe higher level?)
            // compare to nix

            OnKey(info);
        }

        private void EmitMouseEvents(Kernel32.MOUSE_EVENT_RECORD mr)
        {
            // full state is available on every mouse event
            var curState = new MouseStatus(
                mr.dwMousePosition.X,
                mr.dwMousePosition.Y,
                GetButtonState(mr.dwButtonState, Constants.mouse_lmb, MouseButton.Left)
                & GetButtonState(mr.dwButtonState, Constants.mouse_rmb, MouseButton.Right)
                & GetButtonState(mr.dwButtonState, Constants.mouse_mmb, MouseButton.Left)
                );

            OnMouseChange(curState);

            var flags = mr.dwEventFlags;
            switch (flags)
            {
                case 0: // single click
                case 2: // double click
                case 1:  // mouse motion
                    break;
                case 4:
                    // mouse wheel
                    var n = (mr.dwButtonState >> 16);
                    OnMouseWheel(n > 0 ? MouseWheelDirection.Up : MouseWheelDirection.Down, curState);
                    break;
                default:
                    break;
            }
        }

        private static MouseButton GetButtonState(uint buttonStatus, int buttonConstant, MouseButton button)
        {
            return ((buttonStatus & buttonConstant) != 0) ? button : MouseButton.None;
        }

        private void EmitResizeEvent(Kernel32.WINDOW_BUFFER_SIZE_RECORD ir)
        {
            OnResize(ir.dwSize.X, ir.dwSize.Y);
        }
    }
}
