using ConsoleBox.UI;
using ConsoleBox.UI.Elements;
using ConsoleBox.UI.Widgets;
using System.Drawing;
using System.Collections.Generic;

namespace ConsoleBox.Demo
{
    public class ImageWidgetStateCache
    {
        public Dictionary<Size, Dictionary<Point, Cell>> Cached { get; set; }
        public Bitmap Image { get; set; }
    }

    public class ImageWidget : StatefulWidget<ImageWidgetStateCache>
    {

        private class StatelessImageWidget : IWidget
        {
            private ImageWidgetStateCache model;

            public StatelessImageWidget(ImageWidgetStateCache model)
            {
                this.model = model;
            }

            public IElement Build(BuildContext context)
            {
                return new ImageElement(model);
            }
        }

        public string FileName { get; set; }

        public ImageWidget(string name, string file) : base(name)
        {
            FileName = file;
        }

        protected override IWidget Create(ImageWidgetStateCache state, BuildContext context)
        {
            var child = new StatelessImageWidget(state);

            return Decoration != null ? Decoration(state, child) : child;
        }

        protected override ImageWidgetStateCache CreateState(StateManager manager)
        {
            return new ImageWidgetStateCache()
            {
                Image = new Bitmap(FileName),
                Cached = new Dictionary<Size, Dictionary<Point, Cell>>(),
            };
        }

        protected override void TearDownState(StateManager manager, ImageWidgetStateCache state)
        {

        }
    }
}
