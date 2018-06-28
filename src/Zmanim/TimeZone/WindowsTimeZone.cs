#if !NET20
using System;
using Zmanim.Extensions;

namespace Zmanim.TimeZone
{
    /// <summary>
    /// A ITimeZone implementation of the Windows TimeZone
    /// (uses the default .net <see cref="TimeZone"/> class)
    /// </summary>
    public class WindowsTimeZone : ITimeZone
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsTimeZone"/> class.
        /// </summary>
        public WindowsTimeZone() : this(TimeZoneInfo.Local) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsTimeZone"/> class.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        public WindowsTimeZone(TimeZoneInfo timeZone)
        {
            this.TimeZone = timeZone;
        }

#if !NO_FIND_SYSTEM_TIMEZONE_BY_ID
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsTimeZone"/> class.
        /// </summary>
        /// <param name="timeZoneName">Name of the time zone.</param>
        public WindowsTimeZone(string timeZoneName)
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
        }
#endif

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// UTCs the offset.
        /// If Daylight Saving Time is in effect at the specified date,
        /// the offset value is adjusted with the amount of daylight saving.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public int UtcOffset(DateTime dateTime)
        {
            return (int)TimeZone.GetUtcOffset(dateTime).TotalMilliseconds;
        }

        /// <summary>
        /// Ins the daylight time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public bool IsDaylightSavingTime(DateTime dateTime)
        {
            return TimeZone.IsDaylightSavingTime(dateTime);
        }

        /// <summary>
        /// Gets the ID of this time zone.
        /// </summary>
        /// <returns>the ID of this time zone.</returns>
        public string GetId()
        {
            return GetDisplayName();
        }

        /// <summary>
        /// Returns a name of this time zone suitable for presentation to the user in the default locale.
        /// This method returns the long name, not including daylight savings.
        /// If the display name is not available for the locale, then this method returns a string in the normalized custom ID format.
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName()
        {
            return TimeZone.StandardName;
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
        public int GetOffset(long timeFromEpoch)
        {
            return UtcOffset(timeFromEpoch.ToDateTime());
        }
    }
}
#endif