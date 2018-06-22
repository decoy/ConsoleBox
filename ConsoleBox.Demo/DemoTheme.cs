using System.Drawing;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleBox.UI.Widgets;
using ConsoleBox.UI;

namespace ConsoleBox.Demo
{
    public static class DemoTheme
    {
        public static BoxWidget<TWidget> Box<TWidget>(this TWidget widget)
            where TWidget : IWidget
        {
            return new BoxWidget<TWidget>(widget);
        }

        public static TResultWidget Theme<TWidget, TResultWidget>(this TWidget widget, Func<TWidget, TResultWidget> decoration)
           where TWidget : IWidget where TResultWidget : IWidget
        {
            return decoration(widget);
        }

        public static IWidget Theme<TWidget>(this TWidget widget, bool condition, Func<TWidget, IWidget> decoration)
            where TWidget : IWidget
        {
            return condition ? decoration(widget) : widget;
        }

        public static IWidget Button(ButtonWidget widget)
        {
            widget.Decoration = (m, w) =>
            {
                var box = new BoxWidget<IWidget>(w);
                box.Style = new Style();
                box.Style.Foreground = Color.White;

                if (m.IsHovered)
                {
                    box.Style.Background = Color.OliveDrab;
                }
                else
                {
                    box.Style.Background = Color.DarkOliveGreen;
                }

                if (m.IsFocused)
                {
                    box.Border = BorderStyle.DoubleLine;
                }
                else
                {
                    box.Border = BorderStyle.SingleLine;
                }

                return box;
            };

            return widget;
        }

        public static BoxWidget<LabelWidget> FooterLabel(LabelWidget widget)
        {
            var box = Label(widget);

            box.Style.Background = Color.DarkBlue;

            return box;
        }

        public static BoxWidget<LabelWidget> Label(LabelWidget widget)
        {
            var box = new BoxWidget<LabelWidget>(widget);
            box.Style = new Style()
            {
                Foreground = Color.White,
            };

            return box;
        }

        public static BoxWidget<TextInputWidget> Text(TextInputWidget widget)
        {
            var box = widget.Box();
            box.Border = BorderStyle.SingleLine;
            box.Style = new Style();
            box.Style.Foreground = Color.Black;

            widget.Decoration = (state, w) =>
            {
                if (state.IsEnabled && state.IsFocused)
                {
                    box.Style.Background = Color.White;
                }
                else if (state.IsEnabled)
                {
                    box.Style.Background = Color.Silver;
                }
                else
                {
                    box.Style.Background = Color.DarkGray;
                }

                return w;
            };

            return box;
        }

        public static BoxWidget<FlexLayoutWidget> PopupButtonPanel(FlexLayoutWidget layout)
        {
            var box = layout.Box();
            layout.ItemSpacing = FlexSpacing.End;
            return box;
        }

        public static IWidget PopupButton(ButtonWidget button)
        {
            var box = Button(button);
            return box;
        }

        public static BoxWidget<IWidget> PopupContent(IWidget content)
        {
            var box = content.Box();
            return box;
        }

        public static IWidget LayoutHover(string key, Color color, IWidget content)
        {
            var align = new AlignmentWidget(content)
            {
                VerticalAlignment = Alignment.Middle,
                HorizontalAlignment = Alignment.Middle,
            };

            var style = new StyleWidget(align)
            {
                Style = new Style()
            };

            var hover = new HoverWidget(key + ".hover", style);
            hover.Decoration = (h, w) =>
            {
                style.Style.Background = h ? Color.Blue : color;
                return w;
            };

            var box = new PaddingWidget(new Padding(1), hover);

            return box;
        }
    }

}
