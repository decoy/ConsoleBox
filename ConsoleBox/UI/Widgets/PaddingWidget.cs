using ConsoleBox.UI.Elements;

namespace ConsoleBox.UI.Widgets
{
    public class PaddingWidget : IWidget
    {
        public Padding Padding { get; set; }

        public IWidget Child { get; set; }

        public PaddingWidget(IWidget child)
        {
            Child = child;
        }

        public PaddingWidget(Padding pad, IWidget child)
        {
            Child = child;
            Padding = pad;
        }

        public IElement Build(BuildContext context)
        {
            return new PaddingElement(Child.Build(context)) { Padding = Padding };
        }
    }
}
