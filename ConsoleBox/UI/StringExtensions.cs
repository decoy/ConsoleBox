using System;
using System.Text;

namespace ConsoleBox.UI
{
    public static class StringExtensions
    {
        public static string[] PadTextArray(string[] text, int width)
        {
            for (var i = 0; i < text.Length; i++)
            {
                text[i] = text[i].PadRight(width);
            }

            return text;
        }

        public static string[] ClipTextArray(string[] text, int width, int height)
        {
            if (height < 1 || width < 1)
            {
                return new string[] { };
            }

            // clip anything that's not in bounds
            for (var i = 0; i < text.Length && i < height; i++)
            {
                var w = Math.Min(width, text[i].Length);
                text[i] = text[i].Substring(0, w);
            }

            if (text.Length > height)
            {
                var result = new string[height];
                Array.Copy(text, 0, result, 0, height);
                return result;
            }

            return text;
        }

        public static string WrapText(string text, int width, string lineBreak = "\n")
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1) { return text; }

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(lineBreak, pos);

                if (eol == -1)
                {
                    next = eol = text.Length;
                }
                else
                {
                    next = eol + lineBreak.Length;
                }

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;

                        if (len > width)
                        {
                            len = BreakLine(text, pos, width);
                            sb.Append(text, pos, len);
                            sb.Append(lineBreak);

                            // Trim whitespace following break
                            pos += len;

                            while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            {
                                pos++;
                            }
                        }
                        else
                        {
                            sb.Append(text, pos, len);
                            pos += len;
                        }



                    } while (eol > pos);
                }
                else
                {
                    sb.Append(lineBreak); // Empty line
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        public static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }



            if (i < 0)
            {
                return max; // No whitespace found; break at maximum length
            }

            var a = text.Substring(pos, i);

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
            {
                i--;
            }

            var b = text.Substring(pos, i + 1);

            // Return length of text before whitespace
            return i + 1;
        }
    }
}
