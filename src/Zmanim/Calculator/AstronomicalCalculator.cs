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
    ///   An abstract class that all sun time calculating classes extend. This allows
    ///   the algorithm used to be changed at runtime, easily allowing comparison the
    ///   results of using different algorithms.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public abstract class AstronomicalCalculator : IAstronomicalCalculator
    {
        ///<summary>
        ///  getDefault method returns the default sun times calculation engine.
        ///</summary>
        ///<returns> AstronomicalCalculator the default class for calculating sunrise
        ///  and sunset. In the current implementation the default calculator
        ///  returned is the <see cref = "SunTimesCalculator" />. </returns>
        public static IAstronomicalCalculator GetDefault()
        {
            return new SunTimesCalculator();
        }

        double EarthRadius => 6356.9;

        ///<summary>
        ///  Method to return the adjustment to the zenith required to account for the
        ///  elevation. Since a person at a higher elevation can see farther below the
        ///  horizon, the calculation for sunrise / sunset is calculated below the
        ///  horizon used at sea level. This is only used for sunrise and sunset and
        ///  not times above or below it such as
        ///  <see cref = "AstronomicalCalendar.GetBeginNauticalTwilight">nautical twilight</see>
        ///  since those calculations are based on the level of available light at the
        ///  given dip below the horizon, something that is not affected by elevation,
        ///  the adjustment should only made if the zenith == 90°;
        ///  <see cref = "AdjustZenith">adjusted</see> for refraction and solar radius.<br />
        ///  The algorithm used is:
        ///	
        ///  <code>
        ///    elevationAdjustment = MathExtensions.ToDegree(Math.acos(earthRadiusInMeters
        ///    / (earthRadiusInMeters + elevationMeters)));
        ///  </code>
        ///	
        ///  The source of this algorthitm is <a href = "http://www.calendarists.com">Calendrical Calculations</a> by
        ///  Edward M. Reingold and Nachum Dershowitz. An alternate algorithm that
        ///  produces an almost identical (but not accurate) result found in Ma'aglay
        ///  Tzedek by Moishe Kosower and other sources is:
        ///	
        ///  <code>
        ///    elevationAdjustment = 0.0347 * Math.sqrt(elevationMeters);
        ///  </code>
        ///</summary>
        ///<param name = "elevation">
        ///  elevation in Meters. </param>
        ///<returns> the adjusted zenith </returns>
        double GetElevationAdjustment(double elevation)
        {
            return Math.Acos(EarthRadius / (EarthRadius + (elevation / 1000))).ToDegree();
        }

        ///<summary>
        ///  Adjusts the zenith to account for solar refraction, solar radius and
        ///  elevation. The value for Sun's zenith and true rise/set Zenith (used in
        ///  this class and subclasses) is the angle that the center of the Sun makes
        ///  to a line perpendicular to the Earth's surface. If the Sun were a point
        ///  and the Earth were without an atmosphere, true sunset and sunrise would
        ///  correspond to a 90°; zenith. Because the Sun is not a point, and
        ///  because the atmosphere refracts light, this 90°; zenith does not, in
        ///  fact, correspond to true sunset or sunrise, instead the centre of the
        ///  Sun's disk must lie just below the horizon for the upper edge to be
        ///  obscured. This means that a zenith of just above 90°; must be used.
        ///  The Sun subtends an angle of 16 minutes of arc (this can be changed via
        ///  the <see cref = "SolarRadius" /> method , and atmospheric refraction
        ///  accounts for 34 minutes or so (this can be changed via the
        ///  <see cref = "Refraction" /> method), giving a total of 50 arcminutes.
        ///  The total value for ZENITH is 90+(5/6) or 90.8333333°; for true
        ///  sunrise/sunset. Since a person at an elevation can see blow the horizon
        ///  of a person at sea level, this will also adjust the zenith to account for
        ///  elevation if available.
        ///</summary>
        ///<returns> The zenith adjusted to include the
        ///  <seealso cref="SolarRadius">sun's radius</seealso>,
        ///  <seealso cref="Refraction">refraction</seealso> and
        ///  <seealso cref="GetElevationAdjustment">elevation</seealso> adjustment.
        ///</returns>
        protected double AdjustZenith(double zenith, double elevation)
        {
            if (zenith == AstronomicalCalendar.GEOMETRIC_ZENITH)
                return zenith + (SolarRadius + Refraction + GetElevationAdjustment(elevation));

            return zenith;
        }

        ///<summary>
        /// Method to get the refraction value to be used when calculating sunrise and sunset.The default value is 34 arc
        /// minutes. The<a href="http://emr.cs.iit.edu/home/reingold/calendar-book/second-edition/errata.pdf"> Errata and
        /// Notes for Calendrical Calculations: The Millenium Eddition</a> by Edward M. Reingold and Nachum Dershowitz lists
        /// the actual average refraction value as 34.478885263888294 or approximately 34' 29". The refraction value as well
        /// as the solarRadius and elevation adjustment are added to the zenith used to calculate sunrise and sunset.
        ///
        /// Allow overriding the default refraction of the calculator. TODO: At some point in the future, an
	    /// AtmosphericModel or Refraction object that models the atmosphere of different locations might be used for
	    /// increased accuracy.
        ///</summary>
        ///<value>
        ///  The refraction in arc minutes. </value>
        public double Refraction { get; set; } = 34 / 60d;

        ///<summary>
        /// Get or Set the sun's radius. The default value is 16 arc minutes. The sun's radius as it appears from earth is
        /// almost universally given as 16 arc minutes but in fact it differs by the time of the year.At the<a
        /// href= "http://en.wikipedia.org/wiki/Perihelion" > perihelion </ a > it has an apparent radius of 16.293, while at the
        /// <a href = "http://en.wikipedia.org/wiki/Aphelion" > aphelion </ a > it has an apparent radius of 15.755. There is little
        /// affect for most location, but at high and low latitudes the difference becomes more apparent.My Calculations for
        /// the difference at the location of the<a href="http://www.rog.nmm.ac.uk"> Royal Observatory, Greenwich</a> show
        /// only a 4.494 second difference between the perihelion and aphelion radii, but moving into the arctic circle the
        /// difference becomes more noticeable.Tests for Tromso, Norway (latitude 69.672312, longitude 19.049787) show that
        /// on May 17, the rise of the midnight sun, a 2 minute 23 second difference is observed between the perihelion and
        /// aphelion radii using the USNO algorithm, but only 1 minute and 6 seconds difference using the NOAA algorithm.
        /// Areas farther north show an even greater difference. Note that these test are not real valid test cases because
        /// they show the extreme difference on days that are not the perihelion or aphelion, but are shown for illustrative
        /// purposes only.
        ///</summary>
        ///<value>
        ///  The sun&apos;s radius in arc minutes. </value>
        public double SolarRadius { get; set; } = 16 / 60d;

        /// <summary>
        /// A descriptive name of the algorithm.
        /// </summary>
        /// <value></value>
        public abstract string CalculatorName { get; }

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
        public abstract double GetUtcSunrise(IDateWithLocation dateWithLocation, double zenith, bool adjustForElevation);

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
        /// <param name="adjustForElevation">if set to <c>true</c> [adjust for elevation].</param>
        /// <returns>
        /// The UTC time of sunset in 24 hour format. 5:45:00 AM will return
        /// 5.75.0. If an error was encountered in the calculation (expected
        /// behavior for some locations such as near the poles,
        /// <seealso cref="Double.NaN"/> will be returned.
        /// </returns>
        public abstract double GetUtcSunset(IDateWithLocation dateWithLocation, double zenith, bool adjustForElevation);
    }
}