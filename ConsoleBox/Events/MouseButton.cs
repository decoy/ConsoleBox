using System;

namespace ConsoleBox
{
    [Flags]
    public enum MouseButton : byte
    {
        None = 0,
        Left = 1,
        Middle = 2,
        Right = 4,
    }
}
