namespace ConsoleBox.UI
{
    public struct Padding
    {
        public int Top { get; set; }

        public int Left { get; set; }

        public int Bottom { get; set; }

        public int Right { get; set; }

        public bool IsEmpty
        {
            get
            {
                return Top != 0
                    && Left != 0
                    && Bottom != 0
                    && Right != 0;
            }
        }

        public Padding(int pad)
        {
            Top = Left = Bottom = Right = pad;
        }

        public Padding(int padX, int padY)
        {
            Left = Right = padX;
            Top = Bottom = padY;
        }

        public Padding(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}
