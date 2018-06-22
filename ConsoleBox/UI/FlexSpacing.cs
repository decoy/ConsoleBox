namespace ConsoleBox.UI
{
    /// <summary>
    /// How items in a Flex layout are spaced
    /// </summary>
    public enum FlexSpacing
    {
        /// <summary>
        /// Won't expand to fill any empty space.  Default behavior.
        /// </summary>
        None,

        /// <summary>
        /// Free space placed at the end (making the items align to the start)
        /// </summary>
        Start,

        /// <summary>
        /// Free space placed at the beginning (making items align to the end)
        /// </summary>
        End,

        /// <summary>
        /// Free space split between start and end around the children. (centering)
        /// </summary>
        Center,

        /// <summary>
        /// Place the free space evenly between the children as well as half of that space before and after the first and last child.
        /// </summary>
        Around,

        /// <summary>
        /// Place the free space evenly between the children.
        /// </summary>
        Between,

        /// <summary>
        /// Place the free space evenly between the children as well as before and after the first and last child.
        /// </summary>
        Evenly
    }
}
