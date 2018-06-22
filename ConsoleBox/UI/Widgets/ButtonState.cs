using System;
using System.ComponentModel;

namespace ConsoleBox.UI.Widgets
{
    public class ButtonState : IUserInput, IChangeTracking
    {
        private bool isFocused;
        public bool IsFocused
        {
            get { return isFocused; }
            set
            {
                isFocused = value;
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

        private bool isHovered;
        public bool IsHovered
        {
            get { return isHovered; }
            set
            {
                isHovered = value;
                IsChanged = true;
            }
        }

        public Action<ButtonEvent> OnEnter { get; set; }

        public bool IsChanged { get; set; }

        public void Blur()
        {
            IsFocused = false;
        }

        public void Focus()
        {
            IsFocused = true;
        }

        public bool ProcessKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                OnEnter?.Invoke(new ButtonEvent() { Key = key });
            }

            return false;
        }

        public void AcceptChanges()
        {
            IsChanged = false;
        }
    }
}
