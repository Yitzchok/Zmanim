using System;
using Zmanim.Utilities;

namespace Zmanim
{
    /// <summary>
    /// A simple implementation of ITimeZoneDateTime.
    /// </summary>
    public class DateWithLocation : IDateWithLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateWithLocation"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="location"></param>
        public DateWithLocation(DateTime date, IGeoLocation location)
        {
            Date = date;
            Location = location;
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public IGeoLocation Location { get; set; }
    }
}