// * This file is part of Zmanim .NET API.
// *
// * Zmanim .NET API is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * Zmanim .NET API is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License
// * along with Zmanim.NET API.  If not, see <http://www.gnu.org/licenses/lgpl.html>.

using System;

namespace Zmanim.Extensions
{
    /// <summary>
    /// Date extensions.
    /// </summary>
    public static class DateExtensions
    {
        public static DateTime ToDateTime(this long fromEpoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(fromEpoch);
        }

        public static long ToMillisecondsFromEpoch(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

#if ikvm
        /// <summary>
        /// Converts a Date to DateTime with milliseconds.
        /// </summary>
        /// <param name="javaDate">The java date.</param>
        /// <returns></returns>
        public static DateTime ToDateTimeWithMilliseconds(this Date javaDate)
        {
            var calender = new java.util.GregorianCalendar();
            calender.setTime(javaDate);

            return new DateTime(calender.get(java.util.Calendar.YEAR),
                                calender.get(java.util.Calendar.MONTH) + 1,
                                calender.get(java.util.Calendar.DAY_OF_MONTH),
                                calender.get(java.util.Calendar.HOUR_OF_DAY),
                                calender.get(java.util.Calendar.MINUTE),
                                calender.get(java.util.Calendar.SECOND),
                                calender.get(java.util.Calendar.MILLISECOND),
                                DateTimeKind.Local
                );
        }

        /// <summary>
        /// Converts a Date to DateTime. (no milliseconds)
        /// </summary>
        /// <param name="javaDate">The java date.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Date javaDate)
        {
            var calender = new GregorianCalendar();
            calender.setTime(javaDate);

            return new DateTime(calender.get(Calendar.YEAR),
                                calender.get(Calendar.MONTH) + 1,
                                calender.get(Calendar.DAY_OF_MONTH),
                                calender.get(Calendar.HOUR_OF_DAY),
                                calender.get(Calendar.MINUTE),
                                calender.get(Calendar.SECOND),
                                DateTimeKind.Local
                );
        }

        /// <summary>
        /// Converts a Date to DateTime. (no milliseconds)
        /// </summary>
        /// <param name="dateTime">The dateTime.</param>
        /// <returns></returns>
        public static Date ToDate(this DateTime dateTime)
        {
            return new GregorianCalendar(
                dateTime.Year, dateTime.Month - 1, dateTime.Day,
                dateTime.Hour, dateTime.Minute, dateTime.Second).getTime();

        }
#endif
    }
}