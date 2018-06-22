using System.Drawing;

namespace ConsoleBox
{
    public interface IRenderer
    {
        void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr);
        void Flush();
    }
}
