using System;
using System.Globalization;

namespace Frost.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime");

            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }

        public static string ToIso8601(this DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime");
            return dateTime.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz", CultureInfo.InvariantCulture);
        }
    }
}