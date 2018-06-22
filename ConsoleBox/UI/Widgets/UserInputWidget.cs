namespace ConsoleBox.UI.Widgets
{
    public class UserInputWidget : StatefulWidget<UserInputManager, IWidget>
    {
        public IWidget Child { get; private set; }

        public UserInputWidget(IWidget child) : base(UserInputManager.STATE_KEY)
        {
            Child = child;
        }

        protected override IWidget Create(UserInputManager state, BuildContext context)
        {
            return Child;
        }

        protected override UserInputManager CreateState(StateManager manager)
        {
            return new UserInputManager();
        }

        protected override void TearDownState(StateManager manager, UserInputManager state)
        {

        }
    }
}
