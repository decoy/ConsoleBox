using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleBox.UI
{
    public class BorderStyle
    {
        public char TopLeft { get; set; }
        public char TopRight { get; set; }
        public char BottomLeft { get; set; }
        public char BottomRight { get; set; }
        public char Vertical { get; set; }
        public char Horizontal { get; set; }

        //https://en.wikipedia.org/wiki/Box-drawing_character

        public static BorderStyle SingleLine { get; } = new BorderStyle()
        {
            TopLeft = '┌',
            TopRight = '┐',
            BottomLeft = '└',
            BottomRight = '┘',
            Horizontal = '─',
            Vertical = '│',
        };

        public static BorderStyle DoubleLine { get; } = new BorderStyle()
        {
            TopLeft = '╔',
            TopRight = '╗',
            BottomLeft = '╚',
            BottomRight = '╝',
            Horizontal = '═',
            Vertical = '║',
        };
    }
}
