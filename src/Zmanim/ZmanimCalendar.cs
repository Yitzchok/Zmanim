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

using java.util;
using net.sourceforge.zmanim.util;

namespace net.sourceforge.zmanim
{
    /// <summary>
    /// <p> Description: A Java library for calculating zmanim. </p>
    /// The zmanim library is an API is a specialized calendar that can calculate
    /// sunrise and sunset and Jewish <em>zmanim</em> (religious times) for prayers
    /// and other Jewish religious duties. For a much more extensive list of zmanim
    /// use the <seealso cref="ComplexZmanimCalendar"/> that extends this class. This class
    /// contains the main functionality of the Zmanim library. See documentation for
    /// the <seealso cref="ComplexZmanimCalendar"/> and <seealso cref="AstronomicalCalendar"/> for simple
    /// examples on using the API. <h2>Disclaimer:</h2> While I did my best to get
    /// accurate results please do not rely on these zmanim for
    /// <em>halacha lemaaseh</em>
    /// 
    /// @author &copy; Eliyahu Hershfeld 2004 - 2010
    /// @version 1.2 </summary>
    public class ZmanimCalendar : AstronomicalCalendar
    {
        private const long serialVersionUID = 1;

        ///	 <summary> 
        ///	The zenith of 16.1&deg; below geometric zenith (90&deg;). This
        ///	calculation is used for calculating <em>alos</em> (dawn) and
        ///	<em>tzais</em> (nightfall) in some opinions. This calculation is based on
        ///	the calculation that the time between dawn and sunrise (and sunset to
        ///	nightfall) is the time that is takes to walk 4 <em>mil</em> at 18 minutes
        ///	a mil (<em>Ramba"m</em> and others). The sun's position at 72 minutes
        ///	before <seealso cref="AstronomicalCalendar.getSunrise">sunrise</seealso> in Jerusalem on the equinox is
        ///	16.1&deg; below <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>.
        ///	 </summary>
        ///	<seealso cref="getAlosHashachar()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getAlos16Point1Degrees()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getTzais16Point1Degrees()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getSofZmanShmaMGA16Point1Degrees()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getSofZmanTfilaMGA16Point1Degrees()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getMinchaGedola16Point1Degrees()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getMinchaKetana16Point1Degrees()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getPlagHamincha16Point1Degrees()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getPlagAlos16Point1ToTzaisGeonim7Point083Degrees()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getSofZmanShmaAlos16Point1ToSunset()"/>
        protected internal const double ZENITH_16_POINT_1 = GEOMETRIC_ZENITH + 16.1;

        ///	 <summary> 
        ///	The zenith of 8.5&deg; below geometric zenith (90&deg;). This calculation
        ///	is used for calculating <em>alos</em> (dawn) and <em>tzais</em>
        ///	(nightfall) in some opinions. This calculation is based on the position
        ///	of the sun 36 minutes after <seealso cref="AstronomicalCalendar.getSunset">sunset</seealso> in Jerusalem on
        ///	March 16, about 4 days before the equinox, the day that a solar hour is
        ///	one hour, which is 8.5&deg; below <see cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>.
        /// The Ohr Meir considers this the time that 3 small starts are
        ///	visible, later than the required 3 medium stars.
        ///	 </summary>
        ///	<seealso cref="getTzais"/>
        ///	<seealso cref="ComplexZmanimCalendar.getTzaisGeonim8Point5Degrees"/>
        protected internal const double ZENITH_8_POINT_5 = GEOMETRIC_ZENITH + 8.5;

        ///	 <summary> 
        ///	The default Shabbos candle lighting offset is 18 minutes. This can be
        ///	changed via the <seealso cref="setCandleLightingOffset(double)"/> and retrieved by
        ///	the <seealso cref="getCandleLightingOffset()"/>. </summary>
        private double candleLightingOffset = 18;

        ///	 <summary> 
        ///	Default constructor will set a default <seealso cref="GeoLocation"/>,
        ///	a default <see cref="AstronomicalCalculator.getDefault()"> AstronomicalCalculator</see>
        ///  and default the calendar to the current date.
        ///	 </summary>
        ///	<seealso cref="AstronomicalCalendar"/>
        public ZmanimCalendar()
        {
        }

        ///	 <summary> 
        ///	A constructor that takes a <seealso cref="GeoLocation"/> as a parameter.
        ///	 </summary>
        ///	<param name="location">
        ///	           the location </param>
        public ZmanimCalendar(GeoLocation location)
            : base(location)
        {
        }

