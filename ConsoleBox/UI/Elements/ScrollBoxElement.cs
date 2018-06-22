using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ConsoleBox.UI.Elements
{
    public class ScrollBoxElement : IElement
    {
        public bool HorizontalScroll { get; set; }

        public bool VerticalScroll { get; set; }

        public IElement Child { get; set; }

        public Point ScrollOffset { get; set; }

        public ScrollBoxElement(IElement child, Point scrollOffset)
        {
            Child = child;
            ScrollOffset = scrollOffset;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var scroll = GetConstraint(constraint);

            // measure the child, if the child doesn't fit
            // then we can apply the offset.
            // wait.... the offset works... differently... shit we need to pass in a custom 'buffer'
            // TODO: we should validate the offset makes 'sense' here

            // ICK
            //var ctx = new UIContext(context.Console, new ConsoleBuffer(int.MaxValue, int.MaxValue, new Cell()));

            

            throw new NotImplementedException();
        }

        public Size Measure(BoxConstraint constraint)
        {
            var scroll = GetConstraint(constraint);

            var min = Child.Measure(scroll);

            // fits whtaever the child returns into the constraint
            // so if the child doesn't use the full h/w, neither will this measure
            // but if it does, it will max at the constraint max.

            // offset isn't important here since if it's 'used' then this box already fills
            return constraint.Tighten(min).GetMinSize();
        }

        private BoxConstraint GetConstraint(BoxConstraint constraint)
        {
            var scroll = constraint;
            if (HorizontalScroll)
            {
                scroll = scroll.Set(Axis.Horizontal, 0, int.MaxValue);
            }

            if (VerticalScroll)
            {
                scroll = scroll.Set(Axis.Vertical, 0, int.MaxValue);
            }

            return scroll;
        }
    }
}
