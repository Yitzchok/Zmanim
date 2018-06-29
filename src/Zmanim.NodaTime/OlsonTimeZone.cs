using NodaTime;

namespace Zmanim.TzDatebase
{
    /// <summary>
    /// A ITimeZone implementation of the Olson TimeZone DataBase
    /// </summary>
    public class OlsonTimeZone : NodaTimeZone
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OlsonTimeZone"/> class.
        /// </summary>
        /// <param name="timeZoneName">Name of the time zone.</param>
        public OlsonTimeZone(string timeZoneName)
            : base(DateTimeZoneProviders.Tzdb[timeZoneName]) { }

    }
}