using System.IO;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleBox
{
    //http://www.termsys.demon.co.uk/vtansi.htm
    //https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#example
    //https://en.wikipedia.org/wiki/ANSI_escape_code

    /// <summary>
    /// There's a bunch of 'yuck' in here...
    /// </summary>
    public class VTRenderer : IRenderer, IConsoleOutput
    {
        StringBuilder builder = new StringBuilder();
        string builderA = string.Empty;
        readonly TextWriter writer;

        public VTRenderer(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Clear()
        {
            writer.Write("\x1b[2J");
        }

        public void SetCursor(int x, int y)
        {
            writer.Write("\x1b[" + y + ";" + x + "f");
        }

        public void SetCursorVisibility(bool isVisible)
        {
            // TODO - this should probably check if already hidden/visible
            const string show = "\x1b[?25h";
            const string hide = "\x1b[?25l";
            writer.Write(isVisible ? show : hide);
        }

        public void Set(int x, int y, char c, Color fg, Color bg, CellAttributes attr)
        {
            //builder.Append(GetOutput(x, y, c, fg, bg, attr));
            AppendToOutput(builder, x, y, c, fg, bg, attr);
        }

        public void Flush()
        {
            writer.Write(builder);
            writer.Flush();
            builder = new StringBuilder();
        }

        private static void AppendToOutput(StringBuilder builder, int x, int y, char c, Color fg, Color bg, CellAttributes attr)
        {
            // TODO - color conversion options
            //var fgc = ColorExtensions.RGBToAnsi256(fg.R, fg.G, fg.B);
            //var bgc = ColorExtensions.RGBToAnsi256(bg.R, bg.G, bg.B);

            // position
            builder
                .Append("\x1b[")
                .Append(y + 1)
                .Append(";")
                .Append(x + 1)
                .Append("f");

            builder.Append("\x1b[");
            if (attr != CellAttributes.None)
            {
                if (attr.HasFlag(CellAttributes.Bright))
                {
                    builder.Append("1;");
                }
                if (attr.HasFlag(CellAttributes.Dim))
                {
                    builder.Append("2;");
                }
                if (attr.HasFlag(CellAttributes.Underscore))
                {
                    builder.Append("4;");
                }
                if (attr.HasFlag(CellAttributes.Blink))
                {
                    builder.Append("5;");
                }
                if (attr.HasFlag(CellAttributes.Reverse))
                {
                    builder.Append("7;");
                }
                if (attr.HasFlag(CellAttributes.Hidden))
                {
                    builder.Append("8;");
                }
            }

            if (fg.A > 0)
            {
                builder.Append("38;2;" + fg.R + ";" + fg.G + ";" + fg.B + ";");
            }

            if (bg.A > 0)
            {
                builder.Append("48;2;" + bg.R + ";" + bg.G + ";" + bg.B);
            }

            builder.Append("m");

            builder
                .Append(c) // character
                .Append("\x1b[0m") //reset
                ;


        }

        private static string MapAttributesToString(CellAttributes attr, Color fg, Color bg)
        {
            string mapped = "\x1b[";

            if (attr != CellAttributes.None)
            {
                if (attr.HasFlag(CellAttributes.Bright))
                {
                    mapped += "1;";
                }
                if (attr.HasFlag(CellAttributes.Dim))
                {
                    mapped += "2;";
                }
                if (attr.HasFlag(CellAttributes.Underscore))
                {
                    mapped += "4;";
                }
                if (attr.HasFlag(CellAttributes.Blink))
                {
                    mapped += "5;";
                }
                if (attr.HasFlag(CellAttributes.Reverse))
                {
                    mapped += "7;";
                }
                if (attr.HasFlag(CellAttributes.Hidden))
                {
                    mapped += "8;";
                }
            }

            if (fg.A > 0)
            {
                mapped += "38;2;" + fg.R + ";" + fg.G + ";" + fg.B + ";";
            }
            else
            {
                //mapped += "39;";
            }

            if (bg.A > 0)
            {
                mapped += "48;2;" + bg.R + ";" + bg.G + ";" + bg.B;
            }
            else
            {
                //mapped += "49m";
            }

            return mapped + "m";
        }

        private string GetOutput(int x, int y, char c, Color fg, Color bg, CellAttributes attr)
        {
            // TODO - color conversion options
            //var fgc = ColorExtensions.RGBToAnsi256(fg.R, fg.G, fg.B);
            //var bgc = ColorExtensions.RGBToAnsi256(bg.R, bg.G, bg.B);

            //0   Reset all attributes
            //1   Bright
            //2   Dim
            //4   Underscore
            //5   Blink
            //7   Reverse
            //8   Hidden


            //var attrs =
            //   $"{ESC}["
            //   + (attr.HasFlag(CellAttributes.Bright) ? ";1" : string.Empty)
            //   + (attr.HasFlag(CellAttributes.Dim) ? ";2" : string.Empty)
            //   + (attr.HasFlag(CellAttributes.Underscore) ? ";4" : string.Empty)
            //   + (attr.HasFlag(CellAttributes.Blink) ? ";5" : string.Empty)
            //   + (attr.HasFlag(CellAttributes.Reverse) ? ";7" : string.Empty)
            //   + (attr.HasFlag(CellAttributes.Hidden) ? ";8" : string.Empty)
            //   //+ $";38;5;{fgc};48;5;{bgc}m"  // ansi256
            //   //+ ((fg.A > 0) ?  $";38;2;{fg.R};{fg.G};{fg.B}" : ";39")
            //   + ((bg.A > 0) ? $";48;2;{bg.R};{bg.G};{bg.B}" : ";49")
            //   + "m" //ansi24b
            //   ;

            //var attrs = ;

            //if (!(bg.A > 0))
            //{
            //    var bob = fg;
            //}

            //http://www.termsys.demon.co.uk/vtansi.htm
            var zz =
                //"\x1b[s"
                "\x1b[" + (y + 1) + ";" + (x + 1) + "f"
                + MapAttributesToString(attr, fg, bg)
                + c
                + "\x1b[0m"
                //+ "\x1b[u"
                //+ "\x1b[39;49m"
                ;

            return zz;
        }


    }
}
