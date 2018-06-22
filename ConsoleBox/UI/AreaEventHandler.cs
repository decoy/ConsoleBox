using System;

namespace ConsoleBox.UI
{
    public class AreaEventHandler
    {
        public Action<MouseEvent, bool> OnMouseHoverChange { get; set; }
        public Action<MouseButtonEvent> OnMouseClick { get; set; }
    }
}
