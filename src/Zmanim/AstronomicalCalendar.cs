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
using java.math;
using java.util;
using net.sourceforge.zmanim.util;

namespace net.sourceforge.zmanim
{
    ///<summary>
    ///  A Java calendar that calculates astronomical time calculations such as
    ///  <see cref = "getSunrise()">sunrise</see> and <see cref = "getSunset()">sunset</see> times. This
    ///  class contains a <see cref = "getCalendar()">Calendar</see> and can therefore use the
    ///  standard Calendar functionality to change dates etc. The calculation engine
    ///  used to calculate the astronomical times can be changed to a different
    ///  implementation by implementing the <see cref = "AstronomicalCalculator" /> and setting
    ///  it with the <see cref = "setAstronomicalCalculator(AstronomicalCalculator)" />. A
    ///  number of different implementations are included in the util package <br />
    ///  <b>Note:</b> There are times when the algorithms can't calculate proper
    ///  values for sunrise and sunset. This is usually caused by trying to calculate
    ///  times for areas either very far North or South, where sunrise / sunset never
    ///  happen on that date. This is common when calculating twilight with a deep dip
    ///  below the horizon for locations as south of the North Pole as London in the
    ///  northern hemisphere. When the calculations encounter this condition a null
    ///  will be returned when a <see cref = "java.util.Date" /> is expected and
    ///  <see cref = "double.NaN" /> when a double is expected. The reason that
    ///  <c>Exception</c>s are not thrown in these cases is because the lack
    ///  of a rise/set are not exceptions, but expected in many parts of the world.
    /// 
    ///  Here is a simple example of how to use the API to calculate sunrise: <br />
    ///  First create the Calendar for the location you would like to calculate:
    ///  <example>
    ///    <code>
    ///      String locationName = &quot;Lakewood, NJ&quot;
    ///      double latitude = 40.0828; //Lakewood, NJ
    ///      double longitude = -74.2094; //Lakewood, NJ
    ///      double elevation = 20; // optional elevation correction in Meters
    ///      //the String parameter in getTimeZone() has to be a valid timezone listed in <see cref = "java.util.TimeZone.getAvailableIDs()" />
    ///      TimeZone timeZone = TimeZone.getTimeZone(&quot;America/New_York&quot;);
    ///      GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    ///      elevation, timeZone);
    ///      AstronomicalCalendar ac = new AstronomicalCalendar(location);
    ///    </code>
    /// 
    ///    To get the time of sunrise, first set the date (if not set, the date will
    ///    default to today):
    /// 
    ///    <code>
    ///      ac.getCalendar().set(Calendar.MONTH, Calendar.FEBRUARY);
    ///      ac.getCalendar().set(Calendar.DAY_OF_MONTH, 8);
    ///      Date sunrise = ac.getSunrise();
    ///    </code>
    ///  </example>
    ///</summary>
    ///<author>Eliyahu Hershfeld</author>
    public class AstronomicalCalendar : ICloneable
    {
        private const long serialVersionUID = 1;

        ///<summary>
        ///  90° below the vertical. Used for certain calculations.<br />
        ///  <b>Note </b>: it is important to note the distinction between this zenith
        ///  and the <see cref = "AstronomicalCalculator.adjustZenith">adjusted zenith</see> used
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
        internal const long MINUTE_MILLIS = 60*1000;

        /// <summary>
        ///   constant for milliseconds in an hour (3,600,000)
        /// </summary>
        internal const long HOUR_MILLIS = MINUTE_MILLIS*60;

        private AstronomicalCalculator astronomicalCalculator;

        ///<summary>
        ///  The Java Calendar encapsulated by this class to track the current date
        ///  used by the class
        ///</summary>
        private Calendar calendar;

        private GeoLocation geoLocation;

        ///<summary>
        ///  Default constructor will set a default <see cref = "GeoLocation" />,
        ///  a default
        ///  <see cref = "AstronomicalCalculator.getDefault()">AstronomicalCalculator</see> and
        ///  default the calendar to the current date.
        ///</summary>
        public AstronomicalCalendar()
            : this(new GeoLocation())
        {
        }

