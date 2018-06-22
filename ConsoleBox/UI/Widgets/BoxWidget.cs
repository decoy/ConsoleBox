using System.Drawing;

namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class BoxWidget<TWidget> : IWidget
       where TWidget : IWidget
    {
        public TWidget Child { get; set; }

        public Style Style { get; set; }

        public BoxConstraint? Size { get; set; }

        public AreaEventHandler Events { get; set; }

        public Alignment VerticalAlignment { get; set; }

        public Alignment HorizontalAlignment { get; set; }

        public BorderStyle Border { get; set; }

        public Padding Padding { get; set; }

        public Padding Margin { get; set; }

        private IWidget widget;

        public BoxWidget(TWidget child)
        {
            Child = child;
        }

        public IElement Build(BuildContext context)
        {
            widget = Child;

            if (VerticalAlignment != Alignment.Default || HorizontalAlignment != Alignment.Default)
            {
                widget = new AlignmentWidget(widget)
                {
                    VerticalAlignment = VerticalAlignment,
                    HorizontalAlignment = HorizontalAlignment
                };
            }

            if (!Margin.IsEmpty)
            {
                widget = new PaddingWidget(Margin, widget);
            }

            if (Size != null)
            {
                widget = new SizeWidget(Size.Value, widget);
            }

            if (Border != null)
            {
                widget = new BorderWidget(Border, widget);
            }

            if (Style != null)
            {
                widget = new StyleWidget(Style, widget);
            }

            if (Events != null)
            {
                widget = new EventWidget(widget) { Events = Events };
            }

            if (!Padding.IsEmpty)
            {
                widget = new PaddingWidget(Padding, widget);
            }

            return widget.Build(context);
        }
    }
}
