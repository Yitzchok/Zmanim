using System;
using java.util;
using Zmanim.TimeZone;

namespace Zmanim.Java
{
    ///<summary>
    /// Abstraction of <see cref="java.util.TimeZone"/> class.
    ///</summary>
    public class JavaTimeZone : ITimeZone
    {
        private readonly java.util.TimeZone timeZone;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaTimeZone"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public JavaTimeZone(string name)
        {
            timeZone = java.util.TimeZone.getTimeZone(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaTimeZone"/> class.
        /// </summary>
        public JavaTimeZone()
        {
            timeZone = java.util.TimeZone.getDefault();
        }

        #region ITimeZone Members

        /// <summary>
        /// UTCs the offset.
        /// If Daylight Saving Time is in effect at the specified date,
        /// the offset value is adjusted with the amount of daylight saving.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public int UtcOffset(DateTime dateTime)
        {
            int utcOffset = timeZone.getOffset(ToDate(dateTime).getTime());
            return utcOffset;
        }

        /// <summary>
        /// Ins the daylight time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public bool inDaylightTime(DateTime dateTime)
        {
            return timeZone.inDaylightTime(ToDate(dateTime));
        }

        /// <summary>
        /// Gets the ID of this time zone.
        /// </summary>
        /// <returns>the ID of this time zone.</returns>
        public string getID()
        {
            return timeZone.getID();
        }

        /// <summary>
        /// Returns a name of this time zone suitable for presentation to the user in the default locale.
        /// This method returns the long name, not including daylight savings.
        /// If the display name is not available for the locale, then this method returns a string in the normalized custom ID format.
        /// </summary>
        /// <returns></returns>
        public string getDisplayName()
        {
            return timeZone.getDisplayName();
        }

        /// <summary>
        /// Returns the offset of this time zone from UTC at the specified date.
        /// If Daylight Saving Time is in effect at the specified date,
        /// the offset value is adjusted with the amount of daylight saving.
        /// </summary>
        /// <param name="timeFromEpoch">the date represented in milliseconds since January 1, 1970 00:00:00 GMT</param>
        /// <returns>
        /// the amount of time in milliseconds to add to UTC to get local time.
        /// </returns>
        public int getOffset(long timeFromEpoch)
        {
            int offset = timeZone.getOffset(timeFromEpoch);
            return offset;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="javaDate">The java date.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(Date javaDate)
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
        public static Date ToDate(DateTime dateTime)
        {
            return new GregorianCalendar(
                dateTime.Year, dateTime.Month - 1, dateTime.Day,
                dateTime.Hour, dateTime.Minute, dateTime.Second).getTime();
        }
    }
}