        ///<summary>
        ///  A constructor that takes in as a parameter geolocation information
        ///</summary>
        ///<param name = "geoLocation">
        ///  The location information used for astronomical calculating sun
        ///  times. </param>
        public AstronomicalCalendar(GeoLocation geoLocation)
        {
            setCalendar(Calendar.getInstance(geoLocation.getTimeZone()));
            setGeoLocation(geoLocation); // duplicate call
            setAstronomicalCalculator(AstronomicalCalculator.getDefault());
        }

        #region ICloneable Members

        ///<summary>
        ///  A method that creates a <a href = "http://en.wikipedia.org/wiki/Object_copy#Deep_copy">deep copy</a>
        ///  of the object. <br />
        ///  <b>Note:</b> If the <seealso cref = "java.util.TimeZone" /> in the cloned
        ///  <see cref = "GeoLocation" /> will be changed from the
        ///  original, it is critical that
        ///  <see cref = "AstronomicalCalendar.getCalendar()" />.<see cref = "java.util.Calendar.setTimeZone(java.util.TimeZone)">setTimeZone(TimeZone)</see>
        ///  be called in order for the AstronomicalCalendar to output times in the
        ///  expected offset after being cloned.
        ///</summary>
        public virtual object Clone()
        {
            var clone = (AstronomicalCalendar) MemberwiseClone();

            clone.setGeoLocation((GeoLocation) getGeoLocation().Clone());
            clone.setCalendar((Calendar) getCalendar().clone());
            clone.setAstronomicalCalculator((AstronomicalCalculator) getAstronomicalCalculator().Clone());
            return clone;
        }

        #endregion

        ///<summary>
        ///  The getSunrise method Returns a <c>Date</c> representing the
        ///  sunrise time. The zenith used for the calculation uses
        ///  <seealso cref = "GEOMETRIC_ZENITH">geometric zenith</seealso> of 90°. This is adjusted
        ///  by the <seealso cref = "AstronomicalCalculator" /> that adds approximately 50/60 of a
        ///  degree to account for 34 archminutes of refraction and 16 archminutes for
        ///  the sun's radius for a total of
        ///  <seealso cref = "AstronomicalCalculator.adjustZenith">90.83333°</seealso>. See
        ///  documentation for the specific implementation of the
        ///  <seealso cref = "AstronomicalCalculator" /> that you are using.
        ///</summary>
        ///<returns> the <c>Date</c> representing the exact sunrise time. If
        ///  the calculation can not be computed null will be returned. </returns>
        ///<seealso cref = "AstronomicalCalculator.adjustZenith" />
        public virtual Date getSunrise()
        {
            double sunrise = getUTCSunrise(GEOMETRIC_ZENITH);
            if (double.IsNaN(sunrise))
                return null;

            return getDateFromTime(sunrise);
        }

        ///<summary>
        ///  Method that returns the sunrise without correction for elevation.
        ///  Non-sunrise and sunset calculations such as dawn and dusk, depend on the
        ///  amount of visible light, something that is not affected by elevation.
        ///  This method returns sunrise calculated at sea level. This forms the base
        ///  for dawn calculations that are calculated as a dip below the horizon
        ///  before sunrise.
        ///</summary>
        ///<returns> the <c>Date</c> representing the exact sea-level sunrise
        ///  time. If the calculation can not be computed null will be
        ///  returned. </returns>
        ///<seealso cref = "AstronomicalCalendar.getSunrise" />
        ///<seealso cref = "AstronomicalCalendar.getUTCSeaLevelSunrise" />
        public virtual Date getSeaLevelSunrise()
        {
            double sunrise = getUTCSeaLevelSunrise(GEOMETRIC_ZENITH);
            if (double.IsNaN(sunrise))
                return null;

            return getDateFromTime(sunrise);
        }

