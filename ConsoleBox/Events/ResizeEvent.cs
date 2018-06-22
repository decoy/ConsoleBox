using System.Drawing;

namespace ConsoleBox
{
    public class ResizeEvent
    {
        public Size PreviousSize { get; set; }
        public Size Size { get; }

        public ResizeEvent(Size prev, Size next)
        {
            PreviousSize = prev;
            Size = next;
        }
    }
}
