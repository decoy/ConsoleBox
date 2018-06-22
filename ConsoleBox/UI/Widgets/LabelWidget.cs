namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class LabelWidget : IWidget
    {
        public string Content { get; set; }

        public bool WrapContent { get; set; }

        public LabelWidget() { }

        public LabelWidget(string content)
        {
            Content = content;
        }

        public IElement Build(BuildContext context)
        {
            return new LabelElement(Content) { WrapContent = WrapContent };
        }
    }
}
