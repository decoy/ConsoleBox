using System.Drawing;

namespace ConsoleBox
{
    /// <summary>
    /// Defines a console output writer.
    /// </summary>
    public interface IConsoleOutput
    {
        void Clear();

        void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr);

        void SetCursor(int x, int y);

        void SetCursorVisibility(bool isVisible);

        void Flush();
    }
}
