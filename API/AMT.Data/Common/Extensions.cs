using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AMT.Data.Common
{
    public static class Extensions
    {
        public static string DisplayValue(this string s, string format)
        {
            string result = null;

            if (null != s)
            {
                result = string.Format(format, Math.Round(Convert.ToDouble(s), 4));
            }

            return result;
        }

        public static string Truncate(this string s, int maxLength)
        {
            string result = null;

            if (null != s)
            {
                result = (s.Length > maxLength) ? s.Substring(0, maxLength) : s;
            }

            return result;
        }

        public static string Display(this decimal d, ushort decimalPlaces = 3)
        {
            return Display((decimal?)d, decimalPlaces);
        }

        public static string Display(this decimal? d, ushort decimalPlaces = 3)
        {
            if (d == null)
            {
                return null;
            }
            var format = string.Concat("{0:#.", new string('0', decimalPlaces), '}');
            var value = d.Value.RoundTo(decimalPlaces);
            return string.Format(format, value);
        }

        public static string Display(this int i)
        {
            return string.Format("{0}", i);
        }

        public static string Display(this int? i)
        {
            if (i == null)
            {
                return null;
            }

            return string.Format("{0}", i);
        }

        public static bool IsBetween(this DateTime date, DateTime start, DateTime end)
        {
            return date >= start && date <= end;
        }

        public static bool IsIn<T>(this T item, Lazy<HashSet<T>> list)
        {
            return item.IsIn(list.Value);
        }

        public static bool IsIn<T>(this T item, IEnumerable<Lazy<T>> list)
        {
            return item.IsIn(list.Select(x => x.Value));
        }

        public static bool IsIn<T>(this T item, params Lazy<T>[] list)
        {
            return IsIn(item, (IEnumerable<Lazy<T>>)list);
        }

        public static bool IsIn<T>(this T item, IEnumerable<T> list)
        {
            return !ReferenceEquals(item, null) && list.Contains(item);
        }

        public static bool IsIn<T>(this T item, params T[] list)
        {
            return IsIn(item, (IEnumerable<T>)list);
        }

        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> enumerable, Func<T, object> propertyFunc)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }
            return enumerable.Distinct(FuncEqualityComparer.Create<T>((x, y) => Equals(propertyFunc(x), propertyFunc(y))));
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> valueFunc)
        {
            var cdict = dict as ConcurrentDictionary<TKey, TValue>;
            if (cdict != null)
            {
                return cdict.GetOrAdd(key, k => valueFunc());
            }

            TValue resultValue;
            if (!dict.TryGetValue(key, out resultValue))
            {
                dict[key] = resultValue = valueFunc();
            }

            return resultValue;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFunc)
        {
            var cdict = dict as ConcurrentDictionary<TKey, TValue>;
            if (cdict != null)
            {
                return cdict.GetOrAdd(key, valueFunc);
            }

            TValue resultValue;
            if (!dict.TryGetValue(key, out resultValue))
            {
                dict[key] = resultValue = valueFunc(key);
            }

            return resultValue;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            var cdict = dict as ConcurrentDictionary<TKey, TValue>;
            if (cdict != null)
            {
                return cdict.GetOrAdd(key, value);
            }

            return dict.GetOrAdd(key, () => value);
        }

        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable == null || !enumerable.Any(predicate);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.None(e => true);
        }

        public static int LastIndexOf<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
            {
                return -1;
            }

            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public static IEnumerable<T> Pipe<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, params T[] items)
        {
            return Enumerable.Concat(enumerable, items);
        }

        public static decimal RoundTo(this decimal value, ushort precision)
        {
            return Math.Round(value, precision, MidpointRounding.AwayFromZero);
        }

        public static decimal? RoundTo(this decimal? value, ushort precision)
        {
            if (value == null)
                return null;

            return value.Value.RoundTo(precision);
        }

        public static double RoundTo(this double value, ushort precision)
        {
            return Math.Round(value, precision, MidpointRounding.AwayFromZero);
        }

        public static decimal? DivideBySafeAndRound(this decimal? value, decimal? divisor)
        {
            return (value == null) ? null : value.Value.DivideBySafeAndRound(divisor);
        }

        public static decimal? DivideBySafeAndRound(this decimal value, decimal? divisor)
        {
            if (divisor == null || divisor == 0)
            {
                return null;
            }
            return (value / divisor.Value).RoundTo(3);
        }

        public static decimal? DivideBySafeAndRound(this int value, decimal? divisor)
        {
            return DivideBySafeAndRound((decimal)value, divisor);
        }

        public static decimal? DivideBySafeAndRound(this int? value, decimal? divisor)
        {
            return (value == null) ? null : DivideBySafeAndRound((decimal)value.Value, divisor);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.GetValueOrDefault(key, default(TValue));
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            /* added null check which was not in the core.This is to prevent the 
            System.NullReferenceException:Object reference not set to an instance of an object
            in the Spec Flow tests 
            Added null check for key*/

            if (dict != null && key != null)
            {
                TValue result;
                if (!dict.TryGetValue(key, out result))
                {
                    result = defaultValue;
                }

                return result;
            }

            return default(TValue);
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> defaultValue)
        {
            TValue result;
            if (!dict.TryGetValue(key, out result))
            {
                result = defaultValue(key);
            }
            return result;
        }

        public static TValue GetValueCoalesce<TKey, TValue>(this IDictionary<TKey, TValue> dict, params TKey[] keys)
            where TValue : class
        {
            return keys
                .Select(dict.GetValueOrDefault)
                .FirstOrDefault(value => value != null);
        }

        public static TConverted GetValueConverted<TKey, TValue, TConverted>(this IDictionary<TKey, TValue> dict, Func<TValue, TConverted> convert, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                TValue value;
                if (dict.TryGetValue(key, out value))
                {
                    return convert(value);
                }
            }
            throw new KeyNotFoundException("All keys not found: " + string.Join(",", keys));
        }

        public static SortedList<TKey, TValue> ToSortedList<T, TKey, TValue>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, IComparer<TKey> comparer = null)
        {
            if (comparer == null)
            {
                comparer = Comparer<TKey>.Default;
            }
            var result = new SortedList<TKey, TValue>(comparer);
            foreach (var item in enumerable)
            {
                result.Add(keySelector(item), valueSelector(item));
            }
            return result;
        }

        public static SortedDictionary<TKey, TValue> ToSortedDictionary<T, TKey, TValue>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, IComparer<TKey> comparer = null)
        {
            if (comparer == null)
            {
                comparer = Comparer<TKey>.Default;
            }
            var result = new SortedDictionary<TKey, TValue>(comparer);
            foreach (var item in enumerable)
            {
                if (!result.ContainsKey(keySelector(item)))
                {
                    result.Add(keySelector(item), valueSelector(item));
                }

            }
            return result;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                return new HashSet<T>(enumerable);
            }
            return new HashSet<T>(enumerable, comparer);
        }

        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return;
            }

            foreach (var item in enumerable)
            {
                set.Add(item);
            }
        }

        public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }

        public static string CommaSeparatedList<T>(this IEnumerable<T> enumerable)
        {
            return string.Join<string>(",", enumerable.Select(x => x.ToString()));
        }

        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static int Increment<TKey>(this IDictionary<TKey, int> dict, TKey key)
        {
            var value = dict.GetValueOrDefault(key, 0);
            value++;
            dict[key] = value;
            return value;
        }

        public static decimal Increment<TKey>(this IDictionary<TKey, decimal> dict, TKey key, decimal amount)
        {
            var value = dict.GetValueOrDefault(key, 0);
            value += amount;
            dict[key] = value;
            return value;
        }

        public static decimal Decrement<TKey>(this IDictionary<TKey, decimal> dict, TKey key, decimal amount)
        {
            var value = dict.GetValueOrDefault(key, 0);
            value -= amount;
            dict[key] = value;
            return value;
        }

        public static decimal? ToNullableDecimal(this string s)
        {
            decimal dec;
            if (decimal.TryParse(s, out dec)) return dec;
            return null;
        }

        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public static string ToYesNo(this bool b)
        {
            return (b) ? "Yes" : "No";
        }

        public static string ToYesNoNA(this bool? b)
        {
            return (b == null) ? "N/A" : b.Value.ToYesNo();
        }

        /// <summary>
        ///     This method decodes the html sql example 'amp' to '&'  
        /// </summary>
        public static string Decode(this string s)
        {
            string result = s.Replace("&amp;", "&");
            return result;
        }
    }
}