namespace ConsoleBox
{
    public class MouseButtonEvent : MouseEvent
    {
        public MouseButton Button { get; set; }

        public MouseButtonEvent(MouseStatus previous, MouseStatus status, MouseButton button) : base(previous, status)
        {
            Button = button;
        }
    }
}
