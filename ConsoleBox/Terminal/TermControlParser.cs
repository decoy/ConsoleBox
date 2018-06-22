using System;
using System.Collections.Generic;

namespace ConsoleBox.Terminal
{
    /// <summary>
    /// Takes in a buffer, determines if it matches any commands
    /// Emits any related events
    /// </summary>
    public class TermControlParser
    {
        private readonly static ConsoleKeyInfo backspace = new ConsoleKeyInfo('\0', ConsoleKey.Backspace, false, false, false);
        private readonly static ConsoleKeyInfo pause = new ConsoleKeyInfo('\0', ConsoleKey.Pause, false, false, false);

        public List<ITermCommand> Commands { get; private set; } = new List<ITermCommand>();

        private readonly InputEventEmitter emitter;
        public TermControlParser(InputEventEmitter emitter)
        {
            this.emitter = emitter;
        }

        public void Process(ArraySegment<ConsoleKeyInfo> buffer)
        {
            var i = 0;
            while (i < buffer.Count)
            {
                //Backspace   0x7f(DEL)
                //Pause   0x1a(SUB)
                //Escape  0x1b(ESC)
                if (buffer[i].KeyChar == '\x1b')
                {
                    //var t = buffer.Slice(i).AsString().Replace('\x1b', '!');
                    //System.Diagnostics.Trace.WriteLine(t);
                    i += ProcessEscape(buffer.Slice(i));
                }
                else if (buffer[i].KeyChar == '\x1a')
                {
                    emitter.OnKey(pause);
                    i++;
                }
                else if (buffer[i].KeyChar == '\x7f')
                {
                    emitter.OnKey(backspace);
                    i++;
                }
                else
                {
                    emitter.OnKey(buffer[i]);
                    i++;
                }
            }
        }

        private int ProcessEscape(ArraySegment<ConsoleKeyInfo> buffer)
        {
            foreach (var cmd in Commands)
            {
                var r = cmd.Process(emitter, buffer);
                if (r > 0)
                {
                    // matched the command
                    return r;
                }
            }

            // no match, just emit as an escape key
            emitter.OnKey(buffer[0]);
            return 1;
        }
    }
}
