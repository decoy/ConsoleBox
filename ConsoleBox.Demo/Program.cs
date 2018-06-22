using ConsoleBox.Terminal;
using ConsoleBox.UI.Widgets;
using ConsoleBox.Unix;
using ConsoleBox.Windows;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleBox.Demo
{
    class Program
    {
        static void Windows()
        {
            //Console.InputEncoding = System.Text.Encoding.UTF8;
            using (var fs = WinConsole.GetStandardIn())
            using (var reader = new StreamReader(fs, Console.InputEncoding))
            {
                var box = new TerminalBox(reader, Console.Out);
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    // replace this with manual call to enable mouse/vt support on various platforms
                    // TODO shutodown and tracking 'previous' modes for windows
                    var winbox = new WinConsole();
                    winbox.Initialize();

                    box.ResizeEvent += (s, e) =>
                    {
                        // remove scroll on win
                        Console.Clear();
                        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
                    };

                    box.EnableMouse(true);
                    Start(box);
                }
            }
        }

        static void Nix()
        {
            using (var writer = new StreamWriter(Console.OpenStandardOutput()))
            using (var t = File.Open("/dev/tty", FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(t))
            {
                Console.SetIn(new StreamReader(Stream.Null));
                Console.SetOut(new StreamWriter(Stream.Null));

                Console.TreatControlCAsInput = true;
                Interop.InitializeConsoleBeforeRead();

                var box = new TerminalBox(reader, writer);
                box.EnableMouse(true);
                Start(box);
            }
        }

        static void Start(IConsoleBox box)
        {
            box.Initialize();
            var app = new WidgetApplication(box, () => new DemoApp());
            //await app.Start();
            app.StartSync();

            //var test = new TestingApp(box);
            box.PollEvents();

            box.ShutDown();
        }

        static void Main(string[] args)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Windows();
            }
            else
            {
                Nix();
            }
        }
    }
}

