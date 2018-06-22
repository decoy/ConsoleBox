namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public interface IWidget
    {
        /// <summary>
        /// Generates the elements that make up a widget.
        /// Context allows loading state when building the widget
        /// </summary>
        /// <param name="context"></param>
        IElement Build(BuildContext context);
    }
}
