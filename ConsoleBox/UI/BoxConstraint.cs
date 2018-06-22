using System.Drawing;
using System;

namespace ConsoleBox.UI
{
    /// <summary>
    /// Describes a box bounds
    /// </summary>
    public struct BoxConstraint
    {
        public int MinWidth { get; private set; }

        public int MaxWidth { get; private set; }

        public int MinHeight { get; private set; }

        public int MaxHeight { get; private set; }

        public BoxConstraint(int width, int height) : this(width, width, height, height) { }

        public BoxConstraint(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            // normalize all the constraints
            MinWidth = minWidth > 0 ? minWidth : 0;
            MinHeight = minHeight > 0 ? minHeight : 0;

            MaxWidth = minWidth > maxWidth ? minWidth : maxWidth;
            MaxHeight = minHeight > maxHeight ? minHeight : maxHeight;
        }

        public static BoxConstraint Normalize(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            return new BoxConstraint()
            {
                MinWidth = minWidth > 0 ? minWidth : 0,
                MinHeight = minHeight > 0 ? minHeight : 0,
                MaxWidth = minWidth > maxWidth ? minWidth : maxWidth,
                MaxHeight = minHeight > maxHeight ? minHeight : maxHeight,
            };
        }

        public override string ToString()
        {
            return string.Format("MinWidth: {0}, MaxWidth {1}, MinHeight {2}, MaxHeight {3}",
                MinWidth,
                MaxWidth,
                MinHeight,
                MaxHeight);
        }
        /// <summary>
        /// min max the same
        /// </summary>
        public bool IsFixedWidth
        {
            get { return MinWidth == MaxWidth; }
        }

        /// <summary>
        /// min max the same
        /// </summary>
        public bool IsFixedHeight
        {
            get { return MinHeight == MaxHeight; }
        }

        /// <summary>
        /// not infinite width
        /// </summary>
        public bool IsBoundedWidth
        {
            get { return MaxWidth < int.MaxValue; }
        }

        /// <summary>
        /// Not infinite height
        /// </summary>
        public bool IsBoundedHeight
        {
            get { return MaxHeight < int.MaxValue; }
        }

        /// <summary>
        /// The specified axis has a fixed (min=max) value
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool IsFixed(Axis axis)
        {
            return axis == Axis.Horizontal
                ? IsFixedWidth
                : IsFixedHeight;
        }

        /// <summary>
        /// The specified axis is not infinite
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool IsBounded(Axis axis)
        {
            return axis == Axis.Horizontal
                ? IsBoundedWidth
                : IsBoundedHeight;
        }

        /// <summary>
        /// if the size fits into this constraint
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool Fits(Size size)
        {
            return size.Width >= MinWidth && size.Width <= MaxWidth
                && size.Height >= MinHeight && size.Height <= MaxHeight;
        }

        /// <summary>
        /// Creates a new tight constraint fitting the passed width/height into this constraint's
        /// max/min.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public BoxConstraint Tighten(Size size)
        {
            return Tighten(size.Width, size.Height);
        }

        /// <summary>
        /// Creates a new tight constraint fitting the passed width/height into this constraint's
        /// max/min.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public BoxConstraint Tighten(int width, int height)
        {
            var x = width.Clamp(MinWidth, MaxWidth);
            var y = height.Clamp(MinHeight, MaxHeight);

            return new BoxConstraint(x, y);
        }

        /// <summary>
        /// Removes lower bounds
        /// </summary>
        /// <returns></returns>
        public BoxConstraint Loosen()
        {
            return new BoxConstraint(0, MaxWidth, 0, MaxHeight);
        }

        /// <summary>
        /// Creates a tight box constraint set to the min width/height
        /// </summary>
        /// <returns></returns>
        public BoxConstraint Min()
        {
            return new BoxConstraint(MinWidth, MinHeight);
        }

        /// <summary>
        /// creates a tight box constraint set to the max width/height
        /// </summary>
        /// <returns></returns>
        public BoxConstraint Max()
        {
            return new BoxConstraint(MaxWidth, MaxHeight);
        }

        /// <summary>
        /// Gets the maximum size
        /// </summary>
        /// <returns></returns>
        public Size GetMaxSize()
        {
            return new Size(MaxWidth, MaxHeight);
        }

        /// <summary>
        /// Gets the minimum size
        /// </summary>
        /// <returns></returns>
        public Size GetMinSize()
        {
            return new Size(MinWidth, MinHeight);
        }

        public BoxConstraint Clone()
        {
            return new BoxConstraint(MinWidth, MaxWidth, MinHeight, MaxHeight);
        }

        /// <summary>
        /// Squeezes the passed in constraint to fit into this constraint.
        /// (Clamps all the values)
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public BoxConstraint Constrain(BoxConstraint constraint)
        {
            return new BoxConstraint(
                constraint.MinWidth.Clamp(MinWidth, MaxWidth),
                constraint.MaxWidth.Clamp(MinWidth, MaxWidth),
                constraint.MinHeight.Clamp(MinHeight, MaxHeight),
                constraint.MaxHeight.Clamp(MinHeight, MaxHeight));
        }

        /// <summary>
        /// Sets the specified axis to a fixed value
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="fixedValue"></param>
        /// <returns></returns>
        public BoxConstraint Set(Axis axis, int fixedValue)
        {
            return Set(axis, fixedValue, fixedValue);
        }

        /// <summary>
        /// sets the min/max range for the specified axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public BoxConstraint Set(Axis axis, int min, int max)
        {
            return axis == Axis.Horizontal
                ? new BoxConstraint(min, max, MinHeight, MaxHeight)
                : new BoxConstraint(MinWidth, MaxWidth, min, max);
        }

        /// <summary>
        /// Opposite behavior of the box normalization
        /// Creates a 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public BoxConstraint Shrink(int x, int y)
        {
            // opposite behavior of the normalize - max 'wins' on the shrink
            // note 'infinity' - whatever still is 'infinity'
            var maxw = MaxWidth == int.MaxValue ? MaxWidth : MaxWidth - x;
            var minw = MinWidth > maxw ? maxw : MinWidth;
            var maxh = MaxHeight == int.MaxValue ? MaxHeight : MaxHeight - y;
            var minh = MinHeight > maxh ? maxh : MinHeight;

            return new BoxConstraint(minw, maxw, minh, maxh);
        }
    }
}
