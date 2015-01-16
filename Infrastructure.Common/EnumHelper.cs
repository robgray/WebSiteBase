using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Common
{
    public static class Enum<T>
    {
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static List<LookupValueItem> GetLookupValueItemList()
        {
            var list = new List<LookupValueItem>();
            var enumValues = Enum.GetValues(typeof(T));

            var sort = new List<Tuple<int, LookupValueItem>>();

            foreach (var value in enumValues)
            {
                var order = ((Enum)value).GetSortOrder();
                sort.Add(new Tuple<int, LookupValueItem>(order,
                                                          new LookupValueItem
                                                          {
                                                              Description = GetDescription((Enum)value),
                                                              Id = (int)value
                                                          }));
            }

            var sorted = sort.OrderBy(x => x.Item1);

            return sorted.Select(x => x.Item2).ToList();
        }

        public static T GetEnumFromDescription(string description)
        {
            var names = Enum.GetNames(typeof(T));
            foreach (var value in names)
            {
                if (value == description) return Parse(value);

                var desc = value.ToString();
                var fieldInfo = typeof(T).GetField(desc);
                var attributes =
                    (EnumDisplay[])fieldInfo.GetCustomAttributes(typeof(EnumDisplay), false);
                if (attributes != null && attributes.Length > 0)
                {
                    if (attributes[0].Description == description)
                    {
                        return Parse(value);
                    }
                }
            }
            throw new ArgumentOutOfRangeException("Cannot translate '" + description + "' into a value from '" +
                                                  typeof(T).Name);
        }

        public static T TryGetEnumFromDescription(string description, T valueIfNotFound)
        {
            var names = Enum.GetNames(typeof(T));
            foreach (var value in names)
            {
                if (value == description) return Parse(value);

                var desc = value.ToString();
                var fieldInfo = typeof(T).GetField(desc);
                var attributes =
                    (EnumDisplay[])fieldInfo.GetCustomAttributes(typeof(EnumDisplay), false);
                if (attributes != null && attributes.Length > 0)
                {
                    if (attributes[0].Description == description)
                    {
                        return Parse(value);
                    }
                }
            }
            return valueIfNotFound;
        }

        public static T TryGetEnumFromValue(int value, T valueIfNotFound)
        {
            var values = Enum.GetValues(typeof(T));

            foreach (var enumValue in values)
            {
                if ((int)enumValue == value)
                    return (T)enumValue;
            }
            return valueIfNotFound;
        }

        public static string GetDescription(Enum value)
        {
            var description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes =
                (EnumDisplay[])fieldInfo.GetCustomAttributes(typeof(EnumDisplay), false);

            if (attributes != null && attributes.Length > 0)
            {
                if (!string.IsNullOrEmpty(attributes[0].Description))
                    return attributes[0].Description;
            }
            return description;
        }

        public static IList<string> GetAllDescriptions()
        {
            IList<string> descs = new List<string>();
            var enumValues = Enum.GetValues(typeof(T));

            foreach (Enum value in enumValues)
            {
                descs.Add(GetDescription(value));
            }

            return descs;
        }
    }
}
