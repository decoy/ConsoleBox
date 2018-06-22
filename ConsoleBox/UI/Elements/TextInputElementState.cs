namespace ConsoleBox.UI.Elements
{
    public class TextInputElementState
    {
        /// <summary>
        /// Text to render
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// The position of the cursor
        /// </summary>
        public int CursorPosition { get; set; }

        /// <summary>
        /// reflects the current visible position in the UI.
        /// This has to be maintained between renders to display consistently
        /// Unlike most state info, this is set by the Element.
        /// Changes to the start visible should _not_ cause a redraw.
        /// This is passed in as an object so items can retain references to the updated number
        /// </summary>
        public int StartVisiblePosition { get; set; }

        /// <summary>
        /// Whether the cursor should be drawn or not
        /// </summary>
        public bool IsFocused { get; set; }
    }
}
