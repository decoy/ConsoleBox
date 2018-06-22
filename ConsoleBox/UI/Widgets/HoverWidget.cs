namespace ConsoleBox.UI.Widgets
{
    using System;

    public class HoverWidget : StatefulWidget<bool, EventWidget>
    {
        public IWidget Child { get; set; }

        public HoverWidget(string name, IWidget child) : base(name)
        {
            Child = child;
        }

        public HoverWidget(string name, Func<bool, EventWidget, IWidget> decoration, IWidget child) : base(name, decoration)
        {
            Child = child;
        }

        protected override EventWidget Create(bool state, BuildContext context)
        {
            var ev = new EventWidget(Child);

            ev.Events.OnMouseHoverChange = (m, s) =>
            {
                context.State.Set(Name, s);
                context.State.FlagChanged();
            };

            var decorated = Decoration != null ? Decoration(state, ev) : ev;

            return ev;
        }

        protected override bool CreateState(StateManager manager)
        {
            return false;
        }

        protected override void TearDownState(StateManager manager, bool state)
        {

        }
    }
}
