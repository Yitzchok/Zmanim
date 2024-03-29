﻿// * Zmanim .NET API
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
using Zmanim.Calculator;
using Zmanim.Extensions;
using Zmanim.Utilities;

namespace Zmanim
{
    /// <summary>
    /// A calendar that calculates astronomical time calculations such as
    /// <see cref="GetSunrise">sunrise</see> and <see cref="GetSunset">sunset</see> times. This
    /// class contains a <see cref="DateWithLocation">Calendar</see> and can therefore use the
    /// standard Calendar functionality to change dates etc. The calculation engine
    /// used to calculate the astronomical times can be changed to a different
    /// implementation by implementing the <see cref="AstronomicalCalculator"/> and setting
    /// it with the <see cref="AstronomicalCalculator"/>. A
    /// number of different implementations are included in the util package <br/>
    /// 	<b>Note:</b> There are times when the algorithms can't calculate proper
    /// values for sunrise, sunset and twilight. This is usually caused by trying to calculate
    /// times for areas either very far North or South, where sunrise / sunset never
    /// happen on that date. This is common when calculating twilight with a deep dip
    /// below the horizon for locations as south of the North Pole as London in the
    /// northern hemisphere. The sun never reaches this dip at certain
    /// times of the year. When the calculations encounter this condition a null
    /// will be returned when a <see cref="DateTime"/> is expected and
    /// <see cref="long.MinValue"/> when a long is expected. The reason that
    /// <c>Exception</c>s are not thrown in these cases is because the lack
    /// of a rise/set or twilight is not an exception, but expected in many parts of the world.
    /// Here is a simple example of how to use the API to calculate sunrise: <br/>
    /// First create the Calendar for the location you would like to calculate:
    /// <example>
    /// 		<code>
    /// string locationName = "Lakewood, NJ"
    /// double latitude = 40.0828; //Lakewood, NJ
    /// double longitude = -74.2094; //Lakewood, NJ
    /// double elevation = 20; // optional elevation correction in Meters
    /// ITimeZone timeZone = new JavaTimeZone("America/New_York");
    /// GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    /// elevation, timeZone);
    /// AstronomicalCalendar ac = new AstronomicalCalendar(location);
    /// </code>
    /// You can set the Date and Location on the constructor (or else it will default the the current day).
    /// <code>
    /// AstronomicalCalendar ac = new AstronomicalCalendar(new DateTime(2010, 2, 8), location);
    /// </code>
    /// Or you can set the DateTime by calling.
    /// <code>
    /// ac.DateWithLocation.Date = new DateTime(2010, 2, 8);
    /// </code>
    /// To get the time of sunrise
    /// <code>
    /// Date sunrise = ac.getSunrise();
    /// </code>
    /// 	</example>
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class AstronomicalCalendar : IAstronomicalCalendar
    {
        ///<summary>
        ///  90° below the vertical. Used for certain calculations.<br />
        ///  <b>Note </b>: it is important to note the distinction between this zenith
        ///  and the <see cref = "Calculator.AstronomicalCalculator.AdjustZenith">adjusted zenith</see> used
        ///  for some solar calculations. This 90 zenith is only used because some
        ///  calculations in some subclasses are historically calculated as an offset
        ///  in reference to 90.
        ///</summary>
        public const double GEOMETRIC_ZENITH = 90;

        /*
        ///	 <summary> 
        ///	Default value for Sun's zenith and true rise/set Zenith (used in this
        ///	class and subclasses) is the angle that the center of the Sun makes to a
        ///	line perpendicular to the Earth's surface. If the Sun were a point and
        ///	the Earth were without an atmosphere, true sunset and sunrise would
        ///	correspond to a 90° zenith. Because the Sun is not a point, and
        ///	because the atmosphere refracts light, this 90° zenith does not, in
        ///	fact, correspond to true sunset or sunrise, instead the center of the
        ///	Sun's disk must lie just below the horizon for the upper edge to be
        ///	obscured. This means that a zenith of just above 90° must be used.
        ///	The Sun subtends an angle of 16 minutes of arc (this can be changed via
        ///	the <see cref="setSunRadius(double)"/> method , and atmospheric refraction
        ///	accounts for 34 minutes or so (this can be changed via the
        ///	<see cref="AstronomicalCalculator.setRefraction(double)"/> method), giving a total of 50 arcminutes.
        ///	The total value for ZENITH is 90+(5/6) or 90.8333333° for true
        ///	sunrise/sunset. </summary>
        ///	 
        public static double ZENITH = GEOMETRIC_ZENITH + 5.0 / 6.0;
        */

        /// <summary>
        ///   Sun's zenith at civil twilight (96°).
        /// </summary>
        public const double CIVIL_ZENITH = 96;

        /// <summary>
        ///   Sun's zenith at nautical twilight (102°).
        /// </summary>
        public const double NAUTICAL_ZENITH = 102;

        /// <summary>
        ///   Sun's zenith at astronomical twilight (108°).
        /// </summary>
        public const double ASTRONOMICAL_ZENITH = 108;

        /// <summary>
        ///   constant for milliseconds in a minute (60,000)
        /// </summary>
        internal const long MINUTE_MILLIS = 60 * 1000;

        /// <summary>
        ///   constant for milliseconds in an hour (3,600,000)
        /// </summary>
        internal const long HOUR_MILLIS = MINUTE_MILLIS * 60;

        ///<summary>
        ///  Default constructor will set a default <see cref = "GeoLocation" />,
        ///  a default
        ///  <see cref = "Calculator.AstronomicalCalculator.GetDefault()">AstronomicalCalculator</see> and
        ///  default the calendar to the current date.
        ///</summary>
        public AstronomicalCalendar()
            : this(new GeoLocation()) { }

        ///<summary>
        ///  A constructor that takes in as a parameter geolocation information
        ///</summary>
        ///<param name = "geoLocation">
        ///  The location information used for astronomical calculating sun
        ///  times. </param>
        public AstronomicalCalendar(IGeoLocation geoLocation)
            : this(DateTime.Now, geoLocation) { }

        ///<summary>
        ///  A constructor that takes in as a parameter geolocation information
        ///</summary>
        ///<param name="dateTime">The DateTime</param>
        ///<param name = "geoLocation">
        ///  The location information used for astronomical calculating sun
        ///  times. </param>
        public AstronomicalCalendar(DateTime dateTime, IGeoLocation geoLocation)
            : this(new DateWithLocation(dateTime, geoLocation)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AstronomicalCalendar"/> class.
        /// </summary>
        /// <param name="dateWithLocation">The date with location.</param>
        public AstronomicalCalendar(IDateWithLocation dateWithLocation)
        {
            DateWithLocation = dateWithLocation;
            AstronomicalCalculator = Calculator.AstronomicalCalculator.GetDefault();
        }

        /// <summary>
        /// The getSunrise method Returns a <c>DateTime</c> representing the
        /// sunrise time. The zenith used for the calculation uses
        /// <seealso cref="GEOMETRIC_ZENITH">geometric zenith</seealso> of 90°. This is adjusted
        /// by the <seealso cref="AstronomicalCalculator"/> that adds approximately 50/60 of a
        /// degree to account for 34 archminutes of refraction and 16 archminutes for
        /// the sun's radius for a total of
        /// <seealso cref="Calculator.AstronomicalCalculator.AdjustZenith">90.83333°</seealso>. See
        /// documentation for the specific implementation of the
        /// <seealso cref="AstronomicalCalculator"/> that you are using.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the exact sunrise time.
        /// If the calculation can't be computed such as in the Arctic
        /// Circle where there is at least one day a year where the sun does
        /// not rise, and one where it does not set, a null will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        /// <seealso cref="Calculator.AstronomicalCalculator.AdjustZenith"/>
        public virtual DateTime? GetSunrise()
        {
            double sunrise = GetUtcSunrise(GEOMETRIC_ZENITH);
            if (double.IsNaN(sunrise))
                return null;

            return GetDateFromTime(sunrise);
        }

        /// <summary>
        /// Method that returns the sunrise without correction for elevation.
        /// Non-sunrise and sunset calculations such as dawn and dusk, depend on the
        /// amount of visible light, something that is not affected by elevation.
        /// This method returns sunrise calculated at sea level. This forms the base
        /// for dawn calculations that are calculated as a dip below the horizon
        /// before sunrise.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the exact sea-level sunrise
        /// time.
        /// If the calculation can't be computed such as in the Arctic
        /// Circle where there is at least one day a year where the sun does
        /// not rise, and one where it does not set, a null will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        /// <seealso cref="GetSunrise"/>
        /// <seealso cref="GetUtcSeaLevelSunrise"/>
        public virtual DateTime? GetSeaLevelSunrise()
        {
            double sunrise = GetUtcSeaLevelSunrise(GEOMETRIC_ZENITH);
            if (double.IsNaN(sunrise))
                return null;

            return GetDateFromTime(sunrise);
        }

        /// <summary>
        /// A method to return the the beginning of civil twilight (dawn) using a
        /// zenith of <seealso cref="CIVIL_ZENITH">96°</seealso>.
        /// </summary>
        /// <returns>
        /// The <c>DateTime</c> of the beginning of civil twilight using
        /// a zenith of 96°. If the calculation can't be computed (see explanation on top of the page), null
        /// will be returned.
        /// </returns>
        /// <seealso cref="CIVIL_ZENITH"/>
        public virtual DateTime? GetBeginCivilTwilight()
        {
            return GetSunriseOffsetByDegrees(CIVIL_ZENITH);
        }

        ///<summary>
        ///  A method to return the the beginning of nautical twilight using a zenith
        ///  of <see cref = "NAUTICAL_ZENITH">102°</see>.
        ///</summary>
        ///<returns> The <c>DateTime</c> of the beginning of nautical twilight
        ///  using a zenith of 102°. If the calculation can't be
        ///  computed (see explanation on top of the page), null will be returned. </returns>
        ///<seealso cref = "NAUTICAL_ZENITH" />
        public virtual DateTime? GetBeginNauticalTwilight()
        {
            return GetSunriseOffsetByDegrees(NAUTICAL_ZENITH);
        }

        ///<summary>
        ///  A method that returns the the beginning of astronomical twilight using a
        ///  zenith of <see cref = "ASTRONOMICAL_ZENITH">108°</see>.
        ///</summary>
        ///<returns> The <c>DateTime</c> of the beginning of astronomical twilight
        ///  using a zenith of 108°. If the 
        /// calculation can't be computed (see explanation on top of thepage),
        /// null will be returned.
        /// </returns>
        ///<seealso cref = "ASTRONOMICAL_ZENITH" />
        public virtual DateTime? GetBeginAstronomicalTwilight()
        {
            return GetSunriseOffsetByDegrees(ASTRONOMICAL_ZENITH);
        }

        /// <summary>
        /// The getSunset method Returns a <c>DateTime</c> representing the
        /// sunset time. The zenith used for the calculation uses
        /// <see cref="GEOMETRIC_ZENITH">geometric zenith</see> of 90°. This is adjusted
        /// by the <see cref="AstronomicalCalculator"/> that adds approximately 50/60 of a
        /// degree to account for 34 archminutes of refraction and 16 archminutes for
        /// the sun's radius for a total of
        /// <see cref="Calculator.AstronomicalCalculator.AdjustZenith">90.83333°</see>. See
        /// documentation for the specific implementation of the
        /// <see cref="AstronomicalCalculator"/> that you are using. Note: In certain cases
        /// the calculates sunset will occur before sunrise. This will typically
        /// happen when a timezone other than the local timezone is used (calculating
        /// Los Angeles sunset using a GMT timezone for example). In this case the
        /// sunset date will be incremented to the following date.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the exact sunset time. 
        /// If the calculation can't be computed such as in the Arctic
        /// Circle where there is at least one day a year where the sun does
        /// not rise, and one where it does not set, a null will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        /// <seealso cref="Calculator.AstronomicalCalculator.AdjustZenith"/>
        public virtual DateTime? GetSunset()
        {
            double sunset = GetUtcSunset(GEOMETRIC_ZENITH);
            if (double.IsNaN(sunset))
                return null;

            return GetAdjustedSunsetDate(GetDateFromTime(sunset), GetSunrise());
        }

        /// <summary>
        /// A method that will roll the sunset time forward a day if sunset occurs
        /// before sunrise. This will typically happen when a timezone other than the
        /// local timezone is used (calculating Los Angeles sunset using a GMT
        /// timezone for example). In this case the sunset date will be incremented
        /// to the following date.
        /// </summary>
        /// <param name="sunset">the sunset date to adjust if needed</param>
        /// <param name="sunrise">the sunrise to compare to the sunset</param>
        /// <returns>
        /// the adjusted sunset date.
        /// If the calculation can't be computed such as in the Arctic
        /// Circle where there is at least one day a year where the sun does
        /// not rise, and one where it does not set, a null will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        private DateTime? GetAdjustedSunsetDate(DateTime? sunset, DateTime? sunrise)
        {
            if (sunset == null || sunrise == null || sunrise.Value.CompareTo(sunset.Value) < 0)
                return sunset;

            return sunset.Value.AddDays(1);
        }

        /// <summary>
        /// Method that returns the sunset without correction for elevation.
        /// Non-sunrise and sunset calculations such as dawn and dusk, depend on the
        /// amount of visible light, something that is not affected by elevation.
        /// This method returns sunset calculated at sea level. This forms the base
        /// for dusk calculations that are calculated as a dip below the horizon
        /// after sunset.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the exact sea-level sunset time.
        /// If the calculation can't be computed such as in the Arctic
        /// Circle where there is at least one day a year where the sun does
        /// not rise, and one where it does not set, a null will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        /// <seealso cref="GetSunset"/>
        /// <seealso cref="GetUtcSeaLevelSunset"/>
        public virtual DateTime? GetSeaLevelSunset()
        {
            double sunset = GetUtcSeaLevelSunset(GEOMETRIC_ZENITH);
            if (double.IsNaN(sunset))
                return null;

            return GetAdjustedSunsetDate(GetDateFromTime(sunset), GetSeaLevelSunrise());
        }

        /// <summary>
        /// A method to return the the end of civil twilight using a zenith of
        /// <see cref="CIVIL_ZENITH">96°</see>.
        /// </summary>
        /// <returns>
        /// The <c>DateTime</c> of the end of civil twilight using a
        /// zenith of <seealso cref="CIVIL_ZENITH">96°</seealso>. If the 
        /// calculation can't be computed (see explanation on top of thepage),
        /// null will be returned.
        /// </returns>
        /// <seealso cref="CIVIL_ZENITH"/>
        public virtual DateTime? GetEndCivilTwilight()
        {
            return GetSunsetOffsetByDegrees(CIVIL_ZENITH);
        }

        /// <summary>
        /// A method to return the the end of nautical twilight using a zenith of
        /// <see cref="NAUTICAL_ZENITH">102°</see>.
        /// </summary>
        /// <returns>
        /// The <c>DateTime</c> of the end of nautical twilight using a
        /// zenith of <seealso cref="NAUTICAL_ZENITH">102°</seealso>. If the 
        /// calculation can't be computed (see explanation on top of thepage),
        /// null will be returned.
        /// </returns>
        /// <seealso cref="NAUTICAL_ZENITH"/>
        public virtual DateTime? GetEndNauticalTwilight()
        {
            return GetSunsetOffsetByDegrees(NAUTICAL_ZENITH);
        }

        /// <summary>
        /// A method to return the the end of astronomical twilight using a zenith of
        /// <see cref="ASTRONOMICAL_ZENITH">108°</see>.
        /// </summary>
        /// <returns>
        /// The The <c>DateTime</c> of the end of astronomical twilight
        /// using a zenith of <see cref="ASTRONOMICAL_ZENITH">108°</see>.
        /// If the calculation can't be computed (see explanation on top of thepage),
        /// null will be returned.
        /// </returns>
        /// <seealso cref="ASTRONOMICAL_ZENITH"/>
        public virtual DateTime? GetEndAstronomicalTwilight()
        {
            return GetSunsetOffsetByDegrees(ASTRONOMICAL_ZENITH);
        }

        /// <summary>
        /// Utility method that returns a date offset by the offset time passed in.
        /// This method casts the offset as a <code>long</code> and calls
        /// <see cref="GetTimeOffset(System.DateTime,long)"/>.
        /// </summary>
        /// <param name="time">the start time</param>
        /// <param name="offset">the offset in milliseconds to add to the time</param>
        /// <returns>
        /// the <see cref="DateTime"/>with the offset added to it
        /// </returns>
        public virtual DateTime? GetTimeOffset(DateTime time, double offset)
        {
            return GetTimeOffset(time, (long)offset);
        }

        protected virtual DateTime? GetTimeOffset(DateTime? time, double offset)
        {
            return GetTimeOffset(time, (long)offset);
        }

        /// <summary>
        /// A utility method to return a date offset by the offset time passed in.
        /// </summary>
        /// <param name="time">the start time</param>
        /// <param name="offset">the offset in milliseconds to add to the time.</param>
        /// <returns>
        /// the <see cref="DateTime"/> with the offset in milliseconds added
        /// to it
        /// </returns>
        public virtual DateTime? GetTimeOffset(DateTime time, long offset)
        {
            if (offset == long.MinValue)
                return null;

            return time.AddMilliseconds(offset);
        }

        protected virtual DateTime? GetTimeOffset(DateTime? time, long offset)
        {
            if (time == null)
                return null;

            return GetTimeOffset(time.Value, offset);
        }

        /// <summary>
        /// A utility method to return the time of an offset by degrees below or
        /// above the horizon of <see cref="GetSunrise">sunrise</see>.
        /// </summary>
        /// <param name="offsetZenith">the degrees before <see cref="GetSunrise"/> to use in the
        /// calculation. For time after sunrise use negative numbers.</param>
        /// <returns>
        /// The <seealso cref="DateTime"/> of the offset after (or before)
        /// <see cref="GetSunrise"/>.
        /// If the calculation can't be computed such as in the Arctic
        /// Circle where there is at least one day a year where the sun does
        /// not rise, and one where it does not set, a null will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        public virtual DateTime? GetSunriseOffsetByDegrees(double offsetZenith)
        {
            double alos = GetUtcSunrise(offsetZenith);
            if (double.IsNaN(alos))
                return null;

            return GetDateFromTime(alos);
        }

        ///<summary>
        ///  A utility method to return the time of an offset by degrees below or
        ///  above the horizon of <see cref = "GetSunset">sunset</see>.
        ///</summary>
        ///<param name = "offsetZenith">
        ///  the degrees after <see cref = "GetSunset" /> to use in the
        ///  calculation. For time before sunset use negative numbers. </param>
        ///<returns> The <seealso cref = "DateTime" />of the offset after (or before)
        ///  <see cref = "GetSunset" />.
        /// If the calculation can't be computed such as in the Arctic Circle where
        /// there is at least one day a year where the sun does not rise, and
        /// one where it does not set, <see cref="Double.NaN"/> will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        public virtual DateTime? GetSunsetOffsetByDegrees(double offsetZenith)
        {
            double sunset = GetUtcSunset(offsetZenith);
            if (double.IsNaN(sunset))
                return null;

            return GetAdjustedSunsetDate(GetDateFromTime(sunset), GetSunriseOffsetByDegrees(offsetZenith));
        }

        /// <summary>
        /// Method that returns the sunrise in UTC time without correction for time
        /// zone offset from GMT and without using daylight savings time.
        /// </summary>
        /// <param name="zenith">the degrees below the horizon. For time after sunrise use
        /// negative numbers.</param>
        /// <returns>
        /// The time in the format: 18.75 for 18:45:00 UTC/GMT. 
        /// If the calculation can't be computed such as in the Arctic Circle where
        /// there is at least one day a year where the sun does not rise, and
        /// one where it does not set, <see cref="Double.NaN"/> will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        public virtual double GetUtcSunrise(double zenith)
        {
            return AstronomicalCalculator.GetUtcSunrise(DateWithLocation, zenith, true);
        }

        /// <summary>
        /// Method that returns the sunrise in UTC time without correction for time
        /// zone offset from GMT and without using daylight savings time. Non-sunrise
        /// and sunset calculations such as dawn and dusk, depend on the amount of
        /// visible light, something that is not affected by elevation. This method
        /// returns UTC sunrise calculated at sea level. This forms the base for dawn
        /// calculations that are calculated as a dip below the horizon before
        /// sunrise.
        /// </summary>
        /// <param name="zenith">the degrees below the horizon. For time after sunrise use
        /// negative numbers.</param>
        /// <returns>
        /// The time in the format: 18.75 for 18:45:00 UTC/GMT.
        /// If the calculation can't be computed such as in the Arctic Circle where
        /// there is at least one day a year where the sun does not rise, and
        /// one where it does not set, <see cref="Double.NaN"/> will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        /// <seealso cref="GetUtcSunrise"/>
        /// <seealso cref="GetUtcSeaLevelSunset"/>
        public virtual double GetUtcSeaLevelSunrise(double zenith)
        {
            return AstronomicalCalculator.GetUtcSunrise(DateWithLocation, zenith, false);
        }

        /// <summary>
        /// Method that returns the sunset in UTC time without correction for time
        /// zone offset from GMT and without using daylight savings time.
        /// </summary>
        /// <param name="zenith">the degrees below the horizon. For time after before sunset
        /// use negative numbers.</param>
        /// <returns>
        /// The time in the format: 18.75 for 18:45:00 UTC/GMT.
        /// If the calculation can't be computed such as in the Arctic Circle where
        /// there is at least one day a year where the sun does not rise, and
        /// one where it does not set, <see cref="Double.NaN"/> will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        /// <seealso cref="GetUtcSeaLevelSunset"/>
        public virtual double GetUtcSunset(double zenith)
        {
            return AstronomicalCalculator.GetUtcSunset(DateWithLocation, zenith, true);
        }

        /// <summary>
        /// Method that returns the sunset in UTC time without correction for
        /// elevation, time zone offset from GMT and without using daylight savings
        /// time. Non-sunrise and sunset calculations such as dawn and dusk, depend
        /// on the amount of visible light, something that is not affected by
        /// elevation. This method returns UTC sunset calculated at sea level. This
        /// forms the base for dusk calculations that are calculated as a dip below
        /// the horizon after sunset.
        /// </summary>
        /// <param name="zenith">the degrees below the horizon. For time before sunset use
        /// negative numbers.</param>
        /// <returns>
        /// The time in the format: 18.75 for 18:45:00 UTC/GMT.
        /// If the calculation can't be computed such as in the Arctic Circle where
        /// there is at least one day a year where the sun does not rise, and
        /// one where it does not set, <see cref="Double.NaN"/> will be returned.
        /// See detailed explanation on top of the page.
        /// </returns>
        /// <seealso cref="GetUtcSunset"/>
        /// <seealso cref="GetUtcSeaLevelSunrise"/>
        public virtual double GetUtcSeaLevelSunset(double zenith)
        {
            return AstronomicalCalculator.GetUtcSunset(DateWithLocation, zenith, false);
        }

        /// <summary>
        /// Method to return a temporal (solar) hour. The day from sunrise to sunset
        /// is split into 12 equal parts with each one being a temporal hour.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a temporal hour. If
        /// the calculation can't be computed  (see explanation on top of the page) <see cref="long.MinValue"/>
        /// will be returned.
        /// </returns>
        public virtual long GetTemporalHour()
        {
            return GetTemporalHour(GetSunrise().Value, GetSunset().Value);
        }

        /// <summary>
        /// Utility method that will allow the calculation of a temporal (solar) hour
        /// based on the sunrise and sunset passed to this method.
        /// </summary>
        /// <param name="sunrise">The start of the day.</param>
        /// <param name="sunset">The end of the day.</param>
        /// <returns>
        /// the <code>long</code> millisecond length of the temporal hour.
        /// If the calculation can't be computed (see explanation on top of the page)
        /// <see cref="long.MinValue"/> will be returned.
        /// </returns>
        /// <seealso cref="GetTemporalHour()"/>
        public virtual long GetTemporalHour(DateTime sunrise, DateTime sunset)
        {
            //Note: I don't think we need this.
            if (sunrise == DateTime.MinValue || sunset == DateTime.MinValue)
                return long.MinValue;

            return (long)((sunset - sunrise).TotalMilliseconds / 12);
        }

        protected virtual long GetTemporalHour(DateTime? sunrise, DateTime? sunset)
        {
            if (sunrise == null || sunset == null)
            {
                return long.MinValue;
            }
            return (long)((sunset - sunrise).Value.TotalMilliseconds / 12);
        }

        /// <summary>
        /// A method that returns sundial or solar noon. It occurs when the Sun is
        /// <a href="http://en.wikipedia.org/wiki/Transit_%28astronomy%29">transitting</a> the 
        /// <a href="http://en.wikipedia.org/wiki/Meridian_%28astronomy%29">celestial meridian</a>.
        /// In this class it is calculated as halfway between sea level sunrise and sea level sunset,
        /// which can be slightly off the real transit
        /// time due to changes in declination (the lengthening or shortening day).
        /// </summary>
        /// <returns> the <code>Date</code> representing Sun's transit. If the calculation can't be computed such as in the
        ///         Arctic Circle where there is at least one day a year where the sun does not rise, and one where it does
        ///         not set, null will be returned. See detailed explanation on top of the page. </returns>
        /// <seealso cref="GetSunTransit"/>
        /// <seealso cref="GetTemporalHour"/>
        public virtual DateTime? GetSunTransit()
        {
            return GetSunTransit(GetSeaLevelSunrise(), GetSeaLevelSunset());
        }

        /// <summary>
        /// A method that returns sundial or solar noon. It occurs when the Sun is 
        /// <a href="http://en.wikipedia.org/wiki/Transit_%28astronomy%29">transitting</a>
        /// the <a href="http://en.wikipedia.org/wiki/Meridian_%28astronomy%29">celestial meridian</a>. In this class it is
        /// calculated as halfway between the sunrise and sunset passed to this method. This time can be slightly off the
        /// real transit time due to changes in declination (the lengthening or shortening day).
        /// </summary>
        /// <param name="startOfDay">
        ///            the start of day for calculating the sun's transit. This can be sea level sunrise, visual sunrise (or
        ///            any arbitrary start of day) passed to this method. </param>
        /// <param name="endOfDay">
        ///            the end of day for calculating the sun's transit. This can be sea level sunset, visual sunset (or any
        ///            arbitrary end of day) passed to this method.
        /// </param>
        /// <returns> the <code>Date</code> representing Sun's transit. If the calculation can't be computed such as in the
        ///         Arctic Circle where there is at least one day a year where the sun does not rise, and one where it does
        ///         not set, null will be returned. See detailed explanation on top of the page. </returns>
        public virtual DateTime? GetSunTransit(DateTime? startOfDay, DateTime? endOfDay)
        {
            long temporalHour = GetTemporalHour(startOfDay, endOfDay);
            return GetTimeOffset(startOfDay, temporalHour * 6);
        }

        ///<summary>
        ///  A method that returns a <c>DateTime</c> from the time passed in
        ///</summary>
        ///<param name = "time">
        ///  The time to be set as the time for the <c>DateTime</c>.
        ///  The time expected is in the format: 18.75 for 6:45:00 PM </param>
        ///<returns> The Date. </returns>
        protected internal virtual DateTime? GetDateFromTime(double time)
        {
            if (double.IsNaN(time))
                return null;

            time = (time + 240) % 24; // the calculators sometimes return a double
            // that is negative or slightly greater than 24

            var hours = (int)time; // cut off minutes

            time -= hours;
            var minutes = (int)(time *= 60);
            time -= minutes;
            var seconds = (int)(time *= 60);
            time -= seconds; // milliseconds

            var date = DateWithLocation.Date;

            var utcDateTime = new DateTime(
                date.Year, date.Month, date.Day,
                hours, minutes, seconds, (int)(time * 1000),
                DateTimeKind.Unspecified);

            long localOffset = DateWithLocation.Location.TimeZone.UtcOffset(utcDateTime);
            return utcDateTime.AddMilliseconds(localOffset);
        }

        /// <summary>
        /// Will return the dip below the horizon before sunrise that matches the
        /// offset minutes on passed in. For example passing in 72 minutes for a
        /// calendar set to the equinox in Jerusalem returns a value close to
        /// 16.1°
        /// Please note that this method is very slow and inefficient and should NEVER be used in a loop.
        /// <em><b>TODO:</b></em> Improve efficiency.
        /// </summary>
        /// <param name="minutes">offset</param>
        /// <returns>
        /// the degrees below the horizon that match the offset on the
        /// equinox in Jerusalem at sea level.
        /// </returns>
        /// <seealso cref="GetSunsetSolarDipFromOffset"/>
        public virtual double GetSunriseSolarDipFromOffset(double minutes)
        {
            var offsetByDegrees = GetSeaLevelSunrise();
            var offsetByTime = GetTimeOffset(GetSeaLevelSunrise().Value, -(minutes * MINUTE_MILLIS));

            var degrees = 0m;
            var incrementor = 0.0001m;
            while (offsetByDegrees == null || offsetByDegrees.Value.ToUnixEpochMilliseconds() > offsetByTime.Value.ToUnixEpochMilliseconds())
            {
                degrees += incrementor;
                offsetByDegrees = GetSunriseOffsetByDegrees(GEOMETRIC_ZENITH + (double)degrees);
            }
            return (double)degrees;
        }

        /// <summary>
        /// Will return the dip below the horizon after sunset that matches the
        /// offset minutes on passed in. For example passing in 72 minutes for a
        /// calendar set to the equinox in Jerusalem returns a value close to
        /// 16.1°
        /// Please note that this method is very slow and inefficient and should NEVER be used in a loop.
        /// <em><b>TODO:</b></em> Improve efficiency.
        /// </summary>
        /// <param name="minutes">offset</param>
        /// <returns>
        /// the degrees below the horizon that match the offset on the
        /// equinox in Jerusalem at sea level.
        /// </returns>
        /// <seealso cref="GetSunriseSolarDipFromOffset"/>
        public virtual double GetSunsetSolarDipFromOffset(double minutes)
        {
            var offsetByDegrees = GetSeaLevelSunset();
            var offsetByTime = GetTimeOffset(GetSeaLevelSunset().Value, minutes * MINUTE_MILLIS);

            var degrees = 0m;
            var incrementor = 0.001m;
            while (offsetByDegrees == null || offsetByDegrees.Value.ToUnixEpochMilliseconds() < offsetByTime.Value.ToUnixEpochMilliseconds())
            {
                degrees += incrementor;
                offsetByDegrees = GetSunsetOffsetByDegrees(GEOMETRIC_ZENITH + (double)degrees);
            }
            return (double)degrees;
        }

        ///<returns> an XML formatted representation of the class. It returns the
        ///  default output of the
        ///  <see cref = "ZmanimFormatter.ToXml">toXML</see>
        ///  method. </returns>
        ///<seealso cref = "ZmanimFormatter.ToXml" />
        public override string ToString()
        {
            return ZmanimFormatter.ToXml(this);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(obj is AstronomicalCalendar))
                return false;
            var aCal = (AstronomicalCalendar)obj;
            return DateWithLocation.Equals(aCal.DateWithLocation) && DateWithLocation.Location.Equals(aCal.DateWithLocation.Location) &&
                   AstronomicalCalculator.Equals(aCal.AstronomicalCalculator);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int result = 17;
            result = 37 * result + GetType().GetHashCode(); // needed or this and subclasses will return identical hash
            result += 37 * result + DateWithLocation.GetHashCode();
            result += 37 * result + DateWithLocation.Location.GetHashCode();
            result += 37 * result + AstronomicalCalculator.GetHashCode();
            return result;
        }

        /// <summary>
        /// Gets or Sets the current AstronomicalCalculator set.
        /// </summary>
        /// <value>Returns the astronimicalCalculator.</value>
        public virtual IAstronomicalCalculator AstronomicalCalculator { get; set; }

        /// <summary>
        /// Gets or Sets the Date and Location to be used in the calculations.
        /// </summary>
        /// <value>The calendar to set.</value>
        public virtual IDateWithLocation DateWithLocation { get; set; }
    }
}