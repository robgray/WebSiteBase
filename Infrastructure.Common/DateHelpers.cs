using System;

namespace Infrastructure.Common
{
    public static class DateHelpers
    {
        public static string ToIsoDate(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        public static string ToIsoDateTime(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH':'mm':'ss");
        }

        public static bool IsBetween(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            return dateTime >= startDate && dateTime <= endDate;
        }
    }
}