        ///<summary>
        ///  A method to return the the beginning of civil twilight (dawn) using a
        ///  zenith of <seealso cref = "CIVIL_ZENITH">96°</seealso>.
        ///</summary>
        ///<returns> The <c>Date</c> of the beginning of civil twilight using
        ///  a zenith of 96°. If the calculation can not be computed null
        ///  will be returned. </returns>
        ///<seealso cref = "CIVIL_ZENITH" />
        public virtual Date getBeginCivilTwilight()
        {
            return getSunriseOffsetByDegrees(CIVIL_ZENITH);
        }

        ///<summary>
        ///  A method to return the the beginning of nautical twilight using a zenith
        ///  of <see cref = "NAUTICAL_ZENITH">102°</see>.
        ///</summary>
        ///<returns> The <c>Date</c> of the beginning of nautical twilight
        ///  using a zenith of 102°. If the calculation can not be
        ///  computed null will be returned. </returns>
        ///<seealso cref = "NAUTICAL_ZENITH" />
        public virtual Date getBeginNauticalTwilight()
        {
            return getSunriseOffsetByDegrees(NAUTICAL_ZENITH);
        }

        ///<summary>
        ///  A method that returns the the beginning of astronomical twilight using a
        ///  zenith of <see cref = "ASTRONOMICAL_ZENITH">108°</see>.
        ///</summary>
        ///<returns> The <c>Date</c> of the beginning of astronomical twilight
        ///  using a zenith of 108°. If the calculation can not be
        ///  computed null will be returned. </returns>
        ///<seealso cref = "ASTRONOMICAL_ZENITH" />
        public virtual Date getBeginAstronomicalTwilight()
        {
            return getSunriseOffsetByDegrees(ASTRONOMICAL_ZENITH);
        }

        ///<summary>
        ///  The getSunset method Returns a <c>Date</c> representing the
        ///  sunset time. The zenith used for the calculation uses
        ///  <see cref = "GEOMETRIC_ZENITH">geometric zenith</see> of 90°. This is adjusted
        ///  by the <see cref = "AstronomicalCalculator" /> that adds approximately 50/60 of a
        ///  degree to account for 34 archminutes of refraction and 16 archminutes for
        ///  the sun's radius for a total of
        ///  <see cref = "AstronomicalCalculator.adjustZenith">90.83333°</see>. See
        ///  documentation for the specific implementation of the
        ///  <see cref = "AstronomicalCalculator" /> that you are using. Note: In certain cases
        ///  the calculates sunset will occur before sunrise. This will typically
        ///  happen when a timezone other than the local timezone is used (calculating
        ///  Los Angeles sunset using a GMT timezone for example). In this case the
        ///  sunset date will be incremented to the following date.
        ///</summary>
        ///<returns> the <c>Date</c> representing the exact sunset time. If
        ///  the calculation can not be computed null will be returned. If the
        ///  time calculation </returns>
        ///<seealso cref = "AstronomicalCalculator.adjustZenith" />
        public virtual Date getSunset()
        {
            double sunset = getUTCSunset(GEOMETRIC_ZENITH);
            if (double.IsNaN(sunset))
                return null;

            return getAdjustedSunsetDate(getDateFromTime(sunset), getSunrise());
        }

        ///<summary>
        ///  A method that will roll the sunset time forward a day if sunset occurs
        ///  before sunrise. This will typically happen when a timezone other than the
        ///  local timezone is used (calculating Los Angeles sunset using a GMT
        ///  timezone for example). In this case the sunset date will be incremented
        ///  to the following date.
        ///</summary>
        ///<param name = "sunset">
        ///  the sunset date to adjust if needed </param>
        ///<param name = "sunrise">
        ///  the sunrise to compare to the sunset </param>
        ///<returns> the adjusted sunset date </returns>
        private Date getAdjustedSunsetDate(Date sunset, Date sunrise)
        {
            if (sunset != null && sunrise != null && sunrise.compareTo(sunset) >= 0)
            {
                Calendar clonedCalendar = (GregorianCalendar) getCalendar().clone();
                clonedCalendar.setTime(sunset);
                clonedCalendar.add(Calendar.DAY_OF_MONTH, 1);
                return clonedCalendar.getTime();
            }
            else
            {
                return sunset;
            }
        }

