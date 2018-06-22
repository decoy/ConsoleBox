using System;
using System.Drawing;
using System.Threading;

namespace ConsoleBox
{
    //https://docs.microsoft.com/en-us/nuget/guides/create-cross-platform-packages#create-the-project-structure-and-abstraction-code
    //https://godoc.org/github.com/nsf/termbox-go
    public interface IConsoleBox
    {
        int Width { get; }
        int Height { get; }

        event EventHandler<MouseButtonEvent> MouseButtonDown;
        event EventHandler<MouseButtonEvent> MouseButtonUp;
        event EventHandler<MouseEvent> MouseMove;
        event EventHandler<MouseWheelEvent> MouseWheel;
        event EventHandler<MouseButtonEvent> MouseClick;
        event EventHandler<ResizeEvent> ResizeEvent;
        event EventHandler<ConsoleKeyInfo> KeyEvent;

        void Initialize();

        void ShutDown();

        void Clear();

        void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr);

        void SetCursor(int x, int y);

        void SetCursorVisibility(bool isVisible);

        void Flush();

        void PollEvents(CancellationToken cancel = default(CancellationToken));
    }
}
