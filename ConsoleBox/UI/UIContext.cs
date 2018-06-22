using System.Collections.Generic;

namespace ConsoleBox.UI
{
    using System.Drawing;

    public class UIContext
    {
        public ConsoleBuffer Buffer { get; private set; }

        public IConsoleBox Console { get; private set; }

        public Point CursorPosition { get; set; }

        public bool CursorVisible { get; set; }

        public void SetCursor(Point pos, bool visible)
        {
            if (pos.X >= Console.Width || pos.Y >= Console.Height)
            {
                return;
            }

            // modifies the cursor position.
            // last wins.  (if multiple textboxes grab focus for instance.)
            CursorPosition = pos;
            CursorVisible = visible;
        }

        public class AreaEventHandlerRegistration
        {
            public Rectangle Area { get; set; }
            public AreaEventHandler Handler { get; set; }
        }

        public List<AreaEventHandlerRegistration> EventHandlers { get; set; }

        public UIContext(IConsoleBox console, ConsoleBuffer buffer)
        {
            Buffer = buffer;
            Console = console;
            EventHandlers = new List<AreaEventHandlerRegistration>();
        }

        public void AddEventHandler(AreaEventHandlerRegistration handler)
        {
            EventHandlers.Add(handler);
        }

        public void RemoveEventHandler(AreaEventHandlerRegistration handler)
        {
            EventHandlers.Remove(handler);
        }
    }
}