        ///<summary>
        ///  Method that returns the sunset without correction for elevation.
        ///  Non-sunrise and sunset calculations such as dawn and dusk, depend on the
        ///  amount of visible light, something that is not affected by elevation.
        ///  This method returns sunset calculated at sea level. This forms the base
        ///  for dusk calculations that are calculated as a dip below the horizon
        ///  after sunset.
        ///</summary>
        ///<returns> the <c>Date</c> representing the exact sea-level sunset
        ///  time. If the calculation can not be computed null will be
        ///  returned. </returns>
        ///<seealso cref = "AstronomicalCalendar.getSunset" />
        ///<seealso cref = "AstronomicalCalendar.getUTCSeaLevelSunset" />
        public virtual Date getSeaLevelSunset()
        {
            double sunset = getUTCSeaLevelSunset(GEOMETRIC_ZENITH);
            if (double.IsNaN(sunset))
                return null;

            return getAdjustedSunsetDate(getDateFromTime(sunset), getSeaLevelSunrise());
        }

        ///<summary>
        ///  A method to return the the end of civil twilight using a zenith of
        ///  <see cref = "CIVIL_ZENITH">96°</see>.
        ///</summary>
        ///<returns> The <c>Date</c> of the end of civil twilight using a
        ///  zenith of <seealso cref = "CIVIL_ZENITH">96°</seealso>. If the calculation can
        ///  not be computed null will be returned. </returns>
        ///<seealso cref = "CIVIL_ZENITH" />
        public virtual Date getEndCivilTwilight()
        {
            return getSunsetOffsetByDegrees(CIVIL_ZENITH);
        }

        ///<summary>
        ///  A method to return the the end of nautical twilight using a zenith of
        ///  <see cref = "NAUTICAL_ZENITH">102°</see>.
        ///</summary>
        ///<returns> The <c>Date</c> of the end of nautical twilight using a
        ///  zenith of <seealso cref = "NAUTICAL_ZENITH">102°</seealso>. If the calculation
        ///  can not be computed null will be returned. </returns>
        ///<seealso cref = "NAUTICAL_ZENITH" />
        public virtual Date getEndNauticalTwilight()
        {
            return getSunsetOffsetByDegrees(NAUTICAL_ZENITH);
        }

        ///<summary>
        ///  A method to return the the end of astronomical twilight using a zenith of
        ///  <see cref = "ASTRONOMICAL_ZENITH">108°</see>.
        ///</summary>
        ///<returns> The The <c>Date</c> of the end of astronomical twilight
        ///  using a zenith of <see cref = "ASTRONOMICAL_ZENITH">108°</see>. If the
        ///  calculation can not be computed null will be returned. </returns>
        ///<seealso cref = "ASTRONOMICAL_ZENITH" />
        public virtual Date getEndAstronomicalTwilight()
        {
            return getSunsetOffsetByDegrees(ASTRONOMICAL_ZENITH);
        }

        ///<summary>
        ///  Utility method that returns a date offset by the offset time passed in.
        ///  This method casts the offset as a <code>long</code> and calls
        ///  <see cref = "getTimeOffset(Date, long)" />.
        ///</summary>
        ///<param name = "time">
        ///  the start time </param>
        ///<param name = "offset">
        ///  the offset in milliseconds to add to the time </param>
        ///<returns> the <see cref = "java.util.Date" />with the offset added to it </returns>
        public virtual Date getTimeOffset(Date time, double offset)
        {
            return getTimeOffset(time, (long) offset);
        }

        ///<summary>
        ///  A utility method to return a date offset by the offset time passed in.
        ///</summary>
        ///<param name = "time">
        ///  the start time </param>
        ///<param name = "offset">
        ///  the offset in milliseconds to add to the time. </param>
        ///<returns> the <see cref = "java.util.Date" /> with the offset in milliseconds added
        ///  to it </returns>
        public virtual Date getTimeOffset(Date time, long offset)
        {
            if (time == null || offset == long.MinValue)
            {
                return null;
            }
            return new Date(time.getTime() + offset);
        }

