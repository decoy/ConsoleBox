using System;

namespace ConsoleBox.Terminal
{
    /// <summary>
    /// Quick helpers for working with the buffer like a string
    /// </summary>
    public static class ConsoleBufferExtensions
    {
        public static bool StartsWith(this ArraySegment<char> buffer, string prefix)
        {
            if (prefix.Length > buffer.Count) { return false; }

            for (var i = 0; i < prefix.Length; i++)
            {
                if (buffer[i] != prefix[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static int IndexOf(this ArraySegment<char> buffer, char search)
        {
            for (var j = 0; j < buffer.Count; j++)
            {
                if (search == buffer[j])
                {
                    return j;
                }
            }

            return -1;
        }


        public static int LastIndexOf(this ArraySegment<char> buffer, char search)
        {
            for (var j = buffer.Count - 1; j >= 0; j--)
            {
                if (search == buffer[j])
                {
                    return j;
                }
            }

            return -1;
        }

        public static int IndexOfAny(this ArraySegment<char> buffer, char[] search)
        {
            for (var j = 0; j < buffer.Count; j++)
            {
                for (var i = 0; i < search.Length; i++)
                {
                    if (search[i] == buffer[j])
                    {
                        return j;
                    }
                }
            }

            return -1;
        }

        public static string AsString(this ArraySegment<char> buffer)
        {
            var data = string.Empty;
            for (var j = 0; j < buffer.Count; j++)
            {
                data += buffer[j];
            }

            return data;
        }
    }
}
