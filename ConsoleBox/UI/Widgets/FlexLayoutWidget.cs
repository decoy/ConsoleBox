using System.Collections.Generic;
using System.Linq;

namespace ConsoleBox.UI.Widgets
{
    using Elements;

    public class FlexLayoutWidget : IWidget
    {
        public Axis MainAxis { get; set; }

        public FlexSpacing ItemSpacing { get; set; }

        public IEnumerable<IWidget> Children { get; set; }

        public FlexLayoutWidget(IEnumerable<IWidget> children)
        {
            Children = children;
        }

        public FlexLayoutWidget(Axis mainAxis, IEnumerable<IWidget> children)
        {
            Children = children;
            MainAxis = mainAxis;
        }

        public IElement Build(BuildContext context)
        {
            var children = Children.Select(c => c.Build(context));
            return new FlexLayoutElement(MainAxis, children) { ItemSpacing = ItemSpacing };
        }
    }
}