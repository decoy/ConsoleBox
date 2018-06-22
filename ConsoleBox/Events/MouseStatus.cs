using System.Drawing;

namespace ConsoleBox
{
    public class MouseStatus
    {
        public bool LeftButtonDown
        {
            get { return Buttons.HasFlag(MouseButton.Left); }
        }

        public bool MiddleButtonDown
        {
            get { return Buttons.HasFlag(MouseButton.Middle); }
        }

        public bool RightButtonDown
        {
            get { return Buttons.HasFlag(MouseButton.Right); }
        }

        public Point Position { get; }

        public MouseButton Buttons { get; set; }

        public MouseStatus(int x, int y, MouseButton buttons)
        {
            Buttons = buttons;
            Position = new Point { X = x, Y = y };
        }

        public MouseStatus CopyButtonChange(int x, int y, MouseButton button, bool down)
        {
            return new MouseStatus(x, y, down ? Buttons | button : Buttons & ~button);
        }

        public MouseStatus CopyMoved(int x, int y)
        {
            return new MouseStatus(x, y, Buttons);
        }
    }
}
