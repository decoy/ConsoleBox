using System;
using System.Drawing;

namespace ConsoleBox
{
    /// <summary>
    ///  Helper class for processing various events and tracking previous states
    ///  Simply pass in the event info and this tracks the rest.
    ///  Helps normalize the events from various platforms (looking at you mouse support)
    /// </summary>
    public class InputEventEmitter
    {
        public MouseStatus LastMouseStatus { get; set; } = new MouseStatus(0, 0, MouseButton.None);
        public Size LastWindowSize { get; set; } = new Size();

        public event EventHandler<MouseButtonEvent> MouseButtonDown;
        public event EventHandler<MouseButtonEvent> MouseButtonUp;
        public event EventHandler<MouseEvent> MouseMove;
        public event EventHandler<MouseWheelEvent> MouseWheel;
        public event EventHandler<MouseButtonEvent> MouseClick;
        public event EventHandler<ResizeEvent> ResizeEvent;
        public event EventHandler<ConsoleKeyInfo> KeyEvent;

        private object sender;

        public InputEventEmitter()
        {
            sender = this;
        }

        public InputEventEmitter(object sender)
        {
            this.sender = sender;
        }

        public void OnMouseWheel(MouseWheelDirection direction, MouseStatus nextStatus)
        {
            MouseWheel?.Invoke(sender, new MouseWheelEvent(LastMouseStatus, nextStatus, direction));

            OnMouseChange(nextStatus);
        }

        public void OnMouseChange(MouseStatus nextStatus)
        {
            // compare the two mouse statuses and emit appropriate events
            // TODO ... this should be more clever.  this could be handled easier with enum flags
            EmitButtonChange(LastMouseStatus.LeftButtonDown, nextStatus.LeftButtonDown, MouseButton.Left, nextStatus);
            EmitButtonChange(LastMouseStatus.MiddleButtonDown, nextStatus.MiddleButtonDown, MouseButton.Middle, nextStatus);
            EmitButtonChange(LastMouseStatus.RightButtonDown, nextStatus.RightButtonDown, MouseButton.Right, nextStatus);

            // move events
            if (LastMouseStatus.Position != nextStatus.Position)
            {
                MouseMove?.Invoke(sender, new MouseEvent(LastMouseStatus, nextStatus));
            }

            LastMouseStatus = nextStatus;
        }

        private void EmitButtonChange(bool prev, bool next, MouseButton button, MouseStatus nextStatus)
        {
            if (!prev && next)
            {
                MouseButtonDown?.Invoke(sender, new MouseButtonEvent(LastMouseStatus, nextStatus, button));
            }
            else if (prev && !next)
            {
                var ev = new MouseButtonEvent(LastMouseStatus, nextStatus, button);
                MouseButtonUp?.Invoke(sender, ev);
                MouseClick?.Invoke(sender, ev);
            }
        }

        public void OnKey(ConsoleKeyInfo info)
        {
            KeyEvent?.Invoke(sender, info);
        }

        public void OnResize(int width, int height)
        {
            if (LastWindowSize.Width != width || LastWindowSize.Height != height)
            {
                var next = new Size(width, height);
                ResizeEvent?.Invoke(sender, new ResizeEvent(LastWindowSize, next));
                LastWindowSize = next;
            }
        }

    }
}
