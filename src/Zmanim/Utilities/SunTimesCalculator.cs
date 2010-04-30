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

namespace net.sourceforge.zmanim.util
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
        ///<summary>
        ///  Default value for Sun's zenith and true rise/set
        ///</summary>
        public const double ZENITH = 90 + 50.0/60.0;

        private const int TYPE_SUNRISE = 0;

        private const int TYPE_SUNSET = 1;

        // DEG_PER_HOUR is the number of degrees of longitude
        // that corresponds to one hour time difference.
        private const double DEG_PER_HOUR = 360.0/24.0;
        private string calculatorName = "US Naval Almanac Algorithm";

        /// <summary>
        /// </summary>
        /// <returns>the descriptive name of the algorithm.</returns>
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
        /// <param name="adjustForElevation">if set to <c>true</c> [adjust for elevation].</param>
        /// <returns>
        /// The UTC time of sunrise in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <see cref="Double.NaN"/> will be returned.
        /// </returns>
        /// <seealso cref="net.sourceforge.zmanim.util.AstronomicalCalculator.getUTCSunrise(AstronomicalCalendar,double, bool)"/>
        public override double getUTCSunrise(AstronomicalCalendar astronomicalCalendar, double zenith,
                                             bool adjustForElevation)
        {
            double doubleTime = double.NaN;

            if (adjustForElevation)
            {
                zenith = adjustZenith(zenith, astronomicalCalendar.getGeoLocation().getElevation());
            }
            else
            {
                zenith = adjustZenith(zenith, 0);
            }
            doubleTime = getTimeUTC(astronomicalCalendar.getCalendar().Date.Year,
                                    astronomicalCalendar.getCalendar().Date.Month,
                                    astronomicalCalendar.getCalendar().Date.Day,
                                    astronomicalCalendar.getGeoLocation().getLongitude(),
                                    astronomicalCalendar.getGeoLocation().getLatitude(), zenith, TYPE_SUNRISE);
            return doubleTime;
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
        /// <seealso cref="net.sourceforge.zmanim.util.AstronomicalCalculator.getUTCSunset(AstronomicalCalendar,double, bool)"/>
        public override double getUTCSunset(AstronomicalCalendar astronomicalCalendar, double zenith,
                                            bool adjustForElevation)
        {
            double doubleTime = double.NaN;

            if (adjustForElevation)
            {
                zenith = adjustZenith(zenith, astronomicalCalendar.getGeoLocation().getElevation());
            }
            else
            {
                zenith = adjustZenith(zenith, 0);
            }
            doubleTime = getTimeUTC(astronomicalCalendar.getCalendar().Date.Year,
                                    astronomicalCalendar.getCalendar().Date.Month,
                                    astronomicalCalendar.getCalendar().Date.Day,
                                    astronomicalCalendar.getGeoLocation().getLongitude(),
                                    astronomicalCalendar.getGeoLocation().getLatitude(), zenith, TYPE_SUNSET);
            return doubleTime;
        }

        ///<summary>
        ///  sin of an angle in degrees
        ///</summary>
        private static double sinDeg(double deg)
        {
            return Math.Sin(deg*2.0*Math.PI/360.0);
        }

        ///<summary>
        ///  acos of an angle, result in degrees
        ///</summary>
        private static double acosDeg(double x)
        {
            return Math.Acos(x)*360.0/(2*Math.PI);
        }

        ///<summary>
        ///  * asin of an angle, result in degrees
        ///</summary>
        private static double asinDeg(double x)
        {
            return Math.Asin(x)*360.0/(2*Math.PI);
        }

        ///<summary>
        ///  tan of an angle in degrees
        ///</summary>
        private static double tanDeg(double deg)
        {
            return Math.Tan(deg*2.0*Math.PI/360.0);
        }

        ///<summary>
        ///  cos of an angle in degrees
        ///</summary>
        private static double cosDeg(double deg)
        {
            return Math.Cos(deg*2.0*Math.PI/360.0);
        }

        ///<summary>
        ///  * Calculate the day of the year, where Jan 1st is day 1. Note that this
        ///  * method needs to know the year, because leap years have an impact here
        ///</summary>
        private static int getDayOfYear(int year, int month, int day)
        {
            int n1 = 275*month/9;
            int n2 = (month + 9)/12;
            int n3 = (1 + ((year - 4*(year/4) + 2)/3));
            int n = n1 - (n2*n3) + day - 30;
            return n;
        }

        ///<summary>
        ///  Get time difference between location's longitude and the Meridian, in
        ///  hours. West of Meridian has a negative time difference
        ///</summary>
        private static double getHoursFromMeridian(double longitude)
        {
            return longitude/DEG_PER_HOUR;
        }

        ///<summary>
        ///  Gets the approximate time of sunset or sunrise In _days_ since midnight
        ///  Jan 1st, assuming 6am and 6pm events. We need this figure to derive the
        ///  Sun's mean anomaly
        ///</summary>
        private static double getApproxTimeDays(int dayOfYear, double hoursFromMeridian, int type)
        {
            if (type == TYPE_SUNRISE)
            {
                return dayOfYear + ((6.0 - hoursFromMeridian)/24);
            } // if (type == TYPE_SUNSET) 
            else
            {
                return dayOfYear + ((18.0 - hoursFromMeridian)/24);
            }
        }

        ///<summary>
        ///  Calculate the Sun's mean anomaly in degrees, at sunrise or sunset, given
        ///  the longitude in degrees
        ///</summary>
        private static double getMeanAnomaly(int dayOfYear, double longitude, int type)
        {
            return (0.9856*getApproxTimeDays(dayOfYear, getHoursFromMeridian(longitude), type)) - 3.289;
        }

        ///<summary>
        ///  Calculates the Sun's true longitude in degrees. The result is an angle
        ///  gte 0 and lt 360. Requires the Sun's mean anomaly, also in degrees
        ///</summary>
        private static double getSunTrueLongitude(double sunMeanAnomaly)
        {
            double l = sunMeanAnomaly + (1.916*sinDeg(sunMeanAnomaly)) + (0.020*sinDeg(2*sunMeanAnomaly)) + 282.634;

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
        private static double getSunRightAscensionHours(double sunTrueLongitude)
        {
            double a = 0.91764*tanDeg(sunTrueLongitude);
            double ra = 360.0/(2.0*Math.PI)*Math.Atan(a);
            // get result into 0-360 degree range
            // if (ra >= 360.0) ra = ra - 360.0;
            // if (ra < 0) ra = ra + 360.0;

            double lQuadrant = Math.Floor(sunTrueLongitude/90.0)*90.0;
            double raQuadrant = Math.Floor(ra/90.0)*90.0;
            ra = ra + (lQuadrant - raQuadrant);

            return ra/DEG_PER_HOUR; // convert to hours
        }

        ///<summary>
        ///  Gets the cosine of the Sun's local hour angle
        ///</summary>
        private static double getCosLocalHourAngle(double sunTrueLongitude, double latitude, double zenith)
        {
            double sinDec = 0.39782*sinDeg(sunTrueLongitude);
            double cosDec = cosDeg(asinDeg(sinDec));

            double cosH = (cosDeg(zenith) - (sinDec*sinDeg(latitude)))/(cosDec*cosDeg(latitude));

            // Check bounds

            return cosH;
        }

        /*
        ///	 <summary> Gets the cosine of the Sun's local hour angle for default zenith </summary>
        //	private static double getCosLocalHourAngle(double sunTrueLongitude,
        //			double latitude) {
        //		return getCosLocalHourAngle(sunTrueLongitude, latitude, ZENITH);
        //	}
        */

        ///<summary>
        ///  Calculate local mean time of rising or setting. By `local' is meant the
        ///  exact time at the location, assuming that there were no time zone. That
        ///  is, the time difference between the location and the Meridian depended
        ///  entirely on the longitude. We can't do anything with this time directly;
        ///  we must convert it to UTC and then to a local time. The result is
        ///  expressed as a fractional number of hours since midnight
        ///</summary>
        private static double getLocalMeanTime(double localHour, double sunRightAscensionHours, double approxTimeDays)
        {
            return localHour + sunRightAscensionHours - (0.06571*approxTimeDays) - 6.622;
        }

        ///<summary>
        ///  Get sunrise or sunset time in UTC, according to flag.
        ///</summary>
        ///<param name = "year">4-digit year </param>
        ///<param name = "month">month, 1-12 (not the zero based Java month </param>
        ///<param name = "day">day of month, 1-31 </param>
        ///<param name = "longitude">in degrees, longitudes west of Meridian are negative </param>
        ///<param name = "latitude">in degrees, latitudes south of equator are negative </param>
        ///<param name = "zenith">Sun's zenith, in degrees </param>
        ///<param name = "type">type of calculation to carry out <see cref = "TYPE_SUNRISE" /> or
        ///  <see cref = "TYPE_SUNRISE" />.
        ///</param>
        ///<returns> the time as a double. If an error was encountered in the
        ///  calculation (expected behavior for some locations such as near
        ///  the poles, <see cref = "Double.NaN" /> will be returned. </returns>
        private static double getTimeUTC(int year, int month, int day, double longitude, double latitude, double zenith,
                                         int type)
        {
            int dayOfYear = getDayOfYear(year, month, day);
            double sunMeanAnomaly = getMeanAnomaly(dayOfYear, longitude, type);
            double sunTrueLong = getSunTrueLongitude(sunMeanAnomaly);
            double sunRightAscensionHours = getSunRightAscensionHours(sunTrueLong);
            double cosLocalHourAngle = getCosLocalHourAngle(sunTrueLong, latitude, zenith);

            double localHourAngle = 0;
            if (type == TYPE_SUNRISE)
            {
                if (cosLocalHourAngle > 1) // no rise. No need for an Exception
                {
                    // since the calculation
                    // will return Double.NaN
                }
                localHourAngle = 360.0 - acosDeg(cosLocalHourAngle);
            } // if (type == TYPE_SUNSET) 
            else
            {
                if (cosLocalHourAngle < -1) // no SET. No need for an Exception
                {
                    // since the calculation
                    // will return Double.NaN
                }
                localHourAngle = acosDeg(cosLocalHourAngle);
            }
            double localHour = localHourAngle/DEG_PER_HOUR;

            double localMeanTime = getLocalMeanTime(localHour, sunRightAscensionHours,
                                                    getApproxTimeDays(dayOfYear, getHoursFromMeridian(longitude), type));
            double pocessedTime = localMeanTime - getHoursFromMeridian(longitude);
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