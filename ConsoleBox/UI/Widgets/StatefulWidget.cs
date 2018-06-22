using System;

namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public abstract class StatefulWidget<TState> : StatefulWidget<TState, IWidget>
    {
        public StatefulWidget(string name) : base(name) { }
    }

    public abstract class StatefulWidget<TState, TWidget> : IWidget
        where TWidget : IWidget
    {
        private bool built;

        public string Name { get; }

        private TState state;

        public TState State
        {
            get
            {
                if (!built)
                {
                    throw new Exception("Stateful widget must be built before retrieving the state");
                }
                return state;
            }
        }

        private IWidget widget;

        public Func<TState, TWidget, IWidget> Decoration { get; set; }

        public StatefulWidget(string name)
        {
            Name = name;
        }

        public StatefulWidget(string name, Func<TState, TWidget, IWidget> decoration)
        {
            Name = name;
            Decoration = decoration;
        }

        protected abstract TWidget Create(TState state, BuildContext context);

        protected abstract TState CreateState(StateManager manager);

        protected abstract void TearDownState(StateManager manager, TState state);

        public IElement Build(BuildContext context)
        {
            if (!context.State.TryGet<TState>(Name, out var contextState))
            {
                state = CreateState(context.State);
                context.State.Set(Name, state, TearDownState);
            }
            else
            {
                state = contextState;
            }

            built = true;

            widget = Create(state, context);

            return widget.Build(context);
        }
    }
}
