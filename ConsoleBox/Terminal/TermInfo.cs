using System.Threading.Tasks;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleBox.Terminal
{
    /// <summary>
    /// Term db parsing
    /// This is largely ported from 
    /// https://github.com/nsf/termbox
    /// and 
    /// https://github.com/nsf/termbox-go
    /// </summary>
    public class TermInfo
    {
        public enum Things
        {
            T_ENTER_CA,
            T_EXIT_CA,
            T_SHOW_CURSOR,
            T_HIDE_CURSOR,
            T_CLEAR_SCREEN,
            T_SGR0,
            T_UNDERLINE,
            T_BOLD,
            T_BLINK,
            T_REVERSE,
            T_ENTER_KEYPAD,
            T_EXIT_KEYPAD,
            T_ENTER_MOUSE,
            T_EXIT_MOUSE,
            T_FUNCS_NUM,
        }

        public const string ENTER_MOUSE_SEQ = "\x1b[?1000h\x1b[?1002h\x1b[?1015h\x1b[?1006h";
        public const string EXIT_MOUSE_SEQ = "\x1b[?1006l\x1b[?1015l\x1b[?1002l\x1b[?1000l";
        public const int EUNSUPPORTED_TERM = -1;

        public const int ti_magic = 0432;
        public const int ti_header_length = 12;

        //       ti_mouse_enter   = "\x1b[?1000h\x1b[?1002h\x1b[?1015h\x1b[?1006h"
        //ti_mouse_leave   = "\x1b[?1006l\x1b[?1015l\x1b[?1002l\x1b[?1000l"


        public static string[] rxvt_256color_keys = {
            "\033[11~","\033[12~","\033[13~","\033[14~","\033[15~","\033[17~","\033[18~","\033[19~","\033[20~","\033[21~","\033[23~","\033[24~","\033[2~","\033[3~","\033[7~","\033[8~","\033[5~","\033[6~","\033[A","\033[B","\033[D","\033[C", "\0"
        };
        public static string[] rxvt_256color_funcs = {
            "\0337\033[?47h", "\033[2J\033[?47l\0338", "\033[?25h", "\033[?25l", "\033[H\033[2J", "\033[m", "\033[4m", "\033[1m", "\033[5m", "\033[7m", "\033=", "\033>", ENTER_MOUSE_SEQ, EXIT_MOUSE_SEQ,
        };

        // Eterm
        public static string[] eterm_keys = {
            "\033[11~","\033[12~","\033[13~","\033[14~","\033[15~","\033[17~","\033[18~","\033[19~","\033[20~","\033[21~","\033[23~","\033[24~","\033[2~","\033[3~","\033[7~","\033[8~","\033[5~","\033[6~","\033[A","\033[B","\033[D","\033[C", "\0"
        };
        public static string[] eterm_funcs = {
            "\0337\033[?47h", "\033[2J\033[?47l\0338", "\033[?25h", "\033[?25l", "\033[H\033[2J", "\033[m", "\033[4m", "\033[1m", "\033[5m", "\033[7m", "", "", "", "",
        };

        // screen
        public static string[] screen_keys = {
            "\033OP","\033OQ","\033OR","\033OS","\033[15~","\033[17~","\033[18~","\033[19~","\033[20~","\033[21~","\033[23~","\033[24~","\033[2~","\033[3~","\033[1~","\033[4~","\033[5~","\033[6~","\033OA","\033OB","\033OD","\033OC", "\0"
        };
        public static string[] screen_funcs = {
            "\033[?1049h", "\033[?1049l", "\033[34h\033[?25h", "\033[?25l", "\033[H\033[J", "\033[m", "\033[4m", "\033[1m", "\033[5m", "\033[7m", "\033[?1h\033=", "\033[?1l\033>", ENTER_MOUSE_SEQ, EXIT_MOUSE_SEQ,
        };

        // rxvt-unicode
        public static string[] rxvt_unicode_keys = {
            "\033[11~","\033[12~","\033[13~","\033[14~","\033[15~","\033[17~","\033[18~","\033[19~","\033[20~","\033[21~","\033[23~","\033[24~","\033[2~","\033[3~","\033[7~","\033[8~","\033[5~","\033[6~","\033[A","\033[B","\033[D","\033[C", "\0"
        };
        public static string[] rxvt_unicode_funcs = {
            "\033[?1049h", "\033[r\033[?1049l", "\033[?25h", "\033[?25l", "\033[H\033[2J", "\033[m\033(B", "\033[4m", "\033[1m", "\033[5m", "\033[7m", "\033=", "\033>", ENTER_MOUSE_SEQ, EXIT_MOUSE_SEQ,
        };

        // linux
        public static string[] linux_keys = {
            "\033[[A","\033[[B","\033[[C","\033[[D","\033[[E","\033[17~","\033[18~","\033[19~","\033[20~","\033[21~","\033[23~","\033[24~","\033[2~","\033[3~","\033[1~","\033[4~","\033[5~","\033[6~","\033[A","\033[B","\033[D","\033[C", "\0"
        };
        public static string[] linux_funcs = {
            "", "", "\033[?25h\033[?0c", "\033[?25l\033[?1c", "\033[H\033[J", "\033[0;10m", "\033[4m", "\033[1m", "\033[5m", "\033[7m", "", "", "", "",
        };

        // xterm
        public static string[] xterm_keys = {
            "\033OP","\033OQ","\033OR","\033OS","\033[15~","\033[17~","\033[18~","\033[19~","\033[20~","\033[21~","\033[23~","\033[24~","\033[2~","\033[3~","\033OH","\033OF","\033[5~","\033[6~","\033OA","\033OB","\033OD","\033OC", "\0"
        };
        public static string[] xterm_funcs = {
            "\033[?1049h", "\033[?1049l", "\033[?12l\033[?25h", "\033[?25l", "\033[H\033[2J", "\033(B\033[m", "\033[4m", "\033[1m", "\033[5m", "\033[7m", "\033[?1h\033=", "\033[?1l\033>", ENTER_MOUSE_SEQ, EXIT_MOUSE_SEQ,
        };

        public struct Term
        {
            public string name;
            public string[] keys;
            public string[] funcs;
        }

        public static Term[] Terminals = {
            new Term() { name = "rxvt-256color", keys = rxvt_256color_keys, funcs = rxvt_256color_funcs },
            new Term() { name ="Eterm", keys = eterm_keys, funcs = eterm_funcs },
            new Term() { name ="screen", keys = screen_keys, funcs = screen_funcs },
            new Term() { name ="rxvt-unicode", keys = rxvt_unicode_keys, funcs= rxvt_unicode_funcs },
            new Term() { name ="linux", keys = linux_keys, funcs = linux_funcs },
            new Term() { name ="xterm", keys = xterm_keys, funcs = xterm_funcs },
        };

        public static Term[] CompatibilityTable =
        {
            new Term() { name = "xterm", keys = xterm_keys, funcs = xterm_funcs},
            new Term() { name = "rxvt", keys = rxvt_unicode_keys, funcs = rxvt_unicode_funcs},
            new Term() { name = "linux", keys = linux_keys, funcs = linux_funcs},
            new Term() { name = "Eterm", keys = eterm_keys, funcs = eterm_funcs},
            new Term() { name = "screen", keys = screen_keys, funcs = screen_funcs},
            // let's assume that 'cygwin' is xterm compatible
            new Term() { name = "cygwin", keys = xterm_keys, funcs = xterm_funcs},
            new Term() { name = "st", keys = xterm_keys, funcs = xterm_funcs},
        };

        public static bool LoadTermInfo(out byte[] data)
        {
            var term = Environment.GetEnvironmentVariable("TERM");

            if (string.IsNullOrWhiteSpace(term))
            {
                throw new ArgumentException("Term not set");
            }

            // The following behaviour follows the one described in terminfo(5) as
            // distributed by ncurses.

            var terminfo = Environment.GetEnvironmentVariable("TERMINFO");
            if (!string.IsNullOrEmpty(terminfo))
            {
                // if TERMINFO is set, no other directory should be searched
                return TryPath(term, terminfo, out data);
            }

            // next, consider ~/.terminfo
            var home = Environment.GetEnvironmentVariable("HOME");
            if (!string.IsNullOrEmpty(home))
            {
                if (TryPath(term, Path.Combine(home, ".terminfo"), out data))
                {
                    return true;
                }
            }

            // next, TERMINFO_DIRS
            var dirs = Environment.GetEnvironmentVariable("TERMINFO_DIRS");
            if (!string.IsNullOrEmpty(dirs))
            {
                foreach (var dir in dirs.Split(":"))
                {
                    var d = dir;
                    if (string.IsNullOrEmpty(dir))
                    {
                        d = "/usr/share/terminfo";
                    }

                    if (TryPath(term, d, out data))
                    {
                        return true;
                    }
                }
            }

            // fall back to /usr/share/terminfo
            return TryPath(term, "/usr/share/terminfo", out data);
        }

        public static bool TryPath(string term, string path, out byte[] data)
        {
            // first try, the typical *nix path
            var terminfo = Path.Combine(path, term.Substring(0, 1), term);
            if (File.Exists(terminfo))
            {
                data = File.ReadAllBytes(terminfo);
                return true;
            }

            // fallback to darwin specific dirs structure
            //terminfo = Path.Combine(path, BitConverter.ToString(new byte { (byte)term[0] }))
            if (File.Exists(terminfo))
            {
                data = File.ReadAllBytes(terminfo);
                return true;
            }

            data = new byte[0];
            return false;
        }

        public static void SetupTerm()
        {
            byte[] data;
            if (!LoadTermInfo(out data))
            {
                // load the built in stuff
                SetupTermBuiltin();
            }


            var header = new int[6];
            var str_offset = 0;
            var table_offset = 0;

            // 0: magic number, 1: size of names section, 2: size of boolean section, 3:
            // size of numbers section (in integers), 4: size of the strings section (in
            // integers), 5: size of the string table
        }

        public static void SetupTermBuiltin()
        {
            var name = Environment.GetEnvironmentVariable("TERM");
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("TERM environment variable not set");
            }

            foreach (var t in Terminals)
            {
                if (t.name == name)
                {
                    // return t?
                    return;
                }
            }

            foreach (var t in CompatibilityTable)
            {
                if (t.name.Contains(name))
                {
                    // return t?
                    return;
                }
            }

            throw new Exception("Unsupported terminal: " + name);
        }
    }
}
