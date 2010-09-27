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

        public static ElseClause<T, TRet> IfNotNull<T, TRet>(this T obj, Func<T, TRet> func) where T : class
        {
            return new ElseClause<T, TRet>(obj, func);
        }

        public class ElseClause<T, TRet> where T : class
        {
            private readonly T _obj;
            private readonly Func<T, TRet> _func;

            public ElseClause(T obj, Func<T, TRet> func)
            {
                _obj = obj;
                _func = func;
            }

            public TRet Else(TRet elseValue)
            {
                if (_obj != null)
                {
                    return _func(_obj);
                }
                return elseValue;
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

        public static long ValueOrZero(this XAttribute attribute)
        {
            long val;
            return attribute != null && long.TryParse(attribute.Value, out val) ? val : 0;
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
