using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ConsoleBox.UI
{
    using Elements;

    public class UIManager
    {
        private UIContext context;
        private ConsoleBuffer buffer;
        private IConsoleBox box;

        public UIManager(IConsoleBox box)
        {
            this.box = box;
            context = new UIContext(box, buffer);

            Clear();

            this.box = box;
            box.MouseMove += (s, m) =>
            {
                var exits = context
                    .EventHandlers
                    .Where(h => h.Area.Contains(m.PreviousStatus.Position))
                    .Where(h => !h.Area.Contains(m.Status.Position));

                foreach (var e in exits) { e.Handler.OnMouseHoverChange?.Invoke(m, false); }

                var enters = context
                   .EventHandlers
                   .Where(h => h.Area.Contains(m.Status.Position))
                   .Where(h => !h.Area.Contains(m.PreviousStatus.Position));

                foreach (var e in enters) { e.Handler.OnMouseHoverChange?.Invoke(m, true); }
            };

            box.MouseClick += (s, m) =>
            {
                var clicks = context
                    .EventHandlers
                    .Where(h => h.Area.Contains(m.Status.Position));

                foreach (var e in clicks) { e.Handler.OnMouseClick?.Invoke(m); }
            };

            box.ResizeEvent += (s, m) =>
            {
                // new size, need a new buffer
                // anything in the render queue or cache is garbage now anyway
                Clear();
            };
        }
        public void Render(IElement rootComponent)
        {
            var sw1 = Stopwatch.StartNew();

            context = new UIContext(box, buffer);

            rootComponent.Draw(new Point(0, 0), new BoxConstraint(0, box.Width, 0, box.Height), context);
            buffer.ApplyChanges(box);
            box.Flush();

            box.SetCursor(context.CursorPosition.X, context.CursorPosition.Y);
            box.SetCursorVisibility(context.CursorVisible);

            sw1.Stop();
            //Trace.WriteLine(" Render: " + sw1.ElapsedMilliseconds.ToString());
        }

        private void Clear()
        {
            buffer = new ConsoleBuffer(box.Width, box.Height);
        }
    }
}
