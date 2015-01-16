using System;

namespace Infrastructure.Common
{
    public static class DateTimeHelper
    {
        private static Func<DateTime> _currentUtcNow = () => DateTime.UtcNow;
        private static Func<DateTime> _currentNow = () => DateTime.Now;
        private static Func<DateTime> _currentToday = () => DateTime.Today; 

        public static DateTime UtcNow
        {
            get { return _currentUtcNow(); }
        }

        public static DateTimeOffset OffsetUtcNow
        {
            get { return new DateTimeOffset(_currentUtcNow()); }
        }

        public static DateTime Now
        {
            get { return _currentNow(); }
        }

        public static DateTime Today
        {
            get { return _currentToday();  }
        }

        /// <summary>
        /// An object that is set to today's date in the UTC zone, with the time component set to 00:00:00.
        /// </summary>
        public static DateTime UtcToday
        {
            get
            {
                return new DateTime(_currentUtcNow().Year, _currentUtcNow().Month, _currentUtcNow().Day, 0, 0, 0, 0, DateTimeKind.Utc);
            }
        }

        public static void SetCurrentNow(DateTime dt)
        {
            _currentNow = () => dt;
        }

        public static void SetCurrentToday(DateTime dt)
        {
            _currentToday = () => dt;
        }

        public static void SetCurrentUtcNow(DateTime dt)
        {
            _currentUtcNow = () => dt;
        }

        public static void SetCurrentUtcNow(DateTimeOffset dt)
        {
            _currentUtcNow = () => dt.UtcDateTime;
        }

	    public static void ResetToRealTime()
	    {
		    _currentUtcNow = () => DateTime.UtcNow;
		    _currentNow = () => DateTime.Now;
		    _currentToday = () => DateTime.Today;
	    }
    }
}
