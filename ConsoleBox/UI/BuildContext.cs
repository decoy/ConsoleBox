namespace ConsoleBox.UI
{
    public class BuildContext
    {
        public StateManager State { get; private set; }

        public BuildContext()
        {
            State = new StateManager();
        }
    }
}
