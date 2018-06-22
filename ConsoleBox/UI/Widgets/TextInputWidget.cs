using System;

namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class TextInputWidget : StatefulWidget<TextInputState>
    {
        public class StatelessTextInputWidget : IWidget
        {
            private TextInputState model;

            public StatelessTextInputWidget(TextInputState model)
            {
                this.model = model;
            }

            public IElement Build(BuildContext context)
            {
                return new TextInputElement(model.ElementState);
            }
        }

        public string Text { get; set; }

        public Action<TextInputState> OnTextChange { get; set; }

        public Action<TextInputState, bool> OnFocusChange { get; set; }

        public Action<TextInputState, ConsoleKeyInfo> OnKeyPress { get; set; }

        public Action<TextInputState> OnPositionChange { get; set; }

        public TextInputWidget(string name) : base(name) { }

        public TextInputWidget(string name, string text) : base(name)
        {
            Text = text;
        }

        protected override TextInputState CreateState(StateManager manager)
        {
            var man = UserInputManager.From(manager);
            var input = new TextInputState();
            man.Inputs.Add(input);
            return input;
        }

        protected override void TearDownState(StateManager manager, TextInputState state)
        {
            UserInputManager.From(manager).Inputs.Remove(state);
        }

        protected override IWidget Create(TextInputState state, BuildContext context)
        {
            if (Text != null)
            {
                state.Text = Text;
            }

            state.OnFocusChange = OnFocusChange;
            state.OnKeyPress = OnKeyPress;
            state.OnPositionChange = OnPositionChange;
            state.OnTextChange = OnTextChange;

            var child = new StatelessTextInputWidget(state);

            return Decoration != null ? Decoration(state, child) : child;
        }
    }
}
