using System;
using Zmanim.TimeZone;

namespace Zmanim
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
}