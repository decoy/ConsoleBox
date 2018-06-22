namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class AlignmentWidget : IWidget
    {
        public Alignment VerticalAlignment { get; set; }

        public Alignment HorizontalAlignment { get; set; }

        public IWidget Child { get; set; }

        public AlignmentWidget()
        {
        }

        public AlignmentWidget(IWidget child)
        {
            Child = child;
        }

        public AlignmentWidget(Alignment horizontalAlignment, Alignment verticalAlignment, IWidget child)
        {
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
            Child = child;
        }

        public IElement Build(BuildContext context)
        {
            return new AlignmentElement(Child.Build(context))
            {
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment
            };
        }
    }
}
