namespace ConsoleBox
{
    public class MouseWheelEvent : MouseEvent
    {
        public MouseWheelDirection Direction { get; set; }

        public MouseWheelEvent(MouseStatus previous, MouseStatus status, MouseWheelDirection direction) : base(previous, status)
        {
            Direction = direction;
        }
    }
}
