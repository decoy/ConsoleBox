using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    public class PaddingElement : IElement
    {
        public Padding Padding { get; set; }

        public IElement Child { get; set; }

        public PaddingElement(IElement child)
        {
            Child = child;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var x = Padding.Left + Padding.Right;
            var y = Padding.Top + Padding.Bottom;
            var shrink = constraint.Shrink(x, y);
            var pt = new Point(offset.X + Padding.Left, offset.Y + Padding.Top);
            var childSize = Child.Draw(pt, shrink, context);
            return new Size(childSize.Width + x, childSize.Height + y);
        }

        public Size Measure(BoxConstraint constraint)
        {
            var x = Padding.Left + Padding.Right;
            var y = Padding.Top + Padding.Bottom;
            var shrink = constraint.Shrink(x, y);
            var childSize = Child.Measure(shrink);
            return new Size(childSize.Width + x, childSize.Height + y);
        }
    }
}
