using System;
using System.Drawing;

namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class PositionedWidget : IWidget
    {
        public bool Absolute { get; set; }

        public Point Location { get; set; }

        public IWidget Child { get; set; }

        public PositionedWidget(IWidget child)
        {
            Child = child;
        }
        public IElement Build(BuildContext context)
        {
            return new PositionedElement(Child.Build(context))
            {
                Absolute = Absolute,
                Location = Location,
            };
        }
    }
}
