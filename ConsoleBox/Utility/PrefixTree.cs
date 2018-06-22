using System;
using System.Collections.Generic;

namespace ConsoleBox.Utility
{
    /// <summary>
    /// Prefix Tree used to lookup key maps.
    /// Basically a Trie, but a little quirkier to handle searching buffers.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class PrefixTree<TKey, TValue>
    {
        public TKey Data { get; private set; }
        public TValue Value { get; private set; }

        private Dictionary<TKey, PrefixTree<TKey, TValue>> Children { get; set; }

        public PrefixTree(TKey data, IEqualityComparer<TKey> comparer)
        {
            Children = new Dictionary<TKey, PrefixTree<TKey, TValue>>(comparer);
            Data = data;
        }

        public PrefixTree(TKey data)
        {
            Children = new Dictionary<TKey, PrefixTree<TKey, TValue>>();
            Data = data;
        }

        /// <summary>
        /// Matches the deepest that has a matching prefix
        /// Children search starts with data[0]
        /// If no children match, will return self.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public PrefixTree<TKey, TValue> PrefixSearch(ArraySegment<TKey> data)
        {
            var n = this;
            for (var i = 0; i < data.Count; i++)
            {
                var t = n.GetChild(data[i]);
                if (t == null)
                {
                    return n;
                }
                n = t;
            }

            return n;
        }

        /// <summary>
        /// Adds the data + value to the tree, creating nodes as appropriate
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        public void Add(ArraySegment<TKey> data, TValue value)
        {
            var n = GetChild(data[0], true);
            if (data.Count == 1)
            {
                n.Value = value;
            }
            else
            {
                n.Add(data.Slice(1), value);
            }
        }

        private PrefixTree<TKey, TValue> GetChild(TKey c, bool createIfNotExists = false)
        {
            if (Children.TryGetValue(c, out var node))
            {
                return node;
            }

            if (createIfNotExists)
            {
                node = new PrefixTree<TKey, TValue>(c, Children.Comparer);
                Children.Add(c, node);
                return node;
            }

            return null;
        }
    }
}