        ///	 <summary> 
        ///	Returns <em>tzais</em> (nightfall) when the sun is 8.5&deg; below the
        ///	western geometric horizon (90&deg;) after <seealso cref="AstronomicalCalendar.getSunset">sunset</seealso>. For
        ///	information on the source of this calculation see
        ///	<seealso cref="ZENITH_8_POINT_5"/>.
        ///	 </summary>
        ///	<returns> The <code>Date</code> of nightfall. </returns>
        ///	<seealso cref="ZENITH_8_POINT_5"/>
        public virtual Date getTzais()
        {
            return getSunsetOffsetByDegrees(ZENITH_8_POINT_5);
        }

        ///	 <summary> 
        ///	Returns <em>alos</em> (dawn) based on the time when the sun is 16.1&deg;
        ///	below the eastern <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric horizon</seealso> before
        ///	<seealso cref="AstronomicalCalendar.getSunrise">sunrise</seealso>. For more information the source of 16.1&deg;
        ///	see <seealso cref="ZENITH_16_POINT_1"/>.
        ///	 </summary>
        ///	<seealso cref="net.sourceforge.zmanim.ZmanimCalendar#ZENITH_16_POINT_1"/>
        ///	<returns> The <code>Date</code> of dawn. </returns>
        public virtual Date getAlosHashachar()
        {
            return getSunriseOffsetByDegrees(ZENITH_16_POINT_1);
        }

        ///	 <summary> 
        ///	Method to return <em>alos</em> (dawn) calculated using 72 minutes before
        ///	<seealso cref="AstronomicalCalendar.getSeaLevelSunrise">sea level sunrise</seealso> (no adjustment for
        ///	elevation) based on the time to walk the distance of 4 <em>Mil</em> at 18
        ///	minutes a <em>Mil</em>. This is based on the opinion of most
        ///	<em>Rishonim</em> who stated that the time of the <em>Neshef</em> (time
        ///	between dawn and sunrise) does not vary by the time of year or location
        ///	but purely depends on the time it takes to walk the distance of 4
        ///	<em>Mil</em>.
        ///	 </summary>
        ///	<returns> the <code>Date</code> representing the time. </returns>
        public virtual Date getAlos72()
        {
            return getTimeOffset(getSeaLevelSunrise(), -72*MINUTE_MILLIS);
        }

        ///	 <summary> 
        ///	This method returns <em>chatzos</em> (midday) following the opinion of
        ///	the GRA that the day for Jewish halachic times start at
        ///	<seealso cref="AstronomicalCalendar.getSunrise">sunrise</seealso> and ends at <seealso cref="AstronomicalCalendar.getSunset">sunset</seealso>. The
        ///	returned value is identical to <seealso cref="AstronomicalCalendar.getSunTransit()"/>
        ///	 </summary>
        ///	<seealso cref="AstronomicalCalendar#getSunTransit()"/>
        ///	<returns> the <code>Date</code> of chatzos. </returns>
        public virtual Date getChatzos()
        {
            return getSunTransit();
        }

        ///	 <summary> 
        ///	A method that returns "solar" midnight, or the time when the sun is at
        ///	it's <a href="http://en.wikipedia.org/wiki/Nadir">nadir</a>. <br/>
        ///	<br/>
        ///	<b>Note:</b> this method is experimental and might be removed (or moved)
        ///	 </summary>
        ///	<returns> the <code>Date</code> of Solar Midnight (chatzos layla). </returns>
        public virtual Date getSolarMidnight()
        {
            var clonedCal = (ZmanimCalendar) Clone();
            clonedCal.getCalendar().add(Calendar.DAY_OF_MONTH, 1);
            Date sunset = getSunset();
            Date sunrise = clonedCal.getSunrise();
            return getTimeOffset(sunset, getTemporalHour(sunset, sunrise)*6);
        }

        // public Date getChatzosLaylaRSZ() {
        // ZmanimCalendar clonedCal = (ZmanimCalendar)clone();
        // clonedCal.getCalendar().add(Calendar.DAY_OF_MONTH, 1);
        // Date sunset = getSunset();
        // Date sunrise = clonedCal.getAlosHashachar();
        // return getTimeOffset(sunset, getTemporalHour(sunset, sunrise) * 6);
        // }

