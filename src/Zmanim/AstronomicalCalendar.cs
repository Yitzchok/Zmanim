//
// * Zmanim Java API
// * Copyright (C) 2004-2010 Eliyahu Hershfeld
// *
// * Converted to C# by AdminJew
// *
// * This program is free software; you can redistribute it and/or modify it under the terms of the
// * GNU General Public License as published by the Free Software Foundation; either version 2 of the
// * License, or (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// * even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// * General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License along with this program; if
// * not, write to the Free Software Foundation, Inc. 59 Temple Place - Suite 330, Boston, MA
// * 02111-1307, USA or connect to: http://www.fsf.org/copyleft/gpl.html
// 

namespace net.sourceforge.zmanim
{
    using java.math;
    using java.util;
    using net.sourceforge.zmanim.util;
    using System;

    ///
    /// <summary>
    /// A Java calendar that calculates astronomical time calculations such as
    /// <seealso cref="#getSunrise() sunrise"/> and <seealso cref="#getSunset() sunset"/> times. This
    /// class contains a <seealso cref="#getCalendar() Calendar"/> and can therefore use the
    /// standard Calendar functionality to change dates etc. The calculation engine
    /// used to calculate the astronomical times can be changed to a different
    /// implementation by implementing the <seealso cref="AstronomicalCalculator"/> and setting
    /// it with the <seealso cref="#setAstronomicalCalculator(AstronomicalCalculator)"/>. A
    /// number of different implementations are included in the util package <br />
    /// <b>Note:</b> There are times when the algorithms can't calculate proper
    /// values for sunrise and sunset. This is usually caused by trying to calculate
    /// times for areas either very far North or South, where sunrise / sunset never
    /// happen on that date. This is common when calculating twilight with a deep dip
    /// below the horizon for locations as south of the North Pole as London in the
    /// northern hemisphere. When the calculations encounter this condition a null
    /// will be returned when a <code><seealso cref="java.util.Date"/></code> is expected and
    /// <seealso cref="Double#NaN"/> when a double is expected. The reason that
    /// <code>Exception</code>s are not thrown in these cases is because the lack
    /// of a rise/set are not exceptions, but expected in many parts of the world.
    /// 
    /// Here is a simple example of how to use the API to calculate sunrise: <br />
    /// First create the Calendar for the location you would like to calculate:
    /// 
    /// <example>
    /// String locationName = &quot;Lakewood, NJ&quot;
    /// double latitude = 40.0828; //Lakewood, NJ
    /// double longitude = -74.2094; //Lakewood, NJ
    /// double elevation = 20; // optional elevation correction in Meters
    /// //the String parameter in getTimeZone() has to be a valid timezone listed in <seealso cref="java.util.TimeZone#getAvailableIDs()"/>
    /// TimeZone timeZone = TimeZone.getTimeZone(&quot;America/New_York&quot;);
    /// GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    /// 		elevation, timeZone);
    /// AstronomicalCalendar ac = new AstronomicalCalendar(location);
    /// </example>
    /// 
    /// To get the time of sunrise, first set the date (if not set, the date will
    /// default to today):
    /// 
    /// <example>
    /// ac.getCalendar().set(Calendar.MONTH, Calendar.FEBRUARY);
    /// ac.getCalendar().set(Calendar.DAY_OF_MONTH, 8);
    /// Date sunrise = ac.getSunrise();
    /// </example>
    /// 
    /// 
    /// @author &copy; Eliyahu Hershfeld 2004 - 2010
    /// @version 1.2 </summary>
    public class AstronomicalCalendar : ICloneable
    {
        private const long serialVersionUID = 1;

        ///	<summary>
        /// 90&deg; below the vertical. Used for certain calculations.<br />
        ///	<b>Note </b>: it is important to note the distinction between this zenith
        ///	and the <seealso cref="AstronomicalCalculator#adjustZenith adjusted zenith"/> used
        ///	for some solar calculations. This 90 zenith is only used because some
        ///	calculations in some subclasses are historically calculated as an offset
        ///	in reference to 90.
        /// </summary>
        public const double GEOMETRIC_ZENITH = 90.0;

        /// <summary>
        /// Sun's zenith at astronomical twilight (108&deg;).
        /// </summary>
        public const double ASTRONOMICAL_ZENITH = 108.0;


