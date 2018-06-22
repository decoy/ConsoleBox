using System;
using System.Drawing;
using System.Threading;

namespace ConsoleBox
{
    /// <summary>
    /// Just uses the built in functions, no fancy interop calls
    /// Slow rendering on Windows
    /// Only handles basic key presses (console.readkey(true))
    /// </summary>
    public class DefaultConsole : IConsoleBox
    {
        public int Width { get { return Console.WindowWidth; } }
        public int Height { get { return Console.WindowHeight; } }

        public event EventHandler<MouseButtonEvent> MouseButtonDown
        {
            add { events.MouseButtonDown += value; }
            remove { events.MouseButtonDown -= value; }
        }

        public event EventHandler<MouseButtonEvent> MouseButtonUp
        {
            add { events.MouseButtonUp += value; }
            remove { events.MouseButtonUp -= value; }
        }

        public event EventHandler<MouseEvent> MouseMove
        {
            add { events.MouseMove += value; }
            remove { events.MouseMove -= value; }
        }

        public event EventHandler<MouseWheelEvent> MouseWheel
        {
            add { events.MouseWheel += value; }
            remove { events.MouseWheel -= value; }
        }

        public event EventHandler<MouseButtonEvent> MouseClick
        {
            add { events.MouseClick += value; }
            remove { events.MouseClick -= value; }
        }

        public event EventHandler<ResizeEvent> ResizeEvent
        {
            add { events.ResizeEvent += value; }
            remove { events.ResizeEvent -= value; }
        }

        public event EventHandler<ConsoleKeyInfo> KeyEvent
        {
            add { events.KeyEvent += value; }
            remove { events.KeyEvent -= value; }
        }

        private InputEventEmitter events;
        private ConsoleState state;

        private class ConsoleState
        {
            public ConsoleColor ForegroundColor { get; set; }
            public ConsoleColor BackgroundColor { get; set; }
            public int CursorLeft { get; set; }
            public int CursorTop { get; set; }
            public int CursorSize { get; set; }
            public bool CursorVisible { get; set; }
        }

        public DefaultConsole()
        {
            events = new InputEventEmitter(this);
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Initialize()
        {
            SetCursor(0, 0);
            SetCursorVisibility(false);
        }

        public void PollEvents(CancellationToken cancel = default(CancellationToken))
        {
            while (!cancel.IsCancellationRequested)
            {
                var key = Console.ReadKey(true);
                events.OnResize(Console.WindowWidth, Console.WindowHeight);
                events.OnKey(key);
            }
        }

        public void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr)
        {
            // ugh.
            // https://stackoverflow.com/questions/20999247/how-to-prevent-the-console-to-write-a-character-in-the-last-line
            // that bottom line might just have to go.
            // https://msdn.microsoft.com/en-us/library/system.console.setcursorposition(v=vs.110).aspx
            // looks like with some magic might be able to write to the bottom right
            // but have to have a 'scratch' area of the buffer available.
            // only way to do that is to keep a buffer cell...
            // alt: just don't ever draw that cell on this thing.. haha
            if (x >= Width - 1 && y >= Height - 1) return;

            if (state == null) CacheState();
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = bg.ToConsoleColor();
            Console.ForegroundColor = fg.ToConsoleColor();
            Console.Write(c);
        }

        public void ShutDown()
        {

        }

        public void SetCursor(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public void SetCursorVisibility(bool isVisible)
        {
            Console.CursorVisible = isVisible;
        }

        private void CacheState()
        {
            state = new ConsoleState();
            state.ForegroundColor = Console.ForegroundColor;
            state.BackgroundColor = Console.BackgroundColor;
            state.CursorLeft = Console.CursorLeft;
            state.CursorTop = Console.CursorTop;
            state.CursorSize = Console.CursorSize;
            state.CursorVisible = Console.CursorVisible;
        }

        private void ReloadState()
        {
            Console.ForegroundColor = state.ForegroundColor;
            Console.BackgroundColor = state.BackgroundColor;
            Console.CursorSize = state.CursorSize;
            Console.CursorVisible = state.CursorVisible;
            SetCursor(state.CursorLeft, state.CursorTop);
        }

        public void Flush()
        {
            ReloadState();
            state = null;
        }
    }
}
