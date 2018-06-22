namespace ConsoleBox
{
    public class MouseEvent
    {
        public MouseStatus PreviousStatus { get; }

        public MouseStatus Status { get; }

        public MouseEvent(MouseStatus previous, MouseStatus status)
        {
            PreviousStatus = previous;
            Status = status;
        }
    }
}
