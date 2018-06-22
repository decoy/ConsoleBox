using System;
using System.Drawing;
using System.Threading;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace ConsoleBox.Windows
{
    //https://docs.microsoft.com/en-us/windows/console/setconsolemode
    /// <summary>
    /// Windows native mess.
    /// Use the windows apis to handle inputs, drawing, etc.
    /// Basically compatibility mode for OS less than 10
    /// Has to re-implement key input handling (so more flexible, but more brittle)
    /// </summary>
    public class WinConsole : IConsoleBox
    {
        public int Width { get { return Console.WindowWidth; } }
        public int Height { get { return Console.WindowHeight; } }

        public event EventHandler<MouseButtonEvent> MouseButtonDown
        {
            add { events.MouseButtonDown += value; }
            remove { events.MouseButtonDown -= value; }
        }

        public event EventHandler<MouseButtonEvent> MouseButtonUp
        {
            add { events.MouseButtonUp += value; }
            remove { events.MouseButtonUp -= value; }
        }

        public event EventHandler<MouseEvent> MouseMove
        {
            add { events.MouseMove += value; }
            remove { events.MouseMove -= value; }
        }

        public event EventHandler<MouseWheelEvent> MouseWheel
        {
            add { events.MouseWheel += value; }
            remove { events.MouseWheel -= value; }
        }

        public event EventHandler<MouseButtonEvent> MouseClick
        {
            add { events.MouseClick += value; }
            remove { events.MouseClick -= value; }
        }

        public event EventHandler<ResizeEvent> ResizeEvent
        {
            add { events.ResizeEvent += value; }
            remove { events.ResizeEvent -= value; }
        }

        public event EventHandler<ConsoleKeyInfo> KeyEvent
        {
            add { events.KeyEvent += value; }
            remove { events.KeyEvent -= value; }
        }

        InputEventEmitter events;
        IRenderer render;
        IntPtr input;
        IntPtr output;

        public WinConsole()
        {
            events = new InputEventEmitter(this);
        }

        public void Initialize()
        {
            input = Kernel32.GetStdHandle(Constants.STD_INPUT_HANDLE);
            output = Kernel32.GetStdHandle(Constants.STD_OUTPUT_HANDLE);

            Console.CursorVisible = false;

            if (!Kernel32.GetConsoleMode(output, out var mode))
            {
                // output is punkered?
            }

            if (!Kernel32.GetConsoleMode(input, out var inMode))
            {
                // input is punkered?
                // needed to track previous states
            }

            if (!Kernel32.SetConsoleMode(
                output,
                mode | Constants.ENABLE_VIRTUAL_TERMINAL_PROCESSING))
            {
                // fall back to dumb renderer

            }
            else
            {

                // TODO should this grab it's own text writer?
                // does it matter? :|
                render = new VTRenderer(Console.Out);
            }

            if (!Kernel32.SetConsoleMode(
                input,
                Constants.ENABLE_MOUSE_INPUT | Constants.ENABLE_WINDOW_INPUT | Constants.ENABLE_EXTENDED_FLAGS | Constants.ENABLE_VIRTUAL_TERMINAL_INPUT))
            {
                // IT'S ALL GONE WRONG
            }
        }

        public void ShutDown()
        {
            // unhook anything important?

            // put modes back

            // this should be a disposable 

            //   set_console_cursor_info(out, &orig_cursor_info)
            //   set_console_cursor_position(out, coord{ })
            //   set_console_screen_buffer_size(out, orig_size)
            //   set_console_mode(in, orig_mode)
            //   syscall.Close(in)
            //   syscall.Close(out)
            //   syscall.Close(interrupt)
            //   IsInit = false

            throw new NotImplementedException();
        }

        public void SetScrollWindow(bool useFullScreen)
        {
            // if available
            // some testing to determine buffers on various platforms
            // how does scrolling work on nix?
            throw new NotImplementedException();
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr)
        {
            if (render == null)
            {
                throw new Exception("Not initialized");
            }
            render.Set(x, y, c, fg, bg, attr);
        }

        public void Flush()
        {
            render.Flush();
        }

        public void PollEvents(CancellationToken cancel = default(CancellationToken))
        {
            ReadInput(input, cancel);
        }

        public void SetCursor(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public void SetCursorVisibility(bool isVisible)
        {
            Console.CursorVisible = isVisible;
        }

        public static FileStream GetStandardIn()
        {
            var input = Kernel32.GetStdHandle(Constants.STD_INPUT_HANDLE);
            var safe = new SafeFileHandle(input, false);
            return new FileStream(safe, FileAccess.Read);
        }

        private Kernel32.INPUT_RECORD PeekConsoleInput(IntPtr inputHandle)
        {
            int numEventsRead = -1;
            Kernel32.INPUT_RECORD ir;
            var r = Kernel32.PeekConsoleInput(inputHandle, out ir, 1, out numEventsRead);
            if (!r || numEventsRead == 0)
            {
                throw new InvalidOperationException("Redirected output");
            }

            return ir;
        }

        private Kernel32.INPUT_RECORD ReadConsoleInput(IntPtr inputHandle)
        {
            int numEventsRead = -1;
            Kernel32.INPUT_RECORD ir;
            var r = Kernel32.ReadConsoleInputW(inputHandle, out ir, 1, out numEventsRead);
            if (!r || numEventsRead == 0)
            {
                throw new InvalidOperationException("Redirected output");
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

            events.OnKey(info);

            //Console.WriteLine($"{info.Key} {info.KeyChar} {info.Modifiers}");


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

            events.OnMouseChange(curState);

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
                    events.OnMouseWheel(n > 0 ? MouseWheelDirection.Up : MouseWheelDirection.Down, curState);
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
            events.OnResize(ir.dwSize.X, ir.dwSize.Y);
        }
    }
}
