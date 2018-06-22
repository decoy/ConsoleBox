using System;

namespace ConsoleBox.Terminal
{
    using Utility;

    public class InputMapper
    {
        public PrefixTree<char, Func<ArraySegment<char>, int>> Root { get; set; }

        public InputMapper()
        {
            Root = new PrefixTree<char, Func<ArraySegment<char>, int>>((char)0);
        }

        public int Process(ArraySegment<char> buffer)
        {
            var n = Root.PrefixSearch(buffer);
            if (n != null && n.Value != null)
            {
                return n.Value(buffer);
            }
            return 0;
        }

        public void Add(string prefix, Func<ArraySegment<char>, int> mapper)
        {
            Root.Add(prefix.ToCharArray(), mapper);
        }
    }
}
