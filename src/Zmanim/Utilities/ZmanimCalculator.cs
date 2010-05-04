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

namespace Zmanim.Utilities
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
        private string calculatorName = "US Naval Almanac Algorithm";

        /// <summary>
        ///   Gets the name of the calculator/.
        /// </summary>
        /// <returns></returns>
        public override string getCalculatorName()
        {
            return calculatorName;
        }

        /// <summary>
        /// A method that calculates UTC sunrise as well as any time based on an
        /// angle above or below sunrise. This abstract method is implemented by the
        /// classes that extend this class.
        /// </summary>
        /// <param name="astronomicalCalendar">Used to calculate day of year.</param>
        /// <param name="zenith">the azimuth below the vertical zenith of 90 degrees. for
        /// sunrise typically the <see cref="AstronomicalCalculator.adjustZenith">zenith</see> used for
        /// the calculation uses geometric zenith of 90°; and
        /// <see cref="AstronomicalCalculator.adjustZenith">adjusts</see> this slightly to account for
        /// solar refraction and the sun's radius. Another example would
        /// be <see cref="AstronomicalCalendar.getBeginNauticalTwilight()"/>
        /// that passes <see cref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to
        /// this method.</param>
        /// <param name="adjustForElevation"></param>
        /// <returns>
        /// The UTC time of sunrise in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <see cref="Double.NaN"/> will be returned.
        /// </returns>
        public override double getUTCSunrise(AstronomicalCalendar astronomicalCalendar, double zenith,
                                             bool adjustForElevation)
        {
            // zenith = adjustZenithForElevation(astronomicalCalendar, zenith,
            // geoLocation.getElevation());
            // double elevationAdjustment = this.getElevationAdjustment(zenith,
            // geoLocation.getElevation());
            // double refractionAdjustment = this.getRefraction(zenith);
            // zenith = zenith + elevationAdjustment + refractionAdjustment;
            if (adjustForElevation)
                zenith = adjustZenith(zenith, astronomicalCalendar.GeoLocation.Elevation);
            else
                zenith = adjustZenith(zenith, 0);


            // step 1: First calculate the day of the year
            // NOT NEEDED in this implementation

            // step 2: convert the longitude to hour value and calculate an
            // approximate time
            double lngHour = astronomicalCalendar.GeoLocation.Longitude/15;

            double t = astronomicalCalendar.Calendar.Date.DayOfYear + ((6 - lngHour)/24); // use 18 for
            // sunset instead
            // of 6

            // step 3: calculate the sun's mean anomaly
            double m = (0.9856*t) - 3.289;

            // step 4: calculate the sun's true longitude
            double l = m + (1.916*Math.Sin(MathExtensions.ToRadians(m))) + (0.020*Math.Sin(MathExtensions.ToRadians(2*m))) +
                       282.634;
            while (l < 0)
            {
                double Lx = l + 360;
                l = Lx;
            }
            while (l >= 360)
            {
                double Lx = l - 360;
                l = Lx;
            }

            // step 5a: calculate the sun's right ascension
            double RA = MathExtensions.ToDegree(Math.Atan(0.91764*Math.Tan(MathExtensions.ToRadians(l))));

            while (RA < 0)
            {
                double RAx = RA + 360;
                RA = RAx;
            }
            while (RA >= 360)
            {
                double RAx = RA - 360;
                RA = RAx;
            }

            // step 5b: right ascension value needs to be in the same quadrant as L
            double lQuadrant = Math.Floor(l/90)*90;
            double raQuadrant = Math.Floor(RA/90)*90;
            RA = RA + (lQuadrant - raQuadrant);

            // step 5c: right ascension value needs to be converted into hours
            RA /= 15;

            // step 6: calculate the sun's declination
            double sinDec = 0.39782*Math.Sin(MathExtensions.ToRadians(l));
            double cosDec = Math.Cos(Math.Asin(sinDec));

            // step 7a: calculate the sun's local hour angle
            double cosH = (Math.Cos(MathExtensions.ToRadians(zenith)) -
                           (sinDec*Math.Sin(MathExtensions.ToRadians(astronomicalCalendar.GeoLocation.Latitude))))/
                          (cosDec*Math.Cos(MathExtensions.ToRadians(astronomicalCalendar.GeoLocation.Latitude)));

            // the following line would throw an Exception if the sun never rose.
            // this is not needed since the calculation will return a Double.NaN
            // if (cosH > 1) throw new Exception("doesnthappen");

            // FOR SUNSET use the following instead of the above if statement.
            // if (cosH < -1)

            // step 7b: finish calculating H and convert into hours
            double H = 360 - MathExtensions.ToDegree(Math.Acos(cosH));

            // FOR SUNSET remove "360 - " from the above

            H = H/15;

            // step 8: calculate local mean time

            double T = H + RA - (0.06571*t) - 6.622;

            // step 9: convert to UTC
            double UT = T - lngHour;
            while (UT < 0)
            {
                double UTx = UT + 24;
                UT = UTx;
            }
            while (UT >= 24)
            {
                double UTx = UT - 24;
                UT = UTx;
            }
            return UT;
        }

        /// <summary>
        /// A method that calculates UTC sunset as well as any time based on an angle
        /// above or below sunset. This abstract method is implemented by the classes
        /// that extend this class.
        /// </summary>
        /// <param name="astronomicalCalendar">Used to calculate day of year.</param>
        /// <param name="zenith">the azimuth below the vertical zenith of 90°;. For sunset
        /// typically the <see cref="AstronomicalCalculator.adjustZenith">zenith</see> used for the
        /// calculation uses geometric zenith of 90°; and
        /// <see cref="AstronomicalCalculator.adjustZenith">adjusts</see> this slightly to account for
        /// solar refraction and the sun's radius. Another example would
        /// be <see cref="AstronomicalCalendar.getEndNauticalTwilight()"/> that
        /// passes <see cref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to this
        /// method.</param>
        /// <param name="adjustForElevation"></param>
        /// <returns>
        /// The UTC time of sunset in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <seealso cref="Double.NaN"/> will be returned.
        /// </returns>
        public override double getUTCSunset(AstronomicalCalendar astronomicalCalendar, double zenith,
                                            bool adjustForElevation)
        {
            // zenith = adjustZenithForElevation(astronomicalCalendar, zenith,
            // geoLocation.getElevation());
            // double elevationAdjustment = this.getElevationAdjustment(zenith,
            // geoLocation.getElevation());
            // double refractionAdjustment = this.getRefraction(zenith);
            // zenith = zenith + elevationAdjustment + refractionAdjustment;

            if (adjustForElevation)
            {
                zenith = adjustZenith(zenith, astronomicalCalendar.GeoLocation.Elevation);
            }
            else
            {
                zenith = adjustZenith(zenith, 0);
            }

            // step 1: First calculate the day of the year
            // int calendarDayOfYear = calelendar.DAY_OF_YEAR;

            // int N=theday - date(1,1,theday.year()) + 1;
            int N = astronomicalCalendar.Calendar.Date.DayOfYear;

            // step 2: convert the longitude to hour value and calculate an
            // approximate time
            double lngHour = astronomicalCalendar.GeoLocation.Longitude/15;

            double t = N + ((18 - lngHour)/24);

            // step 3: calculate the sun's mean anomaly
            double M = (0.9856*t) - 3.289;

            // step 4: calculate the sun's true longitude
            double L = M + (1.916*Math.Sin(MathExtensions.ToRadians(M))) + (0.020*Math.Sin(MathExtensions.ToRadians(2*M))) +
                       282.634;
            while (L < 0)
            {
                double Lx = L + 360;
                L = Lx;
            }
            while (L >= 360)
            {
                double Lx = L - 360;
                L = Lx;
            }

            // step 5a: calculate the sun's right ascension
            double RA = MathExtensions.ToDegree(Math.Atan(0.91764*Math.Tan(MathExtensions.ToRadians(L))));
            while (RA < 0)
            {
                double RAx = RA + 360;
                RA = RAx;
            }
            while (RA >= 360)
            {
                double RAx = RA - 360;
                RA = RAx;
            }

            // step 5b: right ascension value needs to be in the same quadrant as L
            double Lquadrant = Math.Floor(L/90)*90;
            double RAquadrant = Math.Floor(RA/90)*90;
            RA = RA + (Lquadrant - RAquadrant);

            // step 5c: right ascension value needs to be converted into hours
            RA /= 15;

            // step 6: calculate the sun's declination
            double sinDec = 0.39782*Math.Sin(MathExtensions.ToRadians(L));
            double cosDec = Math.Cos(Math.Asin(sinDec));

            // step 7a: calculate the sun's local hour angle
            double cosH = (Math.Cos(MathExtensions.ToRadians(zenith)) -
                           (sinDec*Math.Sin(MathExtensions.ToRadians(astronomicalCalendar.GeoLocation.Latitude))))/
                          (cosDec*Math.Cos(MathExtensions.ToRadians(astronomicalCalendar.GeoLocation.Latitude)));

            // the following line would throw an Exception if the sun never set.
            // this is not needed since the calculation will return a Double.NaN
            // if (cosH < -1) throw new ZmanimException("doesnthappen");

            // step 7b: finish calculating H and convert into hours
            double H = MathExtensions.ToDegree(Math.Acos(cosH));
            H = H/15;

            // step 8: calculate local mean time

            double T = H + RA - (0.06571*t) - 6.622;

            // step 9: convert to UTC
            double UT = T - lngHour;
            while (UT < 0)
            {
                double UTx = UT + 24;
                UT = UTx;
            }
            while (UT >= 24)
            {
                double UTx = UT - 24;
                UT = UTx;
            }
            return UT;
        }
    }
}