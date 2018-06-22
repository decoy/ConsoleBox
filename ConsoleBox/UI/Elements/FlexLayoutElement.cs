using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleBox.UI.Elements
{
    public class FlexLayoutElement : IElement
    {
        private class MeasuredLayout
        {
            public List<LayoutElement> Elements { get; set; }
            public Size Size { get; set; }
        }

        private class LayoutElement
        {
            public FlexibleElement Child { get; set; }
            public Rectangle Bounds { get; set; }
            public BoxConstraint Constraint { get; set; }
            public int Offset { get; set; }
            public Size Size { get; set; }
        }

        private struct FlexSpacingMeasurement
        {
            public int Start { get; set; }
            public int End { get; set; }
            public int Between { get; set; }
        }

        private struct FlexSize
        {
            public int CrossAxis { get; set; }
            public int MainAxis { get; set; }
        }

        /// <summary>
        /// Does the layout 'flex' along the horizontal or vertical axis?
        /// </summary>
        public Axis MainAxis { get; set; }

        /// <summary>
        /// How extra space is placed around elemented
        /// </summary>
        public FlexSpacing ItemSpacing { get; set; }

        public IEnumerable<IElement> Children { get; set; }

        public FlexLayoutElement(Axis mainAxis, IEnumerable<IElement> children)
        {
            MainAxis = mainAxis;
            Children = children;
        }

        private static void DrawItem(Axis axis, LayoutElement element, Point offset, UIContext context)
        {
            var c = element.Child;

            var p = axis == Axis.Horizontal
                ? new Point(offset.X + element.Offset, offset.Y)
                : new Point(offset.X, offset.Y + element.Offset);

            var childSize = c.Draw(p, element.Constraint, context);

            // assert chidlsize is the same as the expected?

        }

        private static void MeasureItems(List<LayoutElement> items, Axis axis, BoxConstraint constraint)
        {
            // should return the number of items from the list that were used.
            // then that 'row' can be calculated to determine if there's room for the next row.

            var flexible = items.Where(i => i.Child.FlexFactor > 0);
            var inflexible = items.Where(i => i.Child.FlexFactor <= 0);

            // measure each of the inflexible

            var inflexConstraint = constraint.Set(axis, 0, int.MaxValue);

            var inflexTotal = 0;
            foreach (var item in inflexible)
            {
                item.Constraint = inflexConstraint;
                item.Size = item.Child.Measure(item.Constraint);

                var mAxis = item.Size.GetAxis(axis);
                if (mAxis == int.MaxValue)
                {
                    // TODO this should throw an error
                    // no item should ever measure as 'max' (should always return a min value)
                    // with min sizes, parent container will force sizes if necessary to fill
                    item.Child.FlexFactor = 1;
                }
                else
                {
                    inflexTotal += item.Size.GetAxis(axis);
                }
            }

            // now the flexible components using the remaining space
            // shrink that axis?
            // how to calc min/max.. hrm... 
            // if we're passed unbound, we don't want to use it all
            // but we should also be able to go over the min.


            // if bound, flex size should be fixed to the max - inflextotal * flexratio

            int remaining = 0;
            bool isBounded = constraint.IsBounded(axis);

            if (isBounded)
            {
                remaining = constraint.GetMaxSize().GetAxis(axis) - inflexTotal;
            }

            var flexTotal = (double)flexible.Sum(f => f.Child.FlexFactor);

            foreach (var item in flexible)
            {
                if (isBounded)
                {
                    // if bound, flex size should be fixed to the max - inflextotal * flexratio
                    var space = (item.Child.FlexFactor / flexTotal) * remaining;
                    item.Constraint = constraint.Set(axis, (int)Math.Floor(space));
                    item.Size = item.Child.Measure(item.Constraint);
                }
                else
                {
                    // if bounds are unbound, flex size should be unbound, no min, no max.
                    item.Constraint = constraint.Set(axis, 0, int.MaxValue);
                    item.Size = item.Child.Measure(item.Constraint);
                }
            }
        }

        private static void LayoutLine(MeasuredLayout layout, FlexSpacing spacing, Axis axis, BoxConstraint constraint)
        {
            int total = 0;
            try
            {
                total = layout
                    .Elements
                    .Select(l => l.Size.GetAxis(axis))
                    .Sum();
            }
            catch (ArithmeticException ex)
            {
                throw;
            }

            // if !isbounded, it's just a total + maxsize on offset

            // if isbounded, then calc the flex spacing
            // add it all together for the size

            var lineSpacing = constraint.IsBounded(axis)
                ? CalculateLineSpacing(spacing, constraint.GetMaxSize().GetAxis(axis) - total, layout.Elements.Count)
                : new FlexSpacingMeasurement() { Between = 0, End = 0, Start = 0 };

            var used = lineSpacing.Start;
            var crossMax = 0;
            for (var x = 0; x < layout.Elements.Count; x++)
            {
                var item = layout.Elements[x];

                item.Offset = used;

                used += item.Size.GetAxis(axis);

                if (x != layout.Elements.Count - 1)
                {
                    // add between for every item that's not 'last'
                    used += lineSpacing.Between;
                }

                var cross = item.Size.GetAxis(axis.CrossAxis());
                if (cross < int.MaxValue && cross > crossMax)
                {
                    crossMax = cross;
                }
            }

            used += lineSpacing.End;

            foreach (var i in layout.Elements)
            {
                i.Constraint = i.Constraint.Set(axis.CrossAxis(), crossMax);
            }

            layout.Size = axis == Axis.Horizontal
                ? new Size(used, crossMax)
                : new Size(crossMax, used);
        }


        private static FlexSpacingMeasurement CalculateLineSpacing(FlexSpacing spacing, int remainingEmptySpace, int lineItemCount)
        {
            var betweenCount = lineItemCount > 1
                ? lineItemCount - 1
                : 0;

            var layout = new FlexSpacingMeasurement();

            switch (spacing)
            {
                case FlexSpacing.Start:
                    layout.End = remainingEmptySpace;
                    break;
                case FlexSpacing.End:
                    layout.Start = remainingEmptySpace;
                    break;
                case FlexSpacing.Center:
                    layout.Start = (int)Math.Floor(remainingEmptySpace / 2.0);
                    layout.End = (int)Math.Ceiling(remainingEmptySpace / 2.0);
                    break;
                case FlexSpacing.Around:
                    var halved = remainingEmptySpace / 2.0;
                    var elementSpace = (int)Math.Ceiling(halved);
                    var startEndSpace = remainingEmptySpace - elementSpace;

                    layout.Between = betweenCount > 0 ? elementSpace / betweenCount : 0;
                    layout.Start = (int)Math.Floor(startEndSpace / 2.0);
                    layout.End = (int)Math.Ceiling(startEndSpace / 2.0);
                    break;
                case FlexSpacing.Between:
                    layout.Between = betweenCount > 0 ? remainingEmptySpace / betweenCount : 0;
                    break;
                case FlexSpacing.Evenly:
                    var spaceCount = betweenCount + 2; // the space in front and at the end
                    var distributed = remainingEmptySpace / spaceCount;
                    layout.Between = distributed;
                    layout.Start = distributed;
                    layout.End = distributed;
                    break;
                default:
                    break;
            }

            return layout;
        }

        private MeasuredLayout GetLayout(BoxConstraint constraint)
        {
            var layout = new MeasuredLayout();
            layout.Elements = Children.Select(c => new LayoutElement()
            {
                Child = (c is FlexibleElement) ? (FlexibleElement)c : new FlexibleElement(c),
            }).ToList();

            MeasureItems(layout.Elements, MainAxis, constraint);
            LayoutLine(layout, ItemSpacing, MainAxis, constraint);

            return layout;
        }


        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var layout = GetLayout(constraint);

            foreach (var item in layout.Elements)
            {
                DrawItem(MainAxis, item, offset, context);
            }

            return layout.Size;
        }

        public Size Measure(BoxConstraint constraint)
        {
            var layout = GetLayout(constraint);

            return layout.Size;
        }
    }
}
