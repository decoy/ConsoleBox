using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConsoleBox
{
    // TODO - the results for these could be cached
    public static class ColorExtensions
    {
        //https://misc.flogisoft.com/bash/tip_colors_and_formatting
        //https://gist.github.com/MicahElliott/719710
        //https://github.com/chadj2/bash-ui/blob/master/COLORS.md#xterm-colorspaces
        //https://jonasjacek.github.io/colors/
        //https://docs.microsoft.com/en-us/windows/console/console-screen-buffers
        //https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#example
        public static int RGBToAnsi256(int r, int g, int b)
        {
            // we use the extended greyscale palette here, with the exception of
            // black and white. normal palette only has 4 greyscale shades.
            if (r == g && g == b)
            {
                if (r < 8)
                {
                    return 16;
                }

                if (r > 248)
                {
                    return 231;
                }

                return (int)Math.Round(((r - 8) / 247.0) * 24) + 232;
            }

            var ansi = 16
                + (36 * Math.Round(r / 255.0 * 5))
                + (6 * Math.Round(g / 255.0 * 5))
                + Math.Round(b / 255.0 * 5);

            return (int)ansi;
        }

        public static ConsoleColor ToConsoleColor(this Color c)
        {
            //https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor
            int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0; // Bright bit
            index |= (c.R > 64) ? 4 : 0; // Red bit
            index |= (c.G > 64) ? 2 : 0; // Green bit
            index |= (c.B > 64) ? 1 : 0; // Blue bit
            return (ConsoleColor)index;
        }

        public static Color ToColor(this ConsoleColor c)
        {
            // might not be _quite_ accurate
            return Color.FromName(c.ToString());
        }

        public static void GenColors()
        {

            var t = Color.FromArgb(255, 255, 0, 0);

            var z = t.ToArgb();
            var zz = z & 0xFFFFFF; // remove the alpha layer
            var gg = 0xFF0000;

            var ansi16 = new Color[] {
                Color.FromArgb( 0x000000),
                Color.FromArgb( 0x800000),
                Color.FromArgb( 0x008000),
                Color.FromArgb( 0x808000),
                Color.FromArgb( 0x000080),
                Color.FromArgb(  0x800080),
                Color.FromArgb( 0x008080),
                Color.FromArgb(  0xc0c0c0),
                Color.FromArgb(  0x808080),
                Color.FromArgb( 0xff0000),
                Color.FromArgb(  0x00ff00),
                Color.FromArgb( 0xffff00),
                Color.FromArgb(  0x0000ff),
                Color.FromArgb( 0xff00ff),
                Color.FromArgb( 0x00ffff),
                Color.FromArgb( 0xffffff),
            };

            var colors = new Color[255];
            Array.Copy(ansi16, colors, ansi16.Length);

            for (var red = 0; red < 5; red++)
            {
                for (var green = 0; green < 5; green++)
                {
                    for (var blue = 0; blue < 5; blue++)
                    {
                        var c = 16 + (red * 36) + (green * 6) + blue;
                        var r = red > 0 ? red * 40 + 55 : 0;
                        var g = green > 0 ? green * 40 + 55 : 0;
                        var b = blue > 0 ? blue * 40 + 55 : 0;

                        colors[c] = Color.FromArgb(r, g, b);
                    }
                }
            }

            var x = colors;
        }

        // might be more accurate
        //static ConsoleColor FromColor(byte r, byte g, byte b)
        //{
        //    ConsoleColor ret = 0;
        //    double rr = r, gg = g, bb = b, delta = double.MaxValue;

        //    foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
        //    {
        //        var n = Enum.GetName(typeof(ConsoleColor), cc);
        //        var c = System.Drawing.Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
        //        var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
        //        if (t == 0.0)
        //            return cc;
        //        if (t < delta)
        //        {
        //            delta = t;
        //            ret = cc;
        //        }
        //    }
        //    return ret;
        //}

        //https://github.com/ichinaski/pxl/blob/c4bca700451bd113746c455f7cc03389b370302a/color.go

        //        func rgb(c color.Color) (uint16, uint16, uint16) {
        //	r, g, b, _ := c.RGBA()
        //	// Reduce color values to the range [0, 15]
        //	return uint16(r >> 8), uint16(g >> 8), uint16(b >> 8)
        //}

        //    // termColor converts a 24-bit RGB color into a term256 compatible approximation.
        //    func termColor(r, g, b uint16) uint16 {
        //	rterm := (((r* 5) + 127) / 255) * 36
        //	gterm := (((g* 5) + 127) / 255) * 6
        //	bterm := (((b* 5) + 127) / 255)

        //	return rterm + gterm + bterm + 16 + 1 // termbox default color offset
        //}


        //public static int RGBToAnsi16(int r, int g, int b)
        //{
        //    //var value = 1 in arguments? arguments[1] : convert.rgb.hsv(args)[2]; // hsv -> ansi16 optimization

        //    //value = Math.round(value / 50);

        //    //if (value == 0)
        //    //{
        //    //    return 30;
        //    //}

        //    var ansi = 30
        //        + ((Round(b / 255.0) << 2)
        //        | (Round(g / 255.0) << 1)
        //        | Round(r / 255.0));

        //    if (value == 2)
        //    {
        //        ansi += 60;
        //    }

        //    return ansi;
        //}

        private static int Round(double number)
        {
            return (int)Math.Round(number);
        }
    }

}
