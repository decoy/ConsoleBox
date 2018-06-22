using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    public class StyleElement : IElement
    {
        public Style Style { get; set; } = new Style();

        public IElement Child { get; set; }

        public StyleElement(IElement child)
        {
            Child = child;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var measured = Child.Measure(constraint);
            var bounds = new Rectangle(offset, measured);
            if (Style.HasStyles())
            {
                for (var x = 0; x < measured.Width; x++)
                {
                    for (var y = 0; y < measured.Height; y++)
                    {
                        context.Buffer.Modify(bounds, new Point(x, y), Style.Apply);
                    }
                }
            }

            return Child.Draw(offset, constraint, context);
        }

        public Size Measure(BoxConstraint constraint)
        {
            return Child.Measure(constraint);
        }
    }
}
