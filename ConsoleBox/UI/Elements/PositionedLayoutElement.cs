using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ConsoleBox.UI.Elements
{
    public class PositionedLayoutElement : IElement
    {
        public IEnumerable<IElement> Children { get; set; }

        public PositionedLayoutElement(IEnumerable<IElement> children)
        {
            Children = children;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            foreach(var child in Children)
            {
                child.Draw(offset, constraint, context);
            }

            return constraint.GetMaxSize();
        }

        public Size Measure(BoxConstraint constraint)
        {
            // TODO we could actually measure everyone including positioning...
            // but dayum that's fugly
            return constraint.GetMaxSize();
        }
    }
}
