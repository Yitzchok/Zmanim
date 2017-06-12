using System;
using Zmanim.Calculator;
using Zmanim.Utilities;

namespace Zmanim
{
    ///<summary>
    /// A calendar that calculates astronomical time calculations such as
    ///  <see cref = "GetSunrise">sunrise</see> and <see cref = "GetSunset">sunset</see> times.
    ///</summary>
    public interface IAstronomicalCalendar
    {
        ///<summary>
        ///  The getSunrise method Returns a <c>DateTime</c> representing the
        ///  sunrise time. The zenith used for the calculation uses
        ///  <seealso cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso> of 90°. This is adjusted
        ///  by the <seealso cref = "AstronomicalCalendar.AstronomicalCalculator" /> that adds approximately 50/60 of a
        ///  degree to account for 34 archminutes of refraction and 16 archminutes for
        ///  the sun's radius for a total of
        ///  <seealso cref = "Calculator.AstronomicalCalculator.AdjustZenith">90.83333°</seealso>. See
        ///  documentation for the specific implementation of the
        ///  <seealso cref = "AstronomicalCalendar.AstronomicalCalculator" /> that you are using.
        ///</summary>
        ///<returns> the <c>DateTime</c> representing the exact sunrise time. If
        ///  the calculation can not be computed null will be returned. </returns>
        ///<seealso cref = "Calculator.AstronomicalCalculator.AdjustZenith" />
        DateTime? GetSunrise();

        ///<summary>
        ///  The getSunset method Returns a <c>DateTime</c> representing the
        ///  sunset time. The zenith used for the calculation uses
        ///  <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see> of 90°. This is adjusted
        ///  by the <see cref = "AstronomicalCalendar.AstronomicalCalculator" /> that adds approximately 50/60 of a
        ///  degree to account for 34 archminutes of refraction and 16 archminutes for
        ///  the sun's radius for a total of
        ///  <see cref = "Calculator.AstronomicalCalculator.AdjustZenith">90.83333°</see>. See
        ///  documentation for the specific implementation of the
        ///  <see cref = "Calculator.AstronomicalCalculator" /> that you are using. Note: In certain cases
        ///  the calculates sunset will occur before sunrise. This will typically
        ///  happen when a timezone other than the local timezone is used (calculating
        ///  Los Angeles sunset using a GMT timezone for example). In this case the
        ///  sunset date will be incremented to the following date.
        ///</summary>
        ///<returns> the <c>DateTime</c> representing the exact sunset time. If
        ///  the calculation can not be computed null will be returned. If the
        ///  time calculation </returns>
        ///<seealso cref = "Calculator.AstronomicalCalculator.AdjustZenith" />
        DateTime? GetSunset();

        /// <summary>
        /// Gets or Sets the current AstronomicalCalculator set.
        /// </summary>
        /// <value>Returns the astronimicalCalculator.</value>
        IAstronomicalCalculator AstronomicalCalculator { get; set; }

        /// <summary>
        /// Gets or Sets the calender to be used in the calculations.
        /// </summary>
        /// <value>The calendar to set.</value>
        IDateWithLocation DateWithLocation { set; get; }
    }
}