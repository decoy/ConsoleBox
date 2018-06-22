namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class StyleWidget : IWidget
    {
        public Style Style { get; set; }

        public IWidget Child { get; set; }

        public StyleWidget(IWidget child)
        {
            Child = child;
            Style = new Style();
        }

        public StyleWidget(Style style, IWidget child)
        {
            Child = child;
            Style = style;
        }

        public IElement Build(BuildContext context)
        {
            return new StyleElement(Child.Build(context)) { Style = Style };
        }
    }
}
