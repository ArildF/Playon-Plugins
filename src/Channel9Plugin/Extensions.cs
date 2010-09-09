using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.PlayOn.Plugins.Channel9
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static IEnumerable<T> DistinctBy<T, TSelector>(
            this IEnumerable<T> list, Func<T, TSelector> selector)
        {
            return list.Distinct(new SelectorComparer<T, TSelector>(selector));
        }

        public class SelectorComparer<T, TSelector> : IEqualityComparer<T>
        {
            private readonly Func<T, TSelector> _selector;

            public SelectorComparer(Func<T, TSelector> selector)
            {
                _selector = selector;
            }

            public bool Equals(T x, T y)
            {
                if (y == null)
                {
                    return false;
                }

                return _selector(x).Equals(_selector(y));
            }

            public int GetHashCode(T obj)
            {
                return _selector(obj).GetHashCode();
            }
        }
    }
}
