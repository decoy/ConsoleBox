using System.Drawing;

namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class SizeWidget : IWidget
    {
        public BoxConstraint Size { get; set; }

        public IWidget Child { get; set; }

        public SizeWidget() { }

        public SizeWidget(IWidget child)
        {
            Child = child;
        }

        public SizeWidget(BoxConstraint size, IWidget child)
        {
            Size = size;
            Child = child;
        }

        public IElement Build(BuildContext context)
        {
            return new SizeElement(Child.Build(context)) { Size = Size };
        }
    }
}
