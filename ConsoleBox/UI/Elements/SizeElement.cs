using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    public class SizeElement : IElement
    {
        /// <summary>
        /// space to use of the parent.
        /// Note:  constraints win
        /// </summary>
        public double PercentageWidth { get; set; } = 1;

        /// <summary>
        /// space to use of the parent
        /// </summary>
        public double PercentageHeight { get; set; } = 1;

        public BoxConstraint Size { get; set; }

        public IElement Child { get; set; }

        public SizeElement(IElement child)
        {
            Child = child;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var size = constraint
                .Constrain(Size);

            var childSize = Child.Draw(offset, size, context);

            return constraint
                .Tighten(childSize)
                .GetMinSize();
        }

        public Size Measure(BoxConstraint constraint)
        {
            var size = constraint
                .Constrain(Size);

            var childSize = Child.Measure(size);

            return constraint
                .Tighten(childSize)
                .GetMinSize();
        }
    }
}
