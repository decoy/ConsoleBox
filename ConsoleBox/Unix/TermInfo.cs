using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleBox.Unix
{
    public class TermInfo
    {
        internal enum WellKnownNumbers
        {
            Columns = 0,
            Lines = 2,
            MaxColors = 13,
        }

        internal enum WellKnownStrings
        {
            Bell = 1,
            Clear = 5,
            CursorAddress = 10,
            CursorLeft = 14,
            CursorPositionReport = 294,
            OrigPairs = 297,
            OrigColors = 298,
            SetAnsiForeground = 359,
            SetAnsiBackground = 360,
            CursorInvisible = 13,
            CursorVisible = 16,
            FromStatusLine = 47,
            ToStatusLine = 135,
            KeyBackspace = 55,
            KeyClear = 57,
            KeyDelete = 59,
            KeyDown = 61,
            KeyF1 = 66,
            KeyF10 = 67,
            KeyF2 = 68,
            KeyF3 = 69,
            KeyF4 = 70,
            KeyF5 = 71,
            KeyF6 = 72,
            KeyF7 = 73,
            KeyF8 = 74,
            KeyF9 = 75,
            KeyHome = 76,
            KeyInsert = 77,
            KeyLeft = 79,
            KeyPageDown = 81,
            KeyPageUp = 82,
            KeyRight = 83,
            KeyScrollForward = 84,
            KeyScrollReverse = 85,
            KeyUp = 87,
            KeypadXmit = 89,
            KeyBackTab = 148,
            KeyBegin = 158,
            KeyEnd = 164,
            KeyEnter = 165,
            KeyHelp = 168,
            KeyPrint = 176,
            KeySBegin = 186,
            KeySDelete = 191,
            KeySelect = 193,
            KeySHelp = 198,
            KeySHome = 199,
            KeySLeft = 201,
            KeySPrint = 207,
            KeySRight = 210,
            KeyF11 = 216,
            KeyF12 = 217,
            KeyF13 = 218,
            KeyF14 = 219,
            KeyF15 = 220,
            KeyF16 = 221,
            KeyF17 = 222,
            KeyF18 = 223,
            KeyF19 = 224,
            KeyF20 = 225,
            KeyF21 = 226,
            KeyF22 = 227,
            KeyF23 = 228,
            KeyF24 = 229,
        }

        public void DoTheThing()
        {
            // get term env var

            // if not set, return compat/empty

            // if TERMINFO is set, use that.

            // else, try ~/.terminfo

            // then TERMINFO_DIRS

            // fall back to /usr/share/terminfo
        }
    }
}
