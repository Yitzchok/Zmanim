using System;
using Zmanim.TimeZone;

namespace net.sourceforge.zmanim
{
    public class TimeZoneDateTime : ITimeZoneDateTime
    {
        public TimeZoneDateTime(DateTime date)
            : this(date, new OlsonTimeZone())
        {
            Date = date;
        }

        public TimeZoneDateTime(DateTime date, ITimeZone timeZone)
        {
            Date = date;
            TimeZone = timeZone;
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public ITimeZone TimeZone { get; set; }

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
    }
}