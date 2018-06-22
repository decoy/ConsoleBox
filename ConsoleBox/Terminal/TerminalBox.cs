using System;
using System.Drawing;
using System.Threading;
using System.IO;

namespace ConsoleBox.Terminal
{
    //https://github.com/dotnet/corefx/blob/fc225bd023e4e692d18442b02454cada7c252368/src/System.Console/src/System/ConsolePal.Unix.cs

    /// <summary>
    /// A virtual terminal (VT Command) console box.
    /// Uses standard xterm type encodings for mouse events
    /// and ansi encoding for drawing
    /// </summary>
    public class TerminalBox : IConsoleBox
    {
        public int Width { get { return Console.WindowWidth; } }
        public int Height { get { return Console.WindowHeight; } }

        public event EventHandler<MouseButtonEvent> MouseButtonDown
        {
            add { events.MouseButtonDown += value; }
            remove { events.MouseButtonDown -= value; }
        }

        public event EventHandler<MouseButtonEvent> MouseButtonUp
        {
            add { events.MouseButtonUp += value; }
            remove { events.MouseButtonUp -= value; }
        }

        public event EventHandler<MouseEvent> MouseMove
        {
            add { events.MouseMove += value; }
            remove { events.MouseMove -= value; }
        }

        public event EventHandler<MouseWheelEvent> MouseWheel
        {
            add { events.MouseWheel += value; }
            remove { events.MouseWheel -= value; }
        }

        public event EventHandler<MouseButtonEvent> MouseClick
        {
            add { events.MouseClick += value; }
            remove { events.MouseClick -= value; }
        }

        public event EventHandler<ResizeEvent> ResizeEvent
        {
            add { events.ResizeEvent += value; }
            remove { events.ResizeEvent -= value; }
        }

        public event EventHandler<ConsoleKeyInfo> KeyEvent
        {
            add { events.KeyEvent += value; }
            remove { events.KeyEvent -= value; }
        }

        private readonly InputEventEmitter events;
        private VTRenderer render;

        private readonly StreamReader input;
        private readonly TextWriter output;
        private readonly InputMapper map;

        public TerminalBox(StreamReader input, TextWriter output)
        {
            this.input = input;
            this.output = output;

            events = new InputEventEmitter(this);
            map = new InputMapper();

            var keys = new KeysMap(events);
            keys.Register(map);

            var x10 = new X10MouseEncoding(events);
            x10.Register(map);

            var mencode = new ExtendedMouseModeEncoding(events);
            mencode.Register(map);

            // TODO Something is funky with encoding on win
            // sometimes the mouse events show up as \x1b\0[\0 etc...  funky \0 between each character
            // this would generally mean the encoding is punked, but it's fine most of the time.
            // might require manual parsing... ascii sometimes, double byte other times...
            // that's how termbox-go is handling it
            // for now, just going to forcibly ignore bad escapes
            map.Add("\x1b\0", (buf) => buf.Count); // eats it all if it's mangled
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Initialize()
        {
            render = new VTRenderer(output);
            events.LastWindowSize = new Size(Width, Height);
        }

        public void PollEvents(CancellationToken cancel = default(CancellationToken))
        {
            events.OnResize(Width, Height);
            var i = 0;
            var buf = new char[1024];
            while (!cancel.IsCancellationRequested)
            {
                i = input.Read(buf, 0, 1024);
                if (i == 1024)
                {
                    var x = i;
                }

                // process resize events first (avoid that extra redraw)
                events.OnResize(Width, Height);

                if (i > 0)
                {
                    Process(new ArraySegment<char>(buf, 0, i));
                }
            }
        }

        private void Process(ArraySegment<char> buffer)
        {
            var i = 0;
            while (i < buffer.Count)
            {
                if (i > 0)
                {
                    var zz = buffer[0];
                }
                var p = map.Process(buffer);
                if (p > 0)
                {
                    i += p;
                }
                else
                {
                    events.OnKey(new ConsoleKeyInfo(buffer[i], default(ConsoleKey), false, false, false));
                    i++;
                }
            }
        }

        public void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr)
        {
            render.Set(x, y, c, fg, bg, attr);
        }

        public void SetCursor(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public void SetCursorVisibility(bool isVisible)
        {
            Console.CursorVisible = isVisible;
        }

        public void ShutDown()
        {
            // turn off the mousing bits
            // should this cancel the polling?
            DisableMouse();
        }

        private const string ti_mouse_enter = "\x1b[?1000h\x1b[?1002h\x1b[?1015h\x1b[?1006h";
        private const string ti_mouse_enter_move = "\x1b[?1000h\x1b[?1003h\x1b[?1015h\x1b[?1006h";
        private const string ti_mouse_leave = "\x1b[?1006l\x1b[?1015l\x1b[?1002l\x1b[?1000l";
        private const string ti_mouse_leave_move = "\x1b[?1006l\x1b[?1015l\x1b[?1003l\x1b[?1000l";

        public bool MouseEnabled { get; private set; }
        public bool MouseMovementEnable { get; private set; }

        public void EnableMouse(bool trackMovements)
        {
            //https://stackoverflow.com/questions/5966903/how-to-get-mousemove-and-mouseclick-in-bash
            //http://invisible-island.net/xterm/ctlseqs/ctlseqs.pdf
            //https://github.com/chjj/blessed/blob/master/lib/program.js#L642
            //https://github.com/xtermjs/xterm.js/blob/master/src/xterm.js#L874

            output.Write(trackMovements ? ti_mouse_enter_move : ti_mouse_enter);
            output.Flush();
            MouseEnabled = true;
            MouseMovementEnable = trackMovements;
        }

        public void DisableMouse()
        {
            output.Write(MouseMovementEnable ? ti_mouse_leave : ti_mouse_leave_move);
            output.Flush();
            MouseEnabled = true;
            MouseMovementEnable = false;
        }

        public void Flush()
        {
            render.Flush();
        }
    }
}