        ///<summary>
        ///  A utility method to return the time of an offset by degrees below or
        ///  above the horizon of <see cref = "getSunrise()">sunrise</see>.
        ///</summary>
        ///<param name = "offsetZenith">
        ///  the degrees before <see cref = "getSunrise()" /> to use in the
        ///  calculation. For time after sunrise use negative numbers. </param>
        ///<returns> The <seealso cref = "java.util.Date" /> of the offset after (or before)
        ///  <see cref = "getSunrise()" />. If the calculation can not be computed
        ///  null will be returned. </returns>
        public virtual Date getSunriseOffsetByDegrees(double offsetZenith)
        {
            double alos = getUTCSunrise(offsetZenith);
            if (double.IsNaN(alos))
                return null;

            return getDateFromTime(alos);
        }

        ///<summary>
        ///  A utility method to return the time of an offset by degrees below or
        ///  above the horizon of <see cref = "getSunset()">sunset</see>.
        ///</summary>
        ///<param name = "offsetZenith">
        ///  the degrees after <see cref = "getSunset()" /> to use in the
        ///  calculation. For time before sunset use negative numbers. </param>
        ///<returns> The <seealso cref = "java.util.Date" />of the offset after (or before)
        ///  <see cref = "getSunset()" />. If the calculation can not be computed
        ///  null will be returned. </returns>
        public virtual Date getSunsetOffsetByDegrees(double offsetZenith)
        {
            double sunset = getUTCSunset(offsetZenith);
            if (double.IsNaN(sunset))
                return null;

            return getAdjustedSunsetDate(getDateFromTime(sunset), getSunriseOffsetByDegrees(offsetZenith));
        }

        ///<summary>
        ///  Method that returns the sunrise in UTC time without correction for time
        ///  zone offset from GMT and without using daylight savings time.
        ///</summary>
        ///<param name = "zenith">
        ///  the degrees below the horizon. For time after sunrise use
        ///  negative numbers. </param>
        ///<returns> The time in the format: 18.75 for 18:45:00 UTC/GMT. If the
        ///  calculation can not be computed <see cref = "Double.NaN" /> will be
        ///  returned. </returns>
        public virtual double getUTCSunrise(double zenith)
        {
            return getAstronomicalCalculator().getUTCSunrise(this, zenith, true);
        }

        ///<summary>
        ///  Method that returns the sunrise in UTC time without correction for time
        ///  zone offset from GMT and without using daylight savings time. Non-sunrise
        ///  and sunset calculations such as dawn and dusk, depend on the amount of
        ///  visible light, something that is not affected by elevation. This method
        ///  returns UTC sunrise calculated at sea level. This forms the base for dawn
        ///  calculations that are calculated as a dip below the horizon before
        ///  sunrise.
        ///</summary>
        ///<param name = "zenith">
        ///  the degrees below the horizon. For time after sunrise use
        ///  negative numbers. </param>
        ///<returns> The time in the format: 18.75 for 18:45:00 UTC/GMT. If the
        ///  calculation can not be computed <see cref = "Double.NaN" /> will be
        ///  returned. </returns>
        ///<seealso cref = "AstronomicalCalendar.getUTCSunrise" />
        ///<seealso cref = "AstronomicalCalendar.getUTCSeaLevelSunset" />
        public virtual double getUTCSeaLevelSunrise(double zenith)
        {
            return getAstronomicalCalculator().getUTCSunrise(this, zenith, false);
        }

        ///<summary>
        ///  Method that returns the sunset in UTC time without correction for time
        ///  zone offset from GMT and without using daylight savings time.
        ///</summary>
        ///<param name = "zenith">
        ///  the degrees below the horizon. For time after before sunset
        ///  use negative numbers. </param>
        ///<returns> The time in the format: 18.75 for 18:45:00 UTC/GMT. If the
        ///  calculation can not be computed <see cref = "Double.NaN" /> will be
        ///  returned. </returns>
        ///<seealso cref = "AstronomicalCalendar.getUTCSeaLevelSunset" />
        public virtual double getUTCSunset(double zenith)
        {
            return getAstronomicalCalculator().getUTCSunset(this, zenith, true);
        }

