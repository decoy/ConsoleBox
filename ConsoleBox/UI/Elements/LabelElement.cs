using System.Drawing;
using System.Linq;

namespace ConsoleBox.UI.Elements
{
    public class LabelElement : IElement
    {
        public string Content { get; set; }

        public bool WrapContent { get; set; }

        public LabelElement() { }

        public LabelElement(string content)
        {
            Content = content;
        }

        public Size Measure(BoxConstraint constraint)
        {
            var lines = TextArray(constraint.GetMaxSize(), WrapContent, true);

            var max = lines.DefaultIfEmpty(string.Empty).Max(l => l.Length);

            return constraint.Tighten(max, lines.Length).GetMinSize();
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var lines = TextArray(constraint.GetMaxSize(), WrapContent, true);
            var max = lines.DefaultIfEmpty(string.Empty).Max(l => l.Length);
            var size = constraint.Tighten(max, lines.Length).GetMinSize();

            var bounds = new Rectangle(offset, size);

            for (var y = 0; y < lines.Length; y++)
            {
                var l = lines[y];
                for (var x = 0; x < l.Length; x++)
                {
                    context.Buffer.Modify(bounds, new Point(x, y), c =>
                    {
                        c.Char = l[x];
                        return c;
                    });
                }
            }

            return size;
        }

        private string[] TextArray(Size bounds, bool wrap, bool clip)
        {
            if (string.IsNullOrEmpty(Content))
            {
                // sanity check
                return new string[0];
            }

            var text = wrap ? StringExtensions.WrapText(Content, bounds.Width) : Content;
            var lines = text.Split('\n');

            if (clip)
            {
                lines = StringExtensions.ClipTextArray(lines, bounds.Width, bounds.Height);
            }

            // should fill its space if using it
            if (lines.Length > 1)
            {
                lines = StringExtensions.PadTextArray(lines, bounds.Width);
            }

            return lines;
        }
    }
}
