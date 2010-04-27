using System;
using TimeZone = java.util.TimeZone;

namespace net.sourceforge.zmanim
{
    public interface ICalendar : ICloneable
    {
        DateTime Date { get; set; }
        TimeZone TimeZone { get; set; }
    }

    public class DefaultCalendar : ICalendar
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public TimeZone TimeZone { get; set; }

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