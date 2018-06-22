using System;
using System.Collections.Generic;
using System.Text;
using ConsoleBox.UI;
using ConsoleBox.UI.Elements;
using System.Drawing;


namespace ConsoleBox.Demo
{
    public class TestingApp
    {
        public TestingApp(IConsoleBox box)
        {
            var ui = new UIManager(box);
            ui.Render(Create());
        }

        public IElement Create()
        {
            var label = new LabelElement("Test");

            var align = new AlignmentElement(label) { VerticalAlignment = Alignment.Last, HorizontalAlignment = Alignment.Last };

            var border = new BorderElement(align) { Border = BorderStyle.SingleLine };

            var size = new SizeElement(border) { Size = new BoxConstraint(9, int.MaxValue, 5, int.MaxValue) };

            var style = new StyleElement(size)
            {
                Style = new Style() { Background = Color.BurlyWood, Foreground = Color.Black }
            };

            var label2 = new LabelElement("sup");
            var border2 = new BorderElement(label2) { Border = BorderStyle.DoubleLine };
            var align2 = new AlignmentElement(border2) { VerticalAlignment = Alignment.Middle, HorizontalAlignment = Alignment.Last };

            var stack = new FlexLayoutElement(Axis.Vertical, new IElement[]
            {
                style,
                align2
            });

            var stack2 = new FlexLayoutElement(Axis.Horizontal, new IElement[]
            {
                new AlignmentElement( new LabelElement("hi")) { HorizontalAlignment = Alignment.Middle, VerticalAlignment = Alignment.Middle },
                new LabelElement("hib"),
                stack
            })
            { ItemSpacing = FlexSpacing.Evenly };

            return stack2;
        }
    }
}
