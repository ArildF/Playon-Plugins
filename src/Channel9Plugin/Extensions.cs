using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

        public static string ValueOrNull(this XAttribute attribute)
        {
            return attribute != null ? attribute.Value : null;
        }

        public static string ValueOrEmpty(this XAttribute attribute)
        {
            return attribute != null ? attribute.Value : String.Empty;
        }

        public static string ValueOrNull(this XElement element)
        {
            return element != null ? element.Value : null;
        }

        public static string ValueOrEmpty(this XElement element)
        {
            return element != null ? element.Value : String.Empty;
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
