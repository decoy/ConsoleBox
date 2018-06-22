using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    /// <summary>
    /// Gives directions to the parent FlexLayoutElement
    /// on how to layout the child element.  Doesn't modify the bounds itself.
    /// Behavior largely mirrors CSS flex layouts
    /// </summary>
    public class FlexibleElement : IElement
    {
        /// <summary>
        /// How much space the item will claim relative to other elements
        /// if 0, will use the child size
        /// </summary>
        public int FlexFactor { get; set; }

        public IElement Child { get; set; }

        public FlexibleElement(IElement child)
        {
            Child = child;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            return Child.Draw(offset, constraint, context);
        }

        public Size Measure(BoxConstraint constraint)
        {
            return Child.Measure(constraint);
        }
    }
}