        ///	<summary>
        /// Default value for Sun's zenith and true rise/set Zenith (used in this
        ///	class and subclasses) is the angle that the center of the Sun makes to a
        ///	line perpendicular to the Earth's surface. If the Sun were a point and
        ///	the Earth were without an atmosphere, true sunset and sunrise would
        ///	correspond to a 90&deg; zenith. Because the Sun is not a point, and
        ///	because the atmosphere refracts light, this 90&deg; zenith does not, in
        ///	fact, correspond to true sunset or sunrise, instead the center of the
        ///	Sun's disk must lie just below the horizon for the upper edge to be
        ///	obscured. This means that a zenith of just above 90&deg; must be used.
        ///	The Sun subtends an angle of 16 minutes of arc (this can be changed via
        ///	the <seealso cref="#setSunRadius(double)"/> method , and atmospheric refraction
        ///	accounts for 34 minutes or so (this can be changed via the
        ///	<seealso cref="#setRefraction(double)"/> method), giving a total of 50 arcminutes.
        ///	The total value for ZENITH is 90+(5/6) or 90.8333333&deg; for true
        ///	sunrise/sunset.
        /// </summary>
        //public static double ZENITH = GEOMETRIC_ZENITH + 5.0 / 6.0;

        /// <summary>
        /// Sun's zenith at civil twilight (96&deg;).
        /// </summary>
        public const double CIVIL_ZENITH = 96.0;


        /// <summary>
        /// constant for milliseconds in an hour (3,600,000)
        /// </summary>
        internal const long HOUR_MILLIS = 0x36ee80L;

        /// <summary>
        /// constant for milliseconds in a minute (60,000)
        /// </summary>
        internal const long MINUTE_MILLIS = 0xea60L;

        /// <summary>
        /// Sun's zenith at nautical twilight (102&deg;).
        /// </summary>
        public const double NAUTICAL_ZENITH = 102.0;

        /// <summary>
        /// The Java Calendar encapsulated by this class to track the current date
        /// used by the class
        /// </summary>
        private AstronomicalCalculator astronomicalCalculator;
        private Calendar calendar;
        private GeoLocation geoLocation;

        ///	<summary>
        /// Default constructor will set a default <seealso cref="GeoLocation#GeoLocation()"/>,
        ///	a default
        ///	<seealso cref="AstronomicalCalculator#getDefault() AstronomicalCalculator"/> and
        ///	default the calendar to the current date. </summary>
        public AstronomicalCalendar()
            : this(new GeoLocation())
        {
        }

