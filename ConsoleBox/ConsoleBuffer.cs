using System;
using System.Drawing;
using System.Collections.Generic;

namespace ConsoleBox
{
    public class ConsoleBuffer
    {
        private Cell[,] buffer;
        private Dictionary<Point, Cell> changes;
        private int width;
        private int height;

        public ConsoleBuffer(int w, int h)
        {
            width = w;
            height = h;
            buffer = new Cell[w, h];
            changes = new Dictionary<Point, Cell>();
        }

        public void Modify(Rectangle bounds, Point point, Func<Cell, Cell> mod)
        {
            if (point.X >= bounds.Width || point.Y >= bounds.Height)
            {
                return;
            }

            point.Offset(bounds.Location);
            Modify(point, mod);
        }

        private bool InBounds(Point point)
        {
            return (point.X < width
                && point.X >= 0
                && point.Y < height
                && point.Y >= 0);
        }

        public void Modify(Point point, Func<Cell, Cell> mod)
        {
            if (!InBounds(point))
            {
                return;
            }

            if (changes.ContainsKey(point))
            {
                var cell = changes[point];
                changes[point] = mod(cell);
            }
            else
            {
                changes.Add(point, mod(new Cell()));
            }
        }

        public bool Set(int x, int y, Cell c)
        {
            var point = new Point(x, y);

            if (!InBounds(point))
            {
                return false;
            }

            if (changes.ContainsKey(point))
            {
                changes[point] = c;
            }
            else
            {
                changes.Add(point, c);
            }
            return true;
        }

        public void ApplyChanges(IConsoleBox box)
        {
            foreach (var kvp in changes)
            {
                var last = buffer[kvp.Key.X, kvp.Key.Y];

                var changed =
                     last.Char == kvp.Value.Char
                     && last.Background == kvp.Value.Background
                     && last.Foreground == kvp.Value.Foreground
                     && last.Attributes == kvp.Value.Attributes;

                if (!changed)
                {
                    box.Set(
                        kvp.Key.X,
                        kvp.Key.Y,
                        kvp.Value.Char,
                        kvp.Value.Foreground,
                        kvp.Value.Background,
                        kvp.Value.Attributes);

                    buffer[kvp.Key.X, kvp.Key.Y] = kvp.Value;
                }
            }

            changes.Clear();
        }
    }
}