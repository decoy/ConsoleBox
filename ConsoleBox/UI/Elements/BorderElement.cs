using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    /// <summary>
    /// Simplistic bordering, doesn't combine multiple boxes
    /// Just handles spacing/margin and the box lines
    /// </summary>
    public class BorderElement : IElement
    {
        public BorderStyle Border { get; set; }

        public IElement Child { get; set; }

        public BorderElement(IElement child)
        {
            Child = child;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            if (Border == null)
            {
                return Child.Draw(offset, constraint, context);
            }

            // measure for the fitted space
            var shrink = constraint.Shrink(2, 2);
            var childSize = Child.Draw(new Point(offset.X + 1, offset.Y + 1), shrink, context);

            var bordered = new Size(childSize.Width + 2, childSize.Height + 2);
            DrawBorder(new Rectangle(offset, bordered), bordered, context);

            return bordered;
        }

        public Size Measure(BoxConstraint constraint)
        {
            if (Border == null)
            {
                return Child.Measure(constraint);
            }

            // measure for the fitted space
            var shrink = constraint.Shrink(2, 2);
            var childSize = Child.Measure(shrink);

            // now add the border back
            return new Size(childSize.Width + 2, childSize.Height + 2);
        }

        private void DrawBorder(Rectangle bounds, Size bordered, UIContext context)
        {
            // TODO process this more cleverly...

            for (var i = 0; i < bordered.Height; i++)
            {
                context.Buffer.Modify(bounds, new Point(0, i), (c) =>
                {
                    c.Char = Border.Vertical;
                    return c;
                });

                context.Buffer.Modify(bounds, new Point(bordered.Width - 1, i), (c) =>
                {
                    c.Char = Border.Vertical;
                    return c;
                });
            }

            for (var i = 0; i < bordered.Width; i++)
            {
                var btop = Border.Horizontal;
                var bbot = Border.Horizontal;
                if (i == 0)
                {
                    btop = Border.TopLeft;
                    bbot = Border.BottomLeft;
                }
                else if (i == bordered.Width - 1)
                {
                    btop = Border.TopRight;
                    bbot = Border.BottomRight;
                }

                context.Buffer.Modify(bounds, new Point(i, 0), (c) =>
                {
                    c.Char = btop;
                    return c;
                });

                context.Buffer.Modify(bounds, new Point(i, bordered.Height - 1), (c) =>
                {
                    c.Char = bbot;
                    return c;
                });
            }

        }
    }
}
