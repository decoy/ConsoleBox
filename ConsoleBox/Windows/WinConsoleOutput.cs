using System;
using System.Drawing;

namespace ConsoleBox.Windows
{
    public class WinConsoleOutput : IConsoleOutput, IDisposable
    {
        private IntPtr output;
        private WinConsoleMode mode;

        public WinConsoleOutput()
        {
            output = Kernel32.GetStdHandle(Constants.STD_OUTPUT_HANDLE);
            mode = new WinConsoleMode(output);
            mode.Initialize();
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Dispose()
        {
            mode.Reset();
        }

        public void Flush()
        {

        }

        public void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr)
        {
            //https://docs.microsoft.com/en-us/windows/console/reading-and-writing-blocks-of-characters-and-attributes
            //https://github.com/nsf/termbox-go/blob/d51f2f6d6ccb97dd83ed04ae2f79c34234851f39/termbox_windows.go

        }

        public void SetCursor(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public void SetCursorVisibility(bool isVisible)
        {
            Console.CursorVisible = isVisible;
        }
    }
}
