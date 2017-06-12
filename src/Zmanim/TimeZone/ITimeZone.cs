using System;

namespace Zmanim.TimeZone
{
    ///<summary>
    /// Provides the most basic useage of a TimeZone.
    ///</summary>
    public interface ITimeZone
    {
        /// <summary>
        /// UTCs the offset.
        /// If Daylight Saving Time is in effect at the specified date,
        /// the offset value is adjusted with the amount of daylight saving.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        int UtcOffset(DateTime dateTime);

        /// <summary>
        /// Is the current DateTime in daylight time for this time zone.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        bool IsDaylightSavingTime(DateTime dateTime);

        /// <summary>
        /// Gets the ID of this time zone.
        /// </summary>
        /// <returns>the ID of this time zone.</returns>
        string GetId();

        /// <summary>
        /// Returns a name of this time zone suitable for presentation to the user in the default locale. 
        /// This method returns the long name, not including daylight savings.
        /// If the display name is not available for the locale, then this method returns a string in the normalized custom ID format.
        /// </summary>
        /// <returns></returns>
        string GetDisplayName();

        ///<summary>
        /// Returns the offset of this time zone from UTC at the specified date.
        /// If Daylight Saving Time is in effect at the specified date,
        /// the offset value is adjusted with the amount of daylight saving.
        ///</summary>
        ///<param name="timeFromEpoch">the date represented in milliseconds since January 1, 1970 00:00:00 GMT</param>
        ///<returns>the amount of time in milliseconds to add to UTC to get local time.</returns>
        int GetOffset(long timeFromEpoch);
    }
}