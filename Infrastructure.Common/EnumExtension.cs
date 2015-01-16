using System;

namespace Infrastructure.Common
{    
    public static class EnumExtension
    {
        public static string GetDescription(this Enum e)
        {
            if (e == null)
                return string.Empty;

            var description = e.ToString();
            var fieldInfo = e.GetType().GetField(description);
            var attributes =
                (EnumDisplay[])fieldInfo.GetCustomAttributes(typeof(EnumDisplay), false);

            if (attributes != null && attributes.Length > 0)
            {
                if (!string.IsNullOrEmpty(attributes[0].Description))
                    return attributes[0].Description;
            }
            return description;
        }

        public static int GetSortOrder(this Enum e)
        {
            if (e == null) return int.MaxValue;

            var description = e.ToString();
            var fieldInfo = e.GetType().GetField(description);
            var attributes = (EnumDisplay[])fieldInfo.GetCustomAttributes(typeof(EnumDisplay), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].SortOrder;
            }
            return int.MaxValue;
        }
    }    
}