        ///	 <summary> 
        ///	This method returns the latest <em>zman krias shema</em> (time to say
        ///	Shema in the morning). This time is 3
        ///	<em><seealso cref="getShaahZmanisGra">shaos zmaniyos</seealso></em> (solar hours) after
        ///	<seealso cref="AstronomicalCalendar.getSeaLevelSunrise">sea level sunrise</seealso> based on the opinion of
        ///	the <em>GR"A</em> and the <em>Baal Hatanya</em> that the day is
        ///	calculated from sunrise to sunset. This returns the time 3 *
        ///	<seealso cref="getShaahZmanisGra()"/> after <see cref="AstronomicalCalendar.getSeaLevelSunrise()">sea level sunrise</see>.
        ///	 </summary>
        ///	<seealso cref="net.sourceforge.zmanim.ZmanimCalendar#getShaahZmanisGra()"/>
        ///	<returns> the <code>Date</code> of the latest zman shema. </returns>
        public virtual Date getSofZmanShmaGRA()
        {
            return getTimeOffset(getSeaLevelSunrise(), getShaahZmanisGra()*3);
        }

        ///	 <summary> 
        ///	This method returns the latest <em>zman krias shema</em> (time to say
        ///	Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///	<em>alos</em> being 72 minutes before <seealso cref="AstronomicalCalendar.getSunrise">sunrise</seealso>. This
        ///	time is 3 <em> shaos zmaniyos</em> (solar hours) after dawn based on the
        ///	opinion of the <em>MG"A</em> that the day is calculated from a dawn of 72
        ///	minutes before sunrise to nightfall of 72 minutes after sunset. This
        ///	returns the time of 3 * <em>shaos zmaniyos</em> after dawn.
        ///	 </summary>
        ///	<returns> the <code>Date</code> of the latest zman shema. </returns>
        ///	<seealso cref="ComplexZmanimCalendar.getShaahZmanis72Minutes()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getAlos72()"/>
        ///	<seealso cref="ComplexZmanimCalendar.getSofZmanShmaMGA72Minutes()"/>
        public virtual Date getSofZmanShmaMGA()
        {
            return getTimeOffset(getAlos72(), getShaahZmanisMGA()*3);
        }

        ///	 <summary> 
        ///	This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///	of the <em>Ramba"m</em> and <em>Rabainu Tam</em> that <em>tzais</em> is
        ///	calculated as the time it takes to walk 4 <em>Mil</em> at 18 minutes a
        ///	<em>Mil</em> for a total of 72 minutes. Even for locations above sea
        ///	level, this is calculated at sea level, since the darkness level is not
        ///	affected by elevation.
        ///	 </summary>
        ///	<returns> the <code>Date</code> representing 72 minutes after sea level
        ///	        sunset. </returns>
        public virtual Date getTzais72()
        {
            return getTimeOffset(getSeaLevelSunset(), 72*MINUTE_MILLIS);
        }

        ///	 <summary> 
        ///	A method to return candle lighting time. This is calculated as
        ///	<seealso cref="getCandleLightingOffset()"/> minutes before sunset. This will
        ///	return the time for any day of the week, since it can be used to
        ///	calculate candle lighting time for <em>yom tov</em> (holidays) as well.
        ///	 </summary>
        ///	<returns> candle lighting time. </returns>
        ///	<seealso cref="getCandleLightingOffset()"/>
        ///	<seealso cref="#setCandleLightingOffset(double)"/>
        public virtual Date getCandelLighting()
        {
            return getTimeOffset(getSunset(), -getCandleLightingOffset()*MINUTE_MILLIS);
        }

        ///	 <summary> 
        ///	This method returns the latest
        ///	<em>zman tefilah<em> (time to pray morning prayers). This time is 4
        ///	hours into the day based on the opinion of the <em>GR"A</em> and the
        ///	</em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        ///	This returns the time 4 * <seealso cref="getShaahZmanisGra()"/> after
        ///	<seealso cref="AstronomicalCalendar.getSeaLevelSunrise">sea level sunrise</seealso>.
        ///	 </summary>
        ///	<seealso cref="ZmanimCalendar.getShaahZmanisGra()"/>
        ///	<returns> the <code>Date</code> of the latest zman tefilah. </returns>
        public virtual Date getSofZmanTfilaGRA()
        {
            return getTimeOffset(getSeaLevelSunrise(), getShaahZmanisGra()*4);
        }

