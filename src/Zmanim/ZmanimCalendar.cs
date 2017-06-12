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
using Zmanim.Calculator;
using Zmanim.Utilities;

namespace Zmanim
{
    /// <summary>
    /// 	<p> Description: A .NET library for calculating zmanim. </p>
    /// The zmanim library is an API is a specialized calendar that can calculate
    /// sunrise and sunset and Jewish <em>zmanim</em> (religious times) for prayers
    /// and other Jewish religious duties. For a much more extensive list of zmanim
    /// use the <seealso cref="ComplexZmanimCalendar"/> that extends this class. This class
    /// contains the main functionality of the Zmanim library. See documentation for
    /// the <seealso cref="ComplexZmanimCalendar"/> and <seealso cref="AstronomicalCalendar"/> for simple
    /// examples on using the API.<br/>
    /// 	<b>Note:</b> It is important to read the technical notes on top of the
    /// <see cref="AstronomicalCalculator"/> documentation. <h2>
    /// Disclaimer:</h2> While I did my best to get accurate results please do not
    /// rely on these zmanim for <em>halacha lemaaseh</em>.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class ZmanimCalendar : AstronomicalCalendar
    {
        private double candleLightingOffset = 18;

        ///<summary>
        ///  The zenith of 16.1° below geometric zenith (90°). This
        ///  calculation is used for calculating <em>alos</em> (dawn) and
        ///  <em>tzais</em> (nightfall) in some opinions. This calculation is based on
        ///  the calculation that the time between dawn and sunrise (and sunset to
        ///  nightfall) is the time that is takes to walk 4 <em>mil</em> at 18 minutes
        ///  a mil (<em>Ramba"m</em> and others). The sun's position at 72 minutes
        ///  before <see cref="AstronomicalCalendar.GetSunrise">sunrise</see> in Jerusalem on the equinox is
        ///  16.1° below <see cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>.
        ///</summary>
        ///<seealso cref="GetAlosHashachar" />
        ///<seealso cref="ComplexZmanimCalendar.GetAlos16Point1Degrees()" />
        ///<seealso cref="ComplexZmanimCalendar.GetTzais16Point1Degrees()" />
        ///<seealso cref="ComplexZmanimCalendar.GetSofZmanShmaMGA16Point1Degrees()" />
        ///<seealso cref="ComplexZmanimCalendar.GetSofZmanTfilaMGA16Point1Degrees()" />
        ///<seealso cref="ComplexZmanimCalendar.GetMinchaGedola16Point1Degrees()" />
        ///<seealso cref="ComplexZmanimCalendar.GetMinchaKetana16Point1Degrees()" />
        ///<seealso cref="ComplexZmanimCalendar.GetPlagHamincha16Point1Degrees()" />
        ///<seealso cref="ComplexZmanimCalendar.GetPlagAlos16Point1ToTzaisGeonim7Point083Degrees()" />
        ///<seealso cref="ComplexZmanimCalendar.GetSofZmanShmaAlos16Point1ToSunset()" />
        protected internal const double ZENITH_16_POINT_1 = GEOMETRIC_ZENITH + 16.1;

        ///<summary>
        ///  The zenith of 8.5° below geometric zenith (90°). This calculation
        ///  is used for calculating <em>alos</em> (dawn) and <em>tzais</em>
        ///  (nightfall) in some opinions. This calculation is based on the position
        ///  of the sun 36 minutes after <see cref = "AstronomicalCalendar.GetSunset">sunset</see> in Jerusalem on
        ///  March 16, about 4 days before the equinox, the day that a solar hour is
        ///  one hour, which is 8.5° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>.
        ///  The Ohr Meir considers this the time that 3 small starts are
        ///  visible, later than the required 3 medium stars.
        ///</summary>
        ///<seealso cref = "GetTzais" />
        ///<seealso cref = "ComplexZmanimCalendar.GetTzaisGeonim8Point5Degrees" />
        protected internal const double ZENITH_8_POINT_5 = GEOMETRIC_ZENITH + 8.5;

        ///<summary>
        ///  Default constructor will set a default <see cref = "GeoLocation" />,
        ///  a default <see cref = "AstronomicalCalculator.GetDefault"> AstronomicalCalculator</see>
        ///  and default the calendar to the current date.
        ///</summary>
        ///<seealso cref = "AstronomicalCalendar" />
        public ZmanimCalendar() { }

        ///<summary>
        ///  A constructor that takes a <seealso cref = "GeoLocation" /> as a parameter.
        ///</summary>
        ///<param name = "location">
        ///  the location </param>
        public ZmanimCalendar(IGeoLocation location)
            : base(location) { }

        /// <summary>
        /// A constructor that takes a <seealso cref="GeoLocation"/> as a parameter.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="location">the location</param>
        public ZmanimCalendar(DateTime date, IGeoLocation location)
            : base(date, location) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmanimCalendar"/> class.
        /// </summary>
        /// <param name="dateWithLocation">The date with location.</param>
        public ZmanimCalendar(IDateWithLocation dateWithLocation)
            : base(dateWithLocation) { }

        /// <summary>
        /// Returns <em>tzais</em> (nightfall) when the sun is 8.5° below the
        /// western geometric horizon (90°) after <seealso cref="AstronomicalCalendar.GetSunset">sunset</seealso>. For
        /// information on the source of this calculation see
        /// <seealso cref="ZENITH_8_POINT_5"/>.
        /// </summary>
        /// <returns>
        /// The <code>DateTime</code> of nightfall.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZENITH_8_POINT_5"/>
        public virtual DateTime? GetTzais()
        {
            return GetSunsetOffsetByDegrees(ZENITH_8_POINT_5);
        }

        ///<summary>
        ///  Returns <em>alos</em> (dawn) based on the time when the sun is 16.1°
        ///  below the eastern <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric horizon</see> before
        ///  <see cref = "AstronomicalCalendar.GetSunrise">sunrise</see>. For more information the source of 16.1°
        ///  see <see cref = "ZENITH_16_POINT_1" />.
        ///</summary>
        ///<seealso cref = "ZENITH_16_POINT_1" />
        ///<returns>
        ///  The <c>DateTime</c> of dawn.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetAlosHashachar()
        {
            return GetSunriseOffsetByDegrees(ZENITH_16_POINT_1);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 72 minutes before
        ///  <see cref = "AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> (no adjustment for
        ///  elevation) based on the time to walk the distance of 4 <em>Mil</em> at 18
        ///  minutes a <em>Mil</em>. This is based on the opinion of most
        ///  <em>Rishonim</em> who stated that the time of the <em>Neshef</em> (time
        ///  between dawn and sunrise) does not vary by the time of year or location
        ///  but purely depends on the time it takes to walk the distance of 4
        ///  <em>Mil</em>.
        ///</summary>
        ///<returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetAlos72()
        {
            return GetTimeOffset(GetSeaLevelSunrise().Value, -72 * MINUTE_MILLIS);
        }

        ///<summary>
        ///  This method returns <em>chatzos</em> (midday) following the opinion of
        ///  the GRA that the day for Jewish halachic times start at
        ///  <see cref = "AstronomicalCalendar.GetSunrise">sunrise</see> and ends at <see cref = "AstronomicalCalendar.GetSunset">sunset</see>. The
        ///  returned value is identical to <see cref = "AstronomicalCalendar.GetSunTransit" />
        ///</summary>
        ///<seealso cref = "AstronomicalCalendar.GetSunTransit" />
        ///<returns> the <c>DateTime</c> of chatzos.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        ///  </returns>
        public virtual DateTime? GetChatzos()
        {
            return GetSunTransit();
        }

        /// <summary>
        /// A method that returns "solar" midnight, or the time when the sun is at its 
        /// <a href="http://en.wikipedia.org/wiki/Nadir">nadir</a>. <br/>
        /// <br/>
        /// <b>Note:</b> this method is experimental and might be removed.
        /// </summary>
        /// <returns> the <code>Date</code> of Solar Midnight (chatzos layla). If the calculation can't be computed such as in
        ///         the Arctic Circle where there is at least one day a year where the sun does not rise, and one where it
        ///         does not set, a null will be returned. See detailed explanation on top of the
        ///         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetSolarMidnight()
        {
            DateTime? sunset = GetSeaLevelSunset();

            ZmanimCalendar clonedCal = (ZmanimCalendar)MemberwiseClone();
            clonedCal.DateWithLocation = new DateWithLocation(DateWithLocation.Date.AddDays(1), DateWithLocation.Location);
            DateTime? sunrise = clonedCal.GetSeaLevelSunrise();

            return GetTimeOffset(sunset, GetTemporalHour(sunset, sunrise) * 6);
        }

        /// <summary>
        /// This is a generic method for calculating the latest <em>zman krias shema</em> (time to recite Shema in the
        /// morning) based on the start and end of day passed to the method. The time from the start of day to the end of day
        /// are divided into 12 shaos zmaniyos (temporal hours), and <em>zman krias shema</em> is calculated as 3 shaos
        /// zmaniyos from the beginning of the day. As an example, passing <seealso cref="GetSeaLevelSunrise">sea level sunrise</seealso>
        /// and <seealso cref="GetSeaLevelSunset">sea level sunset</seealso> to this method will return <em>zman krias shema</em> according to
        /// the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em>.
        /// </summary>
        /// <param name="startOfDay">
        ///            the start of day for calculating <em>zman krias shema</em>. This can be sunrise or any alos passed to
        ///            this method. </param>
        /// <param name="endOfDay">
        ///            the start of day for calculating <em>zman krias shema</em>. This can be sunset or any tzais passed to
        ///            this method. </param>
        /// <returns> the <code>DateTime</code> of the latest zman shema based on the start and end of day times passed to this
        ///         method. If the calculation can't be computed such as in the Arctic Circle where there is at least one day
        ///         a year where the sun does not rise, and one where it does not set, a null will be returned. See detailed
        ///         explanation on top of the <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetSofZmanShma(DateTime? startOfDay, DateTime? endOfDay)
        {
            long shaahZmanis = GetTemporalHour(startOfDay, endOfDay);
            return GetTimeOffset(startOfDay, shaahZmanis * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to recite Shema in the morning). This time is 3
        /// <em><seealso cref="GetShaahZmanisGra">shaos zmaniyos</seealso></em> (solar hours) after <see cref = "AstronomicalCalendar.GetSeaLevelSunrise">sea level
        /// sunrise</see> based on the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em> that the day is calculated from
        /// sunrise to sunset. This returns the time 3 * <seealso cref="GetShaahZmanisGra"/> after <see cref = "AstronomicalCalendar.GetSeaLevelSunrise"> sea
        /// level sunrise</see>.
        /// </summary>
        /// <seealso cref="GetSofZmanShma"/>
        /// <seealso cref="GetShaahZmanisGra"/>
        /// <returns> the <code>DateTime</code> of the latest zman shema according to the GRA and Baal Hatanya. If the calculation
        ///         can't be computed such as in the Arctic Circle where there is at least one day a year where the sun does
        ///         not rise, and one where it does not set, a null will be returned. See detailed explanation on top of the
        ///         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetSofZmanShmaGRA()
        {
            return GetSofZmanShma(GetSeaLevelSunrise(), GetSeaLevelSunset());
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to recite shema in the morning) in the opinion of
        /// the <em>MGA</em> based on <em>alos</em> being 72 minutes before <seealso cref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 3
        /// <em>shaos zmaniyos</em> (solar hours) after dawn based on the opinion of the <em>MGA</em> that the day is
        /// calculated from a dawn of 72 minutes before sunrise to nightfall of 72 minutes after sunset. This returns the
        /// time of 3 * <em>shaos zmaniyos</em> after dawn.
        /// </summary>
        /// <returns> the <code>DateTime</code> of the latest <em>zman shema</em>. If the calculation can't be computed such as in
        ///         the Arctic Circle where there is at least one day a year where the sun does not rise, and one where it
        ///         does not set, a null will be returned. See detailed explanation on top of the
        ///         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        /// <seealso cref="GetSofZmanShma"/>
        /// <seealso cref="ComplexZmanimCalendar.GetShaahZmanis72Minutes"/>
        /// <seealso cref="ComplexZmanimCalendar.GetAlos72"/>
        /// <seealso cref="ComplexZmanimCalendar.GetSofZmanShmaMGA72Minutes"/>
        public virtual DateTime? GetSofZmanShmaMGA()
        {
            return GetSofZmanShma(GetAlos72(), GetTzais72());
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Ramba"m</em> and <em>Rabainu Tam</em> that <em>tzais</em> is
        ///  calculated as the time it takes to walk 4 <em>Mil</em> at 18 minutes a
        ///  <em>Mil</em> for a total of 72 minutes. Even for locations above sea
        ///  level, this is calculated at sea level, since the darkness level is not
        ///  affected by elevation.
        ///</summary>
        ///<returns> the <c>DateTime</c> representing 72 minutes after sea level sunset.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        ///  </returns>
        public virtual DateTime? GetTzais72()
        {
            return GetTimeOffset(GetSeaLevelSunset().Value, 72 * MINUTE_MILLIS);
        }

        /// <summary>
        /// A method to return candle lighting time. This is calculated as
        /// <seealso cref="CandleLightingOffset"/> minutes before sunset. This will
        /// return the time for any day of the week, since it can be
        /// used to calculate candle lighting time for <em>yom tov</em>
        /// (mid-week holidays) as well. To calculate the offset
        /// of non-sea level sunset, pass the elevation adjusted sunset to <seealso cref="AstronomicalCalendar.GetTimeOffset(System.Nullable{System.DateTime},long)"/>.
        /// </summary>
        /// <returns>
        /// candle lighting time.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="CandleLightingOffset"/>
        public virtual DateTime? GetCandleLighting()
        {
            return GetTimeOffset(GetSeaLevelSunset().Value, -CandleLightingOffset * MINUTE_MILLIS);
        }

        /// <summary>
        /// This is a generic method for calculating the latest <em>zman tefilah<em> (time to recite the morning prayers)
        /// based on the start and end of day passed to the method. The time from the start of day to the end of day
        /// are divided into 12 shaos zmaniyos (temporal hours), and <em>zman krias shema</em> is calculated as 4 shaos
        /// zmaniyos from the beginning of the day. As an example, passing <seealso cref="GetSeaLevelSunrise() sea level sunrise"/>
        /// and <seealso cref="GetSeaLevelSunset sea level sunset"/> to this method will return <em>zman tefilah<em> according to
        /// the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em>.
        /// </summary>
        /// <param name="startOfDay">
        ///            the start of day for calculating <em>zman tefilah<em>. This can be sunrise or any alos passed to
        ///            this method. </param>
        /// <param name="endOfDay">
        ///            the start of day for calculating <em>zman tefilah<em>. This can be sunset or any tzais passed to this
        ///            method. </param>
        /// <returns> the <code>Date</code> of the latest <em>zman tefilah<em> based on the start and end of day times passed
        ///         to this method. If the calculation can't be computed such as in the Arctic Circle where there is at least
        ///         one day a year where the sun does not rise, and one where it does not set, a null will be returned. See
        ///         detailed explanation on top of the <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetSofZmanTfila(DateTime? startOfDay, DateTime? endOfDay)
        {
            long shaahZmanis = GetTemporalHour(startOfDay, endOfDay);
            return GetTimeOffset(startOfDay, shaahZmanis * 4);
        }

        /// <summary>
        /// This method returns the latest
        /// <em>zman tefilah<em> (time to pray morning prayers). This time is 4
        /// hours into the day based on the opinion of the <em>GR"A</em> and the
        /// </em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        /// This returns the time 4 * <seealso cref="GetShaahZmanisGra"/> after
        /// <seealso cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman tefilah.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanisGra"/>
        public virtual DateTime? GetSofZmanTfilaGRA()
        {
            return GetSofZmanTfila(GetSeaLevelSunrise(), GetSeaLevelSunset());
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <seealso cref="GetAlos72">72</seealso> minutes before
        /// <seealso cref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 4
        /// <em><seealso cref="GetShaahZmanisMGA">shaos zmaniyos</seealso></em> (temporal hours)
        /// after <seealso cref="GetAlos72">dawn</seealso> based on the opinion of the <em>MG"A</em>
        /// that the day is calculated from a <seealso cref="GetAlos72">dawn</seealso> of 72 minutes
        /// before sunrise to <seealso cref="GetTzais72">nightfall</seealso> of 72 minutes after
        /// sunset. This returns the time of 4 * <seealso cref="GetShaahZmanisMGA"/> after
        /// <seealso cref="GetAlos72">dawn</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman tfila.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanisMGA"/>
        /// <seealso cref="GetAlos72"/>
        public virtual DateTime? GetSofZmanTfilaMGA()
        {
            return GetTimeOffset(GetAlos72().Value, GetShaahZmanisMGA() * 4);
        }

        /// <summary>
        /// This is a generic method for calulating <em>mincha gedola</em>. <em>Mincha gedola</em> is the earliest time one
        /// can pray mincha (6.5 hours from the begining of the day), based on the start and end of day passed to the method.
        /// The time from the start of day to the end of day are divided into 12 shaos zmaniyos, and <em>Mincha gedola</em>
        /// is calculated as 6.5 hours from the beginning of the day. As an example, passing {@link #getSeaLevelSunrise() sea
        /// level sunrise} and <seealso cref="#getSeaLevelSunset sea level sunset"/> to this method will return <em>Mincha gedola</em>
        /// according to the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em>.
        /// </summary>
        /// <param name="startOfDay">
        ///            the start of day for calculating <em>Mincha gedola</em>. This can be sunrise or any alos passed to
        ///            this method. </param>
        /// <param name="endOfDay">
        ///            the start of day for calculating <em>Mincha gedola</em>. This can be sunrise or any alos passed to
        ///            this method. </param>
        /// <returns> the <code>Date</code> of the time of <em>Mincha gedola</em> based on the start and end of day times
        ///         passed to this method. If the calculation can't be computed such as in the Arctic Circle where there is
        ///         at least one day a year where the sun does not rise, and one where it does not set, a null will be
        ///         returned. See detailed explanation on top of the <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetMinchaGedola(DateTime? startOfDay, DateTime? endOfDay)
        {
            long shaahZmanis = GetTemporalHour(startOfDay, endOfDay);
            return GetTimeOffset(startOfDay, shaahZmanis * 6.5);
        }

        /// <summary>
        /// This method returns the time of <em>mincha gedola</em>.
        /// <em>Mincha gedola</em> is the earliest time one can pray mincha. The
        /// Ramba"m is of the opinion that it is better to delay <em>mincha</em>
        /// until <em><seealso cref="GetMinchaKetana">mincha ketana</seealso></em> while the
        /// <em>Ra"sh,
        /// Tur, GR"A</em> and others are of the opinion that <em>mincha</em> can be
        /// prayed <em>lechatchila</em> starting at <em>mincha gedola</em>. This is
        /// calculated as 6.5 <seealso cref="GetShaahZmanisGra">sea level solar hours</seealso>
        /// after <seealso cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>. This calculation
        /// is calculated based on the opinion of the <em>GR"A</em> and the
        /// <em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        /// This returns the time 6.5 <seealso cref="GetShaahZmanisGra"/> after
        /// <seealso cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of mincha gedola.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanisGra"/>
        /// <seealso cref="GetMinchaKetana"/>
        public virtual DateTime? GetMinchaGedola()
        {
            return GetMinchaGedola(GetSeaLevelSunrise(), GetSeaLevelSunset());
        }
        
        /// <summary>
        /// This is a generic method for calulating <em>mincha ketana</em>. <em>Mincha ketana</em> is the preferred time one
        /// can pray can pray <em>mincha</em> in the opinion of the Rambam and others (9.5 hours from the begining of the
        /// day), based on the start and end of day passed to the method. The time from the start of day to the end of day
        /// are divided into 12 shaos zmaniyos, and <em>mincha ketana</em> is calculated as 9.5 hours from the beginning of
        /// the day. As an example, passing <seealso cref="#getSeaLevelSunrise() sea level sunrise"/> and {@link #getSeaLevelSunset sea
        /// level sunset} to this method will return <em>Mincha ketana</em> according to the opinion of the <em>GRA</em> and
        /// the <em>Baal Hatanya</em>.
        /// </summary>
        /// <param name="startOfDay">
        ///            the start of day for calculating <em>Mincha ketana</em>. This can be sunrise or any alos passed to
        ///            this method. </param>
        /// <param name="endOfDay">
        ///            the start of day for calculating <em>Mincha ketana</em>. This can be sunrise or any alos passed to
        ///            this method. </param>
        /// <returns> the <code>Date</code> of the time of <em>Mincha ketana</em> based on the start and end of day times
        ///         passed to this method. If the calculation can't be computed such as in the Arctic Circle where there is
        ///         at least one day a year where the sun does not rise, and one where it does not set, a null will be
        ///         returned. See detailed explanation on top of the <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetMinchaKetana(DateTime? startOfDay, DateTime? endOfDay)
        {
            long shaahZmanis = GetTemporalHour(startOfDay, endOfDay);
            return GetTimeOffset(startOfDay, shaahZmanis * 9.5);
        }

        /// <summary>
        /// This method returns the time of <em>mincha ketana</em>. This is the
        /// perfered earliest time to pray <em>mincha</em> in the opinion of the
        /// Ramba"m and others. For more information on this see the documentation on
        /// <em><seealso cref="GetMinchaGedola">mincha gedola</seealso></em>. This is calculated as
        /// 9.5 <seealso cref="GetShaahZmanisGra">sea level solar hours</seealso> after
        /// <seealso cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>. This calculation is
        /// calculated based on the opinion of the <em>GR"A</em> and the
        /// <em>Baal Hatanya</em> that the day is calculated from sunrise to sunset.
        /// This returns the time 9.5 * <seealso cref="GetShaahZmanisGra"/> after
        /// <seealso cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of mincha gedola.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanisGra"/>
        /// <seealso cref="GetMinchaGedola"/>
        public virtual DateTime? GetMinchaKetana()
        {
            return GetMinchaKetana(GetSeaLevelSunrise(), GetSeaLevelSunset());
        }
        
        /// <summary>
        /// This is a generic method for calulating <em>plag hamincha</em> (1.25 hours before the end of the day) based on
        /// the start and end of day passed to the method. The time from the start of day to the end of day are divided into
        /// 12 shaos zmaniyos, and plag is calculated as 10.75 hours from the beginning of the day. As an example, passing
        /// <seealso cref="#getSeaLevelSunrise() sea level sunrise"/> and <seealso cref="#getSeaLevelSunset sea level sunset"/> to this method
        /// will return Plag Hamincha according to the opinion of the <em>GRA</em> and the <em>Baal Hatanya</em>.
        /// </summary>
        /// <param name="startOfDay">
        ///            the start of day for calculating plag. This can be sunrise or any alos passed to this method. </param>
        /// <param name="endOfDay">
        ///            the start of day for calculating plag. This can be sunrise or any alos passed to this method. </param>
        /// <returns> the <code>Date</code> of the time of <em>plag hamincha</em> based on the start and end of day times
        ///         passed to this method. If the calculation can't be computed such as in the Arctic Circle where there is
        ///         at least one day a year where the sun does not rise, and one where it does not set, a null will be
        ///         returned. See detailed explanation on top of the <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetPlagHamincha(DateTime? startOfDay, DateTime? endOfDay)
        {
            long shaahZmanis = GetTemporalHour(startOfDay, endOfDay);
            return GetTimeOffset(startOfDay, shaahZmanis * 10.75);
        }

        /// <summary>
        /// This method returns he time of <em>plag hamincha</em>. This is calculated
        /// as 10.75 hours after sunrise. This calculation is calculated based on the
        /// opinion of the <em>GR"A</em> and the <em>Baal Hatanya</em> that the day
        /// is calculated from sunrise to sunset. This returns the time 10.75 *
        /// <see cref="GetShaahZmanisGra"/> after <see cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as northern and southern locations
        /// even south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha()
        {
            return GetPlagHamincha(GetSeaLevelSunrise(), GetSeaLevelSunset());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (
        /// <see cref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)">temporal hour</see>) according to the
        /// opinion of the <em>GR"A</em> and the <em>Baal Hatanya</em>. This
        /// calculation divides the day based on the opinion of the <em>GR"A</em> and
        /// the <em>Baal Hatanya</em> that the day runs from <see cref="AstronomicalCalendar.GetSunrise"> sunrise</see>
        /// to <seealso cref="AstronomicalCalendar.GetSunset">sunset</seealso>. The calculations are based on a
        /// day from <see cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> to
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>. The day is split into 12
        /// equal parts each part with each one being a <em>shaah zmanis</em>. This
        /// method is similar to <see cref="AstronomicalCalendar.GetTemporalHour()"/>, but all calculations are
        /// based on a sealevel sunrise and sunset. For additional information, see
        /// Zmanim Kehilchasam, 2nd Edition by Rabbi Dovid Yehuda Burstein,
        /// Jerusalem, 2007.
        /// </summary>
        /// <returns>
        /// the <code>long</code> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set,
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation on
        /// top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)"/>
        public virtual long GetShaahZmanisGra()
        {
            return GetTemporalHour(GetSeaLevelSunrise().Value, GetSeaLevelSunset().Value);
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        /// opinion of the MGA. This calculation divides the day based on the opinion
        /// of the <em>MGA</em> that the day runs from dawn to dusk (for sof zman
        /// krias shema and tfila). Dawn for this calculation is 72 minutes before
        /// sunrise and dusk is 72 minutes after sunset. This day is split into 12
        /// equal parts with each part being a <em>shaah zmanis</em>. Alternate
        /// mothods of calculating a <em>shaah zmanis</em> are available in the
        /// subclass <see cref="ComplexZmanimCalendar"/>.
        /// </summary>
        /// <returns>
        /// the <code>long</code> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set,
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation on
        /// top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanisMGA()
        {
            return GetTemporalHour(GetAlos72().Value, GetTzais72().Value);
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
            {
                return true;
            }
            if (!(obj is ZmanimCalendar))
            {
                return false;
            }
            var zCal = (ZmanimCalendar)obj;
            // return getCalendar().ToMillisecondsFromEpoch().equals(zCal.getCalendar().ToMillisecondsFromEpoch())
            return DateWithLocation.Equals(zCal.DateWithLocation) && DateWithLocation.Location.Equals(zCal.DateWithLocation.Location) &&
                   AstronomicalCalculator.Equals(zCal.AstronomicalCalculator);
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
            result = 37 * result + GetType().GetHashCode(); // needed or this and
            // subclasses will
            // return identical hash
            result += 37 * result + DateWithLocation.GetHashCode();
            result += 37 * result + DateWithLocation.Location.GetHashCode();
            result += 37 * result + AstronomicalCalculator.GetHashCode();
            return result;
        }

        ///<summary>
        ///  A method to get the offset in minutes before
        ///  <see cref = "AstronomicalCalendar.GetSunset">sunset</see> that is used in
        ///  calculating candle lighting time. The default time used is 18 minutes
        ///  before sunset. Some calendars use 15 minutes, while the custom in
        ///  Jerusalem is to use a 40 minute offset. Please check the local custom for
        ///  Candle lighting time.
        ///</summary>
        ///<value> Returns the candle lighting offset to set in minutes.. </value>
        ///<seealso cref = "GetCandleLighting" />
        public virtual double CandleLightingOffset
        {
            get { return candleLightingOffset; }
            set { candleLightingOffset = value; }
        }
    }
}