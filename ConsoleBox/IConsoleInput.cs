using System;
using System.Threading;

namespace ConsoleBox
{
    public interface IConsoleInput
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

        void PollEvents(CancellationToken cancel = default(CancellationToken));
    }
}
