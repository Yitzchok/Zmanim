// * Zmanim .NET API
// * Copyright (C) 2004-2010 Eliyahu Hershfeld
// *
// * Converted to C# by AdminJew
// *
// * This file is part of Zmanim .NET API.
// *
// * Zmanim .NET API is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * Zmanim .NET API is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License
// * along with Zmanim.NET API.  If not, see <http://www.gnu.org/licenses/lgpl.html>.

using System;
using Zmanim.Utilities;

namespace Zmanim.Calculator
{
    /// <summary>
    ///   Implementation of sunrise and sunset methods to calculate astronomical times.
    ///   This calculator uses the Java algorithm written by <a href = "http://www.kevinboone.com/suntimes.html">Kevin Boone</a> that is based
    ///   on the <a href = "http://aa.usno.navy.mil/">US Naval Observatory's</a><a href = "http://aa.usno.navy.mil/publications/docs/asa.php">Almanac</a> for
    ///   Computer algorithm ( <a href = "http://www.amazon.com/exec/obidos/tg/detail/-/0160515106/">Amazon</a>,
    ///   <a href = "http://search.barnesandnoble.com/booksearch/isbnInquiry.asp?isbn=0160515106">Barnes
    ///     &amp; Noble</a>) and is used with his permission. Added to Kevin's code is
    ///   adjustment of the zenith to account for elevation.
    /// </summary>
    /// <author>Kevin Boone</author>
    /// <author>Eliyahu Hershfeld</author>
    public class SunTimesCalculator : AstronomicalCalculator
    {
        // DEG_PER_HOUR is the number of degrees of longitude
        // that corresponds to one hour time difference.
        private const double DEG_PER_HOUR = 360.0 / 24.0;

        /// <summary>
        /// </summary>
        /// <value>the descriptive name of the algorithm.</value>
        public override string CalculatorName => "US Naval Almanac Algorithm";

        /// <summary>
        /// A method that calculates UTC sunrise as well as any time based on an
        /// angle above or below sunrise. This abstract method is implemented by the
        /// classes that extend this class.
        /// </summary>
        /// <param name="dateWithLocation">Used to calculate day of year.</param>
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
        /// <seealso cref="AstronomicalCalculator.GetUtcSunrise"/>
        public override double GetUtcSunrise(IDateWithLocation dateWithLocation, double zenith,
                                             bool adjustForElevation)
        {
            double elevation = adjustForElevation ? dateWithLocation.Location.Elevation : 0;

            return GetTimeUtc(
                dateWithLocation.Date,
                dateWithLocation.Location,
                AdjustZenith(zenith, elevation),
                true);
        }

        /// <summary>
        /// A method that calculates UTC sunset as well as any time based on an angle
        /// above or below sunset. This abstract method is implemented by the classes
        /// that extend this class.
        /// </summary>
        /// <param name="dateWithLocation">Used to calculate day of year.</param>
        /// <param name="zenith">the azimuth below the vertical zenith of 90°;. For sunset
        /// typically the <see cref="AstronomicalCalculator.AdjustZenith">zenith</see> used for the
        /// calculation uses geometric zenith of 90°; and
        /// <see cref="AstronomicalCalculator.AdjustZenith">adjusts</see> this slightly to account for
        /// solar refraction and the sun's radius. Another example would
        /// be <see cref="AstronomicalCalendar.GetEndNauticalTwilight"/> that
        /// passes <see cref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to this
        /// method.</param>
        /// <param name="adjustForElevation"></param>
        /// <returns>
        /// The UTC time of sunset in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <seealso cref="Double.NaN"/> will be returned.
        /// </returns>
        /// <seealso cref="AstronomicalCalculator.GetUtcSunset"/>
        public override double GetUtcSunset(IDateWithLocation dateWithLocation, double zenith,
                                            bool adjustForElevation)
        {
            double elevation = adjustForElevation ? dateWithLocation.Location.Elevation : 0;

            return GetTimeUtc(
                dateWithLocation.Date,
                dateWithLocation.Location,
                AdjustZenith(zenith, elevation),
                false);
        }

        ///<summary>
        ///  sin of an angle in degrees
        ///</summary>
        private static double SinDeg(double deg)
        {
            return Math.Sin(deg * 2.0 * Math.PI / 360.0);
        }

        ///<summary>
        ///  acos of an angle, result in degrees
        ///</summary>
        private static double AcosDeg(double x)
        {
            return Math.Acos(x) * 360.0 / (2 * Math.PI);
        }

        ///<summary>
        ///  * asin of an angle, result in degrees
        ///</summary>
        private static double AsinDeg(double x)
        {
            return Math.Asin(x) * 360.0 / (2 * Math.PI);
        }

        ///<summary>
        ///  tan of an angle in degrees
        ///</summary>
        private static double TanDeg(double deg)
        {
            return Math.Tan(deg * 2.0 * Math.PI / 360.0);
        }

        ///<summary>
        ///  cos of an angle in degrees
        ///</summary>
        private static double CosDeg(double deg)
        {
            return Math.Cos(deg * 2.0 * Math.PI / 360.0);
        }

        ///<summary>
        ///  Get time difference between location's longitude and the Meridian, in
        ///  hours. West of Meridian has a negative time difference
        ///</summary>
        private static double GetHoursFromMeridian(double longitude)
        {
            return longitude / DEG_PER_HOUR;
        }

