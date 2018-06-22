using System.Drawing;

namespace ConsoleBox
{
    public struct Cell
    {
        public static Cell Empty = default(Cell);

        public char Char;
        public Color Foreground;
        public Color Background;
        public CellAttributes Attributes { get; set; }

        public override string ToString()
        {
            return Char.ToString();
        }
    }
}
