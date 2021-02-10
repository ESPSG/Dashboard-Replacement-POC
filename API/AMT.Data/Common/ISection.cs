using System;
using System.Collections.Generic;
using System.Linq;

namespace AMT.Data.Common
{
    public interface ISection
    {
        string LocalCourseCode { get; }
        short SchoolYear { get; }
        string SchoolKey { get; }
        string SectionIdentifier { get; }
        string SessionName { get; }
    }

    /// <summary>
    ///     Helper class to allow matching two different section types; useful for when streaming in Sections and something
    ///     like TeacherSectionAssociation then ensuring a match between the two.
    /// </summary>
    public class SectionDictionary<T>
        where T : class, ISection
    {
        private readonly IDictionary<int, List<T>> _dict;

        public SectionDictionary()
        {
            _dict = new SortedDictionary<int, List<T>>();
        }

        public void AddSection(T section)
        {
            var sectionList = _dict.GetOrAdd(SectionComparer.Instance.GetHashCode(section), () => new List<T>());
            sectionList.Add(section);
        }

        public T GetMatching(ISection needle)
        {
            var sectionList = _dict.GetValueOrDefault(SectionComparer.Instance.GetHashCode(needle));
            if (sectionList == null)
            {
                return null;
            }
            return sectionList.FirstOrDefault(x => SectionComparer.Instance.Equals(x, needle));
        }
    }

    public class SectionComparer : IComparer<ISection>, IEqualityComparer<ISection>
    {
        public static readonly SectionComparer Instance = new SectionComparer();

        private SectionComparer() { }

        private static int ChainCompare(ISection x, ISection y, params Func<ISection, IComparable>[] properties)
        {
            foreach (var prop in properties)
            {
                var xProp = prop(x);
                var yProp = prop(y);

                if (ReferenceEquals(xProp, null))
                {
                    if (ReferenceEquals(yProp, null))
                    {
                        return 0;
                    }
                    return -1;
                }

                var compareProp = (xProp is string && yProp is string)
                    ? StringComparer.OrdinalIgnoreCase.Compare((string)xProp, (string)yProp)
                    : xProp.CompareTo(yProp);
                if (compareProp != 0)
                {
                    return compareProp;
                }
            }
            return 0;
        }

        public int Compare(ISection x, ISection y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }
            if (ReferenceEquals(x, null))
            {
                return -1;
            }
            if (ReferenceEquals(y, null))
            {
                return 1;
            }
            return ChainCompare(
                x,
                y,
                section => section.SchoolKey,
                section => section.SchoolYear,
                section => section.SectionIdentifier,
                section => section.SessionName,
                section => section.LocalCourseCode);
        }

        public bool Equals(ISection x, ISection y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(ISection obj)
        {
            unchecked
            {
                var hashCode = obj.SectionIdentifier != null ? obj.SectionIdentifier.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (obj.LocalCourseCode != null ? obj.LocalCourseCode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.SessionName != null ? obj.SessionName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ obj.SchoolYear;
                hashCode = (hashCode * 397) ^ (obj.SchoolKey != null ? obj.SchoolKey.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}