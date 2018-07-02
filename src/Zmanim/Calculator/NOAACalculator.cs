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

        /// The <a href="http://en.wikipedia.org/wiki/Julian_day">Julian day</a> of January 1, 2000
        private static double JULIAN_DAY_JAN_1_2000 = 2451545.0;

        /// Julian days per century
        private static double JULIAN_DAYS_PER_CENTURY = 36525.0;

        /// <summary>
        /// Gets the name of the Calculator.
        /// </summary>
        /// <value>the descriptive name of the algorithm.</value>
        public override string CalculatorName => "US National Oceanic and Atmospheric Administration Algorithm";

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
            double adjustedZenith = AdjustZenith(zenith, adjustForElevation ? dateWithLocation.Location.Elevation : 0);

            double sunrise = GetSunriseUTC(GetJulianDay(dateWithLocation.Date),
                                            dateWithLocation.Location.Latitude,
                                            -dateWithLocation.Location.Longitude, adjustedZenith);
            sunrise = sunrise / 60;

            // ensure that the time is >= 0 and < 24
            while (sunrise < 0.0)
            {
                sunrise += 24.0;
            }
            while (sunrise >= 24.0)
            {
                sunrise -= 24.0;
            }
            return sunrise;

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

            double sunset = GetSunsetUTC(
                GetJulianDay(dateWithLocation.Date),
                dateWithLocation.Location.Latitude, -dateWithLocation.Location.Longitude,
                AdjustZenith(zenith, elevation)
            );

            sunset = sunset / 60;

            // ensure that the time is >= 0 and < 24
            while (sunset < 0.0)
            {
                sunset += 24.0;
            }
            while (sunset >= 24.0)
            {
                sunset -= 24.0;
            }
            return sunset;
        }

        ///<summary>
        ///  Generate a Julian day from a .NET date
        ///</summary>
        ///<param name="date">DateTime</param>
        ///<returns> the Julian day corresponding to the date Note: Number is returned
        ///  for start of day. Fractional days should be added later. </returns>
        private static double GetJulianDay(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }
            int a = year / 100;
            int b = 2 - a + a / 4;

            return Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5;
        }

        ///<summary>
        /// Convert <a href="http://en.wikipedia.org/wiki/Julian_day">Julian day</a> to centuries since J2000.0.
        ///</summary>
        ///<param name="julianDay">
        ///  the Julian Day to convert </param>
        ///<returns> the T value corresponding to the Julian Day </returns>
        private static double GetJulianCenturiesFromJulianDay(double julianDay)
        {
            return (julianDay - JULIAN_DAY_JAN_1_2000) / JULIAN_DAYS_PER_CENTURY;
        }

        ///<summary>
        /// Convert centuries since J2000.0 to <a href="http://en.wikipedia.org/wiki/Julian_day">Julian day</a>.
        ///</summary>
        ///<param name="julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns>the Julian Day corresponding to the Julian centuries passed in</returns>
        private static double GetJulianDayFromJulianCenturies(double julianCenturies)
        {
            return julianCenturies * JULIAN_DAYS_PER_CENTURY + JULIAN_DAY_JAN_1_2000;
        }

        ///<summary>
        /// Returns the Geometric <a href="http://en.wikipedia.org/wiki/Mean_longitude">Mean Longitude</a> of the Sun.
        ///</summary>
        ///<param name="julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the Geometric Mean Longitude of the Sun in degrees </returns>
        private static double GetSunGeometricMeanLongitude(double julianCenturies)
        {
            double longitude= 280.46646 + julianCenturies * (36000.76983 + 0.0003032 * julianCenturies);
            while (longitude > 360.0)
            {
                longitude -= 360.0;
            }
            while (longitude < 0.0)
            {
                longitude += 360.0;
            }

            return longitude; // in degrees
        }

        ///<summary>
        ///  Returns the Geometric <a href="http://en.wikipedia.org/wiki/Mean_anomaly">Mean Anomaly</a> of the Sun.
        ///</summary>
        ///<param name="julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the Geometric Mean Anomaly of the Sun in degrees </returns>
        private static double GetSunGeometricMeanAnomaly(double julianCenturies)
        {
            return 357.52911 + julianCenturies * (35999.05029 - 0.0001537 * julianCenturies); // in degrees
        }

        ///<summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Eccentricity_%28orbit%29">eccentricity of earth's orbit</a>.
        ///</summary>
        ///<param name="julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the unitless eccentricity </returns>
        private static double GetEarthOrbitEccentricity(double julianCenturies)
        {
            double e = 0.016708634 - julianCenturies * (0.000042037 + 0.0000001267 * julianCenturies);
            return e; // unitless
        }

        ///<summary>
        /// Returns the <a href="http://en.wikipedia.org/wiki/Equation_of_the_center">equation of center</a> for the sun.
        ///</summary>
        ///<param name = "julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the equation of center for the sun in degrees </returns>
        private static double GetSunEquationOfCenter(double julianCenturies)
        {
            double m = GetSunGeometricMeanAnomaly(julianCenturies);

            double mrad = m.ToRadians();
            double sinm = Math.Sin(mrad);
            double sin2m = Math.Sin(mrad + mrad);
            double sin3m = Math.Sin(mrad + mrad + mrad);

            return sinm * (1.914602 - julianCenturies
                        * (0.004817 + 0.000014 * julianCenturies)) + sin2m 
                        * (0.019993 - 0.000101 * julianCenturies) + sin3m * 0.000289;// in degrees
        }

        ///<summary>
        ///  Calculate the true longitude of the sun
        ///</summary>
        ///<param name = "julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the sun's true longitude in degrees </returns>
        private static double GetSunTrueLongitude(double julianCenturies)
        {
            double sunLongitude = GetSunGeometricMeanLongitude(julianCenturies);
            double center = GetSunEquationOfCenter(julianCenturies);

            return sunLongitude + center;// in degrees
        }

        ///<summary>
        ///  calculate the apparent longitude of the sun
        ///</summary>
        ///<param name = "julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> sun's apparent longitude in degrees </returns>
        private static double GetSunApparentLongitude(double julianCenturies)
        {
            double sunTrueLongitude = GetSunTrueLongitude(julianCenturies);
            double omega = 125.04 - 1934.136 * julianCenturies;
            return sunTrueLongitude - 0.00569 - 0.00478 * Math.Sin(omega.ToRadians()); // in degrees
        }

        ///<summary>
        /// Returns the mean <a href="http://en.wikipedia.org/wiki/Axial_tilt">obliquity of the ecliptic</a> (Axial tilt).
        ///</summary>
        ///<param name = "julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the mean obliquity in degrees </returns>
        private static double GetMeanObliquityOfEcliptic(double julianCenturies)
        {
            double seconds = 
                21.448 - julianCenturies 
              * (46.8150 + julianCenturies * (0.00059 - julianCenturies * (0.001813)));

            return 23.0 + (26.0 + (seconds / 60.0)) / 60.0; // in degrees
        }

        ///<summary>
        /// Returns the corrected <a href="http://en.wikipedia.org/wiki/Axial_tilt">obliquity of the ecliptic</a> (Axial tilt)
        ///</summary>
        ///<param name = "julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> the corrected obliquity in degrees </returns>
        private static double GetObliquityCorrection(double julianCenturies)
        {
            double obliquityOfEcliptic = GetMeanObliquityOfEcliptic(julianCenturies);

            double omega = 125.04 - 1934.136 * julianCenturies;
            return obliquityOfEcliptic + 0.00256 * Math.Cos(omega.ToRadians()); // in degrees
        }

        ///<summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Declination">declination</a> of the sun.
        ///</summary>
        ///<param name = "julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        private static double GetSunDeclination(double julianCenturies)
        {
            double obliquityCorrection = GetObliquityCorrection(julianCenturies);
            double lambda = GetSunApparentLongitude(julianCenturies);

            double sint = Math.Sin(obliquityCorrection.ToRadians()) * Math.Sin(lambda.ToRadians());
            return Math.Asin(sint).ToDegree(); // in degrees
        }

        ///<summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Equation_of_time">Equation of Time</a> - the difference between
        /// true solar time and mean solar time
        ///</summary>
        ///<param name="julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<returns> equation of time in minutes of time </returns>
        private static double GetEquationOfTime(double julianCenturies)
        {
            double epsilon = GetObliquityCorrection(julianCenturies);
            double geomMeanLongSun = GetSunGeometricMeanLongitude(julianCenturies);
            double eccentricityEarthOrbit = GetEarthOrbitEccentricity(julianCenturies);
            double geomMeanAnomalySun = GetSunGeometricMeanAnomaly(julianCenturies);

            double y = Math.Tan(epsilon.ToRadians() / 2.0);
            y *= y;

            double sin2l0 = Math.Sin(2.0 * geomMeanLongSun.ToRadians());
            double sinm = Math.Sin(geomMeanAnomalySun.ToRadians());
            double cos2l0 = Math.Cos(2.0 * geomMeanLongSun.ToRadians());
            double sin4l0 = Math.Sin(4.0 * geomMeanLongSun.ToRadians());
            double sin2m = Math.Sin(2.0 * geomMeanAnomalySun.ToRadians());

            double equationOfTime = 
                y * sin2l0 - 2.0 * eccentricityEarthOrbit * sinm + 4.0 * eccentricityEarthOrbit * y 
              * sinm * cos2l0 - 0.5 * y * y * sin4l0 - 1.25 * eccentricityEarthOrbit * eccentricityEarthOrbit * sin2m;
            return equationOfTime.ToDegree() * 4.0; // in minutes of time
        }

        /// <summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Hour_angle">hour angle</a> of the sun at sunrise for the
        /// latitude.
        /// </summary>
        /// <param name="lat">,
        /// the latitude of observer in degrees</param>
        /// <param name="solarDec">the declination angle of sun in degrees</param>
        /// <param name="zenith">The zenith.</param>
        /// <returns>hour angle of sunrise in radians</returns>
        private static double GetSunHourAngleAtSunrise(double lat, double solarDec, double zenith)
        {
            double latRad = lat.ToRadians();
            double sdRad = solarDec.ToRadians();

            return Math.Acos(Math.Cos(zenith.ToRadians()) / (Math.Cos(latRad) * Math.Cos(sdRad)) -
                             Math.Tan(latRad) * Math.Tan(sdRad)); // in radians
        }

        /// <summary>
        /// Returns the <a href="http://en.wikipedia.org/wiki/Hour_angle">hour angle</a> of the sun at sunset for the
	    /// latitude.
        /// </summary>
        /// <param name="lat">the latitude of observer in degrees</param>
        /// <param name="solarDec">the declination angle of sun in degrees</param>
        /// <param name="zenith">The zenith.</param>
        /// <returns>
        /// the hour angle of sunset in radians.
        /// </returns>
        private static double GetSunHourAngleAtSunset(double lat, double solarDec, double zenith)
        {
            return -GetSunHourAngleAtSunrise(lat, solarDec, zenith);// in radians
        }

        /// <summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Celestial_coordinate_system">Solar Elevation</a> for the
        /// horizontal coordinate system at the given location at the given time.Can be negative if the sun is below the
        /// horizon.Not corrected for altitude.
        ///
        /// <param name="dateWithLocation">the date with location</param>
        public static double GetSolarElevation(IDateWithLocation dateWithLocation)
        {
            double julianDay = GetJulianDay(dateWithLocation.Date);
            double julianCenturies = GetJulianCenturiesFromJulianDay(julianDay);

            double eot = GetEquationOfTime(julianCenturies);

            double longitude = (dateWithLocation.Date.Hour + 12.0)
                               + (dateWithLocation.Date.Minute + eot + dateWithLocation.Date.Second / 60.0) / 60.0;

            longitude = -(longitude * 360.0 / 24.0) % 360.0;
            double hourAngle_rad = (dateWithLocation.Location.Longitude - longitude).ToRadians();
            double declination = GetSunDeclination(julianCenturies);
            double dec_rad = declination.ToRadians();
            double lat_rad = dateWithLocation.Location.Latitude.ToRadians();

            return Math.Asin((Math.Sin(lat_rad) * Math.Sin(dec_rad))
                             + (Math.Cos(lat_rad) * Math.Cos(dec_rad) * Math.Cos(hourAngle_rad))).ToDegree();

        }

        /// <summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Universal_Coordinated_Time">Universal Coordinated Time</a> (UTC)
        /// of sunrise for the given day at the given location on earth
        /// </summary>
        /// <param name="julianDay">the julian day</param>
        /// <param name="latitude">the latitude of observer in degrees</param>
        /// <param name="longitude">the longitude of observer in degrees</param>
        /// <param name="zenith">The zenith.</param>
        /// <returns>the time in minutes from zero Z</returns>
        private static double GetSunriseUTC(double julianDay, double latitude, double longitude, double zenith)
        {
            double julianCenturies = GetJulianCenturiesFromJulianDay(julianDay);

            // Find the time of solar noon at the location, and use that declination.
            // This is better than start of the Julian day

            double noonmin = GetSolarNoonUTC(julianCenturies, longitude);
            double tnoon = GetJulianCenturiesFromJulianDay(julianDay + noonmin / 1440.0);

            // First pass to approximate sunrise (using solar noon)

            double eqTime = GetEquationOfTime(tnoon);
            double solarDec = GetSunDeclination(tnoon);
            double hourAngle = GetSunHourAngleAtSunrise(latitude, solarDec, zenith);

            double delta = longitude - hourAngle.ToDegree();
            double timeDiff = 4 * delta; // in minutes of time
            double timeUTC = 720 + timeDiff - eqTime; // in minutes

            // Second pass includes fractional jday in gamma calc

            double newt = GetJulianCenturiesFromJulianDay(GetJulianDayFromJulianCenturies(julianCenturies) + timeUTC / 1440.0);
            eqTime = GetEquationOfTime(newt);
            solarDec = GetSunDeclination(newt);
            hourAngle = GetSunHourAngleAtSunrise(latitude, solarDec, zenith);
            delta = longitude - hourAngle.ToDegree();
            timeDiff = 4 * delta;
            timeUTC = 720 + timeDiff - eqTime; // in minutes
            return timeUTC;
        }

        ///<summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Universal_Coordinated_Time">Universal Coordinated Time</a> (UTC)
	    /// of<a href="http://en.wikipedia.org/wiki/Noon#Solar_noon"> solar noon</a> for the given day at the given location
        /// on earth.
        ///</summary>
        ///<param name = "julianCenturies">
        ///  the number of Julian centuries since J2000.0 </param>
        ///<param name = "longitude">
        ///  the longitude of observer in degrees </param>
        ///<returns> the time in minutes from zero Z </returns>
        private static double GetSolarNoonUTC(double julianCenturies, double longitude)
        {
            // First pass uses approximate solar noon to calculate eqtime
            double tnoon = GetJulianCenturiesFromJulianDay(GetJulianDayFromJulianCenturies(julianCenturies) + longitude / 360.0);
            double eqTime = GetEquationOfTime(tnoon);
            double solNoonUTC = 720 + (longitude * 4) - eqTime; // min

            double newt = GetJulianCenturiesFromJulianDay(GetJulianDayFromJulianCenturies(julianCenturies) - 0.5 + solNoonUTC / 1440.0);

            eqTime = GetEquationOfTime(newt);
            return 720 + (longitude * 4) - eqTime; // min
        }

        ///<summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Universal_Coordinated_Time">Universal Coordinated Time</a> (UTC)
	    /// of sunset for the given day at the given location on earth
        ///</summary>
        ///<param name = "julianDay">
        ///  the julian day </param>
        ///<param name = "latitude">
        ///  the latitude of observer in degrees </param>
        ///<param name = "longitude"> :
        ///  longitude of observer in degrees </param>
        ///<param name = "zenith"> </param>
        ///<returns> the time in minutes from zero Z </returns>
        private static double GetSunsetUTC(double julianDay, double latitude, double longitude, double zenith)
        {
            double t = GetJulianCenturiesFromJulianDay(julianDay);

            // Find the time of solar noon at the location, and use
            // that declination. This is better than start of the
            // Julian day

            double noonmin = GetSolarNoonUTC(t, longitude);
            double tnoon = GetJulianCenturiesFromJulianDay(julianDay + noonmin / 1440.0);

            // First calculates sunrise and approx length of day

            double eqTime = GetEquationOfTime(tnoon);
            double solarDec = GetSunDeclination(tnoon);
            double hourAngle = GetSunHourAngleAtSunset(latitude, solarDec, zenith);

            double delta = longitude - MathExtensions.ToDegree(hourAngle);
            double timeDiff = 4 * delta;
            double timeUTC = 720 + timeDiff - eqTime;

            // first pass used to include fractional day in gamma calc

            double newt = GetJulianCenturiesFromJulianDay(GetJulianDayFromJulianCenturies(t) + timeUTC / 1440.0);
            eqTime = GetEquationOfTime(newt);
            solarDec = GetSunDeclination(newt);
            hourAngle = GetSunHourAngleAtSunset(latitude, solarDec, zenith);

            delta = longitude - MathExtensions.ToDegree(hourAngle);
            timeDiff = 4 * delta;
            return 720 + timeDiff - eqTime; // in minutes
        }
    }
}