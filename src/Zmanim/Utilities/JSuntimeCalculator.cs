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
    ///   This calculator uses the Java algorithm written by <a href = "http://www.jstot.me.uk/jsuntimes/">Jonathan Stott</a> that is based on
    ///   the implementation by <a href = "http://noaa.gov">NOAA - National Oceanic and
    ///                           Atmospheric Administration</a>'s <a href = "http://www.srrb.noaa.gov/highlights/sunrise/sunrisehtml">Surface Radiation
    ///                                                              Research Branch</a>. NOAA's <a href = "http://www.srrb.noaa.gov/highlights/sunrise/solareqns.PDF">implementation</a>
    ///   is based on equations from <a href = "http://www.willbell.com/math/mc1.htm">Astronomical Algorithms</a> by
    ///   <a href = "http://en.wikipedia.org/wiki/Jean_Meeus">Jean Meeus</a>. Jonathan's
    ///   implementation was released under the GPL. Added to the algorithm is an
    ///   adjustment of the zenith to account for elevation.
    /// </summary>
    /// <seealso cref = "NOAACalculator" />
    /// <author>Jonathan Stott</author>
    /// <author>Eliyahu Hershfeld</author>
    [Obsolete(
        "This class is based on the NOAA algorithm but does not return calculations that match the NOAA algorithm JavaScript implementation. The calculations are about 2 minutes off. This call has been replaced by the NOAACalculator class."
        )]
    public class JSuntimeCalculator : AstronomicalCalculator
    {
        private string calculatorName = "US National Oceanic and Atmospheric Administration Algorithm";

        /// <summary>
        /// </summary>
        /// <value>the descriptive name of the algorithm.</value>
        /// <seealso cref="NOAACalculator.getCalculatorName"/>
        [Obsolete]
        public override string CalculatorName
        {
            get { return calculatorName; }
        }

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
        /// <seealso cref="NOAACalculator.GetUtcSunrise"/>
        /// <seealso cref="AstronomicalCalculator.GetUtcSunrise"/>
        [Obsolete]
        public override double GetUtcSunrise(AstronomicalCalendar astronomicalCalendar, double zenith,
                                             bool adjustForElevation)
        {
            //		if (astronomicalCalendar.getCalendar().get(Calendar.YEAR) == 2000) {
            //			throw new ZmanimException(
            //					"JSuntimeCalculator can not calculate times for the year 2000. Please try a date with a different year.");
            //		}

            if (adjustForElevation)
            {
                zenith = AdjustZenith(zenith, astronomicalCalendar.GeoLocation.Elevation);
            }
            else
            {
                zenith = AdjustZenith(zenith, 0);
            }
            double timeMins = morningPhenomenon(dateToJulian(astronomicalCalendar.Calendar),
                                                astronomicalCalendar.GeoLocation.Latitude,
                                                -astronomicalCalendar.GeoLocation.Longitude, zenith);
            return timeMins/60;
        }

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
        /// be <see cref="AstronomicalCalendar.getEndNauticalTwilight()"/> that
        /// passes <see cref="AstronomicalCalendar.NAUTICAL_ZENITH"/> to this
        /// method.</param>
        /// <param name="adjustForElevation">if set to <c>true</c> [adjust for elevation].</param>
        /// <returns>
        /// The UTC time of sunset in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <seealso cref="Double.NaN"/> will be returned.
        /// </returns>
        /// <seealso cref="NOAACalculator.GetUtcSunset"/>
        /// <seealso cref="AstronomicalCalculator.GetUtcSunset"/>
        [Obsolete]
        public override double GetUtcSunset(AstronomicalCalendar astronomicalCalendar, double zenith,
                                            bool adjustForElevation)
        {
            //		if (astronomicalCalendar.getCalendar().get(Calendar.YEAR) == 2000) {
            //			throw new ZmanimException(
            //					"JSuntimeCalculator can not calculate times for the year 2000. Please try a date with a different year.");
            //		}

            if (adjustForElevation)
            {
                zenith = AdjustZenith(zenith, astronomicalCalendar.GeoLocation.Elevation);
            }
            else
            {
                zenith = AdjustZenith(zenith, 0);
            }
            double timeMins = eveningPhenomenon(dateToJulian(astronomicalCalendar.Calendar),
                                                astronomicalCalendar.GeoLocation.Latitude,
                                                -astronomicalCalendar.GeoLocation.Longitude, zenith);
            return timeMins/60;
        }

        ///<summary>
        ///  Calculate the UTC of a morning phenomenon for the given day at the given
        ///  latitude and longitude on Earth
        ///</summary>
        ///<param name = "julian">
        ///  Julian day </param>
        ///<param name = "latitude">
        ///  latitude of observer in degrees </param>
        ///<param name = "longitude">
        ///  longitude of observer in degrees </param>
        ///<param name = "zenithDistance">
        ///  one of Sun.SUNRISE_SUNSET_ZENITH_DISTANCE,
        ///  Sun.CIVIL_TWILIGHT_ZENITH_DISTANCE,
        ///  Sun.NAUTICAL_TWILIGHT_ZENITH_DISTANCE,
        ///  Sun.ASTRONOMICAL_TWILIGHT_ZENITH_DISTANCE. </param>
        ///<returns> time in minutes from zero Z </returns>
        private static double morningPhenomenon(double julian, double latitude, double longitude, double zenithDistance)
        {
            double t = julianDayToJulianCenturies(julian);
            double eqtime = equationOfTime(t);
            double solarDec = sunDeclination(t);
            double hourangle = hourAngleMorning(latitude, solarDec, zenithDistance);
            double delta = longitude - MathExtensions.ToDegree(hourangle);
            double timeDiff = 4*delta;
            double timeUTC = 720 + timeDiff - eqtime;

            // Second pass includes fractional julian day in gamma calc
            double newt = julianDayToJulianCenturies(julianCenturiesToJulianDay(t) + timeUTC/1440);
            eqtime = equationOfTime(newt);
            solarDec = sunDeclination(newt);
            hourangle = hourAngleMorning(latitude, solarDec, zenithDistance);
            delta = longitude - MathExtensions.ToDegree(hourangle);
            timeDiff = 4*delta;

            double morning = 720 + timeDiff - eqtime;
            return morning;
        }

        ///<summary>
        ///  Calculate the UTC of an evening phenomenon for the given day at the given
        ///  latitude and longitude on Earth
        ///</summary>
        ///<param name = "julian">
        ///  Julian day </param>
        ///<param name = "latitude">
        ///  latitude of observer in degrees </param>
        ///<param name = "longitude">
        ///  longitude of observer in degrees </param>
        ///<param name = "zenithDistance">
        ///  one of Sun.SUNRISE_SUNSET_ZENITH_DISTANCE,
        ///  Sun.CIVIL_TWILIGHT_ZENITH_DISTANCE,
        ///  Sun.NAUTICAL_TWILIGHT_ZENITH_DISTANCE,
        ///  Sun.ASTRONOMICAL_TWILIGHT_ZENITH_DISTANCE. </param>
        ///<returns> time in minutes from zero Z </returns>
        private static double eveningPhenomenon(double julian, double latitude, double longitude, double zenithDistance)
        {
            double t = julianDayToJulianCenturies(julian);

            // First calculates sunrise and approx length of day
            double eqtime = equationOfTime(t);
            double solarDec = sunDeclination(t);
            double hourangle = hourAngleEvening(latitude, solarDec, zenithDistance);

            double delta = longitude - MathExtensions.ToDegree(hourangle);
            double timeDiff = 4*delta;
            double timeUTC = 720 + timeDiff - eqtime;

            // first pass used to include fractional day in gamma calc
            double newt = julianDayToJulianCenturies(julianCenturiesToJulianDay(t) + timeUTC/1440);
            eqtime = equationOfTime(newt);
            solarDec = sunDeclination(newt);
            hourangle = hourAngleEvening(latitude, solarDec, zenithDistance);

            delta = longitude - MathExtensions.ToDegree(hourangle);
            timeDiff = 4*delta;

            double evening = 720 + timeDiff - eqtime;
            return evening;
        }

        private static double dateToJulian(ITimeZoneDateTime date)
        {
            int year = date.Date.Year;
            int month = date.Date.Month;
            int day = date.Date.Day;
            int hour = date.Date.Hour;
            int minute = date.Date.Minute;
            int second = date.Date.Second;

            double extra = (100.0*year) + month - 190002.5;
            double JD = (367.0*year) - (Math.Floor(7.0*(year + Math.Floor((month + 9.0)/12.0))/4.0)) +
                        Math.Floor((275.0*month)/9.0) + day + ((hour + ((minute + (second/60.0))/60.0))/24.0) +
                        1721013.5 - ((0.5*extra)/Math.Abs(extra)) + 0.5;
            return JD;
        }

        ///<summary>
        ///  Convert Julian Day to centuries since J2000.0
        ///</summary>
        ///<param name = "julian">
        ///  The Julian Day to convert </param>
        ///<returns> the value corresponding to the Julian Day </returns>
        private static double julianDayToJulianCenturies(double julian)
        {
            return (julian - 2451545)/36525;
        }

        ///<summary>
        ///  Convert centuries since J2000.0 to Julian Day
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> The Julian Day corresponding to the value of t </returns>
        private static double julianCenturiesToJulianDay(double t)
        {
            return (t*36525) + 2451545;
        }

        ///<summary>
        ///  Calculate the difference between true solar time and mean solar time
        ///</summary>
        ///<param name = "t">Number of Julian centuries since J2000.0</param>
        private static double equationOfTime(double t)
        {
            double epsilon = obliquityCorrection(t);
            double l0 = geomMeanLongSun(t);
            double e = eccentricityOfEarthsOrbit(t);
            double m = geometricMeanAnomalyOfSun(t);
            double y = Math.Pow((Math.Tan(MathExtensions.ToRadians(epsilon)/2)), 2);

            double eTime = y*Math.Sin(2*MathExtensions.ToRadians(l0)) - 2*e*Math.Sin(MathExtensions.ToRadians(m)) +
                           4*e*y*Math.Sin(MathExtensions.ToRadians(m))*Math.Cos(2*MathExtensions.ToRadians(l0)) -
                           0.5*y*y*Math.Sin(4*MathExtensions.ToRadians(l0)) - 1.25*e*e*Math.Sin(2*MathExtensions.ToRadians(m));
            return MathExtensions.ToDegree(eTime)*4;
        }

        ///<summary>
        ///  Calculate the declination of the sun
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> The Sun's declination in degrees </returns>
        private static double sunDeclination(double t)
        {
            double e = obliquityCorrection(t);
            double lambda = sunsApparentLongitude(t);

            double sint = Math.Sin(MathExtensions.ToRadians(e))*Math.Sin(MathExtensions.ToRadians(lambda));
            return MathExtensions.ToDegree(Math.Asin(sint));
        }

        ///<summary>
        ///  calculate the hour angle of the sun for a morning phenomenon for the
        ///  given latitude
        ///</summary>
        ///<param name = "lat">
        ///  Latitude of the observer in degrees </param>
        ///<param name = "solarDec">
        ///  declination of the sun in degrees </param>
        ///<param name = "zenithDistance">
        ///  zenith distance of the sun in degrees </param>
        ///<returns> hour angle of sunrise in radians </returns>
        private static double hourAngleMorning(double lat, double solarDec, double zenithDistance)
        {
            return
                (Math.Acos(Math.Cos(MathExtensions.ToRadians(zenithDistance))/
                                  (Math.Cos(MathExtensions.ToRadians(lat))*Math.Cos(MathExtensions.ToRadians(solarDec))) -
                                  Math.Tan(MathExtensions.ToRadians(lat))*Math.Tan(MathExtensions.ToRadians(solarDec))));
        }

        ///<summary>
        ///  Calculate the hour angle of the sun for an evening phenomenon for the
        ///  given latitude
        ///</summary>
        ///<param name = "lat">
        ///  Latitude of the observer in degrees </param>
        ///<param name = "solarDec">
        ///  declination of the Sun in degrees </param>
        ///<param name = "zenithDistance">
        ///  zenith distance of the sun in degrees </param>
        ///<returns> hour angle of sunset in radians </returns>
        private static double hourAngleEvening(double lat, double solarDec, double zenithDistance)
        {
            return -hourAngleMorning(lat, solarDec, zenithDistance);
        }

        ///<summary>
        ///  Calculate the corrected obliquity of the ecliptic
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> Corrected obliquity in degrees </returns>
        private static double obliquityCorrection(double t)
        {
            return meanObliquityOfEcliptic(t) + 0.00256*Math.Cos(MathExtensions.ToRadians(125.04 - 1934.136*t));
        }

        ///<summary>
        ///  Calculate the mean obliquity of the ecliptic
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> Mean obliquity in degrees </returns>
        private static double meanObliquityOfEcliptic(double t)
        {
            return 23 + (26 + (21.448 - t*(46.815 + t*(0.00059 - t*(0.001813)))/60))/60;
        }

        ///<summary>
        ///  Calculate the geometric mean longitude of the sun
        ///</summary>
        ///<param name = "t">
        ///  number of Julian centuries since J2000.0 </param>
        ///<returns> the geometric mean longitude of the sun in degrees </returns>
        private static double geomMeanLongSun(double t)
        {
            double l0 = 280.46646 + t*(36000.76983 + 0.0003032*t);

            while ((l0 >= 0) && (l0 <= 360))
            {
                if (l0 > 360)
                {
                    l0 = l0 - 360;
                }

                if (l0 < 0)
                {
                    l0 = l0 + 360;
                }
            }
            return l0;
        }

        ///<summary>
        ///  Calculate the eccentricity of Earth's orbit
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> the eccentricity </returns>
        private static double eccentricityOfEarthsOrbit(double t)
        {
            return 0.016708634 - t*(0.000042037 + 0.0000001267*t);
        }

        ///<summary>
        ///  Calculate the geometric mean anomaly of the Sun
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> the geometric mean anomaly of the Sun in degrees </returns>
        private static double geometricMeanAnomalyOfSun(double t)
        {
            return 357.52911 + t*(35999.05029 - 0.0001537*t);
        }

        ///<summary>
        ///  Calculate the apparent longitude of the sun
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> The apparent longitude of the Sun in degrees </returns>
        private static double sunsApparentLongitude(double t)
        {
            return sunsTrueLongitude(t) - 0.00569 - 0.00478*Math.Sin(MathExtensions.ToRadians(125.04 - 1934.136*t));
        }

        ///<summary>
        ///  Calculate the true longitude of the sun
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> The Sun's true longitude in degrees </returns>
        private static double sunsTrueLongitude(double t)
        {
            return geomMeanLongSun(t) + equationOfCentreForSun(t);
        }

        ///<summary>
        ///  Calculate the equation of centre for the Sun
        ///</summary>
        ///<param name = "t">
        ///  Number of Julian centuries since J2000.0 </param>
        ///<returns> The equation of centre for the Sun in degrees </returns>
        private static double equationOfCentreForSun(double t)
        {
            double m = geometricMeanAnomalyOfSun(t);

            return Math.Sin(MathExtensions.ToRadians(m))*(1.914602 - t*(0.004817 + 0.000014*t)) +
                   Math.Sin(2*MathExtensions.ToRadians(m))*(0.019993 - 0.000101*t) +
                   Math.Sin(3*MathExtensions.ToRadians(m))*0.000289;
        }
    }
}