        ///<summary>
        ///  Method that returns the sunset in UTC time without correction for
        ///  elevation, time zone offset from GMT and without using daylight savings
        ///  time. Non-sunrise and sunset calculations such as dawn and dusk, depend
        ///  on the amount of visible light, something that is not affected by
        ///  elevation. This method returns UTC sunset calculated at sea level. This
        ///  forms the base for dusk calculations that are calculated as a dip below
        ///  the horizon after sunset.
        ///</summary>
        ///<param name = "zenith">
        ///  the degrees below the horizon. For time before sunset use
        ///  negative numbers. </param>
        ///<returns> The time in the format: 18.75 for 18:45:00 UTC/GMT. If the
        ///  calculation can not be computed <see cref = "Double.NaN" /> will be
        ///  returned. </returns>
        ///<seealso cref = "AstronomicalCalendar.getUTCSunset" />
        ///<seealso cref = "AstronomicalCalendar.getUTCSeaLevelSunrise" />
        public virtual double getUTCSeaLevelSunset(double zenith)
        {
            return getAstronomicalCalculator().getUTCSunset(this, zenith, false);
        }

        ///<summary>
        ///  A method that adds time zone offset and daylight savings time to the raw
        ///  UTC time.
        ///</summary>
        ///<param name = "time">
        ///  The UTC time to be adjusted. </param>
        ///<returns> The time adjusted for the time zone offset and daylight savings
        ///  time. </returns>
        private double getOffsetTime(double time)
        {
            bool dst = getCalendar().getTimeZone().inDaylightTime(getCalendar().getTime());
            double dstOffset = 0;
            // be nice to Newfies and use a double
            double gmtOffset = getCalendar().getTimeZone().getRawOffset()/(60*MINUTE_MILLIS);
            if (dst)
            {
                dstOffset = getCalendar().getTimeZone().getDSTSavings()/(60*MINUTE_MILLIS);
            }
            return time + gmtOffset + dstOffset;
        }

        ///<summary>
        ///  Method to return a temporal (solar) hour. The day from sunrise to sunset
        ///  is split into 12 equal parts with each one being a temporal hour.
        ///</summary>
        ///<returns> the <c>long</c> millisecond length of a temporal hour. If
        ///  the calculation can not be computed <see cref = "long.MinValue" />
        ///  will be returned. </returns>
        public virtual long getTemporalHour()
        {
            return getTemporalHour(getSunrise(), getSunset());
        }

        ///<summary>
        ///  Utility method that will allow the calculation of a temporal (solar) hour
        ///  based on the sunrise and sunset passed to this method.
        ///</summary>
        ///<param name = "sunrise">
        ///  The start of the day. </param>
        ///<param name = "sunset">
        ///  The end of the day. </param>
        ///<seealso cref = "getTemporalHour()" />
        ///<returns> the <code>long</code> millisecond length of the temporal hour.
        ///  If the calculation can not be computed <see cref = "long.MinValue" />
        ///  will be returned. </returns>
        public virtual long getTemporalHour(Date sunrise, Date sunset)
        {
            if (sunrise == null || sunset == null)
            {
                return long.MinValue;
            }
            return (sunset.getTime() - sunrise.getTime())/12;
        }

        ///<summary>
        ///  A method that returns sundial or solar noon. It occurs when the Sun is <a href = "http://en.wikipedia.org/wiki/Transit_%28astronomy%29">transitting</a>
        ///  the <a href = "http://en.wikipedia.org/wiki/Meridian_%28astronomy%29">celestial
        ///        meridian</a>. In this class it is calculated as halfway between sunrise
        ///  and sunset, which can be slightly off the real transit time due to the
        ///  lengthening or shortening day.
        ///</summary>
        ///<returns> the <c>Date</c> representing Sun's transit. If the
        ///  calculation can not be computed null will be returned. </returns>
        public virtual Date getSunTransit()
        {
            return getTimeOffset(getSunrise(), getTemporalHour()*6);
        }

