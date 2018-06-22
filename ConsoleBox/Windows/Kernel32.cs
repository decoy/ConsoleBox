using System;
using System.Runtime.InteropServices;

namespace ConsoleBox.Windows
{
    /// <summary>
    /// Windows Kernel pinvoke calls
    /// //https://github.com/dotnet/corefx/tree/1fc008a7e174345826e658672b74aa449fb3573f/src/Common/src/Interop/Windows/kernel32s
    /// </summary>
    internal class Kernel32
    {
        const string KERNEL32_NAME = "kernel32";

        [DllImport(KERNEL32_NAME, SetLastError = true)]
        internal extern static bool GetConsoleMode(IntPtr handle, out uint mode);

        [DllImport(KERNEL32_NAME, SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr handle, uint mode);

        [DllImport(KERNEL32_NAME, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "PeekConsoleInputW")]
        internal static extern bool PeekConsoleInput(
            IntPtr hConsoleInput,
            out INPUT_RECORD buffer,
            int numInputRecords_UseOne,
            out int numEventsRead);

        [DllImport(KERNEL32_NAME, CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "ReadConsoleInputW")]
        internal static extern bool ReadConsoleInputW(
            IntPtr hConsoleInput,
            out INPUT_RECORD buffer,
            int numInputRecords_UseOne,
            out int numEventsRead);

        [DllImport(KERNEL32_NAME, SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "WriteConsoleOutputW")]
        internal static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput,
            CHAR_INFO[,] buffer,
            COORD bufferSize,
            COORD bufferCoord,
            ref SMALL_RECT writeRegion);

        [DllImport(KERNEL32_NAME, SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport(KERNEL32_NAME)]
        static extern bool WriteConsoleOutputCharacter(
            IntPtr hConsoleOutput,
            string lpCharacter,
            uint nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfCharsWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputAttribute(
            IntPtr hConsoleOutput,
            ushort[] lpAttribute,
            uint nLength,
            COORD dwWriteCoord,
            out uint lpNumberOfAttrsWritten);

        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUT_RECORD
        {
            [FieldOffset(0)]
            internal ushort EventType;
            [FieldOffset(4)]
            internal KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            internal MOUSE_EVENT_RECORD MouseEvent;
            [FieldOffset(4)]
            internal WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
            [FieldOffset(4)]
            internal MENU_EVENT_RECORD MenuEvent;
            [FieldOffset(4)]
            internal FOCUS_EVENT_RECORD FocusEvent;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct KEY_EVENT_RECORD
        {
            internal int keyDown;
            internal short repeatCount;
            internal short virtualKeyCode;
            internal short virtualScanCode;
            internal char uChar; // Union between WCHAR and ASCII char
            internal int controlKeyState;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSE_EVENT_RECORD
        {
            internal COORD dwMousePosition;
            internal uint dwButtonState;
            internal uint dwControlKeyState;
            internal uint dwEventFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOW_BUFFER_SIZE_RECORD
        {
            internal COORD dwSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MENU_EVENT_RECORD
        {
            internal uint dwCommandId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct FOCUS_EVENT_RECORD
        {
            internal uint bSetFocus;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CHAR_INFO
        {
            ushort charData;
            short attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }
    }
}
