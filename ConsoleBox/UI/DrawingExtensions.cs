using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConsoleBox.UI
{
    public static class DrawingExtensions
    {
        public static int GetOffset(this Alignment align, int total, int used)
        {
            switch (align)
            {
                case Alignment.First:
                    return 0;
                case Alignment.Last:
                    return Math.Max(total - used, 0);
                case Alignment.Middle:
                    var halved = (total - used) / 2.0;
                    return Math.Max((int)Math.Floor(halved), 0);
                default:
                    return 0;
            }
        }

        public static Axis CrossAxis(this Axis axis)
        {
            return axis == Axis.Horizontal ? Axis.Vertical : Axis.Horizontal;
        }

        public static Rectangle OffsetAxis(this Rectangle bounds, Axis axis, int offset)
        {
            bounds.Offset(axis == Axis.Horizontal
                ? new Point(offset, 0)
                : new Point(0, offset));

            return bounds;
        }

        public static Rectangle SetAxis(this Rectangle rectangle, Axis axis, int value)
        {
            rectangle.Size = SetAxis(rectangle.Size, axis, value);
            return rectangle;
        }

        public static int GetAxis(this Size size, Axis axis)
        {
            return axis == Axis.Horizontal ? size.Width : size.Height;
        }

        public static Size SetAxis(this Size size, Axis axis, int value)
        {
            return axis == Axis.Horizontal
                ? new Size(value, size.Height)
                : new Size(size.Width, value);
        }

        public static Size InflateAxis(this Size size, Axis axis, int value)
        {
            return axis == Axis.Horizontal
               ? new Size(size.Width + value, size.Height)
               : new Size(size.Width, size.Height + value);
        }
    }
}
