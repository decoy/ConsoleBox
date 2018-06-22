namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class BorderWidget : IWidget
    {
        public BorderStyle Border { get; set; }

        public IWidget Child { get; set; }

        public BorderWidget() { }

        public BorderWidget(IWidget child)
        {
            Child = child;
        }

        public BorderWidget(BorderStyle borderStyle, IWidget child)
        {
            Child = child;
            Border = borderStyle;
        }

        public IElement Build(BuildContext context)
        {
            return new BorderElement(Child.Build(context))
            {
                Border = Border,
            };
        }
    }
}
