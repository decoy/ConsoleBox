using System.Drawing;

namespace ConsoleBox.UI
{
    public class Style
    {
        public char? Fill { get; set; }
        public Color? Foreground { get; set; }
        public Color? Background { get; set; }
        public CellAttributes Add { get; set; }
        public CellAttributes Remove { get; set; }

        public bool HasStyles()
        {
            return Foreground != null
            || Background != null
            || Fill != null
            || Add != CellAttributes.None
            || Remove != CellAttributes.None;
        }

        public Cell Apply(Cell c)
        {
            if (Foreground != null)
            {
                c.Foreground = (Color)Foreground;
            }

            if (Background != null)
            {
                c.Background = (Color)Background;
            }

            if (Fill != null)
            {
                c.Char = (char)Fill;
            }

            if (Remove != CellAttributes.None)
            {
                c.Attributes = c.Attributes & ~Remove;
            }

            if (Add != CellAttributes.None)
            {
                c.Attributes = c.Attributes | Add;
            }
            return c;
        }
    }
}
