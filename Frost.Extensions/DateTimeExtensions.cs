using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

        public static string ToYearString(this DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime");

            return dateTime.ToString("yyyy");
        }

        public static string ToMonthString(this DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime");

            return dateTime.ToString("MM");
        }

        public static string ToDayString(this DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime");

            return dateTime.ToString("dd");
        }

        public static string ToSqlServerFormat(this DateTime dateTime)
        {
            if (dateTime == null)
                throw new ArgumentNullException("dateTime");

            return dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string ToOracleSqlDate(this DateTime dateTime)
        {
            return String.Format("to_date('{0}','dd.mm.yyyy hh24.mi.ss')", dateTime.ToString("dd.MM.yyyy HH:mm:ss"));
        }

        public static bool IsWeekend(this DateTime value)
        {
            return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
        }

        public static int Age(this DateTime dateOfBirth)
        {
            if (DateTime.Today.Month < dateOfBirth.Month ||
            DateTime.Today.Month == dateOfBirth.Month &&
             DateTime.Today.Day < dateOfBirth.Day)
            {
                return DateTime.Today.Year - dateOfBirth.Year - 1;
            }
            else
                return DateTime.Today.Year - dateOfBirth.Year;
        }

        public static IEnumerable<DateTime> GetDateRangeTo(this DateTime self, DateTime toDate)
        {
            var range = Enumerable.Range(0, new TimeSpan(toDate.Ticks - self.Ticks).Days);

            return from p in range
                   select self.Date.AddDays(p);
        }

        public static Int64 DateDiff(this DateTime startDate, String datePart, DateTime endDate)
        {
            Int64 dateDiffVal = 0;
            Calendar cal = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
            TimeSpan ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (datePart.ToLower().Trim())
            {
                #region year
                case "year":
                case "yy":
                case "yyyy":
                    dateDiffVal = (Int64)(cal.GetYear(endDate) - cal.GetYear(startDate));
                    break;
                #endregion

                #region quarter
                case "quarter":
                case "qq":
                case "q":
                    dateDiffVal = (Int64)((((cal.GetYear(endDate)
                                        - cal.GetYear(startDate)) * 4)
                                        + ((cal.GetMonth(endDate) - 1) / 3))
                                        - ((cal.GetMonth(startDate) - 1) / 3));
                    break;
                #endregion

                #region month
                case "month":
                case "mm":
                case "m":
                    dateDiffVal = (Int64)(((cal.GetYear(endDate)
                                        - cal.GetYear(startDate)) * 12
                                        + cal.GetMonth(endDate))
                                        - cal.GetMonth(startDate));
                    break;
                #endregion

                #region day
                case "day":
                case "d":
                case "dd":
                    dateDiffVal = (Int64)ts.TotalDays;
                    break;
                #endregion

                #region week
                case "week":
                case "wk":
                case "ww":
                    dateDiffVal = (Int64)(ts.TotalDays / 7);
                    break;
                #endregion

                #region hour
                case "hour":
                case "hh":
                    dateDiffVal = (Int64)ts.TotalHours;
                    break;
                #endregion

                #region minute
                case "minute":
                case "mi":
                case "n":
                    dateDiffVal = (Int64)ts.TotalMinutes;
                    break;
                #endregion

                #region second
                case "second":
                case "ss":
                case "s":
                    dateDiffVal = (Int64)ts.TotalSeconds;
                    break;
                #endregion

                #region millisecond
                case "millisecond":
                case "ms":
                    dateDiffVal = (Int64)ts.TotalMilliseconds;
                    break;
                #endregion

                default:
                    throw new Exception(String.Format("DatePart \"{0}\" is unknown", datePart));
            }
            return dateDiffVal;
        }

        public static bool IsWeekday(this DayOfWeek d)
        {
            switch (d)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Saturday: return false;

                default: return true;
            }
        }

        public static DateTime AddWorkdays(this DateTime d, int days)
        {
            // start from a weekday
            while (d.DayOfWeek.IsWeekday()) d = d.AddDays(1.0);
            for (int i = 0; i < days; ++i)
            {
                d = d.AddDays(1.0);
                while (d.DayOfWeek.IsWeekday()) d = d.AddDays(1.0);
            }
            return d;
        }

        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        public static long ToUnixTimestamp(this DateTime date)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan unixTimeSpan = date - unixEpoch;

            return (long)unixTimeSpan.TotalSeconds;
        }

        public static string ToRFC822DateString(this DateTime date)
        {
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            string timeZone = "+" + offset.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            if (offset < 0)
            {
                int i = offset * -1;
                timeZone = "-" + i.ToString().PadLeft(2, '0');
            }
            return date.ToString("ddd, dd MMM yyyy HH:mm:ss " + timeZone.PadRight(5, '0'), CultureInfo.GetCultureInfo("en-US"));
        }

        public static DateTime EndOfTheMonth(this DateTime date)
        {
            var endOfTheMonth = new DateTime(date.Year, date.Month, 1)
                .AddMonths(1)
                .AddDays(-1);

            return endOfTheMonth;
        }

        public static TimeSpan TimeElapsed(this DateTime date)
        {
            return DateTime.Now - date;
        }
        public static Double ElapsedSeconds(this DateTime input)
        {
            return DateTime.Now.Subtract(input).TotalSeconds;
        }

    }
}