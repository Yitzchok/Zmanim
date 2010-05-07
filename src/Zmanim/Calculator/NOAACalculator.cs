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
    ///   Implementation of sunrise and sunset methods to calculate astronomical times based on the <a href = "http://noaa.gov">NOAA</a> algorithm.
    ///   This calculator uses the Java algorithm based on the implementation by <a href = "http://noaa.gov">NOAA - National Oceanic and Atmospheric
    ///                                                                            Administration</a>'s <a href = "http://www.srrb.noaa.gov/highlights/sunrise/sunrisehtml">Surface Radiation
    ///                                                                                                   Research Branch</a>. NOAA's <a href = "http://www.srrb.noaa.gov/highlights/sunrise/solareqns.PDF">implementation</a>
    ///   is based on equations from <a href = "http://www.willbell.com/math/mc1.htm">Astronomical Algorithms</a> by
    ///   <a href = "http://en.wikipedia.org/wiki/Jean_Meeus">Jean Meeus</a>. Added to
    ///   the algorithm is an adjustment of the zenith to account for elevation.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class NOAACalculator : AstronomicalCalculator
    {
        /// <summary>
        /// Gets the name of the Calculator.
        /// </summary>
        /// <value>the descriptive name of the algorithm.</value>
        public override string CalculatorName
        {
            get { return "US National Oceanic and Atmospheric Administration Algorithm"; }
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
        public override double GetUtcSunrise(IAstronomicalCalendar astronomicalCalendar, double zenith,
                                             bool adjustForElevation)
        {
            //		if (astronomicalCalendar.getCalendar().get(Calendar.YEAR) <= 2000) {
            //			throw new ZmanimException(
            //					"NOAACalculator can not calculate times earlier than the year 2000.	Please try a date with a different year.");
            //		}

            if (adjustForElevation)
            {
                zenith = AdjustZenith(zenith, astronomicalCalendar.GeoLocation.Elevation);
            }
            else
            {
                zenith = AdjustZenith(zenith, 0);
            }

            double sunRise = CalcSunriseUtc(CalcJd(astronomicalCalendar.Calendar),
                                            astronomicalCalendar.GeoLocation.Latitude,
                                            -astronomicalCalendar.GeoLocation.Longitude, zenith);
            return sunRise / 60;
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
        public override double GetUtcSunset(IAstronomicalCalendar astronomicalCalendar, double zenith,
                                            bool adjustForElevation)
        {
            // if (astronomicalCalendar.getCalendar().get(Calendar.YEAR) <= 2000) {
            // throw new ZmanimException(
            // "NOAACalculator can not calculate times for the year 2000. Please try
            // a date with a different year.");
            // }

            if (adjustForElevation)
            {
                zenith = AdjustZenith(zenith, astronomicalCalendar.GeoLocation.Elevation);
            }
            else
            {
                zenith = AdjustZenith(zenith, 0);
            }

            double sunSet = CalcSunsetUtc(CalcJd(astronomicalCalendar.Calendar),
                                          astronomicalCalendar.GeoLocation.Latitude,
                                          -astronomicalCalendar.GeoLocation.Longitude, zenith);
            return sunSet / 60;
        }

        ///<summary>
        ///  Generate a Julian day from Java Calendar
        ///</summary>
        ///<param name = "date">
        ///  Java Calendar </param>
        ///<returns> the Julian day corresponding to the date Note: Number is returned
        ///  for start of day. Fractional days should be added later. </returns>
        private static double CalcJd(ITimeZoneDateTime date)
        {
            int year = date.Date.Year;
            int month = date.Date.Month + 1;
            int day = date.Date.Day;
            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }
            double A = Math.Floor((double)(year / 100));
            double B = 2 - A + Math.Floor(A / 4);

            return Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + B - 1524.5;
        }

        ///<summary>
        ///  convert Julian Day to centuries since J2000.0.
        ///</summary>
        ///<param name = "jd">
        ///  the Julian Day to convert </param>
        ///<returns> the T value corresponding to the Julian Day </returns>
        private static double CalcTimeJulianCent(double jd)
        {
            return (jd - 2451545.0) / 36525.0;
        }

        ///<summary>
        ///  Convert centuries since J2000.0 to Julian Day.
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the Julian Day corresponding to the t value </returns>
        private static double CalcJdFromJulianCent(double t)
        {
            return t * 36525.0 + 2451545.0;
        }

        ///<summary>
        ///  calculates the Geometric Mean Longitude of the Sun
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the Geometric Mean Longitude of the Sun in degrees </returns>
        private static double CalcGeomMeanLongSun(double t)
        {
            double L0 = 280.46646 + t * (36000.76983 + 0.0003032 * t);
            while (L0 > 360.0)
            {
                L0 -= 360.0;
            }
            while (L0 < 0.0)
            {
                L0 += 360.0;
            }

            return L0; // in degrees
        }

        ///<summary>
        ///  Calculate the Geometric Mean Anomaly of the Sun
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the Geometric Mean Anomaly of the Sun in degrees </returns>
        private static double CalcGeomMeanAnomalySun(double t)
        {
            return 357.52911 + t * (35999.05029 - 0.0001537 * t);
        }

        ///<summary>
        ///  calculate the eccentricity of earth's orbit
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the unitless eccentricity </returns>
        private static double CalcEccentricityEarthOrbit(double t)
        {
            double e = 0.016708634 - t * (0.000042037 + 0.0000001267 * t);
            return e; // unitless
        }

        ///<summary>
        ///  Calculate the equation of center for the sun
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the equation of center for the sun in degrees </returns>
        private static double CalcSunEqOfCenter(double t)
        {
            double m = CalcGeomMeanAnomalySun(t);

            double mrad = MathExtensions.ToRadians(m);
            double sinm = Math.Sin(mrad);
            double sin2m = Math.Sin(mrad + mrad);
            double sin3m = Math.Sin(mrad + mrad + mrad);

            return sinm * (1.914602 - t * (0.004817 + 0.000014 * t)) + sin2m * (0.019993 - 0.000101 * t) + sin3m * 0.000289;
        }

        ///<summary>
        ///  Calculate the true longitude of the sun
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the sun's true longitude in degrees </returns>
        private static double CalcSunTrueLong(double t)
        {
            double l0 = CalcGeomMeanLongSun(t);
            double c = CalcSunEqOfCenter(t);

            return l0 + c;
        }

        //	/**
        //	 * Calculate the true anamoly of the sun
        //	 *
        //	 * @param t
        //	 *            the number of Julian centuries since J2000.0
        //	 * @return the sun's true anamoly in degrees
        //	 */
        //	private static double calcSunTrueAnomaly(double t) {
        //		double m = calcGeomMeanAnomalySun(t);
        //		double c = calcSunEqOfCenter(t);
        //
        //		double v = m + c;
        //		return v; // in degrees
        //	}

        ///<summary>
        ///  calculate the apparent longitude of the sun
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> sun's apparent longitude in degrees </returns>
        private static double CalcSunApparentLong(double t)
        {
            double o = CalcSunTrueLong(t);

            double omega = 125.04 - 1934.136 * t;
            double lambda = o - 0.00569 - 0.00478 * Math.Sin(MathExtensions.ToRadians(omega));
            return lambda; // in degrees
        }

        ///<summary>
        ///  Calculate the mean obliquity of the ecliptic
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the mean obliquity in degrees </returns>
        private static double CalcMeanObliquityOfEcliptic(double t)
        {
            double seconds = 21.448 - t * (46.8150 + t * (0.00059 - t * (0.001813)));
            double e0 = 23.0 + (26.0 + (seconds / 60.0)) / 60.0;
            return e0; // in degrees
        }

        ///<summary>
        ///  calculate the corrected obliquity of the ecliptic
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the corrected obliquity in degrees </returns>
        private static double CalcObliquityCorrection(double t)
        {
            double e0 = CalcMeanObliquityOfEcliptic(t);

            double omega = 125.04 - 1934.136 * t;
            double e = e0 + 0.00256 * Math.Cos(MathExtensions.ToRadians(omega));
            return e; // in degrees
        }

        ///<summary>
        ///  Calculate the declination of the sun
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        private static double CalcSunDeclination(double t)
        {
            double e = CalcObliquityCorrection(t);
            double lambda = CalcSunApparentLong(t);

            double sint = Math.Sin(MathExtensions.ToRadians(e)) * Math.Sin(MathExtensions.ToRadians(lambda));
            double theta = MathExtensions.ToDegree(Math.Asin(sint));
            return theta; // in degrees
        }

        ///<summary>
        ///  calculate the difference between true solar time and mean solar time
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> equation of time in minutes of time </returns>
        private static double CalcEquationOfTime(double t)
        {
            double epsilon = CalcObliquityCorrection(t);
            double l0 = CalcGeomMeanLongSun(t);
            double e = CalcEccentricityEarthOrbit(t);
            double m = CalcGeomMeanAnomalySun(t);

            double y = Math.Tan(MathExtensions.ToRadians(epsilon) / 2.0);
            y *= y;

            double sin2l0 = Math.Sin(2.0 * MathExtensions.ToRadians(l0));
            double sinm = Math.Sin(MathExtensions.ToRadians(m));
            double cos2l0 = Math.Cos(2.0 * MathExtensions.ToRadians(l0));
            double sin4l0 = Math.Sin(4.0 * MathExtensions.ToRadians(l0));
            double sin2m = Math.Sin(2.0 * MathExtensions.ToRadians(m));

            double Etime = y * sin2l0 - 2.0 * e * sinm + 4.0 * e * y * sinm * cos2l0 - 0.5 * y * y * sin4l0 - 1.25 * e * e * sin2m;
            return MathExtensions.ToDegree(Etime) * 4.0; // in minutes of time
        }

        /// <summary>
        /// Calculate the hour angle of the sun at sunrise for the latitude
        /// </summary>
        /// <param name="lat">,
        /// the latitude of observer in degrees</param>
        /// <param name="solarDec">the declination angle of sun in degrees</param>
        /// <param name="zenith">The zenith.</param>
        /// <returns>hour angle of sunrise in radians</returns>
        private static double CalcHourAngleSunrise(double lat, double solarDec, double zenith)
        {
            double latRad = MathExtensions.ToRadians(lat);
            double sdRad = MathExtensions.ToRadians(solarDec);

            // double HAarg =
            // (Math.cos(MathExtensions.ToRadians(zenith))/(Math.cos(latRad)*Math.cos(sdRad))-Math.tan(latRad)
            // * Math.tan(sdRad));

            double HA =
                (Math.Acos(Math.Cos(MathExtensions.ToRadians(zenith)) / (Math.Cos(latRad) * Math.Cos(sdRad)) -
                           Math.Tan(latRad) * Math.Tan(sdRad)));
            return HA; // in radians
        }

        /// <summary>
        /// Calculate the hour angle of the sun at sunset for the latitude
        /// </summary>
        /// <param name="lat">the latitude of observer in degrees</param>
        /// <param name="solarDec">the declination angle of sun in degrees</param>
        /// <param name="zenith">The zenith.</param>
        /// <returns>
        /// the hour angle of sunset in radians
        /// TODO: use - calcHourAngleSunrise implementation
        /// </returns>
        private static double CalcHourAngleSunset(double lat, double solarDec, double zenith)
        {
            double latRad = MathExtensions.ToRadians(lat);
            double sdRad = MathExtensions.ToRadians(solarDec);

            // double HAarg =
            // (Math.cos(MathExtensions.ToRadians(zenith))/(Math.cos(latRad)*Math.cos(sdRad))-Math.tan(latRad)
            // * Math.tan(sdRad));

            double HA =
                (Math.Acos(Math.Cos(MathExtensions.ToRadians(zenith)) / (Math.Cos(latRad) * Math.Cos(sdRad)) -
                           Math.Tan(latRad) * Math.Tan(sdRad)));
            return -HA; // in radians
        }

        /// <summary>
        /// Calculate the Universal Coordinated Time (UTC) of sunrise for the given
        /// day at the given location on earth
        /// </summary>
        /// <param name="JD">the julian day</param>
        /// <param name="latitude">the latitude of observer in degrees</param>
        /// <param name="longitude">the longitude of observer in degrees</param>
        /// <param name="zenith">The zenith.</param>
        /// <returns>the time in minutes from zero Z</returns>
        private static double CalcSunriseUtc(double JD, double latitude, double longitude, double zenith)
        {
            double t = CalcTimeJulianCent(JD);

            // *** Find the time of solar noon at the location, and use
            // that declination. This is better than start of the
            // Julian day

            double noonmin = CalcSolNoonUtc(t, longitude);
            double tnoon = CalcTimeJulianCent(JD + noonmin / 1440.0);

            // *** First pass to approximate sunrise (using solar noon)

            double eqTime = CalcEquationOfTime(tnoon);
            double solarDec = CalcSunDeclination(tnoon);
            double hourAngle = CalcHourAngleSunrise(latitude, solarDec, zenith);

            double delta = longitude - MathExtensions.ToDegree(hourAngle);
            double timeDiff = 4 * delta; // in minutes of time
            double timeUTC = 720 + timeDiff - eqTime; // in minutes

            // *** Second pass includes fractional jday in gamma calc

            double newt = CalcTimeJulianCent(CalcJdFromJulianCent(t) + timeUTC / 1440.0);
            eqTime = CalcEquationOfTime(newt);
            solarDec = CalcSunDeclination(newt);
            hourAngle = CalcHourAngleSunrise(latitude, solarDec, zenith);
            delta = longitude - MathExtensions.ToDegree(hourAngle);
            timeDiff = 4 * delta;
            timeUTC = 720 + timeDiff - eqTime; // in minutes
            return timeUTC;
        }

        ///<summary>
        ///  calculate the Universal Coordinated Time (UTC) of solar noon for the
        ///  given day at the given location on earth
        ///</summary>
        ///<param name = "t">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<param name = "longitude">
        ///  the longitude of observer in degrees </param>
        ///<returns> the time in minutes from zero Z </returns>
        private static double CalcSolNoonUtc(double t, double longitude)
        {
            // First pass uses approximate solar noon to calculate eqtime
            double tnoon = CalcTimeJulianCent(CalcJdFromJulianCent(t) + longitude / 360.0);
            double eqTime = CalcEquationOfTime(tnoon);
            double solNoonUTC = 720 + (longitude * 4) - eqTime; // min

            double newt = CalcTimeJulianCent(CalcJdFromJulianCent(t) - 0.5 + solNoonUTC / 1440.0);

            eqTime = CalcEquationOfTime(newt);
            return 720 + (longitude * 4) - eqTime; // min
        }

        ///<summary>
        ///  calculate the Universal Coordinated Time (UTC) of sunset for the given
        ///  day at the given location on earth
        ///</summary>
        ///<param name = "JD">
        ///  the julian day </param>
        ///<param name = "latitude">
        ///  the latitude of observer in degrees </param>
        ///<param name = "longitude"> :
        ///  longitude of observer in degrees </param>
        ///<param name = "zenith"> </param>
        ///<returns> the time in minutes from zero Z </returns>
        private static double CalcSunsetUtc(double JD, double latitude, double longitude, double zenith)
        {
            double t = CalcTimeJulianCent(JD);

            // *** Find the time of solar noon at the location, and use
            // that declination. This is better than start of the
            // Julian day

            double noonmin = CalcSolNoonUtc(t, longitude);
            double tnoon = CalcTimeJulianCent(JD + noonmin / 1440.0);

            // First calculates sunrise and approx length of day

            double eqTime = CalcEquationOfTime(tnoon);
            double solarDec = CalcSunDeclination(tnoon);
            double hourAngle = CalcHourAngleSunset(latitude, solarDec, zenith);

            double delta = longitude - MathExtensions.ToDegree(hourAngle);
            double timeDiff = 4 * delta;
            double timeUTC = 720 + timeDiff - eqTime;

            // first pass used to include fractional day in gamma calc

            double newt = CalcTimeJulianCent(CalcJdFromJulianCent(t) + timeUTC / 1440.0);
            eqTime = CalcEquationOfTime(newt);
            solarDec = CalcSunDeclination(newt);
            hourAngle = CalcHourAngleSunset(latitude, solarDec, zenith);

            delta = longitude - MathExtensions.ToDegree(hourAngle);
            timeDiff = 4 * delta;
            return 720 + timeDiff - eqTime; // in minutes
        }
    }
}