using System;
using System.ComponentModel;

namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class TextInputState : IUserInput, IChangeTracking
    {
        public TextInputElementState ElementState { get; set; } = new TextInputElementState();

        public string Text
        {
            get { return ElementState.Text; }
            set
            {
                ElementState.Text = value;
                IsChanged = true;
            }
        }

        public int CursorPosition
        {
            get { return ElementState.CursorPosition; }
            set
            {
                ElementState.CursorPosition = value;
                IsChanged = true;
            }
        }

        public bool IsFocused
        {
            get { return ElementState.IsFocused; }
            set
            {
                ElementState.IsFocused = value;
                IsChanged = true;
            }
        }

        private bool isEnabled = true;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                IsChanged = true;
            }
        }

        public Action<TextInputState> OnTextChange { get; set; }

        public Action<TextInputState, bool> OnFocusChange { get; set; }

        public Action<TextInputState, ConsoleKeyInfo> OnKeyPress { get; set; }

        public Action<TextInputState> OnPositionChange { get; set; }

        public bool IsChanged { get; set; }

        public bool ProcessKey(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    Backspace();
                    break;
                case ConsoleKey.End:
                    End();
                    break;
                case ConsoleKey.Home:
                    Home();
                    break;
                case ConsoleKey.LeftArrow:
                    Left();
                    break;
                case ConsoleKey.UpArrow:
                    break;
                case ConsoleKey.RightArrow:
                    Right();
                    break;
                case ConsoleKey.DownArrow:
                    break;
                case ConsoleKey.Insert:
                    break;
                case ConsoleKey.Delete:
                    Delete();
                    break;
                case ConsoleKey.Tab:
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Spacebar:
                default:
                    if (key.KeyChar != '\0')
                    {
                        AddText(key.KeyChar.ToString());
                    }
                    break;
            }

            OnKeyPress?.Invoke(this, key);

            // allow the key event to bubble back up
            return false;
        }

        public void InsertText(string text, int index)
        {
            // adds text at the specified index.
            // cursor pos is adjusted as approp
            // but doesn't fire the cursor move event
            Text = Text.Insert(index, text);

            if (index <= CursorPosition)
            {
                // move the cursor pos to match
                CursorPosition += text.Length;
            }

            OnTextChange?.Invoke(this);
        }

        public void AddText(string text)
        {
            Text = Text.Insert(CursorPosition, text);
            CursorPosition += text.Length;
            OnTextChange?.Invoke(this);
            OnPositionChange?.Invoke(this);
        }

        public void Focus()
        {
            IsFocused = true;
            OnFocusChange?.Invoke(this, true);
        }

        public void Blur()
        {
            IsFocused = false;
            //CursorPosition = 0;
            OnFocusChange?.Invoke(this, false);
        }

        public void MoveCursor(int index)
        {
            CursorPosition += index;
            OnPositionChange?.Invoke(this);
        }

        public void Backspace()
        {
            if (CursorPosition > 0)
            {
                Text = Text.Remove(CursorPosition - 1, 1);
                CursorPosition -= 1;
                OnTextChange?.Invoke(this);
                OnPositionChange?.Invoke(this);
            }
        }

        public void Delete()
        {
            if (CursorPosition < Text.Length)
            {
                Text = Text.Remove(CursorPosition, 1);
                OnTextChange?.Invoke(this);
            }
        }

        public void End()
        {
            ChangePos(Text.Length);
        }

        public void Home()
        {
            ChangePos(0);
        }

        public void Left()
        {
            ChangePos(CursorPosition - 1);
        }

        public void Right()
        {
            ChangePos(CursorPosition + 1);
        }

        private void ChangePos(int pos)
        {
            if (CursorPosition != pos)
            {
                if (pos > Text.Length) { pos = Text.Length; }
                if (pos < 0) { pos = 0; }
                CursorPosition = pos;
                OnPositionChange?.Invoke(this);
            }
        }

        public void AcceptChanges()
        {
            IsChanged = false;
        }
    }
}
