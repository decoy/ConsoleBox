using System;
using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    public class AlignmentElement : IElement
    {
        public Alignment VerticalAlignment { get; set; }

        public Alignment HorizontalAlignment { get; set; }

        public IElement Child { get; set; }

        public AlignmentElement(IElement child)
        {
            Child = child;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var pt = GetOffset(constraint);
            var shrink = constraint.Shrink(pt.X, pt.Y).Loosen();
            var childSize = Child.Draw(new Point(pt.X + offset.X, pt.Y + offset.Y), shrink, context);
            return GetUsedSize(constraint, childSize);
        }

        public Size Measure(BoxConstraint constraint)
        {
            var pt = GetOffset(constraint);
            var shrink = constraint.Shrink(pt.X, pt.Y).Loosen();
            var childSize = Child.Measure(shrink);
            return GetUsedSize(constraint, childSize);
        }

        private Size GetUsedSize(BoxConstraint constraint, Size childSize)
        {
            // if alignment is 'set' then we use the minimum width or the child width, whichever is greater
            // (that's what we aligned to)
            var w = HorizontalAlignment != Alignment.Default
                ? Math.Max(constraint.MinWidth, childSize.Width)
                : childSize.Width;

            var h = VerticalAlignment != Alignment.Default
                ? Math.Max(constraint.MinHeight, childSize.Height)
                : childSize.Height;

            return constraint.Tighten(w, h).GetMinSize();
        }

        private Point GetOffset(BoxConstraint constraint)
        {
            var c = Child.Measure(constraint.Loosen());

            int x = HorizontalAlignment.GetOffset(constraint.MinWidth, c.Width);

            int y = VerticalAlignment.GetOffset(constraint.MinHeight, c.Height);

            return new Point(x, y);
        }
    }
}
