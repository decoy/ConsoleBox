using System.Collections.Generic;
using ConsoleBox.UI;
using ConsoleBox.UI.Elements;
using System.Drawing;

namespace ConsoleBox.Demo
{
    public class ImageElement : IElement
    {
        public const char HalfBlock = '▄';

        public ImageWidgetStateCache Cache { get; set; }

        public ImageElement(ImageWidgetStateCache img)
        {
            Cache = img;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var size = constraint.GetMaxSize();
            var bounds = new Rectangle(offset, size);

            Dictionary<Point, Cell> buffer;

            if (Cache.Cached.ContainsKey(size))
            {
                buffer = Cache.Cached[size];
            }
            else
            {
                buffer = GetCellsFromImage(size, Cache.Image);
                Cache.Cached.Add(size, buffer);
            }

            foreach (var kvp in buffer)
            {
                context.Buffer.Modify(bounds, kvp.Key, c => kvp.Value);
            }

            return constraint.GetMaxSize();
        }

        public static Dictionary<Point, Cell> GetCellsFromImage(Size size, Bitmap img)
        {
            var dictionary = new Dictionary<Point, Cell>();

            using (var resized = new Bitmap(img, new Size(size.Width, size.Height * 2)))
            {
                for (var x = 0; x < resized.Width; x++)
                {
                    for (var y = 0; y < resized.Height; y++)
                    {
                        var pxl = resized.GetPixel(x, y);

                        var pt = new Point(x, y / 2);

                        if (y == resized.Height - 1)
                        {
                            // it's done, bottom pixel is a blank
                            dictionary.Add(pt, new Cell()
                            {
                                Background = pxl,
                                Foreground = Color.Black,
                                Char = HalfBlock,
                            });
                        }
                        else
                        {
                            // we can add the 'bottom' one
                            var bot = resized.GetPixel(x, y + 1);
                            dictionary.Add(pt, new Cell()
                            {
                                Background = pxl,
                                Foreground = bot,
                                Char = HalfBlock,
                            });
                            y++;
                        }
                    }
                }
            }

            return dictionary;
        }

        public Size Measure(BoxConstraint constraint)
        {
            return constraint.GetMaxSize();
        }
    }
}
