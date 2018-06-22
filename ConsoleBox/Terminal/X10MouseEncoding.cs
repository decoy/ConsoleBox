using System;

namespace ConsoleBox.Terminal
{
    // X10 mouse encoding, the simplest one
    // \033 [ M Cb Cx Cy
    public class X10MouseEncoding
    {
        public const string PREFIX = "\x1b[M";

        private readonly InputEventEmitter emit;
        public X10MouseEncoding(InputEventEmitter emit)
        {
            this.emit = emit;
        }

        public void Register(InputMapper mapper)
        {
            mapper.Add(PREFIX, Process);
        }

        public int Process(ArraySegment<char> buffer)
        {
            if (buffer.Count < 6) { return 0; }

            var b = buffer[3] - 32;
            // the coord is 1,1 for upper left
            var x = buffer[4] - 1 - 32;
            var y = buffer[5] - 1 - 32;

            if ((b & 64) != 0)
            {
                // mouse wheel event
                var wheel = (b & 3) == 0 ? MouseWheelDirection.Up : MouseWheelDirection.Down;
                var status = emit.LastMouseStatus.CopyMoved(x, y);
                emit.OnMouseWheel(wheel, status);
            }
            else
            {
                switch (b & 3)
                {
                    case 0:
                        emit.OnMouseChange(emit.LastMouseStatus.CopyButtonChange(x, y, MouseButton.Left, true));
                        break;
                    case 1:
                        emit.OnMouseChange(emit.LastMouseStatus.CopyButtonChange(x, y, MouseButton.Middle, true));
                        break;
                    case 2:
                        emit.OnMouseChange(emit.LastMouseStatus.CopyButtonChange(x, y, MouseButton.Right, true));
                        break;
                    case 3:
                    default:
                        // mouse release or garbage.  release all buttons
                        // the assumption is we have no idea what's going on since _which_ button is released is unspecified
                        emit.OnMouseChange(new MouseStatus(x, y, MouseButton.None));
                        break;
                }
            }

            if ((b & 32) != 0)
            {
                // event.Mod |= ModMotion
                // this is probably already handled by the onmouse change processing
            }

            return 6;
        }
    }

}
