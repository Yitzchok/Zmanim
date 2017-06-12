using System;

namespace Zmanim.Calculator
{

    /// <summary>
    /// A interface that defines the sservices needed to calculate sunrise and sunset.
    /// </summary>
    public interface IAstronomicalCalculator
    {
        ///<summary>
        /// A descriptive name of the algorithm.
        ///</summary>
        string CalculatorName { get; }

        /// <summary>
        /// A method that calculates UTC sunrise as well as any time based on an
        /// angle above or below sunrise. This abstract method is implemented by the
        /// classes that extend this class.
        /// </summary>
        /// <param name="astronomicalCalendar">Used to calculate day of year.</param>
        /// <param name="zenith">the azimuth below the vertical zenith of 90 degrees. for
        /// sunrise typically the <see cref="AstronomicalCalculator.AdjustZenith">zenith</see> used for
        /// the calculation uses geometric zenith of 90°; and
        /// <see cref="AstronomicalCalculator.AdjustZenith">adjusts</see> this slightly to account for
        /// solar refraction and the sun's radius. Another example would
        /// be <see cref="AstronomicalCalendar.GetBeginNauticalTwilight"/>
        /// that passes <see cref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to
        /// this method.</param>
        /// <param name="adjustForElevation">if set to <c>true</c> [adjust for elevation].</param>
        /// <returns>
        /// The UTC time of sunrise in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <see cref="Double.NaN"/> will be returned.
        /// </returns>
        double GetUtcSunrise(IDateWithLocation dateWithLocation, double zenith, bool adjustForElevation);

        /// <summary>
        /// A method that calculates UTC sunset as well as any time based on an angle
        /// above or below sunset. This abstract method is implemented by the classes
        /// that extend this class.
        /// </summary>
        /// <param name="astronomicalCalendar">Used to calculate day of year.</param>
        /// <param name="zenith">the azimuth below the vertical zenith of 90°;. For sunset
        /// typically the <see cref="AstronomicalCalculator.AdjustZenith">zenith</see> used for the
        /// calculation uses geometric zenith of 90°; and
        /// <see cref="AstronomicalCalculator.AdjustZenith">adjusts</see> this slightly to account for
        /// solar refraction and the sun's radius. Another example would
        /// be <see cref="AstronomicalCalendar.GetEndNauticalTwilight"/> that
        /// passes <see cref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to this
        /// method.</param>
        /// <param name="adjustForElevation">if set to <c>true</c> [adjust for elevation].</param>
        /// <returns>
        /// The UTC time of sunset in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <seealso cref="Double.NaN"/> will be returned.
        /// </returns>
        double GetUtcSunset(IDateWithLocation dateWithLocation, double zenith, bool adjustForElevation);
    }
}