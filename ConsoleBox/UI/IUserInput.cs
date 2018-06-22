using System;

namespace ConsoleBox.UI
{
    public interface IUserInput
    {
        bool IsEnabled { get; }
        void Blur();
        void Focus();
        bool ProcessKey(ConsoleKeyInfo key);
    }
}
