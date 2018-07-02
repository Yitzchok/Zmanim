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
using Zmanim.Extensions;

namespace Zmanim.Calculator
{
    /// <summary>
    ///   Implementation of sunrise and sunset methods to calculate astronomical times.
    ///   This implementation is a port of the C++ algorithm written by Ken Bloom for
    ///   the sourceforge.net <a href = "http://sourceforge.net/projects/zmanim/">Zmanim</a>
    ///   project. Ken's algorithm is based on the US Naval Almanac algorithm. Added to
    ///   Ken's code is adjustment of the zenith to account for elevation.
    /// </summary>
    /// <author>Ken Bloom</author>
    /// <author>Eliyahu Hershfeld</author>
    /// <remarks>
    ///   Changed to LGPL with permission from the authors.
    /// </remarks>
    public class ZmanimCalculator : AstronomicalCalculator
    {

        /// <summary>
        ///   Gets the name of the calculator/.
        /// </summary>
        /// <value></value>
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
        /// <param name="adjustForElevation"></param>
        /// <returns>
        /// The UTC time of sunrise in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <see cref="Double.NaN"/> will be returned.
        /// </returns>
        public override double GetUtcSunrise(
            IDateWithLocation dateWithLocation, double zenith, bool adjustForElevation)
        {
            return GetUtcSunriseSunset(dateWithLocation, zenith, adjustForElevation, true);
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
        public override double GetUtcSunset(
            IDateWithLocation dateWithLocation, double zenith, bool adjustForElevation)
        {
            return GetUtcSunriseSunset(dateWithLocation, zenith, adjustForElevation, false);
        }

        private double GetUtcSunriseSunset(
            IDateWithLocation dateWithLocation, double zenith, bool adjustForElevation, bool isSunrise)
        {
            double elevation = adjustForElevation ? dateWithLocation.Location.Elevation : 0;
            double adjustedZenith = AdjustZenith(zenith, elevation);

            // step 1: First calculate the day of the year
            int dayOfYear = dateWithLocation.Date.DayOfYear;

            // step 2: convert the longitude to hour value and calculate an
            // approximate time
            double lngHour = dateWithLocation.Location.Longitude / 15;

            double t = dayOfYear + (((isSunrise ? 6 : 18) - lngHour) / 24);

            // step 3: calculate the sun's mean anomaly
            double meanAnomaly = (0.9856 * t) - 3.289;

            // step 4: calculate the sun's true longitude
            double trueLongitude =
                meanAnomaly + (1.916 * Math.Sin(meanAnomaly.ToRadians()))
                  + (0.020 * Math.Sin((2 * meanAnomaly).ToRadians())) + 282.634;

            while (trueLongitude < 0) trueLongitude = trueLongitude + 360;
            while (trueLongitude >= 360) trueLongitude = trueLongitude - 360;

            // step 5a: calculate the sun's right ascension
            double rAscension = Math.Atan(0.91764 * Math.Tan(trueLongitude.ToRadians())).ToDegree();

            while (rAscension < 0) rAscension = rAscension + 360;
            while (rAscension >= 360) rAscension = rAscension - 360;

            // step 5b: right ascension value needs to be in the same quadrant as L
            double lQuadrant = Math.Floor(trueLongitude / 90) * 90;
            double rQuadrant = Math.Floor(rAscension / 90) * 90;
            rAscension = rAscension + (lQuadrant - rQuadrant);

            // step 5c: right ascension value needs to be converted into hours
            rAscension /= 15;

            // step 6: calculate the sun's declination
            double sinDec = 0.39782 * Math.Sin(trueLongitude.ToRadians());
            double cosDec = Math.Cos(Math.Asin(sinDec));

            var latitudeRadians = dateWithLocation.Location.Latitude.ToRadians();

            // step 7a: calculate the sun's local hour angle
            double cosH = (Math.Cos(adjustedZenith.ToRadians()) -
                           (sinDec * Math.Sin(latitudeRadians))) /
                          (cosDec * Math.Cos(latitudeRadians));

            // step 7b: finish calculating H and convert into hours
            double hours = Math.Acos(cosH).ToDegree();
            if (isSunrise) hours = 360 - hours;

            hours = hours / 15;

            // step 8: calculate local mean time

            double localMeanTime = hours + rAscension - (0.06571 * t) - 6.622;

            // step 9: convert to UTC
            double utc = localMeanTime - lngHour;

            while (utc < 0) utc = utc + 24;
            while (utc >= 24) utc = utc - 24;

            return utc;
        }
    }
}