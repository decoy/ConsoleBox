using System;

namespace ConsoleBox
{
    [Flags]
    public enum CellAttributes : byte
    {
        None = 0,
        Bright = 1,
        Dim = 2,
        Underscore = 4,
        Blink = 8,
        Reverse = 16,
        Hidden = 32,
    }
}
