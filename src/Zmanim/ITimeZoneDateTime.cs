using System;
using Zmanim.TimeZone;
using TimeZone = java.util.TimeZone;

namespace net.sourceforge.zmanim
{
    /// <summary>
    /// A DateTime that works with the TimeZone.
    /// </summary>
    public interface ITimeZoneDateTime : ICloneable
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        ITimeZone TimeZone { get; set; }
    }

    public class TimeZoneDateTime : ITimeZoneDateTime
    {
        public TimeZoneDateTime(DateTime date)
            : this(date,new JavaTimeZone())
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