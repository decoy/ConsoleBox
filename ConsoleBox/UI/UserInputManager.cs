using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleBox.UI
{
    public class UserInputManager
    {
        public const string STATE_KEY = "ConsoleBox.UI.UserInputManager";
        public IUserInput Focused { get; set; }

        public List<IUserInput> Inputs { get; private set; } = new List<IUserInput>();

        public static UserInputManager From(StateManager manager)
        {
            if (!manager.TryGet<UserInputManager>(STATE_KEY, out var userInput))
            {
                throw new ArgumentException("no state manager!");
            }
            return userInput;
        }

        // TODO - remove and add need to deal with focus fixes (next focus on remove)

        public void SwitchInput(bool back = false)
        {
            // todo, skip disabled elements
            // additional todo - tab stop?
            //var enabled = Inputs.Where(e => e.IsEnabled);

            // sanity
            if (Inputs.Count() < 1) return;

            var i = (Focused != null) ? Inputs.IndexOf(Focused) : -1;
            i += back ? -1 : 1;

            if (i >= Inputs.Count) { i = 0; }
            if (i < 0) { i = Inputs.Count - 1; }

            Focus(Inputs[i]);
        }

        public void Focus(IUserInput input)
        {
            var old = Focused;

            if (old != input)
            {
                old?.Blur();
            }

            Focused = input;
            Focused?.Focus();
        }

        public void OnKey(ConsoleKeyInfo key)
        {
            if (!(Focused?.ProcessKey(key) ?? false))
            {
                // focused input didn't handle the key, process here
                if (key.Key == ConsoleKey.Tab)
                {
                    SwitchInput(key.Modifiers.HasFlag(ConsoleModifiers.Shift));
                }
            }
        }
    }
}