        ///	<summary>
        /// A constructor that takes in as a parameter geolocation information
        ///	 </summary>
        ///	<param name="geoLocation">
        ///	           The location information used for astronomical calculating sun
        ///	           times. </param>
        public AstronomicalCalendar(GeoLocation geoLocation)
        {
            this.setCalendar(Calendar.getInstance(geoLocation.getTimeZone()));
            this.setGeoLocation(geoLocation);
            this.setAstronomicalCalculator(AstronomicalCalculator.getDefault());
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is AstronomicalCalendar))
            {
                return false;
            }
            AstronomicalCalendar calendar = (AstronomicalCalendar)obj;
            return ((this.getCalendar().equals(calendar.getCalendar()) && this.getGeoLocation().Equals(calendar.getGeoLocation())) && java.lang.Object.instancehelper_equals(this.getAstronomicalCalculator(), calendar.getAstronomicalCalculator()));
        }

        ///	<summary>
        /// The getSunrise method Returns a <code>Date</code> representing the
        ///	sunrise time. The zenith used for the calculation uses
        ///	<seealso cref="#GEOMETRIC_ZENITH geometric zenith"/> of 90&deg;. This is adjusted
        ///	by the <seealso cref="AstronomicalCalculator"/> that adds approximately 50/60 of a
        ///	degree to account for 34 archminutes of refraction and 16 archminutes for
        ///	the sun's radius for a total of
        ///	<seealso cref="AstronomicalCalculator#adjustZenith 90.83333&deg;"/>. See
        ///	documentation for the specific implementation of the
        ///	<seealso cref="AstronomicalCalculator"/> that you are using.
        ///	 </summary>
        ///	<returns> the <code>Date</code> representing the exact sunrise time. If
        ///	        the calculation can not be computed null will be returned. </returns>
        ///	<seealso cref= AstronomicalCalculator#adjustZenith </seealso>
        public virtual Date getSunrise()
        {
            double v = this.getUTCSunrise(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

        ///	<summary>
        /// Method that returns the sunrise without correction for elevation.
        ///	Non-sunrise and sunset calculations such as dawn and dusk, depend on the
        ///	amount of visible light, something that is not affected by elevation.
        ///	This method returns sunrise calculated at sea level. This forms the base
        ///	for dawn calculations that are calculated as a dip below the horizon
        ///	before sunrise.
        ///	 </summary>
        ///	<returns> the <code>Date</code> representing the exact sea-level sunrise
        ///	        time. If the calculation can not be computed null will be
        ///	        returned. </returns>
        ///	<seealso cref="AstronomicalCalendar#getSunrise"/>
        ///	<seealso cref="stronomicalCalendar#getUTCSeaLevelSunrise"/>
        public virtual Date getSeaLevelSunrise()
        {
            double v = this.getUTCSeaLevelSunrise(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

        ///	<summary>
        /// A method to return the the beginning of civil twilight (dawn) using a
        ///	zenith of <seealso cref="#CIVIL_ZENITH 96&deg;"/>.
        ///	</summary>
        ///	<returns> The <code>Date</code> of the beginning of civil twilight using
        ///	        a zenith of 96&deg;. If the calculation can not be computed null
        ///	        will be returned. </returns>
        ///	<seealso cref= #CIVIL_ZENITH </seealso>
        public virtual Date getBeginCivilTwilight()
        {
            return this.getSunriseOffsetByDegrees(96.0);
        }

        ///	<summary>
        /// A method to return the the beginning of nautical twilight using a zenith
        ///	of <seealso cref="#NAUTICAL_ZENITH 102&deg;"/>.
        ///	 </summary>
        ///	<returns> The <code>Date</code> of the beginning of nautical twilight
        ///	        using a zenith of 102&deg;. If the calculation can not be
        ///	        computed null will be returned. </returns>
        ///	<seealso cref= #NAUTICAL_ZENITH </seealso>
        public virtual Date getBeginNauticalTwilight()
        {
            return this.getSunriseOffsetByDegrees(102.0);
        }

        ///	<summary>
        /// A method that returns the the beginning of astronomical twilight using a
        ///	zenith of <seealso cref="#ASTRONOMICAL_ZENITH 108&deg;"/>.
        ///	 </summary>
        ///	<returns> The <code>Date</code> of the beginning of astronomical twilight
        ///	        using a zenith of 108&deg;. If the calculation can not be
        ///	        computed null will be returned. </returns>
        public virtual Date getBeginAstronomicalTwilight()
        {
            return this.getSunriseOffsetByDegrees(108.0);
        }

        ///	<summary>
        /// The getSunset method Returns a <code>Date</code> representing the
        ///	sunset time. The zenith used for the calculation uses
        ///	<seealso cref="#GEOMETRIC_ZENITH geometric zenith"/> of 90&deg;. This is adjusted
        ///	by the <seealso cref="AstronomicalCalculator"/> that adds approximately 50/60 of a
        ///	degree to account for 34 archminutes of refraction and 16 archminutes for
        ///	the sun's radius for a total of
        ///	<seealso cref="AstronomicalCalculator#adjustZenith 90.83333&deg;"/>. See
        ///	documentation for the specific implementation of the
        ///	<seealso cref="AstronomicalCalculator"/> that you are using. Note: In certain cases
        ///	the calculates sunset will occur before sunrise. This will typically
        ///	happen when a timezone other than the local timezone is used (calculating
        ///	Los Angeles sunset using a GMT timezone for example). In this case the
        ///	sunset date will be incremented to the following date.
        ///	 </summary>
        ///	<returns> the <code>Date</code> representing the exact sunset time. If
        ///	        the calculation can not be computed null will be returned. If the
        ///	        time calculation </returns>
        public virtual Date getSunset()
        {
            double v = this.getUTCSunset(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSunrise());
        }

        ///	<summary>
        /// A method that will roll the sunset time forward a day if sunset occurs
        ///	before sunrise. This will typically happen when a timezone other than the
        ///	local timezone is used (calculating Los Angeles sunset using a GMT
        ///	timezone for example). In this case the sunset date will be incremented
        ///	to the following date.
        ///	 </summary>
        ///	<param name="sunset">
        ///	           the sunset date to adjust if needed </param>
        ///	<param name="sunrise">
        ///	           the sunrise to compare to the sunset </param>
        ///	<returns> the adjusted sunset date </returns>
        private Date getAdjustedSunsetDate(Date date1, Date date2)
        {
            if (((date1 != null) && (date2 != null)) && (date2.compareTo(date1) >= 0))
            {
                GregorianCalendar calendar = (GregorianCalendar)this.getCalendar().clone();
                calendar.setTime(date1);
                calendar.add(5, 1);
                return calendar.getTime();
            }
            return date1;
        }

        ///	<summary>
        /// Method that returns the sunset without correction for elevation.
        ///	Non-sunrise and sunset calculations such as dawn and dusk, depend on the
        ///	amount of visible light, something that is not affected by elevation.
        ///	This method returns sunset calculated at sea level. This forms the base
        ///	for dusk calculations that are calculated as a dip below the horizon
        ///	after sunset.
        ///	 </summary>
        ///	<returns> the <code>Date</code> representing the exact sea-level sunset
        ///	        time. If the calculation can not be computed null will be
        ///	        returned. </returns>
        ///	<seealso cref= AstronomicalCalendar#getSunset </seealso>
        ///	<seealso cref= AstronomicalCalendar#getUTCSeaLevelSunset </seealso>
        public virtual Date getSeaLevelSunset()
        {
            double v = this.getUTCSeaLevelSunset(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSeaLevelSunrise());
        }

        ///	<summary>
        /// A method to return the the end of civil twilight using a zenith of
        ///	<seealso cref="#CIVIL_ZENITH 96&deg;"/>.
        ///	 </summary>
        ///	<returns> The <code>Date</code> of the end of civil twilight using a
        ///	        zenith of <seealso cref="#CIVIL_ZENITH 96&deg;"/>. If the calculation can
        ///	        not be computed null will be returned. </returns>
        ///	<seealso cref= #CIVIL_ZENITH </seealso>
        public virtual Date getEndCivilTwilight()
        {
            return this.getSunsetOffsetByDegrees(96.0);
        }

        ///	<summary>
        /// A method to return the the end of nautical twilight using a zenith of
        ///	<seealso cref="#NAUTICAL_ZENITH 102&deg;"/>.
        ///	 </summary>
        ///	<returns> The <code>Date</code> of the end of nautical twilight using a
        ///	        zenith of <seealso cref="#NAUTICAL_ZENITH 102&deg;"/>. If the calculation
        ///	        can not be computed null will be returned. </returns>
        ///	<seealso cref= #NAUTICAL_ZENITH </seealso>
        public virtual Date getEndNauticalTwilight()
        {
            return this.getSunsetOffsetByDegrees(102.0);
        }

        ///	<summary>
        /// A method to return the the end of astronomical twilight using a zenith of
        ///	<seealso cref="#ASTRONOMICAL_ZENITH 108&deg;"/>.
        ///	 </summary>
        ///	<returns> The The <code>Date</code> of the end of astronomical twilight
        ///	        using a zenith of <seealso cref="#ASTRONOMICAL_ZENITH 108&deg;"/>. If the
        ///	        calculation can not be computed null will be returned. </returns>
        ///	<seealso cref= #ASTRONOMICAL_ZENITH </seealso>
        public virtual Date getEndAstronomicalTwilight()
        {
            return this.getSunsetOffsetByDegrees(108.0);
        }

        ///	<summary>
        /// Utility method that returns a date offset by the offset time passed in.
        ///	This method casts the offset as a <code>long</code> and calls
        ///	<seealso cref="#getTimeOffset(Date, long)"/>.
        ///	 </summary>
        ///	<param name="time">
        ///	           the start time </param>
        ///	<param name="offset">
        ///	           the offset in milliseconds to add to the time </param>
        ///	<returns> the <seealso cref="java.util.Date"/>with the offset added to it </returns>
        public virtual Date getTimeOffset(Date time, double offset)
        {
            return this.getTimeOffset(time, (long)offset);
        }

        ///	<summary>
        /// A utility method to return a date offset by the offset time passed in.
        ///	 </summary>
        ///	<param name="time">
        ///	           the start time </param>
        ///	<param name="offset">
        ///	           the offset in milliseconds to add to the time. </param>
        ///	<returns> the <seealso cref="java.util.Date"/> with the offset in milliseconds added
        ///	        to it </returns>
        public virtual Date getTimeOffset(Date time, long offset)
        {
            if ((time == null) || (offset == -9223372036854775808L))
            {
                return null;
            }
            return new Date(time.getTime() + offset);
        }

        ///	<summary> 
        /// A method that returns the currently set <seealso cref="GeoLocation"/> that contains
        ///	location information used for the astronomical calculations.
        ///	</summary>
        ///	<returns> Returns the geoLocation. </returns>
        public virtual GeoLocation getGeoLocation()
        {
            return this.geoLocation;
        }

        ///	<summary>
        /// A method that adds time zone offset and daylight savings time to the raw
        ///	UTC time.
        ///	 </summary>
        ///	<param name="time">
        ///	           The UTC time to be adjusted. </param>
        ///	<returns> The time adjusted for the time zone offset and daylight savings
        ///	        time. </returns>
        private double getOffsetTime(double num1)
        {
            bool isInDaylightTime = this.getCalendar().getTimeZone().inDaylightTime(this.getCalendar().getTime());
            double num2 = 0f;
            double num3 = ((long)this.getCalendar().getTimeZone().getRawOffset()) / 0x36ee80L;
            if (isInDaylightTime)
            {
                num2 = ((long)this.getCalendar().getTimeZone().getDSTSavings()) / 0x36ee80L;
            }
            return ((num1 + num3) + num2);
        }

        ///	<summary>
        /// A method to return the current AstronomicalCalculator set.
        ///	 </summary>
        ///	<returns> Returns the astronimicalCalculator. </returns>
        ///	<seealso cref= #setAstronomicalCalculator(AstronomicalCalculator) </seealso>
        public virtual AstronomicalCalculator  getAstronomicalCalculator()
        {
            return this.astronomicalCalculator;
        }

        ///	<summary>
        /// returns the Calendar object encapsulated in this class.
        ///	 </summary>
        ///	<returns> Returns the calendar. </returns>
        public virtual Calendar getCalendar()
        {
            return this.calendar;
        }

        ///	<summary>
        /// A method that returns a <code>Date</code> from the time passed in
        ///	 </summary>
        ///	<param name="time">
        ///	           The time to be set as the time for the <code>Date</code>.
        ///	           The time expected is in the format: 18.75 for 6:45:00 PM </param>
        ///	<returns> The Date. </returns>
        protected internal virtual Date getDateFromTime(double time)
        {
            if (java.lang.Double.isNaN(time))
            {
                return null;
            }

            time = getOffsetTime(time);
            time = (time + 240) % 24; // the calculators sometimes return a double
            // that is negative or slightly greater than 24
            Calendar cal = new GregorianCalendar();
            cal.clear();
            cal.set(Calendar.YEAR, getCalendar().get(Calendar.YEAR));
            cal.set(Calendar.MONTH, getCalendar().get(Calendar.MONTH));
            cal.set(Calendar.DAY_OF_MONTH, getCalendar().get(Calendar.DAY_OF_MONTH));

            int hours = (int)time; // cut off minutes

            time -= hours;
            int minutes = (int)(time *= 60);
            time -= minutes;
            int seconds = (int)(time *= 60);
            time -= seconds; // milliseconds

            cal.set(Calendar.HOUR_OF_DAY, hours);
            cal.set(Calendar.MINUTE, minutes);
            cal.set(Calendar.SECOND, seconds);
            cal.set(Calendar.MILLISECOND, (int)(time * 1000));
            return cal.getTime();
        }

        ///	<summary>
        /// A utility method to return the time of an offset by degrees below or
        ///	above the horizon of <seealso cref="#getSunrise() sunrise"/>.
        ///	 </summary>
        ///	<param name="offsetZenith">
        ///	           the degrees before <seealso cref="#getSunrise()"/> to use in the
        ///	           calculation. For time after sunrise use negative numbers. </param>
        ///	<returns> The <seealso cref="java.util.Date"/> of the offset after (or before)
        ///	        <seealso cref="#getSunrise()"/>. If the calculation can not be computed
        ///	        null will be returned. </returns>
        public virtual Date getSunriseOffsetByDegrees(double offsetZenith)
        {
            double v = this.getUTCSunrise(offsetZenith);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

        ///	<summary>
        /// Will return the dip below the horizon before sunrise that matches the
        ///	offset minutes on passed in. For example passing in 72 minutes for a
        ///	calendar set to the equinox in Jerusalem returns a value close to
        ///	16.1&deg;
        ///	Please note that this method is very slow and inefficient and should NEVER be used in a loop.
        ///	TODO: Improve efficiency. </summary>
        ///	<param name="minutes">offset </param>
        ///	<returns> the degrees below the horizon that match the offset on the
        ///	        equinox in Jerusalem at sea level. </returns>
        public virtual double getSunriseSolarDipFromOffset(double minutes)
        {
            Date offsetByDegrees = this.getSeaLevelSunrise();
            Date offsetByTime = this.getTimeOffset(this.getSeaLevelSunrise(), -(minutes * 60000.0));

            var degrees = new BigDecimal(0);
            var incrementor = new BigDecimal("0.0001");
            while (offsetByDegrees == null || offsetByDegrees.getTime() > offsetByTime.getTime())
            {
                degrees = degrees.add(incrementor);
                offsetByDegrees = getSunriseOffsetByDegrees(GEOMETRIC_ZENITH + degrees.doubleValue());
            }
            return degrees.doubleValue();
        }

        ///	<summary>
        /// A utility method to return the time of an offset by degrees below or
        ///	above the horizon of <seealso cref="#getSunset() sunset"/>.
        ///	 </summary>
        ///	<param name="offsetZenith">
        ///	           the degrees after <seealso cref="#getSunset()"/> to use in the
        ///	           calculation. For time before sunset use negative numbers. </param>
        ///	<returns> The <seealso cref="java.util.Date"/>of the offset after (or before)
        ///	        <seealso cref="#getSunset()"/>. If the calculation can not be computed
        ///	        null will be returned. </returns>
        public virtual Date getSunsetOffsetByDegrees(double offsetZenith)
        {
            double v = this.getUTCSunset(offsetZenith);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSunriseOffsetByDegrees(offsetZenith));
        }

        ///	<summary>
        /// Will return the dip below the horizon after sunset that matches the
        ///	offset minutes on passed in. For example passing in 72 minutes for a
        ///	calendar set to the equinox in Jerusalem returns a value close to
        ///	16.1&deg;
        ///	Please note that this method is very slow and inefficient and should NEVER be used in a loop.
        ///	<em><b>TODO:</b></em> Improve efficiency. </summary>
        ///	<param name="minutes"> offset </param>
        ///	<returns> the degrees below the horizon that match the offset on the
        ///	        equinox in Jerusalem at sea level. </returns>
        ///	<seealso cref= #getSunriseSolarDipFromOffset(double) </seealso>
        public virtual double getSunsetSolarDipFromOffset(double minutes)
        {
            Date offsetByDegrees = this.getSeaLevelSunset();
            Date offsetByTime = this.getTimeOffset(this.getSeaLevelSunset(), (double)(minutes * 60000.0));
            BigDecimal degrees = new BigDecimal(0);
            BigDecimal incrementor = new BigDecimal("0.0001");

            while (offsetByDegrees == null || offsetByDegrees.getTime() < offsetByTime.getTime())
            {
                degrees = degrees.add(incrementor);
                offsetByDegrees = getSunsetOffsetByDegrees(GEOMETRIC_ZENITH + degrees.doubleValue());
            }
            return degrees.doubleValue();
        }

        ///	<summary>
        /// A method that returns sundial or solar noon. It occurs when the Sun is 
        /// <a href="http://en.wikipedia.org/wiki/Transit_%28astronomy%29">transitting</a>
        ///	the <a href="http://en.wikipedia.org/wiki/Meridian_%28astronomy%29">celestial
        ///	meridian</a>. In this class it is calculated as halfway between sunrise
        ///	and sunset, which can be slightly off the real transit time due to the
        ///	lengthening or shortening day.
        ///	 </summary>
        ///	<returns> the <code>Date</code> representing Sun's transit. If the
        ///	        calculation can not be computed null will be returned. </returns>
        public virtual Date getSunTransit()
        {
            return getTimeOffset(getSunrise(), getTemporalHour() * 6);
        }

        ///	<summary>
        /// Method to return a temporal (solar) hour. The day from sunrise to sunset
        ///	is split into 12 equal parts with each one being a temporal hour.
        ///	 </summary>
        ///	<returns> the <code>long</code> millisecond length of a temporal hour. If
        ///	        the calculation can not be computed <seealso cref="Long#MIN_VALUE"/> will
        ///	        be returned. </returns>
        public virtual long getTemporalHour()
        {
            return this.getTemporalHour(this.getSunrise(), this.getSunset());
        }

        ///	<summary>
        /// Utility method that will allow the calculation of a temporal (solar) hour
        ///	based on the sunrise and sunset passed to this method.
        ///	 </summary>
        ///	<param name="sunrise">
        ///	           The start of the day. </param>
        ///	<param name="sunset">
        ///	           The end of the day. </param>
        ///	<seealso cref= #getTemporalHour() </seealso>
        ///	<returns> the <code>long</code> millisecond length of the temporal hour.
        ///	        If the calculation can not be computed <seealso cref="Long#MIN_VALUE"/>
        ///	        will be returned. </returns>
        public virtual long getTemporalHour(Date sunrise, Date sunset)
        {
            if (sunrise == null || sunset == null)
            {
                return long.MinValue;
            }
            return (sunset.getTime() - sunrise.getTime()) / 12;
        }

        ///	<summary>
        /// Method that returns the sunrise in UTC time without correction for time
        ///	zone offset from GMT and without using daylight savings time. Non-sunrise
        ///	and sunset calculations such as dawn and dusk, depend on the amount of
        ///	visible light, something that is not affected by elevation. This method
        ///	returns UTC sunrise calculated at sea level. This forms the base for dawn
        ///	calculations that are calculated as a dip below the horizon before
        ///	sunrise.
        ///	 </summary>
        ///	<param name="zenith">
        ///	           the degrees below the horizon. For time after sunrise use
        ///	           negative numbers. </param>
        ///	<returns> The time in the format: 18.75 for 18:45:00 UTC/GMT. If the
        ///	        calculation can not be computed <seealso cref="Double#NaN"/> will be
        ///	        returned. </returns>
        ///	<seealso cref= AstronomicalCalendar#getUTCSunrise </seealso>
        ///	<seealso cref= AstronomicalCalendar#getUTCSeaLevelSunset </seealso>
        public virtual double getUTCSeaLevelSunrise(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunrise(this, zenith, false);
        }

        ///	<summary>
        /// Method that returns the sunset in UTC time without correction for
        ///	elevation, time zone offset from GMT and without using daylight savings
        ///	time. Non-sunrise and sunset calculations such as dawn and dusk, depend
        ///	on the amount of visible light, something that is not affected by
        ///	elevation. This method returns UTC sunset calculated at sea level. This
        ///	forms the base for dusk calculations that are calculated as a dip below
        ///	the horizon after sunset.
        ///	 </summary>
        ///	<param name="zenith">
        ///	           the degrees below the horizon. For time before sunset use
        ///	           negative numbers. </param>
        ///	<returns> The time in the format: 18.75 for 18:45:00 UTC/GMT. If the
        ///	        calculation can not be computed <seealso cref="Double#NaN"/> will be
        ///	        returned. </returns>
        ///	<seealso cref= AstronomicalCalendar#getUTCSunset </seealso>
        ///	<seealso cref= AstronomicalCalendar#getUTCSeaLevelSunrise </seealso>
        public virtual double getUTCSeaLevelSunset(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunset(this, zenith, false);
        }

        ///	<summary>
        /// Method that returns the sunrise in UTC time without correction for time
        ///	zone offset from GMT and without using daylight savings time.
        ///	 </summary>
        ///	<param name="zenith">
        ///	           the degrees below the horizon. For time after sunrise use
        ///	           negative numbers. </param>
        ///	<returns> The time in the format: 18.75 for 18:45:00 UTC/GMT. If the
        ///	        calculation can not be computed <seealso cref="Double#NaN"/> will be
        ///	        returned. </returns>
        public virtual double getUTCSunrise(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunrise(this, zenith, true);
        }

        ///	<summary>
        /// Method that returns the sunset in UTC time without correction for time
        ///	zone offset from GMT and without using daylight savings time.
        ///	 </summary>
        ///	<param name="zenith">
        ///	           the degrees below the horizon. For time after before sunset
        ///	           use negative numbers. </param>
        ///	<returns> The time in the format: 18.75 for 18:45:00 UTC/GMT. If the
        ///	        calculation can not be computed <seealso cref="Double#NaN"/> will be
        ///	        returned. </returns>
        ///	<seealso cref= AstronomicalCalendar#getUTCSeaLevelSunset </seealso>
        public virtual double getUTCSunset(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunset(this, zenith, true);
        }

        public override int GetHashCode()
        {
            int num = 0x11;
            num = (0x25 * num) + java.lang.Object.instancehelper_hashCode(base.GetType());
            num += (0x25 * num) + this.getCalendar().hashCode();
            num += (0x25 * num) + this.getGeoLocation().GetHashCode();
            return (num + ((0x25 * num) + java.lang.Object.instancehelper_hashCode(this.getAstronomicalCalculator())));
        }

        ///	<summary>
        /// A method to set the <seealso cref="AstronomicalCalculator"/> used for astronomical
        ///	calculations. The Zmanim package ships with a number of different
        ///	implementations of the <code>abstract</code>
        ///	<seealso cref="AstronomicalCalculator"/> based on different algorithms, including
        ///	<seealso cref="net.sourceforge.zmanim.util.SunTimesCalculator one implementation"/>
        ///	based on the <a href = "http://aa.usno.navy.mil/">US Naval Observatory's</a>
        ///	algorithm, and
        ///	<seealso cref="net.sourceforge.zmanim.util.JSuntimeCalculator another"/> based on
        ///	<a href=""http://noaa.gov">NOAA's</a> algorithm. This allows easy
        ///	runtime switching and comparison of different algorithms.
        ///	 </summary>
        ///	<param name="astronomicalCalculator">
        ///	           The astronimicalCalculator to set. </param>
        public virtual void setAstronomicalCalculator(AstronomicalCalculator astronomicalCalculator)
        {
            this.astronomicalCalculator = astronomicalCalculator;
        }

        ///	<param name="calendar">
        ///	           The calendar to set. </param>
        public virtual void setCalendar(Calendar calendar)
        {
            this.calendar = calendar;
            if (this.getGeoLocation() != null)
            {
                this.getCalendar().setTimeZone(this.getGeoLocation().getTimeZone());
            }
        }

        ///	<summary>
        /// Set the <seealso cref="GeoLocation"/> to be used for astronomical calculations.
        ///	 </summary>
        ///	<param name="geoLocation">
        ///	           The geoLocation to set. </param>
        public virtual void setGeoLocation(GeoLocation geoLocation)
        {
            this.geoLocation = geoLocation;
            this.getCalendar().setTimeZone(geoLocation.getTimeZone());
        }

        ///	 <summary>
        /// A method that creates a <a
        ///	href="http://en.wikipedia.org/wiki/Object_copy#Deep_copy">deep copy</a>
        ///	of the object. <br />
        ///	<b>Note:</b> If the <seealso cref="java.util.TimeZone"/> in the cloned
        ///	<seealso cref="net.sourceforge.zmanim.util.GeoLocation"/> will be changed from the
        ///	original, it is critical that
        ///	<seealso cref="net.sourceforge.zmanim.AstronomicalCalendar#getCalendar()"/>.<seealso cref="java.util.Calendar#setTimeZone(TimeZone) setTimeZone(TimeZone)"/>
        ///	be called in order for the AstronomicalCalendar to output times in the
        ///	expected offset after being cloned.
        ///	 </summary>
        ///	<seealso cref= java.lang.Object#clone() @since 1.1 </seealso>
        public object Clone()
        {
            var clonedCalendar = (AstronomicalCalendar)this.MemberwiseClone();
            clonedCalendar.setGeoLocation((GeoLocation)this.getGeoLocation().Clone());
            clonedCalendar.setCalendar((Calendar)this.getCalendar().clone());
            clonedCalendar.setAstronomicalCalculator((AstronomicalCalculator)this.getAstronomicalCalculator().Clone());

            return clonedCalendar;
        }
    }
}