using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleBox.Terminal
{
    public class KeysMap
    {
        public Dictionary<string, ConsoleKeyInfo> Keys { get; set; }

        private readonly InputEventEmitter emit;

        private void Add(byte key, char c, ConsoleKey k = default(ConsoleKey), bool shift = false, bool alt = false, bool ctrl = false)
        {
            Add(((char)key).ToString(), c, k, shift, alt, ctrl);
        }

        private void Add(string key, char c, ConsoleKey k = default(ConsoleKey), bool shift = false, bool alt = false, bool ctrl = false)
        {
            var cinfo = new ConsoleKeyInfo(c, k, shift, alt, ctrl);

            if (Keys.ContainsKey(key))
            {
                Keys[key] = cinfo;
            }
            else
            {
                Keys.Add(key.ToString(), cinfo);
            }
        }

        /// <summary>
        /// "Standard" Windows console mapping when VT is enabled
        /// </summary>
        /// <param name="emit"></param>
        public KeysMap(InputEventEmitter emit)
        {
            this.emit = emit;
            Keys = new Dictionary<string, ConsoleKeyInfo>();

            //KeyCtrlTilde = 0x00,
            //KeyCtrl2 = 0x00,
            //KeyCtrlSpace = 0x00,

            //KeyCtrlA = 0x01,
            //KeyCtrlB = 0x02,
            //KeyCtrlC = 0x03,
            //KeyCtrlD = 0x04,
            //KeyCtrlE = 0x05,
            //KeyCtrlF = 0x06,
            //KeyCtrlG = 0x07,
            //Add(0x01, 'a', ctrl: true);
            //Add(0x02, 'b', ctrl: true);
            //Add(0x03, 'c', ctrl: true);
            //Add(0x04, 'd', ctrl: true);
            //Add(0x05, 'e', ctrl: true);
            //Add(0x06, 'f', ctrl: true);
            //Add(0x07, 'g', ctrl: true);

            //KeyBackspace = 0x08,
            //KeyCtrlH = 0x08,
            //Add(0x08, 'h', ctrl: true);

            //KeyTab = 0x09,
            //KeyCtrlI = 0x09,
            //Add(0x09, '\t', ConsoleKey.Tab);


            // Default keys
            // This is punked - clobbers things like period with delete
            foreach (ConsoleKey k in Enum.GetValues(typeof(ConsoleKey)))
            {
                //Add((byte)k, (char)k, k);
            }


            //KeyCtrlJ = 0x0A,
            //KeyCtrlK = 0x0B,
            //KeyCtrlL = 0x0C,
            //KeyEnter = 0x0D,
            //KeyCtrlM = 0x0D,
            //KeyCtrlN = 0x0E,
            //KeyCtrlO = 0x0F,
            //KeyCtrlP = 0x10,
            //KeyCtrlQ = 0x11,
            //KeyCtrlR = 0x12,
            //KeyCtrlS = 0x13,
            //KeyCtrlT = 0x14,
            //KeyCtrlU = 0x15,
            //KeyCtrlV = 0x16,
            //KeyCtrlW = 0x17,
            //KeyCtrlX = 0x18,
            //KeyCtrlY = 0x19,
            //KeyCtrlZ = 0x1A,

            //KeyEsc = 0x1B,

            //KeyCtrlLsqBracket = 0x1B,
            //KeyCtrl3 = 0x1B,
            //KeyCtrl4 = 0x1C,
            //KeyCtrlBackslash = 0x1C,
            //KeyCtrl5 = 0x1D,
            //KeyCtrlRsqBracket = 0x1D,
            //KeyCtrl6 = 0x1E,
            //KeyCtrl7 = 0x1F,
            //KeyCtrlSlash = 0x1F,
            //KeyCtrlUnderscore = 0x1F,
            //KeySpace = 0x20,
            //KeyBackspace2 = 0x7F,
            //KeyCtrl8 = 0x7F

            var aaaaa = (ushort)(0xFFFF - 13);
            var z = 0x0E;


            for (var i = 0; i < TermInfo.eterm_keys.Length; i++)
            {
                var s = TermInfo.eterm_keys[i];
                var k = (char)(0xFFFF - i);
                var test = "fark";
            }

            Add("\t", '\t', ConsoleKey.Tab);
            Add(" ", ' ', ConsoleKey.Spacebar);
            Add("\r", '\r', ConsoleKey.Enter);

            Add("\x1b", '\0', ConsoleKey.Escape);
            Add("\x7f", '\0', ConsoleKey.Backspace);
            Add("\x1a", '\0', ConsoleKey.Pause);

            Add("\x1b[H", '\0', ConsoleKey.Home);
            Add("\x1b[F", '\0', ConsoleKey.End);

            Add("\x1b[2~", '\0', ConsoleKey.Insert);
            Add("\x1b[3~", '\0', ConsoleKey.Delete);
            Add("\x1b[5~", '\0', ConsoleKey.PageUp);
            Add("\x1b[6~", '\0', ConsoleKey.PageDown);

            Add("\x1bOP", '\0', ConsoleKey.F1);
            Add("\x1bOQ", '\0', ConsoleKey.F2);
            Add("\x1bOR", '\0', ConsoleKey.F3);
            Add("\x1bOS", '\0', ConsoleKey.F4);

            Add("\x1b[15~", '\0', ConsoleKey.F5);
            Add("\x1b[17~", '\0', ConsoleKey.F6);
            Add("\x1b[18~", '\0', ConsoleKey.F7);
            Add("\x1b[19~", '\0', ConsoleKey.F8);
            Add("\x1b[20~", '\0', ConsoleKey.F9);
            Add("\x1b[21~", '\0', ConsoleKey.F10);
            Add("\x1b[23~", '\0', ConsoleKey.F11);
            Add("\x1b[24~", '\0', ConsoleKey.F12);

            Add("\x1b[A", '\0', ConsoleKey.UpArrow);
            Add("\x1b[B", '\0', ConsoleKey.DownArrow);
            Add("\x1b[C", '\0', ConsoleKey.RightArrow);
            Add("\x1b[D", '\0', ConsoleKey.LeftArrow);

            Add("\x1b[1;5A", '\0', ConsoleKey.UpArrow, ctrl: true);
            Add("\x1b[1;5B", '\0', ConsoleKey.DownArrow, ctrl: true);
            Add("\x1b[1;5C", '\0', ConsoleKey.RightArrow, ctrl: true);
            Add("\x1b[1;5D", '\0', ConsoleKey.LeftArrow, ctrl: true);
        }

        public void Register(InputMapper map)
        {
            foreach (var kvp in Keys)
            {
                map.Add(kvp.Key, (buf) =>
                {
                    emit.OnKey(kvp.Value);
                    return kvp.Key.Length;
                });
            }
        }

    }
}
