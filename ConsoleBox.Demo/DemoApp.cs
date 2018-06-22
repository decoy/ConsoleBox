using ConsoleBox.UI;
using System;
using System.Drawing;
using System.Collections.Generic;
using ConsoleBox.UI.Widgets;

namespace ConsoleBox.Demo
{
    /// <summary>
    ///  example app layout
    /// </summary>
    public class DemoApp : StatefulWidget<DemoState>
    {
        public DemoApp() : base("DemoApp") { }

        protected override IWidget Create(DemoState state, BuildContext context)
        {
            //var fl = new FlexLayoutWidget(Axis.Horizontal, new IWidget[]
            //{
            //    new StyleWidget(new Style() { Background = Color.Bisque },
            //        new SizeWidget(new BoxConstraint(15, 5), new LabelWidget("abcdefg"))),
            //    new FlexibleWidget(1,
            //        new StyleWidget(new Style(){ Background = Color.Green },
            //                new LabelWidget("abcdef"))),
            //    new FlexibleWidget(1, new StyleWidget(new Style(){ Background = Color.Orange }, new LabelWidget("abcd"))),
            //    new LabelWidget("a---|"),
            //})
            //{ ItemSpacing = FlexSpacing.None };

            //return new StyleWidget(new Style()
            //{
            //    Foreground = Color.Yellow,
            //    Background = Color.DarkBlue,
            //}, fl);

            if (state.ShowLayout)
            {
                return new LayoutDemoWidget(state);
            }

            var stack = new FlexLayoutWidget(Axis.Vertical, new IWidget[]
            {
                new AlignmentWidget() {
                    Child = Header(state),
                },
                new SizeWidget(
                    new BoxConstraint(10, int.MaxValue, 1, int.MaxValue),
                    new LabelWidget(state.FooterText)),
                CreateContent(state),
                new AlignmentWidget() {
                    VerticalAlignment = Alignment.Last,
                    Child = Footer(state)
                },
            })
            { ItemSpacing = FlexSpacing.Between }.Box();

            stack.Style = new Style()
            {
                Background = Color.DarkBlue
            };

            return stack;
        }

        protected override DemoState CreateState(StateManager manager)
        {
            return new DemoState();
        }

        protected override void TearDownState(StateManager manager, DemoState state)
        {

        }

        private static IWidget CreateContent(DemoState state)
        {
            var a = new AlignmentWidget()
            {
                VerticalAlignment = Alignment.Middle,
                Child = new ButtonWidget("ButtonA", new LabelWidget("[Click Me]"))
                {
                    OnClick = (ev) => state.ToggleLayout()
                }.Theme(DemoTheme.Button),
            };

            //var b = new ImageWidget("imagea", state.Image);
            return new FlexLayoutWidget(new IWidget[]
            {
                a, //new SizeWidget(new BoxConstraint(60, 20), b)
            });
        }

        private static IWidget Popup(DemoState state)
        {
            var box = new BorderWidget()
            {
                Child = new SizeWidget()
                {
                    Size = new BoxConstraint(15, 10),
                    Child = new AlignmentWidget()
                    {
                        VerticalAlignment = Alignment.Middle,
                        HorizontalAlignment = Alignment.Middle,
                        Child = new LabelWidget() { Content = "Test" }
                    }
                }
            };

            return new PositionedWidget(box)
            {
                Absolute = true,
                Location = new Point(0, 0),
            };
        }

        private static IWidget Header(DemoState state)
        {
            return new AlignmentWidget()
            {
                HorizontalAlignment = Alignment.Middle,
                Child = new LabelWidget(state.HeaderText + " " + state.Count).Theme(DemoTheme.Label)
            };
        }

        // basically a functional widget
        private static IWidget Footer(DemoState state)
        {
            var input = new TextInputWidget("Main.Input")
            {
                OnTextChange = (s) => state.ChangeFooterText(s.Text)
            }.Theme(DemoTheme.Text);

            var button = new ButtonWidget("main.button", new LabelWidget("Go"))
            {
                OnClick = state.Click
            }.Theme(DemoTheme.Button);

            return new FlexLayoutWidget(new IWidget[]
            {
                new FlexibleWidget(1, input),
                button,
            });
        }
    }


}