        ///<summary>
        ///  Gets the approximate time of sunset or sunrise In _days_ since midnight
        ///  Jan 1st, assuming 6am and 6pm events. We need this figure to derive the
        ///  Sun's mean anomaly
        ///</summary>
        private static double GetApproxTimeDays(int dayOfYear, double hoursFromMeridian, bool isSunrise)
        {
            var sunriseSunsetVar = isSunrise ? 6.0 : 18.0;
            return dayOfYear + ((sunriseSunsetVar - hoursFromMeridian) / 24);
        }

        ///<summary>
        ///  Calculate the Sun's mean anomaly in degrees, at sunrise or sunset, given
        ///  the longitude in degrees
        ///</summary>
        private static double GetMeanAnomaly(int dayOfYear, double longitude, bool isSunrise)
        {
            return (0.9856 * GetApproxTimeDays(dayOfYear, GetHoursFromMeridian(longitude), isSunrise)) - 3.289;
        }

        ///<summary>
        ///  Calculates the Sun's true longitude in degrees. The result is an angle
        ///  gte 0 and lt 360. Requires the Sun's mean anomaly, also in degrees
        ///</summary>
        private static double GetSunTrueLongitude(double sunMeanAnomaly)
        {
            double l = sunMeanAnomaly + (1.916 * SinDeg(sunMeanAnomaly)) + (0.020 * SinDeg(2 * sunMeanAnomaly)) + 282.634;

            // get longitude into 0-360 degree range
            if (l >= 360.0)
            {
                l = l - 360.0;
            }
            if (l < 0)
            {
                l = l + 360.0;
            }
            return l;
        }

        ///<summary>
        ///  Calculates the Sun's right ascension in hours, given the Sun's true
        ///  longitude in degrees. Input and output are angles gte 0 and lt 360.
        ///</summary>
        private static double GetSunRightAscensionHours(double sunTrueLongitude)
        {
            double a = 0.91764 * TanDeg(sunTrueLongitude);
            double ra = 360.0 / (2.0 * Math.PI) * Math.Atan(a);

            double lQuadrant = Math.Floor(sunTrueLongitude / 90.0) * 90.0;
            double raQuadrant = Math.Floor(ra / 90.0) * 90.0;
            ra = ra + (lQuadrant - raQuadrant);

            return ra / DEG_PER_HOUR; // convert to hours
        }

        ///<summary>
        ///  Gets the cosine of the Sun's local hour angle
        ///</summary>
        private static double GetCosLocalHourAngle(double sunTrueLongitude, double latitude, double zenith)
        {
            double sinDec = 0.39782 * SinDeg(sunTrueLongitude);
            double cosDec = CosDeg(AsinDeg(sinDec));
            return (CosDeg(zenith) - (sinDec * SinDeg(latitude))) / (cosDec * CosDeg(latitude));
        }

        ///<summary>
        ///  Calculate local mean time of rising or setting. By `local' is meant the
        ///  exact time at the location, assuming that there were no time zone. That
        ///  is, the time difference between the location and the Meridian depended
        ///  entirely on the longitude. We can't do anything with this time directly;
        ///  we must convert it to UTC and then to a local time. The result is
        ///  expressed as a fractional number of hours since midnight
        ///</summary>
        private static double GetLocalMeanTime(double localHour, double sunRightAscensionHours, double approxTimeDays)
        {
            return localHour + sunRightAscensionHours - (0.06571 * approxTimeDays) - 6.622;
        }

        /// <summary>
        ///   Get sunrise or sunset time in UTC, according to flag.
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="location">The location</param>
        /// <param name="zenith">Sun's zenith, in degrees</param>
        /// <param name="isSunrise">type of calculation to carry out sunrise or sunset.
        /// </param>
        /// <returns> the time as a double. If an error was encountered in the
        ///   calculation (expected behavior for some locations such as near
        ///   the poles, <see cref="Double.NaN" /> will be returned. </returns>
        private static double GetTimeUtc(
            DateTime date,
            IGeoLocation location,
            double zenith, bool isSunrise)
        {
            int dayOfYear = date.DayOfYear;
            double sunMeanAnomaly = GetMeanAnomaly(dayOfYear, location.Longitude, isSunrise);
            double sunTrueLong = GetSunTrueLongitude(sunMeanAnomaly);
            double sunRightAscensionHours = GetSunRightAscensionHours(sunTrueLong);
            double cosLocalHourAngle = GetCosLocalHourAngle(sunTrueLong, location.Latitude, zenith);

            double localHourAngle = 0;
            if (isSunrise)
            {
                if (cosLocalHourAngle > 1) // no rise. No need for an Exception
                {
                    // since the calculation
                    // will return Double.NaN
                }
                localHourAngle = 360.0 - AcosDeg(cosLocalHourAngle);
            }
            else
            {
                if (cosLocalHourAngle < -1) // no SET. No need for an Exception
                {
                    // since the calculation
                    // will return Double.NaN
                }
                localHourAngle = AcosDeg(cosLocalHourAngle);
            }
            double localHour = localHourAngle / DEG_PER_HOUR;

            double localMeanTime = GetLocalMeanTime(localHour, sunRightAscensionHours,
                                                    GetApproxTimeDays(dayOfYear, GetHoursFromMeridian(location.Longitude), isSunrise));
            double pocessedTime = localMeanTime - GetHoursFromMeridian(location.Longitude);
            while (pocessedTime < 0.0)
            {
                pocessedTime += 24.0;
            }
            while (pocessedTime >= 24.0)
            {
                pocessedTime -= 24.0;
            }
            return pocessedTime;
        }
    }
}