using System;
using System.Drawing;
using System.Threading;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ConsoleBox.Unix
{
    public static class Interop
    {
        internal const string SystemNative = "System.Native";

        [DllImport(SystemNative, EntryPoint = "SystemNative_InitializeConsoleBeforeRead")]
        public static extern void InitializeConsoleBeforeRead(byte minChars = 1, byte decisecondsTimeout = 0);

        [DllImport(SystemNative, EntryPoint = "SystemNative_UninitializeConsoleAfterRead")]
        public static extern void UninitializeConsoleAfterRead();
    }
}