        ///	 <summary> 
        ///	This method returns the latest <em>zman tfila</em> (time to say the
        ///	morning prayers) in the opinion of the <em>MG"A</em> based on
        ///	<em>alos</em> being <seealso cref="getAlos72">72</seealso> minutes before
        ///	<seealso cref="AstronomicalCalendar.getSunrise">sunrise</seealso>. This time is 4
        ///	<em><seealso cref="getShaahZmanisMGA">shaos zmaniyos</seealso></em> (temporal hours)
        ///	after <seealso cref="getAlos72">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ///	that the day is calculated from a <seealso cref="getAlos72">dawn</seealso> of 72 minutes
        ///	before sunrise to <seealso cref="getTzais72">nightfall</seealso> of 72 minutes after
        ///	sunset. This returns the time of 4 * <seealso cref="getShaahZmanisMGA()"/> after
        ///	<seealso cref="getAlos72">dawn</seealso>.
        ///	 </summary>
        ///	<returns> the <code>Date</code> of the latest zman tfila. </returns>
        ///	<seealso cref="getShaahZmanisMGA()"/>
        ///	<seealso cref="getAlos72()"/>
        public virtual Date getSofZmanTfilaMGA()
        {
            return getTimeOffset(getAlos72(), getShaahZmanisMGA()*4);
        }

        ///	 <summary> 
        ///	This method returns the time of <em>mincha gedola</em>.
        ///	<em>Mincha gedola</em> is the earliest time one can pray mincha. The
        ///	Ramba"m is of the opinion that it is better to delay <em>mincha</em>
        ///	until <em><seealso cref="getMinchaKetana">mincha ketana</seealso></em> while the
        ///	<em>Ra"sh,
        ///	Tur, GR"A</em> and others are of the opinion that <em>mincha</em> can be
        ///	prayed <em>lechatchila</em> starting at <em>mincha gedola</em>. This is
        ///	calculated as 6.5 <seealso cref="getShaahZmanisGra">sea level solar hours</seealso>
        ///	after <seealso cref="AstronomicalCalendar.getSeaLevelSunrise">sea level sunrise</seealso>. This calculation
        ///	is calculated based on the opinion of the <em>GR"A</em> and the
        ///	<em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        ///	This returns the time 6.5 <seealso cref="getShaahZmanisGra()"/> after
        ///	<seealso cref="AstronomicalCalendar.getSeaLevelSunrise">sea level sunrise</seealso>.
        ///	 </summary>
        ///	<seealso cref="getShaahZmanisGra()"/>
        ///	<seealso cref="getMinchaKetana()"/>
        ///	<returns> the <code>Date</code> of the time of mincha gedola. </returns>
        public virtual Date getMinchaGedola()
        {
            return getTimeOffset(getSeaLevelSunrise(), getShaahZmanisGra()*6.5);
        }

        ///	 <summary> 
        ///	This method returns the time of <em>mincha ketana</em>. This is the
        ///	perfered earliest time to pray <em>mincha</em> in the opinion of the
        ///	Ramba"m and others. For more information on this see the documentation on
        ///	<em><seealso cref="getMinchaGedola">mincha gedola</seealso></em>. This is calculated as
        ///	9.5 <seealso cref="getShaahZmanisGra">sea level solar hours</seealso> after
        ///	<seealso cref="AstronomicalCalendar.getSeaLevelSunrise">sea level sunrise</seealso>. This calculation is
        ///	calculated based on the opinion of the <em>GR"A</em> and the
        ///	<em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        ///	This returns the time 9.5 * <seealso cref="getShaahZmanisGra()"/> after
        ///	<seealso cref="AstronomicalCalendar.getSeaLevelSunrise">sea level sunrise</seealso>.
        ///	 </summary>
        ///	<seealso cref="getShaahZmanisGra()"/>
        ///	<seealso cref="getMinchaGedola()"/>
        ///	<returns> the <code>Date</code> of the time of mincha gedola. </returns>
        public virtual Date getMinchaKetana()
        {
            return getTimeOffset(getSeaLevelSunrise(), getShaahZmanisGra()*9.5);
        }

        ///	 <summary> 
        ///	This method returns he time of <em>plag hamincha</em>. This is calculated
        ///	as 10.75 hours after sunrise. This calculation is calculated based on the
        ///	opinion of the <em>GR"A</em> and the <em>Baal Hatanya</em> that the day
        ///	is calculated from sunrise to sunset. This returns the time 10.75 *
        ///	<seealso cref="getShaahZmanisGra()"/> after <see cref="AstronomicalCalendar.getSeaLevelSunrise()">sea level sunrise</see>.
        ///	 </summary>
        ///	<returns> the <code>Date</code> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha()
        {
            return getTimeOffset(getSeaLevelSunrise(), getShaahZmanisGra()*10.75);
        }

