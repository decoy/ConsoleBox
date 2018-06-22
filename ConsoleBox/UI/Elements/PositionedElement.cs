using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    public class PositionedElement : IElement
    {
        public bool Absolute { get; set; }

        public Point Location { get; set; }

        public IElement Child { get; set; }

        public PositionedElement(IElement child)
        {
            Child = child;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var pt = Absolute
                ? Location
                : new Point(offset.X + Location.X, offset.Y + Location.Y);

            // TODO what should we do with the constraints??
            // the resulting size would be off, too
            // it might make more to 'remove' it from the layout
            // at least if it's absolutely positioned
            return Child.Draw(offset, constraint, context);
        }

        public Size Measure(BoxConstraint constraint)
        {
            return Child.Measure(constraint);
        }
    }
}
