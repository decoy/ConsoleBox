namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class FlexibleWidget : IWidget
    {
        public int FlexFactor { get; set; }

        public IWidget Child { get; set; }

        public FlexibleWidget(IWidget child)
        {
            Child = child;
        }

        public FlexibleWidget(int flexFactor, IWidget child)
        {
            FlexFactor = flexFactor;
            Child = child;
        }

        public IElement Build(BuildContext context)
        {
            return new FlexibleElement(Child.Build(context))
            {
                FlexFactor = FlexFactor,
            };
        }
    }
}
