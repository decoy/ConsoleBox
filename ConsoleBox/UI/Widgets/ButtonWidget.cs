using System;

namespace ConsoleBox.UI.Widgets
{
    public class ButtonWidget : StatefulWidget<ButtonState, IWidget>
    {
        public IWidget Child { get; set; }

        public Action<ButtonEvent> OnClick { get; set; }

        public Action<KeyEvent> OnEnter { get; set; }

        public ButtonWidget(string name, IWidget child) : base(name)
        {
            Child = child;
        }

        public ButtonWidget(string name, Func<ButtonState, IWidget, IWidget> decoration, IWidget child) : base(name, decoration)
        {
            Child = child;
        }

        protected override IWidget Create(ButtonState state, BuildContext context)
        {
            var input = UserInputManager.From(context.State);

            state.OnEnter = OnClick;

            var decorated = Decoration != null ? Decoration(state, Child) : Child;

            var evw = new EventWidget(decorated)
            {
                Events = new AreaEventHandler()
                {
                    OnMouseClick = (ev) =>
                    {
                        if (state.IsEnabled)
                        {
                            input.Focus(state);
                            OnClick?.Invoke(new ButtonEvent() { MouseButtonEvent = ev });
                        }
                    },
                    OnMouseHoverChange = (m, s) =>
                    {
                        state.IsChanged = true;
                        state.IsHovered = s;
                    },
                }
            };

            return evw;
        }

        protected override ButtonState CreateState(StateManager manager)
        {
            var button = new ButtonState();

            var input = UserInputManager.From(manager);
            input.Inputs.Add(button);

            return button;
        }

        protected override void TearDownState(StateManager manager, ButtonState state)
        {
            UserInputManager.From(manager).Inputs.Remove(state);
        }
    }
}
