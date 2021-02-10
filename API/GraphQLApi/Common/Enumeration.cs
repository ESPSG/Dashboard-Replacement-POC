using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GraphQLApi.Common
{
    [Serializable]
    [DebuggerDisplay("{DisplayName,nq} - {Value}")]
    public abstract class Enumeration<TEnumeration> : Enumeration<TEnumeration, int>
        where TEnumeration : Enumeration<TEnumeration>
    {
        protected Enumeration(int value, string displayName)
            : base(value, displayName) {}

        public static TEnumeration FromInt32(int? value)
        {
            return value == null ? null : FromValue(value.Value);
        }

        public static bool TryFromInt32(int listItemValue, out TEnumeration result)
        {
            return TryParse(listItemValue, out result);
        }
    }

    [Serializable]
    [DebuggerDisplay("{DisplayName,nq} - {Value}")]
    public abstract class Enumeration<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>
        where TEnumeration : Enumeration<TEnumeration, TValue>
        where TValue : IComparable
    {
        private static readonly Lazy<TEnumeration[]> Enumerations = new Lazy<TEnumeration[]>(GetEnumerations);
        private readonly string _displayName;
        private TValue _value;

        protected Enumeration(TValue value, string displayName)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException();
            }

            _value = value;
            _displayName = displayName;
        }

        public TValue Value
        {
            get { return _value; }
        }

        public void SetValue(TValue value)
        {
            _value = value;
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other == default(TEnumeration) ? default(TValue) : other.Value);
        }

        public bool Equals(TEnumeration other)
        {
            return other != null && ValueEquals(other.Value);
        }

        public override sealed string ToString()
        {
            return DisplayName;
        }

        public static TEnumeration[] GetAll()
        {
            return Enumerations.Value;
        }

        private static TEnumeration[] GetEnumerations()
        {
            var enumerationType = typeof (TEnumeration);
            var enumerations = enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<TEnumeration>()
                .ToArray();
            CheckForDuplicates(enumerations);
            return enumerations;
        }

        private static void CheckForDuplicates(TEnumeration[] enumerations)
        {
            var enumValueDupes = enumerations.GroupBy(x => x.Value, x => x)
                .Where(x => x.Count() > 1)
                .ToArray();
            if (enumValueDupes.Any())
            {
                var message = enumValueDupes.Aggregate(
                    new StringBuilder("For enum " + typeof (TEnumeration).Name),
                    (sb, grp) => sb.AppendFormat(" Value {0} is duplicated on {1};", grp.Key, string.Join(", ", grp.Select(x => x.DisplayName))));
                message.Replace(';', '.', message.Length - 1, 1);
                throw new FatalException(message.ToString());
            }

            var enumDisplayNameDupes = enumerations.GroupBy(x => x.DisplayName, x => x)
                .Where(x => x.Count() > 1)
                .ToArray();
            if (enumDisplayNameDupes.Any())
            {
                var message = enumDisplayNameDupes.Aggregate(
                    new StringBuilder("For enum " + typeof (TEnumeration).Name),
                    (sb, grp) => sb.AppendFormat(" DisplayName {0} is duplicated for values {1};", grp.Key, string.Join(", ", grp.Select(x => x.Value))));
                message.Replace(';', '.', message.Length - 1, 1);
                throw new FatalException(message.ToString());
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
        {
            return !Equals(left, right);
        }

        public static TEnumeration FromValue(TValue value)
        {
            return Parse(value, "value", item => item.Value.Equals(value));
        }

        public static TEnumeration Parse(string displayName, StringComparison stringComparison = StringComparison.Ordinal)
        {
            return Parse(displayName, "display name", item => string.Equals(item.DisplayName, displayName, stringComparison));
        }

        private static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            result = GetAll()
                .FirstOrDefault(predicate);
            return result != null;
        }

        private static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate)
        {
            TEnumeration result;

            if (!TryParse(predicate, out result))
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof (TEnumeration));
                throw new ArgumentException(message, "value");
            }

            return result;
        }

        public static bool TryParse(TValue value, out TEnumeration result)
        {
            return TryParse(e => e.ValueEquals(value), out result);
        }

        public static bool TryParse(string displayName, out TEnumeration result)
        {
            return TryParse(e => e.DisplayName == displayName, out result);
        }

        protected virtual bool ValueEquals(TValue value)
        {
            return Value.Equals(value);
        }
    }
}