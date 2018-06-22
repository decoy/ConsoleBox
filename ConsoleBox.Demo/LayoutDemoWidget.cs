using ConsoleBox.UI;
using ConsoleBox.UI.Widgets;
using System.Drawing;

namespace ConsoleBox.Demo
{
    public class LayoutDemoWidget : StatelessWidget
    {
        private DemoState state;
        public LayoutDemoWidget(DemoState state)
        {
            this.state = state;
        }

        public override IWidget Create()
        {
            var north = DemoTheme.LayoutHover("north", Color.Green, new LabelWidget("North"));
            var south = DemoTheme.LayoutHover("south", Color.Green, new LabelWidget("South"));

            var west = DemoTheme.LayoutHover("west", Color.Red, new LabelWidget("West"));
            var east = DemoTheme.LayoutHover("east", Color.Red, new LabelWidget("East"));

            var middlenorth = DemoTheme.LayoutHover("mnorth", Color.Orange, new LabelWidget("Middle North") { WrapContent = true });
            var middlesouth = DemoTheme.LayoutHover("msouth", Color.Orange, new LabelWidget("Middle South") { WrapContent = true });

            var middle = DemoTheme.LayoutHover("middle.stack", Color.SlateGray, new FlexLayoutWidget(Axis.Vertical, new IWidget[]
            {
                new FlexibleWidget(1, middlenorth),
                new FlexibleWidget(1, middlesouth),
            }));

            var center = DemoTheme.LayoutHover("center.stack", Color.Violet, new FlexLayoutWidget(Axis.Horizontal, new IWidget[]
            {
                new FlexibleWidget(1, east),
                new FlexibleWidget(1, middle),
                new FlexibleWidget(1, west),
            }));

            var stack = DemoTheme.LayoutHover("stack", Color.Yellow, new FlexLayoutWidget(Axis.Vertical, new IWidget[]
            {
                new FlexibleWidget(1, north),
                new FlexibleWidget(2, center),
                new FlexibleWidget(1, south),
            }));

            var fullstyle = new StyleWidget(stack)
            {
                Style = new Style() { Background = Color.Black }
            };

            var click = new ButtonWidget("clickme", fullstyle);
            click.OnClick = (ev) => state.ToggleLayout();

            return click;
        }
    }
}
