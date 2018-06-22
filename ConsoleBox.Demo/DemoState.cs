using ConsoleBox.UI.Widgets;
using System.ComponentModel;

namespace ConsoleBox.Demo
{
    public class DemoState : IChangeTracking
    {
        public bool ShowLayout { get; set; }

        public int Count { get; set; }

        public string HeaderText { get; set; } = "This is the Header";

        public string FooterText { get; set; } = "This is the footer";

        public string Content { get; set; }

        public string[] LeftBarContent { get; set; }

        public bool IsChanged { get; set; }

        public string Image = @"C:\Users\k\Downloads\backgrounds\linuxmint-serena\ajilden_blossom.jpg";

        public void AcceptChanges()
        {
            IsChanged = false;
        }

        public void Click(ButtonEvent ev)
        {
            Count++;
            IsChanged = true;
        }

        public void ChangeFooterText(string txt)
        {
            FooterText = txt;
            IsChanged = true;
        }

        public void ToggleLayout()
        {
            ShowLayout = !ShowLayout;
            IsChanged = true;
        }
    }
}
