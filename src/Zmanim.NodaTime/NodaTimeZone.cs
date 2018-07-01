using System;
using NodaTime;
using Zmanim.Extensions;
using Zmanim.TimeZone;

namespace Zmanim.TzDatebase
{
    /// <summary>
    /// A ITimeZone implementation of the NodaTime TimeZone
    /// </summary>
    public class NodaTimeZone : ITimeZone
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodaTimeZone"/> class.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        public NodaTimeZone(DateTimeZone timeZone) => TimeZone = timeZone;

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public DateTimeZone TimeZone { get; }

        /// <summary>
        /// UTCs the offset.
        /// If Daylight Saving Time is in effect at the specified date,
        /// the offset value is adjusted with the amount of daylight saving.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public int UtcOffset(DateTime dateTime) => GetDateTime(dateTime).Offset.Milliseconds;

        /// <summary>
        /// Ins the daylight time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public bool IsDaylightSavingTime(DateTime dateTime) => GetDateTime(dateTime).IsDaylightSavingTime();

        private ZonedDateTime GetDateTime(DateTime dateTime) => TimeZone.AtLeniently(LocalDateTime.FromDateTime(dateTime));

        /// <summary>
        /// Gets the ID of this time zone.
        /// </summary>
        /// <returns>the ID of this time zone.</returns>
        public string GetId() => GetDisplayName();

        /// <summary>
        /// Returns a name of this time zone suitable for presentation to the user in the default locale.
        /// This method returns the long name, not including daylight savings.
        /// If the display name is not available for the locale, then this method returns a string in the normalized custom ID format.
        /// </summary>
        /// <returns></returns>
        public string GetDisplayName() => TimeZone.Id;

        /// <summary>
        /// Returns the offset of this time zone from UTC at the specified date.
        /// If Daylight Saving Time is in effect at the specified date,
        /// the offset value is adjusted with the amount of daylight saving.
        /// </summary>
        /// <param name="timeFromEpoch">the date represented in milliseconds since January 1, 1970 00:00:00 GMT</param>
        /// <returns>
        /// the amount of time in milliseconds to add to UTC to get local time.
        /// </returns>
        public int GetOffset(long timeFromEpoch) => UtcOffset(timeFromEpoch.ToDateTime());
    }
}