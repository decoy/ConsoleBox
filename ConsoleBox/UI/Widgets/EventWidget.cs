namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class EventWidget : IWidget
    {
        public IWidget Child { get; set; }

        public AreaEventHandler Events { get; set; }

        public EventWidget() : this(null, new AreaEventHandler()) { }

        public EventWidget(IWidget child) : this(child, new AreaEventHandler()) { }

        public EventWidget(IWidget child, AreaEventHandler events)
        {
            Child = child;
            Events = events;
        }

        public IElement Build(BuildContext context)
        {
            return new EventElement(Child.Build(context)) { EventHandler = Events };
        }
    }
}
