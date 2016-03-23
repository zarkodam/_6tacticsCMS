using System;
using System.Collections.Generic;
using System.Linq;
//using _6tactics.Utilities.Interfaces;

namespace _6tactics.Utilities.Common.ExtensionMehods
{
    //public interface IItem <T>
    //{
    //    T Item { get; set; }
    //}

    public interface IRecursion<out T>
    {
        int Depth { get; }
        T Item { get; }
    }

    public static class CustomExtensions
    {
        private class Recursion<T> : IRecursion<T>
        {
            private readonly int _depth;
            private readonly T _item;

            public int Depth { get { return _depth; } }
            public T Item { get { return _item; } }
            public Recursion(int depth, T item)
            {
                _depth = depth;
                _item = item;
            }
        }

        private static IEnumerable<IRecursion<T>> Hierarchical<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> selector, Func<IRecursion<T>, bool> predicate, int depth)
        {
            if (source == null) yield break;

            var q = source.Select(item => new Recursion<T>(depth, item)).Cast<IRecursion<T>>();

            if (predicate != null) q = q.Where(predicate);

            foreach (var item in q)
            {
                yield return item;
                foreach (var childItem in Hierarchical(selector(item.Item), selector, predicate, depth + 1))
                    yield return childItem;
            }
        }

        public static IEnumerable<IRecursion<T>> Hierarchical<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            return Hierarchical(source, selector, null);
        }

        public static IEnumerable<IRecursion<T>> Hierarchical<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector, Func<IRecursion<T>, bool> predicate)
        {
            return Hierarchical(source, selector, predicate, 0);
        }


    }

}