        ///<summary>
        ///  A method that returns a <c>Date</c> from the time passed in
        ///</summary>
        ///<param name = "time">
        ///  The time to be set as the time for the <c>Date</c>.
        ///  The time expected is in the format: 18.75 for 6:45:00 PM </param>
        ///<returns> The Date. </returns>
        protected internal virtual Date getDateFromTime(double time)
        {
            if (double.IsNaN(time))
                return null;

            time = getOffsetTime(time);
            time = (time + 240)%24; // the calculators sometimes return a double
            // that is negative or slightly greater than 24
            Calendar cal = new GregorianCalendar();
            cal.clear();
            cal.set(Calendar.YEAR, getCalendar().get(Calendar.YEAR));
            cal.set(Calendar.MONTH, getCalendar().get(Calendar.MONTH));
            cal.set(Calendar.DAY_OF_MONTH, getCalendar().get(Calendar.DAY_OF_MONTH));

            var hours = (int) time; // cut off minutes

            time -= hours;
            var minutes = (int) (time *= 60);
            time -= minutes;
            var seconds = (int) (time *= 60);
            time -= seconds; // milliseconds

            cal.set(Calendar.HOUR_OF_DAY, hours);
            cal.set(Calendar.MINUTE, minutes);
            cal.set(Calendar.SECOND, seconds);
            cal.set(Calendar.MILLISECOND, (int) (time*1000));
            return cal.getTime();
        }

        ///<summary>
        ///  Will return the dip below the horizon before sunrise that matches the
        ///  offset minutes on passed in. For example passing in 72 minutes for a
        ///  calendar set to the equinox in Jerusalem returns a value close to
        ///  16.1°
        ///  Please note that this method is very slow and inefficient and should NEVER be used in a loop.
        ///  TODO: Improve efficiency.
        ///</summary>
        ///<param name = "minutes">
        ///  offset </param>
        ///<returns> the degrees below the horizon that match the offset on the
        ///  equinox in Jerusalem at sea level. </returns>
        public virtual double getSunriseSolarDipFromOffset(double minutes)
        {
            Date offsetByDegrees = getSeaLevelSunrise();
            Date offsetByTime = getTimeOffset(getSeaLevelSunrise(), -(minutes*MINUTE_MILLIS));

            var degrees = new BigDecimal(0);
            var incrementor = new BigDecimal("0.0001");
            while (offsetByDegrees == null || offsetByDegrees.getTime() > offsetByTime.getTime())
            {
                degrees = degrees.add(incrementor);
                offsetByDegrees = getSunriseOffsetByDegrees(GEOMETRIC_ZENITH + degrees.doubleValue());
            }
            return degrees.doubleValue();
        }

        ///<summary>
        ///  Will return the dip below the horizon after sunset that matches the
        ///  offset minutes on passed in. For example passing in 72 minutes for a
        ///  calendar set to the equinox in Jerusalem returns a value close to
        ///  16.1°
        ///  Please note that this method is very slow and inefficient and should NEVER be used in a loop.
        ///  <em><b>TODO:</b></em> Improve efficiency.
        ///</summary>
        ///<param name = "minutes"> offset </param>
        ///<returns> the degrees below the horizon that match the offset on the
        ///  equinox in Jerusalem at sea level. </returns>
        ///<seealso cref = "getSunriseSolarDipFromOffset(double)" />
        public virtual double getSunsetSolarDipFromOffset(double minutes)
        {
            Date offsetByDegrees = getSeaLevelSunset();
            Date offsetByTime = getTimeOffset(getSeaLevelSunset(), minutes*MINUTE_MILLIS);

            var degrees = new BigDecimal(0);
            var incrementor = new BigDecimal("0.0001");
            while (offsetByDegrees == null || offsetByDegrees.getTime() < offsetByTime.getTime())
            {
                degrees = degrees.add(incrementor);
                offsetByDegrees = getSunsetOffsetByDegrees(GEOMETRIC_ZENITH + degrees.doubleValue());
            }
            return degrees.doubleValue();
        }