        ///	 <summary> 
        ///	Method to return a <em>shaah zmanis</em> (
        ///	<see cref="AstronomicalCalendar.getTemporalHour(Date, Date)">temporal hour</see>) according to the
        ///	opinion of the <em>GR"A</em> and the <em>Baal Hatanya</em>. This
        ///	calculation divides the day based on the opinion of the <em>GR"A</em> and
        ///	the <em>Baal Hatanya</em> that the day runs from <see cref="AstronomicalCalendar.getSunrise()"> sunrise</see>
        ///  to <seealso cref="AstronomicalCalendar.getSunset">sunset</seealso>. The calculations are based on a
        ///	day from <seealso cref="AstronomicalCalendar.getSeaLevelSunrise">sea level sunrise</seealso> to
        ///	<seealso cref="AstronomicalCalendar.getSeaLevelSunset">sea level sunset</seealso>. The day is split into 12
        ///	equal parts each part with each one being a <em>shaah zmanis</em>. This
        ///	method is similar to <seealso cref="#getTemporalHour"/>, but all calculations are
        ///	based on a sealevel sunrise and sunset. For additional information, see
        ///	Zmanim Kehilchasam, 2nd Edition by Rabbi Dovid Yehuda Burstein,
        ///	Jerusalem, 2007.
        ///	 </summary>
        ///	<returns> the <code>long</code> millisecond length of a
        ///	        <em>shaah zmanis</em>. </returns>
        ///	<seealso cref="AstronomicalCalendar.getTemporalHour(Date, Date)"/>
        public virtual long getShaahZmanisGra()
        {
            return getTemporalHour(getSeaLevelSunrise(), getSeaLevelSunset());
        }

        ///	 <summary> 
        ///	Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///	opinion of the MGA. This calculation divides the day based on the opinion
        ///	of the <em>MGA</em> that the day runs from dawn to dusk (for sof zman
        ///	krias shema and tfila). Dawn for this calculation is 72 minutes before
        ///	sunrise and dusk is 72 minutes after sunset. This day is split into 12
        ///	equal parts with each part being a <em>shaah zmanis</em>. Alternate
        ///	mothods of calculating a <em>shaah zmanis</em> are available in the
        ///	subclass <seealso cref="ComplexZmanimCalendar"/>.
        ///	 </summary>
        ///	<returns> the <code>long</code> millisecond length of a
        ///	        <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanisMGA()
        {
            return getTemporalHour(getAlos72(), getTzais72());
        }

        ///    
        ///	<seealso cref="java.lang.Object#equals(Object)"/>
        public override bool Equals(object @object)
        {
            if (this == @object)
            {
                return true;
            }
            if (!(@object is ZmanimCalendar))
            {
                return false;
            }
            var zCal = (ZmanimCalendar) @object;
            // return getCalendar().getTime().equals(zCal.getCalendar().getTime())
            return getCalendar().Equals(zCal.getCalendar()) && getGeoLocation().Equals(zCal.getGeoLocation()) &&
                   getAstronomicalCalculator().Equals(zCal.getAstronomicalCalculator());
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 37*result + GetType().GetHashCode(); // needed or this and
            // subclasses will
            // return identical hash
            result += 37*result + getCalendar().GetHashCode();
            result += 37*result + getGeoLocation().GetHashCode();
            result += 37*result + getAstronomicalCalculator().GetHashCode();
            return result;
        }

        ///	 <summary> 
        ///	A method to get the offset in minutes before
        ///	<seealso cref="AstronomicalCalendar.getSunset()">sunset</seealso> that is used in
        ///	calculating candle lighting time. The default time used is 18 minutes
        ///	before sunset. Some calendars use 15 minutes, while the custom in
        ///	Jerusalem is to use a 40 minute offset. Please check the local custom for
        ///	candel lighting time.
        ///	 </summary>
        ///	<returns> Returns the candle lighting offset to set in minutes.. </returns>
        ///	<seealso cref="getCandelLighting()"/>
        public virtual double getCandleLightingOffset()
        {
            return candleLightingOffset;
        }

        ///	 <summary> 
        ///	A method to set the offset in minutes before
        ///	<seealso cref="AstronomicalCalendar.getSunset()">sunset</seealso> that is used in
        ///	calculating candle lighting time. The default time used is 18 minutes
        ///	before sunset. Some calendars use 15 minutes, while the custom in
        ///	Jerusalem is to use a 40 minute offset.
        ///	 </summary>
        ///	<param name="candleLightingOffset">
        ///	           The candle lighting offset to set in minutes. </param>
        ///	<seealso cref="getCandelLighting()"/>
        public virtual void setCandleLightingOffset(double candleLightingOffset)
        {
            this.candleLightingOffset = candleLightingOffset;
        }
    }
}