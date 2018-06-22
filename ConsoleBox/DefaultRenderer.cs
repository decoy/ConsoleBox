using System;
using System.Drawing;

namespace ConsoleBox
{
    public class DefaultRenderer : IRenderer
    {
        ConsoleState state = new ConsoleState();
        public bool RenderCorner { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public void Begin()
        {
            CacheState();
        }

        public void Flush()
        {
            SetState();
        }

        public void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr)
        {
            // ugh.
            // https://stackoverflow.com/questions/20999247/how-to-prevent-the-console-to-write-a-character-in-the-last-line
            // https://msdn.microsoft.com/en-us/library/system.console.setcursorposition(v=vs.110).aspx
            // looks like with some magic might be able to write to the bottom right
            // but have to have a 'scratch' area of the buffer available.
            // only way to do that is to keep a buffer cell...
            // also, nix can't handle that buffer...
            // alt: just don't ever draw that cell on this thing.. haha

            // fugly bit here is how to deal with the corner pos? pass it along?
            // who controls it?
            if (!RenderCorner && x >= Width - 1 && y >= Height - 1) return;

            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = bg.ToConsoleColor();
            Console.ForegroundColor = fg.ToConsoleColor();
            Console.Write(c);
        }

        private class ConsoleState
        {
            public ConsoleColor ForegroundColor { get; set; }
            public ConsoleColor BackgroundColor { get; set; }
            public int CursorLeft { get; set; }
            public int CursorTop { get; set; }
            public int CursorSize { get; set; }
            public bool CursorVisible { get; set; }
        }

        private void CacheState()
        {
            state.ForegroundColor = Console.ForegroundColor;
            state.BackgroundColor = Console.BackgroundColor;
            state.CursorLeft = Console.CursorLeft;
            state.CursorTop = Console.CursorTop;
            state.CursorSize = Console.CursorSize;
            state.CursorVisible = Console.CursorVisible;
        }

        private void SetState()
        {
            Console.ForegroundColor = state.ForegroundColor;
            Console.BackgroundColor = state.BackgroundColor;
            Console.CursorSize = state.CursorSize;
            Console.CursorVisible = state.CursorVisible;
            Console.SetCursorPosition(state.CursorLeft, state.CursorTop);
        }
    }
}
