using System;
using Zmanim.TimeZone;

namespace Zmanim
{
    /// <summary>
    /// A simple implementation of ITimeZoneDateTime.
    /// </summary>
    public class TimeZoneDateTime : ITimeZoneDateTime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZoneDateTime"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        public TimeZoneDateTime(DateTime date)
            : this(date, new OlsonTimeZone())
        {
            Date = date;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZoneDateTime"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="timeZone">The time zone.</param>
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