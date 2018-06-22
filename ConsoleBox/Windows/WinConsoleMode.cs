using System;

namespace ConsoleBox.Windows
{
    /// <summary>
    /// Helper for tracking the modes of a console handle
    /// </summary>
    internal class WinConsoleMode
    {
        public uint OriginalMode { get; private set; }
        public uint CurrentMode { get; private set; }
        public IntPtr Handle { get; }

        public WinConsoleMode(IntPtr handle)
        {
            Handle = handle;
        }

        public void Initialize()
        {
            if (!Kernel32.GetConsoleMode(Handle, out var inMode))
            {
                throw new Exception("Unable to retrieve current console modes.");
            }

            OriginalMode = inMode;
            CurrentMode = inMode;
        }

        public void Remove(uint mode)
        {
            if ((CurrentMode & mode) == 0)
            {
                // mode is not already set
                var m = CurrentMode & ~mode;
                if (!Kernel32.SetConsoleMode(Handle, m))
                {
                    throw new Exception("Failed to remove console mode.  " + mode);
                }
                CurrentMode = m;
            }
        }

        public void Add(uint mode)
        {
            if ((CurrentMode & mode) == 0)
            {
                // mode is not already set
                var m = CurrentMode | mode;
                if (!Kernel32.SetConsoleMode(Handle, m))
                {
                    throw new Exception("Failed to set console mode.  " + mode);
                }
                CurrentMode = m;
            }
        }

        public void Reset()
        {
            if (CurrentMode != OriginalMode)
            {
                if (!Kernel32.SetConsoleMode(Handle, OriginalMode))
                {
                    throw new Exception("Unable to reset console mode to original.  ");
                }

                CurrentMode = OriginalMode;
            }
        }
    }
}
