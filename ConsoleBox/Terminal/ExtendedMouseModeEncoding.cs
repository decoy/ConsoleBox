using System;
using System.Linq;

namespace ConsoleBox.Terminal
{
    // xterm 1006 extended mode or urxvt 1015 extended mode
    // xterm: \033 [ < Cb ; Cx ; Cy (M or m)
    // urxvt: \033 [ Cb ; Cx ; Cy M
    public class ExtendedMouseModeEncoding
    {
        public const string PREFIX = "\x1b[";
        public const string PREFIX_URXVT = "\x1b[<";

        private readonly InputEventEmitter emit;
        public ExtendedMouseModeEncoding(InputEventEmitter emit)
        {
            this.emit = emit;
        }

        public void Register(InputMapper mapper)
        {
            mapper.Add(PREFIX, Process);
        }

        public int Process(ArraySegment<char> buffer)
        {
            // find the first M or m, that's where we stop
            var mi = buffer.IndexOfAny(new[] { 'M', 'm' });
            if (mi == -1)
            {
                // nope.
                return 0;
            }

            // whether it's a capital M or not
            // on xterm mouse release is signaled by lowercase m
            var isM = buffer[mi] == 'M';

            // whether it's urxvt or not
            var isU = false;

            // buf[2] is safe here, because having M or m found means we have at
            // least 3 bytes in a string
            if (buffer[2] == '<')
            {
                buffer = buffer.Slice(3, mi - 3);
            }
            else
            {
                isU = true;
                buffer = buffer.Slice(2, mi - 2);
            }

            var s1 = buffer.IndexOf(';');
            var s2 = buffer.LastIndexOf(';');
            // not found or only one ';'
            if (s1 == -1 || s2 == -1 || s1 == s2)
            {
                return 0;
            }

            //parse the ints - button status, x, y
            if (!int.TryParse(buffer.Slice(0, s1).AsString(), out var b))
            {
                return 0;
            }

            if (!int.TryParse(buffer.Slice(s1 + 1, s2 - s1 - 1).AsString(), out var x))
            {
                return 0;
            }
            x -= 1;

            if (!int.TryParse(buffer.Slice(s2 + 1).AsString(), out var y))
            {
                return 0;
            }
            y -= 1;

            // on urxvt, first number is encoded exactly as in X10, but we need to
            // make it zero-based, on xterm it is zero-based already
            if (isU)
            {
                b -= 32;
            }

            if ((b & 64) != 0)
            {
                var wheel = ((b & 3) == 0) ? MouseWheelDirection.Up : MouseWheelDirection.Down;
                var status = emit.LastMouseStatus.CopyMoved(x, y);
                emit.OnMouseWheel(wheel, status);
            }

            switch (b & 3)
            {
                case 0:
                    emit.OnMouseChange(emit.LastMouseStatus.CopyButtonChange(x, y, MouseButton.Left, isM));
                    break;
                case 1:
                    emit.OnMouseChange(emit.LastMouseStatus.CopyButtonChange(x, y, MouseButton.Middle, isM));
                    break;
                case 2:
                    emit.OnMouseChange(emit.LastMouseStatus.CopyButtonChange(x, y, MouseButton.Right, isM));
                    break;
                case 3:
                default:
                    // mouse release or garbage.  release all buttons
                    // the assumption is we have no idea what's going on since _which_ button is released is unspecified
                    emit.OnMouseChange(new MouseStatus(x, y, MouseButton.None));
                    break;
            }

            if ((b & 32) != 0)
            {
                // event.Mod |= ModMotion
                // this is probably already handled by the onmouse change processing
            }

            return mi + 1;

        }
    }
}