        ///<returns> an XML formatted representation of the class. It returns the
        ///  default output of the
        ///  <see cref = "ZmanimFormatter.toXML(AstronomicalCalendar)">toXML</see>
        ///  method. </returns>
        ///<seealso cref = "ZmanimFormatter.toXML(AstronomicalCalendar)" />
        public override string ToString()
        {
            return ZmanimFormatter.toXML(this);
        }

        public override bool Equals(object @object)
        {
            if (this == @object)
                return true;
            if (!(@object is AstronomicalCalendar))
                return false;
            var aCal = (AstronomicalCalendar) @object;
            return getCalendar().Equals(aCal.getCalendar()) && getGeoLocation().Equals(aCal.getGeoLocation()) &&
                   getAstronomicalCalculator().Equals(aCal.getAstronomicalCalculator());
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 37*result + GetType().GetHashCode(); // needed or this and subclasses will return identical hash
            result += 37*result + getCalendar().GetHashCode();
            result += 37*result + getGeoLocation().GetHashCode();
            result += 37*result + getAstronomicalCalculator().GetHashCode();
            return result;
        }

        ///<summary>
        ///  A method that returns the currently set <seealso cref = "GeoLocation" /> that contains
        ///  location information used for the astronomical calculations.
        ///</summary>
        ///<returns> Returns the geoLocation. </returns>
        public virtual GeoLocation getGeoLocation()
        {
            return geoLocation;
        }

        ///<summary>
        ///  Set the <seealso cref = "GeoLocation" /> to be used for astronomical calculations.
        ///</summary>
        ///<param name = "geoLocation">
        ///  The geoLocation to set. </param>
        public virtual void setGeoLocation(GeoLocation geoLocation)
        {
            this.geoLocation = geoLocation;
            // if not set the output will be in the original timezone. The call
            // below is also in the constructor
            getCalendar().setTimeZone(geoLocation.getTimeZone());
        }

        ///<summary>
        ///  A method to return the current AstronomicalCalculator set.
        ///</summary>
        ///<returns> Returns the astronimicalCalculator. </returns>
        ///<seealso cref = "setAstronomicalCalculator(AstronomicalCalculator)" />
        public virtual AstronomicalCalculator getAstronomicalCalculator()
        {
            return astronomicalCalculator;
        }

        ///<summary>
        ///  A method to set the <seealso cref = "AstronomicalCalculator" /> used for astronomical
        ///  calculations. The Zmanim package ships with a number of different
        ///  implementations of the <code>abstract</code>
        ///  <see cref = "AstronomicalCalculator" /> based on different algorithms, including
        ///  <see cref = "net.sourceforge.zmanim.util.SunTimesCalculator">one implementation</see>
        ///  based on the <a href = "http://aa.usno.navy.mil/">US Naval Observatory's</a>
        ///  algorithm, and <see cref = "JSuntimeCalculator">another</see> based on
        ///  <a href = "http://noaa.gov">NOAA's</a> algorithm. This allows easy
        ///  runtime switching and comparison of different algorithms.
        ///</summary>
        ///<param name = "astronomicalCalculator">
        ///  The astronimicalCalculator to set. </param>
        public virtual void setAstronomicalCalculator(AstronomicalCalculator astronomicalCalculator)
        {
            this.astronomicalCalculator = astronomicalCalculator;
        }

        ///<summary>
        ///  returns the Calendar object encapsulated in this class.
        ///</summary>
        ///<returns> Returns the calendar. </returns>
        public virtual Calendar getCalendar()
        {
            return calendar;
        }

        /// <summary>
        ///   Set the calender to be used in the calculations.
        /// </summary>
        /// <param name = "calendar">The calendar to set.</param>
        public virtual void setCalendar(Calendar calendar)
        {
            this.calendar = calendar;
            if (getGeoLocation() != null) // set the timezone if possible
            {
                // Always set the Calendar's timezone to match the GeoLocation
                // TimeZone
                getCalendar().setTimeZone(getGeoLocation().getTimeZone());
            }
        }
    }
}