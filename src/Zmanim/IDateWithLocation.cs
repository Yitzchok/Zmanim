using System;
using Zmanim.Utilities;

namespace Zmanim
{
    /// <summary>
    /// The GeoLocation and DateTime.
    /// </summary>
    public interface IDateWithLocation
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        IGeoLocation Location { get; set; }
    }
}