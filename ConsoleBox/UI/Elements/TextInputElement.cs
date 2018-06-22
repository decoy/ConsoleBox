using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    public class TextInputElement : IElement
    {
        public TextInputElementState Input { get; set; }

        public Style CursorStyle { get; set; }

        public TextInputElement(TextInputElementState input)
        {
            Input = input;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var w = constraint.IsBoundedWidth ? constraint.MaxWidth : constraint.MinWidth;
            var size = new Size(w, 1); // TODO yeuks

            // sanity check
            if (w < 1)
            {
                return new Size(0, 0);
            }

            if (Input.CursorPosition < Input.StartVisiblePosition)
            {
                // cursor is behind start
                Input.StartVisiblePosition = Input.CursorPosition;
            }
            else if (Input.Text.Length + 1 <= w)
            {
                // everything fits, just remove 'visible' back to 0
                Input.StartVisiblePosition = 0;
            }
            else if (Input.CursorPosition - Input.StartVisiblePosition >= w)
            {
                // cursor is out of range ahoy!
                // move start pos to match cursor
                Input.StartVisiblePosition = Input.CursorPosition - w + 1;
            }
            else if (Input.Text.Length - Input.StartVisiblePosition + 1 < w)
            {
                // start pos is further right that it needs to be
                Input.StartVisiblePosition = Input.Text.Length - w + 1;
            }

            var bounds = new Rectangle(offset, size);
            if (Input.IsFocused)
            {
                // draw the cursor pos style if it has focus
                var z = new Point(Input.CursorPosition - Input.StartVisiblePosition, 0);
                if (CursorStyle != null)
                {
                    context.Buffer.Modify(bounds, z, CursorStyle.Apply);
                }
                else
                {
                    z.Offset(bounds.X, bounds.Y);
                    context.SetCursor(z, true);
                }
            }

            // cut the text as approp
            var output = Input.Text.Substring(Input.StartVisiblePosition);
            var l = w > output.Length ? output.Length : w;
            for (var x = 0; x < l; x++)
            {
                context.Buffer.Modify(bounds, new Point(x, 0), c =>
                {
                    c.Char = output[x];
                    return c;
                });
            }

            return constraint.Tighten(size).GetMinSize();
        }

        public Size Measure(BoxConstraint constraint)
        {
            return constraint.Tighten(constraint.MaxWidth, 1).GetMinSize();
        }
    }
}
