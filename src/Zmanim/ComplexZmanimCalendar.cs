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
using Zmanim.Extensions;
using Zmanim.Utilities;

namespace Zmanim
{
    /// <summary>
    /// This class extends ZmanimCalendar and provides many more zmanim than
    /// available in the ZmanimCalendar. The basis for most zmanim in this class are
    /// from the <em>sefer</em> <b>Yisroel Vehazmanim</b> by <b>Rabbi Yisroel Dovid  Harfenes</b>. <br/>
    /// For an example of the number of different <em>zmanim</em> made available by
    /// this class, there are methods to return 12 different calculations for
    /// <em>alos</em> (dawn) available in this class. The real power of this API is
    /// the ease in calculating <em>zmanim</em> that are not part of the API. The
    /// methods for doing <em>zmanim</em> calculations not present in this or it's
    /// superclass the <see cref="ZmanimCalendar"/> are contained in the
    /// <see cref="AstronomicalCalendar"/>, the base class of the calendars in our API
    /// since they are generic methods for calculating time based on degrees or time
    /// before or after <see cref="AstronomicalCalendar.GetSunrise">sunrise"</see> and <see cref="AstronomicalCalendar.GetSunset">sunset</see> and
    /// are of interest for calculation beyond <em>zmanim</em> calculations. Here are
    /// some examples: <br/>
    /// First create the Calendar for the location you would like to calculate:
    /// <example>
    /// 		<code>
    /// string locationName = "Lakewood, NJ"
    /// double latitude = 40.0828; //Lakewood, NJ
    /// double longitude = -74.2094; //Lakewood, NJ
    /// double elevation = 0;
    /// ITimeZone timeZone = new JavaTimeZone("America/New_York");
    /// GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    /// elevation, timeZone);
    /// ComplexZmanimCalendar czc = new ComplexZmanimCalendar(DateTime.Now, location);
    /// </code>
    /// 	</example>
    /// Note: For locations such as Israel where the beginning and end of daylight
    /// savings time can fluctuate from year to year create a
    /// <see cref="Zmanim.TimeZone.ITimeZone"/> with the known start and end of DST. <br/>
    /// To get alos calculated as 14° below the horizon (as calculated in the
    /// calendars published in Montreal) use:
    /// <code>
    /// DateTime alos14 = czc.getSunriseOffsetByDegrees(14);
    /// </code>
    /// To get <em>mincha gedola</em> calculated based on the MGA using a <em>shaah zmanis</em> based on the day starting 16.1° below the horizon (and ending
    /// 16.1° after sunset the following calculation can be used:
    /// <code>
    /// DateTime minchaGedola = czc.getTimeOffset(czc.getAlos16point1Degrees(),
    /// czc.getShaahZmanis16Point1Degrees() * 6.5);
    /// </code>
    /// A little more complex example would be calculating <em>plag hamincha</em>
    /// based on a shaah zmanis that was not present in this class. While a drop more
    /// complex it is still rather easy. For example if you wanted to calculate
    /// <em>plag</em> based on the day starting 12° before sunrise and ending
    /// 12° after sunset as calculated in the calendars in Manchester, England
    /// (there is nothing that would prevent your calculating the day using sunrise
    /// and sunset offsets that are not identical degrees, but this would lead to
    /// chatzos being a time other than the <see cref="AstronomicalCalendar.GetSunTransit">solar transit</see>
    /// (solar midday)). The steps involved would be to first calculate the
    /// <em>shaah zmanis</em> and than use that time in milliseconds to calculate
    /// 10.75 hours after sunrise starting at 12° before sunset
    /// <code>
    /// long shaahZmanis = czc.getTemporalHour(czc.getSunriseOffsetByDegrees(12),
    /// czc.getSunsetOffsetByDegrees(12));
    /// DateTime plag = getTimeOffset(czc.getSunriseOffsetByDegrees(12),
    /// shaahZmanis * 10.75);
    /// </code>
    /// 	<h2>Disclaimer:</h2> While I did my best to get accurate results please do
    /// not rely on these zmanim for <em>halacha lemaaseh</em>
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class ComplexZmanimCalendar : ZmanimCalendar
    {
        ///<summary>
        ///  The zenith of 3.7° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  opinion of the Geonim that <em>tzais</em> is the time it takes to walk
        ///  3/4 of a Mil at 18 minutes a Mil, or 13.5 minutes after sunset. The sun
        ///  is 3.7° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see> at this time
        ///  in Jerusalem on March 16, about 4 days before the equinox, the day that a
        ///  solar hour is one hour.
        ///</summary>
        protected internal const double ZENITH_3_POINT_7 = GEOMETRIC_ZENITH + 3.7;

        ///<summary>
        ///  The zenith of 5.95° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  position of the sun 24 minutes after sunset in Jerusalem on March 16,
        ///  about 4 days before the equinox, the day that a solar hour is one hour,
        ///  which calculates to 5.95° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///</summary>
        ///<seealso cref = "GetTzaisGeonim5Point95Degrees()" />
        protected internal const double ZENITH_5_POINT_95 = GEOMETRIC_ZENITH + 5.95;

        ///<summary>
        ///  The zenith of 7.083° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This is often referred to as 7°5' or 7° and 5 minutes.
        ///  This calculation is used for calculating <em>alos</em> (dawn) and
        ///  <em>tzais</em> (nightfall) according to some opinions. This calculation
        ///  is based on the position of the sun 30 minutes after sunset in Jerusalem
        ///  on March 16, about 4 days before the equinox, the day that a solar hour
        ///  is one hour, which calculates to 7.0833333° below
        ///  <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>. This is time some opinions
        ///  consider dark enough for 3 stars to be visible. This is the opinion of
        ///  the Shu"t Melamed Leho'il, Shu"t Binyan Tziyon, Tenuvas Sadeh and very
        ///  close to the time of the Mekor Chesed on the Sefer chasidim.
        ///</summary>
        ///<seealso cref = "GetTzaisGeonim7Point083Degrees()" />
        ///<seealso cref = "GetBainHasmashosRT13Point5MinutesBefore7Point083Degrees()" />
        protected internal const double ZENITH_7_POINT_083 = GEOMETRIC_ZENITH + 7 + (5 / 60);

        ///<summary>
        ///  The zenith of 10.2° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>misheyakir</em>
        ///  according to some opinions. This calculation is based on the position of
        ///  the sun 45 minutes before <see cref = "AstronomicalCalendar.GetSunrise">sunrise</see> in Jerusalem on
        ///  March 16, about 4 days before the equinox, the day that a solar hour is
        ///  one hour which calculates to 10.2° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH"> geometric zenith</see>
        ///</summary>
        ///<seealso cref = "GetMisheyakir10Point2Degrees()" />
        protected internal const double ZENITH_10_POINT_2 = GEOMETRIC_ZENITH + 10.2;

        ///<summary>
        ///  The zenith of 11° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>misheyakir</em>
        ///  according to some opinions. This calculation is based on the position of
        ///  the sun 48 minutes before <see cref ="AstronomicalCalendar.GetSunrise">sunrise</see> in Jerusalem on
        ///  March 16, about 4 days before the equinox, the day that a solar hour is
        ///  one hour which calculates to 11° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH"> geometric zenith</see>
        ///</summary>
        ///<seealso cref = "GetMisheyakir11Degrees()" />
        protected internal const double ZENITH_11_DEGREES = GEOMETRIC_ZENITH + 11;

        ///<summary>
        ///  The zenith of 11.5° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>misheyakir</em>
        ///  according to some opinions. This calculation is based on the position of
        ///  the sun 52 minutes before <see cref ="AstronomicalCalendar.GetSunrise">sunrise</see> in Jerusalem on
        ///  March 16, about 4 days before the equinox, the day that a solar hour is
        ///  one hour which calculates to 11.5° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH"> geometric zenith</see>
        ///</summary>
        ///<seealso cref = "GetMisheyakir11Point5Degrees()" />
        protected internal const double ZENITH_11_POINT_5 = GEOMETRIC_ZENITH + 11.5;

        ///<summary>
        ///  The zenith of 13° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating
        ///  <em>Rabainu Tam's bain hashmashos</em> according to some opinions. <br />
        ///  <br />
        ///  <b>FIXME:</b> See comments on <see cref = "GetBainHasmashosRT13Degrees" />. This
        ///  should be changed to 13.2477 after confirmation.
        ///</summary>
        ///<seealso cref = "GetBainHasmashosRT13Degrees" />
        protected internal const double ZENITH_13_DEGREES = GEOMETRIC_ZENITH + 13;

        ///<summary>
        ///  The zenith of 19.8° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>alos</em> (dawn)
        ///  and <em>tzais</em> (nightfall) according to some opinions. This
        ///  calculation is based on the position of the sun 90 minutes after sunset
        ///  in Jerusalem on March 16, about 4 days before the equinox, the day that a
        ///  solar hour is one hour which calculates to 19.8° below
        ///  <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///</summary>
        ///<seealso cref = "GetTzais19Point8Degrees()" />
        ///<seealso cref = "GetAlos19Point8Degrees()" />
        ///<seealso cref = "GetAlos90()" />
        ///<seealso cref = "GetTzais90()" />
        protected internal const double ZENITH_19_POINT_8 = GEOMETRIC_ZENITH + 19.8;

        ///<summary>
        ///  The zenith of 26° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>alos</em> (dawn)
        ///  and <em>tzais</em> (nightfall) according to some opinions. This
        ///  calculation is based on the position of the sun <see cref = "GetAlos120()">120 minutes</see>
        ///  after sunset in Jerusalem on March 16, about 4 days before the
        ///  equinox, the day that a solar hour is one hour which calculates to
        ///  26° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///</summary>
        ///<seealso cref = "GetAlos26Degrees()" />
        ///<seealso cref = "GetTzais26Degrees()" />
        ///<seealso cref = "GetAlos120()" />
        ///<seealso cref = "GetTzais120()" />
        protected internal const double ZENITH_26_DEGREES = GEOMETRIC_ZENITH + 26.0;

        ///NOTE: Experimental and may not make the final 1.3 cut
        ///<summary>
        ///  The zenith of 4.37° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  position of the sun <see cref = "GetTzaisGeonim4Point37Degrees()">16 7/8 minutes</see>
        ///  after sunset (3/4 of a 22.5 minute Mil) in Jerusalem on March
        ///  16, about 4 days before the equinox, the day that a solar hour is one
        ///  hour which calculates to 4.37° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH"> geometric zenith</see>
        ///</summary>
        ///<seealso cref = "GetTzaisGeonim4Point37Degrees()" />
        protected internal const double ZENITH_4_POINT_37 = GEOMETRIC_ZENITH + 4.37;

        ///<summary>
        ///  The zenith of 4.61° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  position of the sun <see cref = "GetTzaisGeonim4Point37Degrees">18 minutes</see>
        ///  after sunset (3/4 of a 24 minute Mil) in Jerusalem on March 16, about 4
        ///  days before the equinox, the day that a solar hour is one hour which
        ///  calculates to 4.61° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///</summary>
        ///<seealso cref = "GetTzaisGeonim4Point61Degrees()" />
        protected internal const double ZENITH_4_POINT_61 = GEOMETRIC_ZENITH + 4.61;

        /// <summary>
        /// The zenith of 4.8° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>(90°).
        /// </summary>
        protected internal const double ZENITH_4_POINT_8 = GEOMETRIC_ZENITH + 4.8;

        ///<summary>
        ///  The zenith of 3.65° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  position of the sun <see cref = "GetTzaisGeonim3Point65Degrees">13.5 minutes</see>
        ///  after sunset (3/4 of an 18 minute Mil) in Jerusalem on March 16, about 4
        ///  days before the equinox, the day that a solar hour is one hour which
        ///  calculates to 3.65° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">see zenith</see>
        ///</summary>
        ///<seealso cref = "GetTzaisGeonim3Point65Degrees()" />
        protected internal const double ZENITH_3_POINT_65 = GEOMETRIC_ZENITH + 3.65;

        /// <summary>
        /// The zenith of 5.88° below <see cref = "AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        /// (90°).
        /// </summary>
        protected internal const double ZENITH_5_POINT_88 = GEOMETRIC_ZENITH + 5.88;

        private double ateretTorahSunsetOffset = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexZmanimCalendar"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        public ComplexZmanimCalendar(IGeoLocation location)
            : base(location) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexZmanimCalendar"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="location">The location.</param>
        public ComplexZmanimCalendar(DateTime date, IGeoLocation location)
            : base(date, location) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexZmanimCalendar"/> class.
        /// </summary>
        /// <param name="dateWithLocation">The date with location.</param>
        public ComplexZmanimCalendar(IDateWithLocation dateWithLocation)
            : base(dateWithLocation) { }

        /// <summary>
        /// Default constructor will set a default <see cref="GeoLocation"/>,
        /// a default <see cref="AstronomicalCalculator.GetDefault"> AstronomicalCalculator</see>
        /// and default the calendar to the current date.
        /// </summary>
        /// <seealso cref="AstronomicalCalendar"/>
        public ComplexZmanimCalendar() { }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        /// a 19.8° dip. This calculation divides the day based on the opinion of
        /// the MGA that the day runs from dawn to dusk. Dawn for this calculation is
        /// when the sun is 19.8° below the eastern geometric horizon before
        /// sunrise. Dusk for this is when the sun is 19.8° below the western
        /// geometric horizon after sunset. This day is split into 12 equal parts
        /// with each part being a <em>shaah zmanis</em>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as northern and southern locations even south of the Arctic
        /// Circle and north of the Antarctic Circle where the sun may not
        /// reach low enough below the horizon for this calculation, a
        /// <seealso cref="long.MinValue"/> will be returned. See detailed explanation on
        /// top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanis19Point8Degrees()
        {
            return GetTemporalHour(GetAlos19Point8Degrees(), GetTzais19Point8Degrees());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        /// a 18° dip. This calculation divides the day based on the opinion of
        /// the MGA that the day runs from dawn to dusk. Dawn for this calculation is
        /// when the sun is 18° below the eastern geometric horizon before
        /// sunrise. Dusk for this is when the sun is 18° below the western
        /// geometric horizon after sunset. This day is split into 12 equal parts
        /// with each part being a <em>shaah zmanis</em>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as northern and southern locations even south of the Arctic
        /// Circle and north of the Antarctic Circle where the sun may not
        /// reach low enough below the horizon for this calculation, a
        /// <seealso cref="long.MinValue"/> will be returned. See detailed explanation on
        /// top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanis18Degrees()
        {
            return GetTemporalHour(GetAlos18Degrees(), GetTzais18Degrees());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        /// a dip of 26°. This calculation divides the day based on the opinion
        /// of the MGA that the day runs from dawn to dusk. Dawn for this calculation
        /// is when the sun is <see cref="GetAlos26Degrees">26°</see> below the eastern
        /// geometric horizon before sunrise. Dusk for this is when the sun is
        /// <see cref="GetTzais26Degrees">26°</see> below the western geometric horizon
        /// after sunset. This day is split into 12 equal parts with each part being
        /// a <em>shaah zmanis</em>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a
        /// <em>shaah zmanis</em>. If the calculation can't be computed such
        /// as northern and southern locations even south of the Arctic
        /// Circle and north of the Antarctic Circle where the sun may not
        /// reach low enough below the horizon for this calculation, a
        /// <seealso cref="long.MinValue"/> will be returned. See detailed explanation on
        /// top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanis26Degrees()
        {
            return GetTemporalHour(GetAlos26Degrees(), GetTzais26Degrees());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        /// a dip of 16.1°. This calculation divides the day based on the opinion
        /// that the day runs from dawn to dusk. Dawn for this calculation is when
        /// the sun is 16.1° below the eastern geometric horizon before sunrise
        /// and dusk is when the sun is 16.1° below the western geometric horizon
        /// after sunset. This day is split into 12 equal parts with each part being
        /// a <em>shaah zmanis</em>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as northern and southern locations even south of the Arctic
        /// Circle and north of the Antarctic Circle where the sun may not
        /// reach low enough below the horizon for this calculation, a
        /// <seealso cref="long.MinValue"/> will be returned. See detailed explanation on
        /// top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos16Point1Degrees()"/>
        /// <seealso cref="GetTzais16Point1Degrees()"/>
        /// <seealso cref="GetSofZmanShmaMGA16Point1Degrees()"/>
        /// <seealso cref="GetSofZmanTfilaMGA16Point1Degrees()"/>
        /// <seealso cref="GetMinchaGedola16Point1Degrees()"/>
        /// <seealso cref="GetMinchaKetana16Point1Degrees()"/>
        /// <seealso cref="GetPlagHamincha16Point1Degrees()"/>
        public virtual long GetShaahZmanis16Point1Degrees()
        {
            return GetTemporalHour(GetAlos16Point1Degrees(), GetTzais16Point1Degrees());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (solar hour) according to the
        /// opinion of the MGA. This calculation divides the day based on the opinion
        /// of the <em>MGA</em> that the day runs from dawn to dusk. Dawn for this
        /// calculation is 60 minutes before sunrise and dusk is 60 minutes after
        /// sunset. This day is split into 12 equal parts with each part being a
        /// <em>shaah zmanis</em>. Alternate mothods of calculating a
        /// <em>shaah zmanis</em> are available in the subclass
        /// <see cref="ComplexZmanimCalendar"/>
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanis60Minutes()
        {
            return GetTemporalHour(GetAlos60(), GetTzais60());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (solar hour) according to the
        /// opinion of the MGA. This calculation divides the day based on the opinion
        /// of the <em>MGA</em> that the day runs from dawn to dusk. Dawn for this
        /// calculation is 72 minutes before sunrise and dusk is 72 minutes after
        /// sunset. This day is split into 12 equal parts with each part being a
        /// <em>shaah zmanis</em>. Alternate mothods of calculating a
        /// <em>shaah zmanis</em> are available in the subclass
        /// <see cref="ComplexZmanimCalendar"/>
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanis72Minutes()
        {
            return GetShaahZmanisMGA();
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        /// opinion of the MGA based on <em>alos</em> being
        /// <see cref="GetAlos72Zmanis">72</see> minutes <em>zmaniyos</em> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This calculation divides the day based on
        /// the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        /// for this calculation is 72 minutes <em>zmaniyos</em> before sunrise and
        /// dusk is 72 minutes <em>zmaniyos</em> after sunset. This day is split into
        /// 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        /// identical to 1/10th of the day from <see cref="AstronomicalCalendar.GetSunrise">sunrise</see> to
        /// <see cref="AstronomicalCalendar.GetSunset">sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos72Zmanis()"/>
        /// <seealso cref="GetTzais72Zmanis()"/>
        public virtual long GetShaahZmanis72MinutesZmanis()
        {
            return GetTemporalHour(GetAlos72Zmanis(), GetTzais72Zmanis());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        /// a dip of 90 minutes. This calculation divides the day based on the
        /// opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        /// calculation is 90 minutes before sunrise and dusk is 90 minutes after
        /// sunset. This day is split into 12 equal parts with each part being a
        /// <em>shaah zmanis</em>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanis90Minutes()
        {
            return GetTemporalHour(GetAlos90(), GetTzais90());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        /// opinion of the MGA based on <em>alos</em> being
        /// <see cref="GetAlos90Zmanis">90</see> minutes <em>zmaniyos</em> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This calculation divides the day based on
        /// the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        /// for this calculation is 90 minutes <em>zmaniyos</em> before sunrise and
        /// dusk is 90 minutes <em>zmaniyos</em> after sunset. This day is split into
        /// 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        /// identical to 1/8th of the day from <see cref="AstronomicalCalendar.GetSunrise">sunrise</see> to
        /// <see cref="AstronomicalCalendar.GetSunset">sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos90Zmanis()"/>
        /// <seealso cref="GetTzais90Zmanis()"/>
        public virtual long GetShaahZmanis90MinutesZmanis()
        {
            return GetTemporalHour(GetAlos90Zmanis(), GetTzais90Zmanis());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        /// opinion of the MGA based on <em>alos</em> being
        /// <see cref="GetAlos96Zmanis">96</see> minutes <em>zmaniyos</em> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This calculation divides the day based on
        /// the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        /// for this calculation is 96 minutes <em>zmaniyos</em> before sunrise and
        /// dusk is 96 minutes <em>zmaniyos</em> after sunset. This day is split into
        /// 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        /// identical to 1/7.5th of the day from <see cref="AstronomicalCalendar.GetSunrise">sunrise</see> to
        /// <see cref="AstronomicalCalendar.GetSunset">sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos96Zmanis()"/>
        /// <seealso cref="GetTzais96Zmanis()"/>
        public virtual long GetShaahZmanis96MinutesZmanis()
        {
            return GetTemporalHour(GetAlos96Zmanis(), GetTzais96Zmanis());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        /// opinion of the Chacham Yosef Harari-Raful of Yeshivat Ateret Torah
        /// calculated with <em>alos</em> being 1/10th of sunrise to sunset day, or
        /// <see cref="GetAlos72Zmanis">72</see> minutes <em>zmaniyos</em> of such a day
        /// before <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>, and tzais is usually calculated as
        /// <see cref="GetTzaisAteretTorah">40 minutes</see> after <see cref="AstronomicalCalendar.GetSunset"> sunset</see>.
        /// This day is split into 12 equal parts with each part being a
        /// <em>shaah zmanis</em>. Note that with this system, chatzos (mid-day) will
        /// not be the point that the sun is <see cref="AstronomicalCalendar.GetSunTransit">halfway across the sky</see>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos72Zmanis()"/>
        /// <seealso cref="GetTzaisAteretTorah()"/>
        /// <seealso cref="AteretTorahSunsetOffset"/>
        public virtual long GetShaahZmanisAteretTorah()
        {
            return GetTemporalHour(GetAlos72Zmanis(), GetTzaisAteretTorah());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        /// a dip of 96 minutes. This calculation divides the day based on the
        /// opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        /// calculation is 96 minutes before sunrise and dusk is 96 minutes after
        /// sunset. This day is split into 12 equal parts with each part being a
        /// <em>shaah zmanis</em>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanis96Minutes()
        {
            return GetTemporalHour(GetAlos96(), GetTzais96());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        /// a dip of 120 minutes. This calculation divides the day based on the
        /// opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        /// calculation is 120 minutes before sunrise and dusk is 120 minutes after
        /// sunset. This day is split into 12 equal parts with each part being a
        /// <em>shaah zmanis</em>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual long GetShaahZmanis120Minutes()
        {
            return GetTemporalHour(GetAlos120(), GetTzais120());
        }

        /// <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        /// opinion of the MGA based on <em>alos</em> being
        /// <see cref="GetAlos120Zmanis">120</see> minutes <em>zmaniyos</em> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This calculation divides the day based on
        /// the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        /// for this calculation is 120 minutes <em>zmaniyos</em> before sunrise and
        /// dusk is 120 minutes <em>zmaniyos</em> after sunset. This day is split
        /// into 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        /// identical to 1/6th of the day from <see cref="AstronomicalCalendar.GetSunrise">sunrise</see> to
        /// <see cref="AstronomicalCalendar.GetSunset">sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>long</c> millisecond length of a <em>shaah zmanis</em>.
        /// If the calculation can't be computed such
        /// as in the Arctic Circle where there is at least one day a year
        /// where the sun does not rise, and one where it does not set, a
        /// <see cref="long.MinValue"/> will be returned. See detailed explanation
        /// on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos120Zmanis()"/>
        /// <seealso cref="GetTzais120Zmanis()"/>
        public virtual long GetShaahZmanis120MinutesZmanis()
        {
            return GetTemporalHour(GetAlos120Zmanis(), GetTzais120Zmanis());
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <see cref="GetAlos120Zmanis">dawn</see>. The
        /// formula used is:<br/>
        /// 10.75 * <see cref="GetShaahZmanis120MinutesZmanis()"/> after
        /// <see cref="GetAlos120Zmanis">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha120MinutesZmanis()
        {
            return GetTimeOffset(GetAlos120Zmanis(), GetShaahZmanis120MinutesZmanis() * 10.75);
        }


        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <see cref="GetAlos120">dawn</see>. The formula
        /// used is:<br/>
        /// 10.75 <see cref="GetShaahZmanis120Minutes()"/> after <see cref="GetAlos120()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha120Minutes()
        {
            return GetTimeOffset(GetAlos120(), GetShaahZmanis120Minutes() * 10.75);
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated using 60 minutes before
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> on the time to walk the
        /// distance of 4 <em>Mil</em> at 15 minutes a <em>Mil</em> (the opinion of
        /// the Chavas Yair. See the Divray Malkiel). This is based on the opinion of
        /// most <em>Rishonim</em> who stated that the time of the <em>Neshef</em>
        /// (time between dawn and sunrise) does not vary by the time of year or
        /// location but purely depends on the time it takes to walk the distance of
        /// 4 <em>Mil</em>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetAlos60()
        {
            return GetTimeOffset(GetSeaLevelSunrise(), -60 * MINUTE_MILLIS);
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated using 72 minutes
        /// <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/10th
        /// of the day before sea level sunrise. This is based on an 18 minute
        /// <em>Mil</em> so the time for 4 <em>Mil</em> is 72 minutes which is 1/10th
        /// of a day (12 * 60 = 720) based on the day starting at
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> and ending at
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>. The actual alculation is
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunrise"/>- ( <see cref="ZmanimCalendar.GetShaahZmanisGra"/> * 1.2).
        /// This calculation is used in the calendars published by
        /// <em>Hisachdus Harabanim D'Artzos Habris Ve'Kanada</em>
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetShaahZmanisGra"/>
        public virtual DateTime? GetAlos72Zmanis()
        {
            long shaahZmanis = GetShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return GetTimeOffset(GetSeaLevelSunrise(), (long)(shaahZmanis * -1.2));
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated using 96 minutes before
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> based on the time to walk
        /// the distance of 4 <em>Mil</em> at 24 minutes a <em>Mil</em>. This is
        /// based on the opinion of most <em>Rishonim</em> who stated that the time
        /// of the <em>Neshef</em> (time between dawn and sunrise) does not vary by
        /// the time of year or location but purely depends on the time it takes to
        /// walk the distance of 4 <em>Mil</em>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetAlos96()
        {
            return GetTimeOffset(GetSeaLevelSunrise(), -96 * MINUTE_MILLIS);
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated using 90 minutes
        /// <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/8th
        /// of the day before sea level sunrise. This is based on a 22.5 minute
        /// <em>Mil</em> so the time for 4 <em>Mil</em> is 90 minutes which is 1/8th
        /// of a day (12 * 60 = 720) /8 =90 based on the day starting at
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see> and ending at <seealso cref="AstronomicalCalendar.GetSunset">sunset</seealso>.
        /// The actual calculation is <see cref="AstronomicalCalendar.GetSunrise"/> - (
        /// <see cref="ZmanimCalendar.GetShaahZmanisGra"/> * 1.5).
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetShaahZmanisGra"/>
        public virtual DateTime? GetAlos90Zmanis()
        {
            long shaahZmanis = GetShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
                return null;

            return GetTimeOffset(GetSeaLevelSunrise(), (long)(shaahZmanis * -1.5));
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated using 90 minutes
        /// <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/8th
        /// of the day before sea level sunrise. This is based on a 24 minute
        /// <em>Mil</em> so the time for 4 <em>Mil</em> is 90 minutes which is
        /// 1/7.5th of a day (12 * 60 = 720) / 7.5 =96 based on the day starting at
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see> and ending at <see cref="AstronomicalCalendar.GetSunset">sunset</see>.
        /// The actual calculation is <seealso cref="AstronomicalCalendar.GetSunrise"/> - (
        /// <see cref="ZmanimCalendar.GetShaahZmanisGra"/> * 1.6).
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetShaahZmanisGra"/>
        public virtual DateTime? GetAlos96Zmanis()
        {
            long shaahZmanis = GetShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return GetTimeOffset(GetSeaLevelSunrise(), (long)(shaahZmanis * -1.6));
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated using 90 minutes before
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> on the time to walk the
        /// distance of 4 <em>Mil</em> at 22.5 minutes a <em>Mil</em>. This is based
        /// on the opinion of most <em>Rishonim</em> who stated that the time of the
        /// <em>Neshef</em> (time between dawn and sunrise) does not vary by the time
        /// of year or location but purely depends on the time it takes to walk the
        /// distance of 4 <em>Mil</em>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetAlos90()
        {
            return GetTimeOffset(GetSeaLevelSunrise(), -90 * MINUTE_MILLIS);
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated using 120 minutes before
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see> (no adjustment for
        /// elevation is made) based on the time to walk the distance of 5
        /// <em>Mil</em>( <em>Ula</em>) at 24 minutes a <em>Mil</em>. This is based
        /// on the opinion of most <em>Rishonim</em> who stated that the time of the
        /// <em>Neshef</em> (time between dawn and sunrise) does not vary by the time
        /// of year or location but purely depends on the time it takes to walk the
        /// distance of 5 <em>Mil</em>(<em>Ula</em>).
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetAlos120()
        {
            return GetTimeOffset(GetSeaLevelSunrise(), -120 * MINUTE_MILLIS);
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated using 120 minutes
        /// <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/6th
        /// of the day before sea level sunrise. This is based on a 24 minute
        /// <em>Mil</em> so the time for 5 <em>Mil</em> is 120 minutes which is 1/6th
        /// of a day (12 * 60 = 720) / 6 =120 based on the day starting at
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see> and ending at <see cref="AstronomicalCalendar.GetSunset">sunset</see>.
        /// The actual calculation is <seealso cref="AstronomicalCalendar.GetSunrise"/> - (
        /// <see cref="ZmanimCalendar.GetShaahZmanisGra"/> * 2).
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetShaahZmanisGra"/>
        public virtual DateTime? GetAlos120Zmanis()
        {
            long shaahZmanis = GetShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return GetTimeOffset(GetSeaLevelSunrise(), shaahZmanis * -2);
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated when the sun is
        /// <see cref="ZENITH_26_DEGREES">26°</see> below the eastern geometric horizon
        /// before sunrise. This calculation is based on the same calculation of
        /// <see cref="GetAlos120">120 minutes</see> but uses a degree based calculation
        /// instead of 120 exact minutes. This calculation is based on the position
        /// of the sun 120 minutes before sunrise in Jerusalem in the equinox which
        /// calculates to 26° below <see cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing <em>alos</em>.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="ZENITH_26_DEGREES"/>
        /// <seealso cref="GetAlos120()"/>
        /// <seealso cref="GetTzais120()"/>
        public virtual DateTime? GetAlos26Degrees()
        {
            return GetSunriseOffsetByDegrees(ZENITH_26_DEGREES);
        }

        /// <summary>
        /// to return <em>alos</em> (dawn) calculated when the sun is
        /// <see cref="AstronomicalCalendar.ASTRONOMICAL_ZENITH">18°</see> below the eastern geometric horizon
        /// before sunrise.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing <em>alos</em>.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="AstronomicalCalendar.ASTRONOMICAL_ZENITH"/>
        public virtual DateTime? GetAlos18Degrees()
        {
            return GetSunriseOffsetByDegrees(ASTRONOMICAL_ZENITH);
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated when the sun is
        /// <seealso cref="ZENITH_19_POINT_8">19.8°</seealso> below the eastern geometric horizon
        /// before sunrise. This calculation is based on the same calculation of
        /// <seealso cref="GetAlos90">90 minutes</seealso> but uses a degree based calculation
        /// instead of 90 exact minutes. This calculation is based on the position of
        /// the sun 90 minutes before sunrise in Jerusalem in the equinox which
        /// calculates to 19.8° below <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing <em>alos</em>.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="ZENITH_19_POINT_8"/>
        /// <seealso cref="GetAlos90()"/>
        public virtual DateTime? GetAlos19Point8Degrees()
        {
            return GetSunriseOffsetByDegrees(ZENITH_19_POINT_8);
        }

        /// <summary>
        /// Method to return <em>alos</em> (dawn) calculated when the sun is
        /// <seealso cref="ZmanimCalendar.ZENITH_16_POINT_1">16.1°</seealso> below the eastern geometric horizon
        /// before sunrise. This calculation is based on the same calculation of
        /// <seealso cref="ZmanimCalendar.GetAlos72">72 minutes</seealso> but uses a degree based calculation
        /// instead of 72 exact minutes. This calculation is based on the position of
        /// the sun 72 minutes before sunrise in Jerusalem in the equinox which
        /// calculates to 16.1° below <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing <em>alos</em>.
        /// If the
        /// calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.ZENITH_16_POINT_1"/>
        /// <seealso cref="ZmanimCalendar.GetAlos72"/>
        public virtual DateTime? GetAlos16Point1Degrees()
        {
            return GetSunriseOffsetByDegrees(ZENITH_16_POINT_1);
        }

        /// <summary>
        /// This method returns <em>misheyakir</em> based on the position of the sun
        /// when it is <seealso cref="ZENITH_11_DEGREES">11.5°</seealso> below
        /// <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso> (90°). This calculation is
        /// used for calculating <em>misheyakir</em> according to some opinions. This
        /// calculation is based on the position of the sun 52 minutes before
        /// <seealso cref="AstronomicalCalendar.GetSunrise">sunrise</seealso>in Jerusalem in the equinox which calculates
        /// to 11.5° below <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of misheyakir. If the calculation can't be
        /// computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// ///
        /// <seealso cref="ZENITH_11_POINT_5"/>
        public virtual DateTime? GetMisheyakir11Point5Degrees()
        {
            return GetSunriseOffsetByDegrees(ZENITH_11_POINT_5);
        }

        /// <summary>
        /// This method returns <em>misheyakir</em> based on the position of the sun
        /// when it is <seealso cref="ZENITH_11_DEGREES">11°</seealso> below
        /// <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso> (90°). This calculation is
        /// used for calculating <em>misheyakir</em> according to some opinions. This
        /// calculation is based on the position of the sun 48 minutes before
        /// <seealso cref="AstronomicalCalendar.GetSunrise">sunrise</seealso>in Jerusalem in the equinox which calculates
        /// to 11° below <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>
        /// </summary>
        /// <returns>
        /// If the calculation can't be computed such as northern and
        /// southern locations even south of the Arctic Circle and north of
        /// the Antarctic Circle where the sun may not reach low enough below
        /// the horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="ZENITH_11_DEGREES"/>
        public virtual DateTime? GetMisheyakir11Degrees()
        {
            return GetSunriseOffsetByDegrees(ZENITH_11_DEGREES);
        }

        /// <summary>
        /// This method returns <em>misheyakir</em> based on the position of the sun
        /// when it is <seealso cref="ZENITH_10_POINT_2">10.2°</seealso> below
        /// <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso> (90°). This calculation is
        /// used for calculating <em>misheyakir</em> according to some opinions. This
        /// calculation is based on the position of the sun 45 minutes before
        /// <seealso cref="AstronomicalCalendar.GetSunrise">sunrise</seealso> in Jerusalem in the equinox which calculates
        /// to 10.2° below <seealso cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</seealso>
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest misheyakir.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="ZENITH_10_POINT_2"/>
        public virtual DateTime? GetMisheyakir10Point2Degrees()
        {
            return GetSunriseOffsetByDegrees(ZENITH_10_POINT_2);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <seealso cref="GetAlos19Point8Degrees()">19.8°</seealso> before
        /// <seealso cref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 3
        /// <em><seealso cref="GetShaahZmanis19Point8Degrees">shaos zmaniyos</seealso></em> (solar
        /// hours) after <seealso cref="GetAlos19Point8Degrees">dawn</seealso> based on the opinion
        /// of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        /// with both being 19.8° below sunrise or sunset. This returns the time
        /// of 3 <seealso cref="GetShaahZmanis19Point8Degrees()"/> after
        /// <seealso cref="GetAlos19Point8Degrees">dawn</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis19Point8Degrees()"/>
        /// <seealso cref="GetAlos19Point8Degrees()"/>
        public virtual DateTime? GetSofZmanShmaMGA19Point8Degrees()
        {
            return GetTimeOffset(GetAlos19Point8Degrees(), GetShaahZmanis19Point8Degrees() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <seealso cref="GetAlos16Point1Degrees()">16.1°</seealso> before
        /// <seealso cref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 3
        /// <em><seealso cref="GetShaahZmanis16Point1Degrees">shaos zmaniyos</seealso></em> (solar
        /// hours) after <seealso cref="GetAlos16Point1Degrees">dawn</seealso> based on the opinion
        /// of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        /// with both being 16.1° below sunrise or sunset. This returns the time
        /// of 3 <seealso cref="GetShaahZmanis16Point1Degrees()"/> after
        /// <seealso cref="GetAlos16Point1Degrees">dawn</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis16Point1Degrees()"/>
        /// <seealso cref="GetAlos16Point1Degrees()"/>
        public virtual DateTime? GetSofZmanShmaMGA16Point1Degrees()
        {
            return GetTimeOffset(GetAlos16Point1Degrees(), GetShaahZmanis16Point1Degrees() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="ZmanimCalendar.GetAlos72">72</see> minutes before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        /// <em><see cref="GetShaahZmanis72Minutes">shaos zmaniyos</see></em> (solar hours)
        /// after <see cref="ZmanimCalendar.GetAlos72">dawn</see> based on the opinion of the <em>MG"A</em>
        /// that the day is calculated from a <see cref="ZmanimCalendar.GetAlos72">dawn</see> of 72 minutes
        /// before sunrise to <see cref="ZmanimCalendar.GetTzais72">nightfall</see> of 72 minutes after
        /// sunset. This returns the time of 3 * <seealso cref="GetShaahZmanis72Minutes()"/>
        /// after <see cref="ZmanimCalendar.GetAlos72">dawn</see>. This class returns an identical time to
        /// <see cref="ZmanimCalendar.GetSofZmanShmaMGA"/> and is repeated here for clarity.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis72Minutes()"/>
        /// <seealso cref="ZmanimCalendar.GetAlos72"/>
        /// <seealso cref="ZmanimCalendar.GetSofZmanShmaMGA"/>
        public virtual DateTime? GetSofZmanShmaMGA72Minutes()
        {
            return GetSofZmanShmaMGA();
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos72Zmanis">72</see> minutes
        /// <em>zmaniyos</em>, or 1/10th of the day before <see cref="AstronomicalCalendar.GetSunrise"> sunrise</see>
        /// . This time is 3
        /// <em><see cref="GetShaahZmanis90MinutesZmanis">shaos zmaniyos</see></em> (solar
        /// hours) after <see cref="GetAlos72Zmanis">dawn</see> based on the opinion of the
        /// <em>MG"A</em> that the day is calculated from a
        /// <see cref="GetAlos72Zmanis">dawn</see> of 72 minutes <em>zmaniyos</em>, or
        /// 1/10th of the day before <see cref="AstronomicalCalendar.GetSeaLevelSunrise">sea level sunrise</see>
        /// to <see cref="GetTzais72Zmanis">nightfall</see> of 72 minutes <em>zmaniyos</em>
        /// after <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>. This returns the
        /// time of 3 * <see cref="GetShaahZmanis72MinutesZmanis()"/> after
        /// <see cref="GetAlos72Zmanis">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis72MinutesZmanis()"/>
        /// <seealso cref="GetAlos72Zmanis()"/>
        public virtual DateTime? GetSofZmanShmaMGA72MinutesZmanis()
        {
            return GetTimeOffset(GetAlos72Zmanis(), GetShaahZmanis72MinutesZmanis() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos90">90</see> minutes before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        /// <em><see cref="GetShaahZmanis90Minutes">shaos zmaniyos</see></em> (solar hours)
        /// after <see cref="GetAlos90">dawn</see> based on the opinion of the <em>MG"A</em>
        /// that the day is calculated from a <see cref="GetAlos90">dawn</see> of 90 minutes
        /// before sunrise to <see cref="GetTzais90">nightfall</see> of 90 minutes after
        /// sunset. This returns the time of 3 * <see cref="GetShaahZmanis90Minutes()"/>
        /// after <see cref="GetAlos90">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis90Minutes()"/>
        /// <seealso cref="GetAlos90()"/>
        public virtual DateTime? GetSofZmanShmaMGA90Minutes()
        {
            return GetTimeOffset(GetAlos90(), GetShaahZmanis90Minutes() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos90Zmanis">90</see> minutes
        /// <em>zmaniyos</em> before <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        /// <em><see cref="GetShaahZmanis90MinutesZmanis">shaos zmaniyos</see></em> (solar
        /// hours) after <see cref="GetAlos90Zmanis">dawn</see> based on the opinion of the
        /// <em>MG"A</em> that the day is calculated from a
        /// <see cref="GetAlos90Zmanis">dawn</see> of 90 minutes <em>zmaniyos</em> before
        /// sunrise to <see cref="GetTzais90Zmanis">nightfall</see> of 90 minutes
        /// <em>zmaniyos</em> after sunset. This returns the time of 3 *
        /// <see cref="GetShaahZmanis90MinutesZmanis()"/> after <see cref="GetAlos90Zmanis()"> dawn</see>
        /// .
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis90MinutesZmanis()"/>
        /// <seealso cref="GetAlos90Zmanis()"/>
        public virtual DateTime? GetSofZmanShmaMGA90MinutesZmanis()
        {
            return GetTimeOffset(GetAlos90Zmanis(), GetShaahZmanis90MinutesZmanis() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos96">96</see> minutes before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        /// <em><see cref="GetShaahZmanis96Minutes">shaos zmaniyos</see></em> (solar hours)
        /// after <see cref="GetAlos96">dawn</see> based on the opinion of the <em>MG"A</em>
        /// that the day is calculated from a <see cref="GetAlos96">dawn</see> of 96 minutes
        /// before sunrise to <see cref="GetTzais96">nightfall</see> of 96 minutes after
        /// sunset. This returns the time of 3 * <see cref="GetShaahZmanis96Minutes()"/>
        /// after <see cref="GetAlos96">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis96Minutes()"/>
        /// <seealso cref="GetAlos96()"/>
        public virtual DateTime? GetSofZmanShmaMGA96Minutes()
        {
            return GetTimeOffset(GetAlos96(), GetShaahZmanis96Minutes() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos90Zmanis">96</see> minutes
        /// <em>zmaniyos</em> before <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        /// <em><see cref="GetShaahZmanis96MinutesZmanis">shaos zmaniyos</see></em> (solar
        /// hours) after <see cref="GetAlos96Zmanis">dawn</see> based on the opinion of the
        /// <em>MG"A</em> that the day is calculated from a
        /// <see cref="GetAlos96Zmanis">dawn</see> of 96 minutes <em>zmaniyos</em> before
        /// sunrise to <see cref="GetTzais90Zmanis">nightfall</see> of 96 minutes
        /// <em>zmaniyos</em> after sunset. This returns the time of 3 *
        /// <see cref="GetShaahZmanis96MinutesZmanis()"/> after <see cref="GetAlos96Zmanis()"> dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis96MinutesZmanis()"/>
        /// <seealso cref="GetAlos96Zmanis()"/>
        public virtual DateTime? GetSofZmanShmaMGA96MinutesZmanis()
        {
            return GetTimeOffset(GetAlos96Zmanis(), GetShaahZmanis96MinutesZmanis() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) calculated as 3 hours (regular and not zmaniyos)
        /// before <see cref="ZmanimCalendar.GetChatzos"/>. This is the opinion of the
        /// <em>Shach</em> in the
        /// <em>Nekudas Hakesef (Yora Deah 184), Shevus Yaakov, Chasan Sofer</em> and
        /// others.This returns the time of 3 hours before
        /// <see cref="ZmanimCalendar.GetChatzos"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetChatzos"/>
        /// <seealso cref="GetSofZmanTfila2HoursBeforeChatzos()"/>
        public virtual DateTime? GetSofZmanShma3HoursBeforeChatzos()
        {
            return GetTimeOffset(GetChatzos(), -180 * MINUTE_MILLIS);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos120">120</see> minutes or 1/6th of the day
        /// before <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 3
        /// <em><see cref="GetShaahZmanis120Minutes">shaos zmaniyos</see></em> (solar hours)
        /// after <see cref="GetAlos120">dawn</see> based on the opinion of the
        /// <em>MG"A</em> that the day is calculated from a <see cref="GetAlos120()"> dawn</see>
        /// of 120 minutes before sunrise to <see cref="GetTzais120">nightfall</see>
        /// of 120 minutes after sunset. This returns the time of 3 *
        /// <see cref="GetShaahZmanis120Minutes()"/> after <see cref="GetAlos120">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis120Minutes()"/>
        /// <seealso cref="GetAlos120()"/>
        public virtual DateTime? GetSofZmanShmaMGA120Minutes()
        {
            return GetTimeOffset(GetAlos120(), GetShaahZmanis120Minutes() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) based on the opinion that the day starts at
        /// <em><see cref="GetAlos16Point1Degrees">alos 16.1°</see></em> and ends at
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>. 3 shaos zmaniyos are
        /// calculated based on this day and added to
        /// <see cref="GetAlos16Point1Degrees">alos</see>to reach this time. This time is 3
        /// <em>shaos zmaniyos</em> (solar hours) after
        /// <see cref="GetAlos16Point1Degrees">dawn</see> based on the opinion that the day
        /// is calculated from a <see cref="GetAlos16Point1Degrees">alos 16.1°</see> to
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.<br/>
        /// 	<b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        /// midday.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema based on this day.
        /// If the calculation can't be computed such as northern and
        /// southern locations even south of the Arctic Circle and north of
        /// the Antarctic Circle where the sun may not reach low enough below
        /// the horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="GetAlos16Point1Degrees()"/>
        /// <seealso cref="AstronomicalCalendar.GetSeaLevelSunset"/>
        public virtual DateTime? GetSofZmanShmaAlos16Point1ToSunset()
        {
            long shaahZmanis = GetTemporalHour(GetAlos16Point1Degrees(), GetSeaLevelSunset());
            return GetTimeOffset(GetAlos16Point1Degrees(), shaahZmanis * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) based on the opinion that the day starts at
        /// <em><see cref="GetAlos16Point1Degrees">alos 16.1°</see></em> and ends at
        /// <see cref="GetTzaisGeonim7Point083Degrees">tzais 7.083°</see>. 3
        /// <em>shaos zmaniyos</em> are calculated based on this day and added to
        /// <see cref="GetAlos16Point1Degrees">alos</see> to reach this time. This time is 3
        /// <em>shaos zmaniyos</em> (temporal hours) after
        /// <see cref="GetAlos16Point1Degrees">alos 16.1°</see> based on the opinion
        /// that the day is calculated from a <see cref="GetAlos16Point1Degrees()">alos 16.1°</see>
        /// to
        /// <em><see cref="GetTzaisGeonim7Point083Degrees">tzais 7.083°</see></em>.<br/>
        /// 	<b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        /// midday.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema based on this
        /// calculation. If the calculation can't be computed such as
        /// northern and southern locations even south of the Arctic Circle
        /// and north of the Antarctic Circle where the sun may not reach low
        /// enough below the horizon for this calculation, a null will be
        /// returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos16Point1Degrees()"/>
        /// <seealso cref="GetTzaisGeonim7Point083Degrees()"/>
        public virtual DateTime? GetSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            long shaahZmanis = GetTemporalHour(GetAlos16Point1Degrees(), GetTzaisGeonim7Point083Degrees());
            return GetTimeOffset(GetAlos16Point1Degrees(), shaahZmanis * 3);
        }

        /// <summary>
        /// From the GR"A in Kol Eliyahu on Berachos #173 that states that zman krias
        /// shema is calculated as half the time from <see cref="AstronomicalCalendar.GetSeaLevelSunrise"> sea level sunset</see>
        /// to fixed local chatzos. The GR"A himself seems to
        /// contradic this when he stated that zman krias shema is 1/4 of the day
        /// from sunrise to sunset. See Sarah Lamoed #25 in Yisroel Vehazmanim Vol
        /// III page 1016.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema based on this calculation.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetFixedLocalChatzos()"/>
        public virtual DateTime? GetSofZmanShmaKolEliyahu()
        {
            DateTime? chatzos = GetFixedLocalChatzos();
            if (chatzos == DateTime.MinValue || GetSunrise() == DateTime.MinValue)
            {
                return null;
            }
            long diff = (chatzos.Value.ToUnixEpochMilliseconds() - GetSeaLevelSunrise().Value.ToUnixEpochMilliseconds()) / 2;
            return GetTimeOffset(chatzos, -diff);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos19Point8Degrees()">19.8°</see> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        /// <em><see cref="GetShaahZmanis19Point8Degrees">shaos zmaniyos</see></em> (solar
        /// hours) after <see cref="GetAlos19Point8Degrees">dawn</see> based on the opinion
        /// of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        /// with both being 19.8° below sunrise or sunset. This returns the time
        /// of 4 <see cref="GetShaahZmanis19Point8Degrees()"/> after
        /// <see cref="GetAlos19Point8Degrees">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis19Point8Degrees()"/>
        /// <seealso cref="GetAlos19Point8Degrees()"/>
        public virtual DateTime? GetSofZmanTfilaMGA19Point8Degrees()
        {
            return GetTimeOffset(GetAlos19Point8Degrees(), GetShaahZmanis19Point8Degrees() * 4);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos19Point8Degrees()">16.1°</see> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        /// <em><see cref="GetShaahZmanis16Point1Degrees">shaos zmaniyos</see></em> (solar
        /// hours) after <see cref="GetAlos16Point1Degrees">dawn</see> based on the opinion
        /// of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        /// with both being 16.1° below sunrise or sunset. This returns the time
        /// of 4 <see cref="GetShaahZmanis16Point1Degrees()"/> after
        /// <see cref="GetAlos16Point1Degrees">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis16Point1Degrees()"/>
        /// <seealso cref="GetAlos16Point1Degrees()"/>
        public virtual DateTime? GetSofZmanTfilaMGA16Point1Degrees()
        {
            return GetTimeOffset(GetAlos16Point1Degrees(), GetShaahZmanis16Point1Degrees() * 4);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="ZmanimCalendar.GetAlos72">72</see> minutes before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        /// <em><see cref="GetShaahZmanis72Minutes">shaos zmaniyos</see></em> (solar hours)
        /// after <see cref="ZmanimCalendar.GetAlos72">dawn</see> based on the opinion of the <em>MG"A</em>
        /// that the day is calculated from a <see cref="ZmanimCalendar.GetAlos72">dawn</see> of 72 minutes
        /// before sunrise to <see cref="ZmanimCalendar.GetTzais72">nightfall</see> of 72 minutes after
        /// sunset. This returns the time of 4 * <see cref="GetShaahZmanis72Minutes()"/>
        /// after <see cref="ZmanimCalendar.GetAlos72">dawn</see>. This class returns an identical time to
        /// <see cref="ZmanimCalendar.GetSofZmanTfilaMGA"/> and is repeated here for clarity.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman tfila.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis72Minutes()"/>
        /// <seealso cref="ZmanimCalendar.GetAlos72"/>
        /// <seealso cref="ZmanimCalendar.GetSofZmanShmaMGA"/>
        public virtual DateTime? GetSofZmanTfilaMGA72Minutes()
        {
            return GetSofZmanTfilaMGA();
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to the morning
        /// prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        /// <see cref="GetAlos72Zmanis">72</see> minutes <em>zmaniyos</em> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        /// <em><see cref="GetShaahZmanis72MinutesZmanis">shaos zmaniyos</see></em> (solar
        /// hours) after <see cref="GetAlos72Zmanis">dawn</see> based on the opinion of the
        /// <em>MG"A</em> that the day is calculated from a
        /// <see cref="GetAlos72Zmanis">dawn</see> of 72 minutes <em>zmaniyos</em> before
        /// sunrise to <see cref="GetTzais72Zmanis">nightfall</see> of 72 minutes
        /// <em>zmaniyos</em> after sunset. This returns the time of 4 *
        /// <see cref="GetShaahZmanis72MinutesZmanis()"/> after <see cref="GetAlos72Zmanis()"> dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis72MinutesZmanis()"/>
        /// <seealso cref="GetAlos72Zmanis()"/>
        public virtual DateTime? GetSofZmanTfilaMGA72MinutesZmanis()
        {
            return GetTimeOffset(GetAlos72Zmanis(), GetShaahZmanis72MinutesZmanis() * 4);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <seealso cref="GetAlos90">90</seealso> minutes before
        /// <seealso cref="AstronomicalCalendar.GetSunrise">sunrise</seealso>. This time is 4
        /// <em><seealso cref="GetShaahZmanis90Minutes">shaos zmaniyos</seealso></em> (solar hours)
        /// after <seealso cref="GetAlos90">dawn</seealso> based on the opinion of the <em>MG"A</em>
        /// that the day is calculated from a <seealso cref="GetAlos90">dawn</seealso> of 90 minutes
        /// before sunrise to <seealso cref="GetTzais90">nightfall</seealso> of 90 minutes after
        /// sunset. This returns the time of 4 * <seealso cref="GetShaahZmanis90Minutes()"/>
        /// after <seealso cref="GetAlos90">dawn</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman tfila.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis90Minutes()"/>
        /// <seealso cref="GetAlos90()"/>
        public virtual DateTime? GetSofZmanTfilaMGA90Minutes()
        {
            return GetTimeOffset(GetAlos90(), GetShaahZmanis90Minutes() * 4);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to the morning
        /// prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        /// <see cref="GetAlos90Zmanis">90</see> minutes <em>zmaniyos</em> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        /// <em><see cref="GetShaahZmanis90MinutesZmanis">shaos zmaniyos</see></em> (solar
        /// hours) after <see cref="GetAlos90Zmanis">dawn</see> based on the opinion of the
        /// <em>MG"A</em> that the day is calculated from a
        /// <see cref="GetAlos90Zmanis">dawn</see> of 90 minutes <em>zmaniyos</em> before
        /// sunrise to <seealso cref="GetTzais90Zmanis">nightfall</seealso> of 90 minutes
        /// <em>zmaniyos</em> after sunset. This returns the time of 4 *
        /// <see cref="GetShaahZmanis90MinutesZmanis()"/> after <see cref="GetAlos90Zmanis()"> dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis90MinutesZmanis()"/>
        /// <seealso cref="GetAlos90Zmanis()"/>
        public virtual DateTime? GetSofZmanTfilaMGA90MinutesZmanis()
        {
            return GetTimeOffset(GetAlos90Zmanis(), GetShaahZmanis90MinutesZmanis() * 4);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos96">96</see> minutes before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        /// <em><see cref="GetShaahZmanis96Minutes">shaos zmaniyos</see></em> (solar hours)
        /// after <see cref="GetAlos96">dawn</see> based on the opinion of the <em>MG"A</em>
        /// that the day is calculated from a <see cref="GetAlos96">dawn</see> of 96 minutes
        /// before sunrise to <see cref="GetTzais96">nightfall</see> of 96 minutes after
        /// sunset. This returns the time of 4 * <see cref="GetShaahZmanis96Minutes()"/>
        /// after <see cref="GetAlos96">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman tfila.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis96Minutes()"/>
        /// <seealso cref="GetAlos96()"/>
        public virtual DateTime? GetSofZmanTfilaMGA96Minutes()
        {
            return GetTimeOffset(GetAlos96(), GetShaahZmanis96Minutes() * 4);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to the morning
        /// prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        /// <see cref="GetAlos96Zmanis">96</see> minutes <em>zmaniyos</em> before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        /// <em><see cref="GetShaahZmanis96MinutesZmanis">shaos zmaniyos</see></em> (solar
        /// hours) after <see cref="GetAlos96Zmanis">dawn</see> based on the opinion of the
        /// <em>MG"A</em> that the day is calculated from a
        /// <see cref="GetAlos96Zmanis">dawn</see> of 96 minutes <em>zmaniyos</em> before
        /// sunrise to <see cref="GetTzais96Zmanis">nightfall</see> of 96 minutes
        /// <em>zmaniyos</em> after sunset. This returns the time of 4 *
        /// <see cref="GetShaahZmanis96MinutesZmanis()"/> after <see cref="GetAlos96Zmanis()"> dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis90MinutesZmanis()"/>
        /// <seealso cref="GetAlos90Zmanis()"/>
        public virtual DateTime? GetSofZmanTfilaMGA96MinutesZmanis()
        {
            return GetTimeOffset(GetAlos96Zmanis(), GetShaahZmanis96MinutesZmanis() * 4);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) in the opinion of the <em>MG"A</em> based on
        /// <em>alos</em> being <see cref="GetAlos120">120</see> minutes before
        /// <see cref="AstronomicalCalendar.GetSunrise">sunrise</see>. This time is 4
        /// <em><see cref="GetShaahZmanis120Minutes">shaos zmaniyos</see></em> (solar hours)
        /// after <see cref="GetAlos120">dawn</see> based on the opinion of the
        /// <em>MG"A</em> that the day is calculated from a <see cref="GetAlos120()"> dawn</see>
        /// of 120 minutes before sunrise to <see cref="GetTzais120">nightfall</see>
        /// of 120 minutes after sunset. This returns the time of 4 *
        /// <see cref="GetShaahZmanis120Minutes()"/> after <see cref="GetAlos120">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis120Minutes()"/>
        /// <seealso cref="GetAlos120()"/>
        public virtual DateTime? GetSofZmanTfilaMGA120Minutes()
        {
            return GetTimeOffset(GetAlos120(), GetShaahZmanis120Minutes() * 4);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) calculated as 2 hours befor
        /// <see cref="ZmanimCalendar.GetChatzos"/>. This is based on the opinions that
        /// calculate <em>sof zman krias shema</em> as
        /// <see cref="GetSofZmanShma3HoursBeforeChatzos()"/>. This returns the time of 2
        /// hours before <seealso cref="ZmanimCalendar.GetChatzos"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetChatzos"/>
        /// <seealso cref="GetSofZmanShma3HoursBeforeChatzos()"/>
        public virtual DateTime? GetSofZmanTfila2HoursBeforeChatzos()
        {
            return GetTimeOffset(GetChatzos(), -120 * MINUTE_MILLIS);
        }

        /// <summary>
        /// This method returns mincha gedola calculated as 30 minutes after
        /// <em><see cref="ZmanimCalendar.GetChatzos">chatzos</see></em> and not 1/2 of a
        /// <em><see cref="ZmanimCalendar.GetShaahZmanisGra">shaah zmanis</see></em> after
        /// <em><see cref="ZmanimCalendar.GetChatzos">chatzos</see></em> as calculated by
        /// <see cref="ZmanimCalendar.GetMinchaGedola"/>. Some use this time to delay the start of mincha
        /// in the winter when 1/2 of a
        /// <em><see cref="ZmanimCalendar.GetShaahZmanisGra">shaah zmanis</see></em> is less than 30
        /// minutes. See <seealso cref="GetMinchaGedolaGreaterThan30()"/>for a conveniance
        /// method that returns the later of the 2 calculations. One should not use
        /// this time to start <em>mincha</em> before the standard
        /// <em><see cref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. See <em>Shulchan Aruch
        /// Orach Chayim Siman Raish Lamed Gimel seif alef</em> and the
        /// <em>Shaar Hatziyon seif katan ches</em>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of 30 mintes after <em>chatzos</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        /// <seealso cref="GetMinchaGedolaGreaterThan30()"/>
        public virtual DateTime? GetMinchaGedola30Minutes()
        {
            return GetTimeOffset(GetChatzos(), MINUTE_MILLIS * 30);
        }

        /// <summary>
        /// This method returns the time of <em>mincha gedola</em> according to the
        /// Magen Avraham with the day starting 72 minutes before sunrise and ending
        /// 72 minutes after sunset. This is the earliest time to pray
        /// <em>mincha</em>. For more information on this see the documentation on
        /// <em><see cref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        /// 6.5 <see cref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)">solar hours</see> after alos. The calculation
        /// used is 6.5 * <see cref="GetShaahZmanis72Minutes()"/> after
        /// <see cref="ZmanimCalendar.GetAlos72">alos</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of mincha gedola.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetAlos72"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaKetana"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        public virtual DateTime? GetMinchaGedola72Minutes()
        {
            return GetTimeOffset(GetAlos72(), GetShaahZmanis72Minutes() * 6.5);
        }

        /// <summary>
        /// This method returns the time of <em>mincha gedola</em> according to the
        /// Magen Avraham with the day starting and ending 16.1° below the
        /// horizon. This is the earliest time to pray <em>mincha</em>. For more
        /// information on this see the documentation on
        /// <em><see cref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        /// 6.5 <see cref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)">solar hours</see> after alos. The calculation
        /// used is 6.5 * <see cref="GetShaahZmanis16Point1Degrees()"/> after
        /// <see cref="GetAlos16Point1Degrees">alos</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of mincha gedola.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis16Point1Degrees()"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaKetana"/>
        public virtual DateTime? GetMinchaGedola16Point1Degrees()
        {
            return GetTimeOffset(GetAlos16Point1Degrees(), GetShaahZmanis16Point1Degrees() * 6.5);
        }

        /// <summary>
        /// This is a conveniance methd that returns the later of
        /// <see cref="ZmanimCalendar.GetMinchaGedola"/> and <see cref="GetMinchaGedola30Minutes()"/>. In
        /// the winter when a <em><see cref="ZmanimCalendar.GetShaahZmanisGra">shaah zmanis</see></em> is
        /// less than 30 minutes <see cref="GetMinchaGedola30Minutes()"/> will be
        /// returned, otherwise <see cref="ZmanimCalendar.GetMinchaGedola"/> will be returned.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the later of <see cref="ZmanimCalendar.GetMinchaGedola"/>
        /// and <see cref="GetMinchaGedola30Minutes()"/>
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetMinchaGedolaGreaterThan30()
        {
            if (GetMinchaGedola30Minutes() == null || GetMinchaGedola() == null)
                return null;

            return GetMinchaGedola30Minutes().Value.CompareTo(GetMinchaGedola()) > 0
                       ? GetMinchaGedola30Minutes()
                       : GetMinchaGedola();
        }

        /// <summary>
        /// This method returns the time of <em>mincha ketana</em> according to the
        /// Magen Avraham with the day starting and ending 16.1° below the
        /// horizon. This is the perfered earliest time to pray <em>mincha</em> in
        /// the opinion of the Ramba"m and others. For more information on this see
        /// the documentation on <em><seealso cref="ZmanimCalendar.GetMinchaGedola">mincha gedola</seealso></em>.
        /// This is calculated as 9.5 <seealso cref="AstronomicalCalendar.GetTemporalHour(System.DateTime,System.DateTime)">solar hours</seealso> after
        /// alos. The calculation used is 9.5 *
        /// <seealso cref="GetShaahZmanis16Point1Degrees()"/> after
        /// <seealso cref="GetAlos16Point1Degrees">alos</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of mincha ketana.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis16Point1Degrees()"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaKetana"/>
        public virtual DateTime? GetMinchaKetana16Point1Degrees()
        {
            return GetTimeOffset(GetAlos16Point1Degrees(), GetShaahZmanis16Point1Degrees() * 9.5);
        }

        /// <summary>
        /// This method returns the time of <em>mincha ketana</em> according to the
        /// Magen Avraham with the day starting 72 minutes before sunrise and ending
        /// 72 minutes after sunset. This is the perfered earliest time to pray
        /// <em>mincha</em> in the opinion of the Ramba"m and others. For more
        /// information on this see the documentation on
        /// <em><see cref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        /// 9.5 <see cref="GetShaahZmanis72Minutes()"/> after alos. The calculation used
        /// is 9.5 * <see cref="GetShaahZmanis72Minutes()"/> after <see cref="ZmanimCalendar.GetAlos72"> alos</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of mincha ketana.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis16Point1Degrees()"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaKetana"/>
        public virtual DateTime? GetMinchaKetana72Minutes()
        {
            return GetTimeOffset(GetAlos72(), GetShaahZmanis72Minutes() * 9.5);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <see cref="GetAlos60">dawn</see>. The formula
        /// used is:<br/>
        /// 10.75 <see cref="GetShaahZmanis60Minutes()"/> after <seealso see="getAlos60()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha60Minutes()
        {
            return GetTimeOffset(GetAlos60(), GetShaahZmanis60Minutes() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <see cref="ZmanimCalendar.GetAlos72">dawn</see>. The formula
        /// used is:<br/>
        /// 10.75 <see cref="GetShaahZmanis72Minutes()"/> after <see cref="ZmanimCalendar.GetAlos72"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha72Minutes()
        {
            return GetTimeOffset(GetAlos72(), GetShaahZmanis72Minutes() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <seealso cref="GetAlos90">dawn</seealso>. The formula
        /// used is:<br/>
        /// 10.75 <seealso cref="GetShaahZmanis90Minutes()"/> after <seealso cref="GetAlos90()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha90Minutes()
        {
            return GetTimeOffset(GetAlos90(), GetShaahZmanis90Minutes() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <seealso cref="GetAlos96">dawn</seealso>. The formula
        /// used is:<br/>
        /// 10.75 <seealso cref="GetShaahZmanis96Minutes()"/> after <seealso cref="GetAlos96()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha96Minutes()
        {
            return GetTimeOffset(GetAlos96(), GetShaahZmanis96Minutes() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <seealso cref="GetAlos96Zmanis">dawn</seealso>. The
        /// formula used is:<br/>
        /// 10.75 * <seealso cref="GetShaahZmanis96MinutesZmanis()"/> after
        /// <seealso cref="GetAlos96Zmanis">dawn</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha96MinutesZmanis()
        {
            return GetTimeOffset(GetAlos96Zmanis(), GetShaahZmanis96MinutesZmanis() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <seealso cref="GetAlos90Zmanis">dawn</seealso>. The
        /// formula used is:<br/>
        /// 10.75 * <seealso cref="GetShaahZmanis90MinutesZmanis()"/> after
        /// <seealso cref="GetAlos90Zmanis">dawn</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha90MinutesZmanis()
        {
            return GetTimeOffset(GetAlos90Zmanis(), GetShaahZmanis90MinutesZmanis() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em>. This is
        /// calculated as 10.75 hours after <seealso cref="GetAlos72Zmanis">dawn</seealso>. The
        /// formula used is:<br/>
        /// 10.75 * <seealso cref="GetShaahZmanis72MinutesZmanis()"/> after
        /// <seealso cref="GetAlos72Zmanis">dawn</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha72MinutesZmanis()
        {
            return GetTimeOffset(GetAlos72Zmanis(), GetShaahZmanis72MinutesZmanis() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em> based on the
        /// opinion that the day starts at
        /// <em><seealso cref="GetAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        /// <em><seealso cref="GetTzais16Point1Degrees">tzais 16.1°</seealso></em>. This is
        /// calculated as 10.75 hours <em>zmaniyos</em> after
        /// <seealso cref="GetAlos16Point1Degrees">dawn</seealso>. The formula is<br/>
        /// 10.75 * <seealso cref="GetShaahZmanis16Point1Degrees()"/> after
        /// <seealso cref="GetAlos16Point1Degrees()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha16Point1Degrees()
        {
            return GetTimeOffset(GetAlos16Point1Degrees(), GetShaahZmanis16Point1Degrees() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em> based on the
        /// opinion that the day starts at
        /// <em><seealso cref="GetAlos19Point8Degrees">alos 19.8°</seealso></em> and ends at
        /// <em><seealso cref="GetTzais19Point8Degrees">tzais 19.8°</seealso></em>. This is
        /// calculated as 10.75 hours <em>zmaniyos</em> after
        /// <seealso cref="GetAlos19Point8Degrees">dawn</seealso>. The formula is<br/>
        /// 10.75 * <seealso cref="GetShaahZmanis19Point8Degrees()"/> after
        /// <seealso cref="GetAlos19Point8Degrees()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha19Point8Degrees()
        {
            return GetTimeOffset(GetAlos19Point8Degrees(), GetShaahZmanis19Point8Degrees() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em> based on the
        /// opinion that the day starts at
        /// <em><seealso cref="GetAlos26Degrees">alos 26°</seealso></em> and ends at
        /// <em><seealso cref="GetTzais26Degrees">tzais 26°</seealso></em>. This is calculated
        /// as 10.75 hours <em>zmaniyos</em> after <seealso cref="GetAlos26Degrees">dawn</seealso>.
        /// The formula is<br/>
        /// 10.75 * <seealso cref="GetShaahZmanis26Degrees()"/> after
        /// <seealso cref="GetAlos26Degrees()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha26Degrees()
        {
            return GetTimeOffset(GetAlos26Degrees(), GetShaahZmanis26Degrees() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em> based on the
        /// opinion that the day starts at
        /// <em><seealso cref="GetAlos18Degrees">alos 18°</seealso></em> and ends at
        /// <em><seealso cref="GetTzais18Degrees">tzais 18°</seealso></em>. This is calculated
        /// as 10.75 hours <em>zmaniyos</em> after <seealso cref="GetAlos18Degrees">dawn</seealso>.
        /// The formula is<br/>
        /// 10.75 * <seealso cref="GetShaahZmanis18Degrees()"/> after
        /// <seealso cref="GetAlos18Degrees()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of <em>plag hamincha</em>.
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        public virtual DateTime? GetPlagHamincha18Degrees()
        {
            return GetTimeOffset(GetAlos18Degrees(), GetShaahZmanis18Degrees() * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em> based on the
        /// opinion that the day starts at
        /// <em><seealso cref="GetAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        /// <seealso cref="AstronomicalCalendar.GetSunset">sunset</seealso>. 10.75 shaos zmaniyos are calculated based on
        /// this day and added to <seealso cref="GetAlos16Point1Degrees">alos</seealso> to reach
        /// this time. This time is 10.75 <em>shaos zmaniyos</em> (temporal hours)
        /// after <seealso cref="GetAlos16Point1Degrees">dawn</seealso> based on the opinion that
        /// the day is calculated from a <seealso cref="GetAlos16Point1Degrees">dawn</seealso> of
        /// 16.1 degrees before sunrise to <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>
        /// . This returns the time of 10.75 * the calculated
        /// <em>shaah zmanis</em> after <seealso cref="GetAlos16Point1Degrees">dawn</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the plag.
        /// If the calculation can't be computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos16Point1Degrees()"/>
        /// <seealso cref="AstronomicalCalendar.GetSeaLevelSunset"/>
        public virtual DateTime? GetPlagAlosToSunset()
        {
            long shaahZmanis = GetTemporalHour(GetAlos16Point1Degrees(), GetSeaLevelSunset());
            return GetTimeOffset(GetAlos16Point1Degrees(), shaahZmanis * 10.75);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em> based on the
        /// opinion that the day starts at
        /// <em><see cref="GetAlos16Point1Degrees">alos 16.1°</see></em> and ends at
        /// <see cref="GetTzaisGeonim7Point083Degrees">tzais</see>. 10.75 shaos zmaniyos are
        /// calculated based on this day and added to
        /// <see cref="GetAlos16Point1Degrees">alos</see> to reach this time. This time is
        /// 10.75 <em>shaos zmaniyos</em> (temporal hours) after
        /// <see cref="GetAlos16Point1Degrees">dawn</see> based on the opinion that the day
        /// is calculated from a <see cref="GetAlos16Point1Degrees">dawn</see> of 16.1
        /// degrees before sunrise to <see cref="GetTzaisGeonim7Point083Degrees">tzais</see>
        /// . This returns the time of 10.75 * the calculated <em>shaah zmanis</em>
        /// after <see cref="GetAlos16Point1Degrees">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the plag.
        /// If the calculation can't be computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos16Point1Degrees()"/>
        /// <seealso cref="GetTzaisGeonim7Point083Degrees()"/>
        public virtual DateTime? GetPlagAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            long shaahZmanis = GetTemporalHour(GetAlos16Point1Degrees(), GetTzaisGeonim7Point083Degrees());
            return GetTimeOffset(GetAlos16Point1Degrees(), shaahZmanis * 10.75);
        }

        /// <summary>
        /// This method returns Bain Hashmashos of Rabainu Tam calculated as the time
        /// the sun is 13° below <see cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see>
        /// (90°). <br/>
        /// 	<br/>
        /// 	<b>FIXME:</b> As per Yisroel Vehazmanim Vol III page 1028 No 50, the
        /// 13° is slightly inaccurate. He lists it as a drop less than 13°.
        /// Calculations show that is seems to be 13.2477° below the horizon at
        /// that time. This makes a difference of 1 minute and 10 seconds in
        /// Jerusalem in the Equinox, and 1 minute 29 seconds in the solstice. for NY
        /// in the solstice, the difference is 1 minute 56 seconds.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the sun being 13° below
        /// <see cref="AstronomicalCalendar.GEOMETRIC_ZENITH">geometric zenith</see> (90°).
        /// If the calculation can't be computed such as northern and southern
        /// locations even south of the Arctic Circle and north of the
        /// Antarctic Circle where the sun may not reach low enough below the
        /// horizon for this calculation, a null will be returned. See
        /// detailed explanation on top of the <seealso cref="AstronomicalCalendar"/>
        /// documentation.
        /// </returns>
        /// <seealso cref="ZENITH_13_DEGREES"/>
        public virtual DateTime? GetBainHasmashosRT13Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_13_DEGREES);
        }

        /// <summary>
        /// This method returns Bain Hashmashos of Rabainu Tam calculated as a 58.5
        /// minute offset after sunset. Bain hashmashos is 3/4 of a mil before tzais
        /// or 3 1/4 mil after sunset. With a mil calculated as 18 minutes, 3.25 * 18
        /// = 58.5 minutes.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of 58.5 minutes after sunset.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetBainHasmashosRT58Point5Minutes()
        {
            return GetTimeOffset(GetSeaLevelSunset(), 58.5 * MINUTE_MILLIS);
        }

        /// <summary>
        /// This method returns the time of <em>bain hashmashos</em> based on the
        /// calculation of 13.5 minutes (3/4 of an 18 minute mil before shkiah
        /// calculated as <seealso cref="GetTzaisGeonim7Point083Degrees">7.083°</seealso>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the bain hashmashos of Rabainu Tam in this calculation.
        /// If the calculation can't be computed such as
        /// northern and southern locations even south of the Arctic Circle
        /// and north of the Antarctic Circle where the sun may not reach low
        /// enough below the horizon for this calculation, a null will be
        /// returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetTzaisGeonim7Point083Degrees()"/>
        public virtual DateTime? GetBainHasmashosRT13Point5MinutesBefore7Point083Degrees()
        {
            return GetTimeOffset(GetSunsetOffsetByDegrees(ZENITH_7_POINT_083), -13.5 * MINUTE_MILLIS);
        }

        /// <summary>
        /// This method returns <em>bain hashmashos</em> of Rabainu Tam calculated in
        /// the opinion of the Divray Yosef (see Yisrael Vehazmanim) calculated
        /// 5/18th (27.77%) of the time between alos (calculated as 19.8° before
        /// sunrise) and sunrise. This is added to sunset to arrive at the time for
        /// bain hashmashos of Rabainu Tam).
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of bain hashmashos of Rabainu Tam for this calculation.
        /// If the calculation can't be computed such as
        /// northern and southern locations even south of the Arctic Circle
        /// and north of the Antarctic Circle where the sun may not reach low
        /// enough below the horizon for this calculation, a null will be
        /// returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        public virtual DateTime? GetBainHasmashosRT2Stars()
        {
            var alos19Point8 = GetAlos19Point8Degrees();
            var sunrise = GetSeaLevelSunrise();
            if (alos19Point8 == null || sunrise == null)
                return null;

            return GetTimeOffset(GetSeaLevelSunset(), (sunrise.Value.ToUnixEpochMilliseconds() - alos19Point8.Value.ToUnixEpochMilliseconds()) * (5 / 18d));
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated at the sun's position at
        ///  <seealso cref = "ZENITH_5_POINT_95">5.95°</seealso> below the western horizon.
        ///</summary>
        ///<returns> the <c>DateTime</c> representing the time when the sun is
        ///  5.95° below sea level. </returns>
        ///<seealso cref = "ZENITH_5_POINT_95" />
        // public Date getTzaisGeonim3Point7Degrees() {
        // return getSunsetOffsetByDegrees(ZENITH_3_POINT_7);
        // }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the <em>Geonim</em> calculated at the sun's position at
        /// <seealso cref="ZENITH_5_POINT_95">5.95°</seealso> below the western horizon.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time when the sun is 5.95° below sea level.
        /// If the calculation can't be computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZENITH_5_POINT_95"/>
        public virtual DateTime? GetTzaisGeonim5Point95Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_5_POINT_95);
        }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the <em>Geonim</em> calculated calculated as 3/4 of a <a href="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a> based on an 18 minute Mil, or 13.5 minutes. It is the sun's
        /// position at <seealso cref="ZENITH_3_POINT_65">3.65°</seealso> below the western
        /// horizon. This is a very early zman and should not be relied on without
        /// Rabbinical guidance.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time when the sun is 3.65° below sea level.
        /// If the calculation can't be
        /// computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZENITH_3_POINT_65"/>
        public virtual DateTime? GetTzaisGeonim3Point65Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_3_POINT_65);
        }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the <em>Geonim</em> calculated as 3/4 of a <a href="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a> based on a 24 minute Mil, or 18 minutes. It is the sun's
        /// position at <seealso cref="ZENITH_4_POINT_61">4.61°</seealso> below the western
        /// horizon. This is a very early zman and should not be relied on without
        /// Rabbinical guidance.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time when the sun is 4.61° below sea level.
        /// If the calculation can't be computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZENITH_4_POINT_61"/>
        public virtual DateTime? GetTzaisGeonim4Point61Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_4_POINT_61);
        }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the <em>Geonim</em> calculated as 3/4 of a <a href="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a>, based on a 22.5 minute Mil, or 16 7/8 minutes. It is the sun's
        /// position at <seealso cref="ZENITH_4_POINT_37">4.37°</seealso> below the western
        /// horizon. This is a very early zman and should not be relied on without
        /// Rabbinical guidance.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time when the sun is 4.37° below sea level.
        /// If the calculation can't be computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZENITH_4_POINT_37"/>
        public virtual DateTime? GetTzaisGeonim4Point37Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_4_POINT_37);
        }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the <em>Geonim</em> calculated as 3/4 of a <a href="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a>. It is based on the Baal Hatanya based on a Mil being 24
        /// minutes, and is calculated as 18 +2 + 4 for a total of 24 minutes (FIXME:
        /// additional details needed). It is the sun's position at
        /// <see cref="ZENITH_5_POINT_88">5.88°</see> below the western horizon. This is a
        /// very early zman and should not be relied on without Rabbinical guidance.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time when the sun is
        /// 5.88° below sea level.
        /// If the calculation can't be
        /// computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZENITH_5_POINT_88"/>
        public virtual DateTime? GetTzaisGeonim5Point88Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_5_POINT_88);
        }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the <em>Geonim</em> calculated as 3/4 of a <a href="http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">
        /// Mil</a>. It is the sun's position at <see cref="ZENITH_4_POINT_8">4.8°</see>
        /// below the western horizon based on Rabbi Leo Levi's calculations. (FIXME:
        /// additional documentation needed) This is the This is a very early zman
        /// and should not be relied on without Rabbinical guidance.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time when the sun is 4.8° below sea level.
        /// If the calculation can't be
        /// computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZENITH_4_POINT_8"/>
        public virtual DateTime? GetTzaisGeonim4Point8Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_4_POINT_8);
        }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the <em>Geonim</em> calculated at the sun's position at
        /// <see cref="ZENITH_7_POINT_083">7.083°</see> below the western horizon.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time when the sun is 7.083° below sea level.
        /// If the calculation can't be
        /// computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZENITH_7_POINT_083"/>
        public virtual DateTime? GetTzaisGeonim7Point083Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_7_POINT_083);
        }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the <em>Geonim</em> calculated at the sun's position at
        /// <see cref="ZmanimCalendar.ZENITH_8_POINT_5">8.5°</see> below the western horizon.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time when the sun is 8.5° below sea level.
        /// If the calculation can't be computed
        /// such as northern and southern locations even south of the Arctic
        /// Circle and north of the Antarctic Circle where the sun may not
        /// reach low enough below the horizon for this calculation, a
        /// null will be returned. See detailed explanation on
        /// top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.ZENITH_8_POINT_5"/>
        public virtual DateTime? GetTzaisGeonim8Point5Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_8_POINT_5);
        }

        /// <summary>
        /// This method returns the <em>tzais</em> (nightfall) based on the opinion
        /// of the Chavas Yair and Divray Malkiel that the time to walk the distance
        /// of a Mil is 15 minutes for a total of 60 minutes for 4 mil after
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing 60 minutes after sea level sunset.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos60()"/>
        public virtual DateTime? GetTzais60()
        {
            return GetTimeOffset(GetSeaLevelSunset(), 60 * MINUTE_MILLIS);
        }

        /// <summary>
        /// This method returns tzais usually calculated as 40 minutes after sunset.
        /// Please note that Chacham Yosef Harari-Raful of Yeshivat Ateret Torah who
        /// uses this time, does so only for calculating various other zmanai hayom
        /// such as Sof Zman Krias Shema and Plag Hamincha. His calendars do not
        /// publish a zman for Tzais. It should also be noted that Chacham
        /// Harari-Raful provided a 25 minute zman for Israel. This API uses 40
        /// minutes year round in any place on the globe by default. This offset can
        /// be changed by calling <see cref="AteretTorahSunsetOffset"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing 40 minutes
        /// (setable via <see cref="AteretTorahSunsetOffset"/>) after sea level sunset.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="AteretTorahSunsetOffset"/>
        public virtual DateTime? GetTzaisAteretTorah()
        {
            return GetTimeOffset(GetSeaLevelSunset(), AteretTorahSunsetOffset * MINUTE_MILLIS);
        }

        /// <summary>
        /// Gets or Sets the offset in minutes after sunset for the Ateret Torah
        /// zmanim. The default if unset is 40 minutes. Chacham Yosef Harari-Raful of
        /// Yeshivat Ateret Torah uses 40 minutes globally with the exception of
        /// Israel where a 25 minute offset is used. This 25 minute (or any other)
        /// offset can be overridden by this methd. This offset impacts all Ateret
        /// Torah methods.
        /// --
        /// Returns the offset in minutes after sunset used to calculate
        /// <em>tzais</em> for the Ateret Torah zmanim. The defaullt value is 40
        /// minutes.
        /// </summary>
        /// <value>the number of minutes after sunset to use as an offset for the
        ///   Ateret Torah &lt;em&gt;tzais&lt;/em&gt;</value>
        public virtual double AteretTorahSunsetOffset
        {
            get { return ateretTorahSunsetOffset; }
            set { ateretTorahSunsetOffset = value; }
        }

        /// <summary>
        /// This method returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) based on the calculation of Chacham Yosef
        /// Harari-Raful of Yeshivat Ateret Torah, that the day starts
        /// <see cref="GetAlos72Zmanis">1/10th of the day</see> before sunrise and is
        /// usually calculated as ending <see cref="GetTzaisAteretTorah()">40 minutes after sunset</see>
        /// . <em>shaos zmaniyos</em> are calculated based on this day
        /// and added to <see cref="GetAlos72Zmanis">alos</see> to reach this time. This
        /// time is 3 <em>
        /// 		<see cref="GetShaahZmanisAteretTorah">shaos zmaniyos</see></em>
        /// (temporal hours) after <see cref="GetAlos72Zmanis">alos 72 zmaniyos</see>.<br/>
        /// 	<b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        /// midday.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema based on this
        /// calculation.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos72Zmanis()"/>
        /// <seealso cref="GetTzaisAteretTorah()"/>
        /// <seealso cref="AteretTorahSunsetOffset"/>
        /// <seealso cref="GetShaahZmanisAteretTorah()"/>
        public virtual DateTime? GetSofZmanShmaAteretTorah()
        {
            return GetTimeOffset(GetAlos72Zmanis(), GetShaahZmanisAteretTorah() * 3);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) based on the calculation of Chacham Yosef Harari-Raful
        /// of Yeshivat Ateret Torah, that the day starts <see cref="GetAlos72Zmanis()"> 1/10th of the day</see>
        /// before sunrise and and is usually calculated as ending
        /// <see cref="GetTzaisAteretTorah">40 minutes after sunset</see>.
        /// <em>shaos zmaniyos</em> are calculated based on this day and added to
        /// <see cref="GetAlos72Zmanis">alos</see> to reach this time. This time is 4
        /// <em><see cref="GetShaahZmanisAteretTorah">shaos zmaniyos</see></em> (temporal
        /// hours) after <see cref="GetAlos72Zmanis">alos 72 zmaniyos</see>.<br/>
        /// 	<b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        /// midday.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema based on this
        /// calculation.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos72Zmanis()"/>
        /// <seealso cref="GetTzaisAteretTorah()"/>
        /// <seealso cref="GetShaahZmanisAteretTorah()"/>
        /// <seealso cref="AteretTorahSunsetOffset"/>
        public virtual DateTime? GetSofZmanTfilahAteretTorah()
        {
            return GetTimeOffset(GetAlos72Zmanis(), GetShaahZmanisAteretTorah() * 4);
        }

        /// <summary>
        /// This method returns the time of <em>mincha gedola</em> based on the
        /// calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        /// the day starts <see cref="GetAlos72Zmanis">1/10th of the day</see> before
        /// sunrise and and is usually calculated as ending
        /// <see cref="GetTzaisAteretTorah">40 minutes after sunset</see>. This is the
        /// perfered earliest time to pray <em>mincha</em> in the opinion of the
        /// Ramba"m and others. For more information on this see the documentation on
        /// <em><see cref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        /// 6.5 <see cref="GetShaahZmanisAteretTorah">solar hours</see> after alos. The
        /// calculation used is 6.5 * <seealso cref="GetShaahZmanisAteretTorah()"/> after
        /// <see cref="GetAlos72Zmanis">alos</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of mincha gedola.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos72Zmanis()"/>
        /// <seealso cref="GetTzaisAteretTorah()"/>
        /// <seealso cref="GetShaahZmanisAteretTorah()"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        /// <seealso cref="GetMinchaKetanaAteretTorah()"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        public virtual DateTime? GetMinchaGedolaAteretTorah()
        {
            return GetTimeOffset(GetAlos72Zmanis(), GetShaahZmanisAteretTorah() * 6.5);
        }

        /// <summary>
        /// This method returns the time of <em>mincha ketana</em> based on the
        /// calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        /// the day starts <see cref="GetAlos72Zmanis">1/10th of the day</see> before
        /// sunrise and and is usually calculated as ending
        /// <see cref="GetTzaisAteretTorah">40 minutes after sunset</see>. This is the
        /// perfered earliest time to pray <em>mincha</em> in the opinion of the
        /// Ramba"m and others. For more information on this see the documentation on
        /// <em><see cref="ZmanimCalendar.GetMinchaGedola">mincha gedola</see></em>. This is calculated as
        /// 9.5 <see cref="GetShaahZmanisAteretTorah">solar hours</see> after
        /// <see cref="GetAlos72Zmanis">alos</see>. The calculation used is 9.5 *
        /// <see cref="GetShaahZmanisAteretTorah()"/> after <see cref="GetAlos72Zmanis()"> alos</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the time of mincha ketana.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos72Zmanis()"/>
        /// <seealso cref="GetTzaisAteretTorah()"/>
        /// <seealso cref="GetShaahZmanisAteretTorah()"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaGedola"/>
        /// <seealso cref="ZmanimCalendar.GetMinchaKetana"/>
        public virtual DateTime? GetMinchaKetanaAteretTorah()
        {
            return GetTimeOffset(GetAlos72Zmanis(), GetShaahZmanisAteretTorah() * 9.5);
        }

        /// <summary>
        /// This method returns the time of <em>plag hamincha</em> based on the
        /// calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        /// the day starts <see cref="GetAlos72Zmanis">1/10th of the day</see> before
        /// sunrise and and is usually calculated as ending
        /// <see cref="GetTzaisAteretTorah">40 minutes after sunset</see>.
        /// <em>shaos zmaniyos</em> are calculated based on this day and added to
        /// <see cref="GetAlos72Zmanis">alos</see> to reach this time. This time is 10.75
        /// <em><see cref="GetShaahZmanisAteretTorah">shaos zmaniyos</see></em> (temporal
        /// hours) after <see cref="GetAlos72Zmanis">dawn</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the plag.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos72Zmanis()"/>
        /// <seealso cref="GetTzaisAteretTorah()"/>
        /// <seealso cref="GetShaahZmanisAteretTorah()"/>
        public virtual DateTime? GetPlagHaminchaAteretTorah()
        {
            return GetTimeOffset(GetAlos72Zmanis(), GetShaahZmanisAteretTorah() * 10.75);
        }

        /*
        ///	 <summary> 
        ///	This method returns the time of <em>misheyakir</em> based on the common
        ///	calculation of the Syrian community in NY that the alos is a fixed minute
        ///	offset from day starting <seealso cref="GetAlos72Zmanis">1/10th of the day</seealso>
        ///	before sunrise. The common offsets are 6 minutes (based on th Pri
        ///	Megadim, but not linked to the calculation of Alos as 1/10th of the day),
        ///	8 and 18 minutes (possibly attributed to Chacham Baruch Ben Haim). Since
        ///	there is no universal accepted offset, the user of this API will have to
        ///	specify one. Chacham Yosef Harari-Raful of Yeshivat Ateret Torah does not
        ///	supply any zman for misheyakir and does not endorse any specific
        ///	calculation for misheyakir. For that reason, this method is not enabled.
        ///	 </summary>
        ///	<param name="minutes">
        ///	           the number of minutes after alos calculated as
        ///	           <seealso cref="GetAlos72Zmanis">1/10th of the day</seealso> </param>
        ///	<returns> the <c>DateTime</c> of misheyakir </returns>
        ///	<seealso cref="GetAlos72Zmanis()"/>
        ///	 
        // public Date getMesheyakirAteretTorah(double minutes) {
        // return getTimeOffset(GetAlos72Zmanis(), minutes * MINUTE_MILLIS);
        // }
        */

        /// <summary>
        /// Method to return <em>tzais</em> (dusk) calculated as 72 minutes zmaniyos,
        /// or 1/10th of the day after <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos72Zmanis()"/>
        public virtual DateTime? GetTzais72Zmanis()
        {
            long shaahZmanis = GetShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
                return null;

            return GetTimeOffset(GetSeaLevelSunset(), shaahZmanis * 1.2);
        }

        /// <summary>
        /// Method to return <em>tzais</em> (dusk) calculated using 90 minutes
        /// zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos90Zmanis()"/>
        public virtual DateTime? GetTzais90Zmanis()
        {
            long shaahZmanis = GetShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
                return null;

            return GetTimeOffset(GetSeaLevelSunset(), shaahZmanis * 1.5);
        }

        /// <summary>
        /// Method to return <em>tzais</em> (dusk) calculated using 96 minutes
        /// zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos96Zmanis()"/>
        public virtual DateTime? GetTzais96Zmanis()
        {
            long shaahZmanis = GetShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
                return null;

            return GetTimeOffset(GetSeaLevelSunset(), shaahZmanis * 1.6);
        }

        /// <summary>
        /// Method to return <em>tzais</em> (dusk) calculated as 90 minutes after sea
        /// level sunset. This method returns <em>tzais</em> (nightfall) based on the
        /// opinion of the Magen Avraham that the time to walk the distance of a Mil
        /// in the Ramba"m's opinion is 18 minutes for a total of 90 minutes based on
        /// the opinion of <em>Ula</em> who calculated <em>tzais</em> as 5 Mil after
        /// sea level shkiah (sunset). A similar calculation
        /// <see cref="GetTzais19Point8Degrees()"/>uses solar position calculations based
        /// on this time.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetTzais19Point8Degrees()"/>
        /// <seealso cref="GetAlos90()"/>
        public virtual DateTime? GetTzais90()
        {
            return GetTimeOffset(GetSeaLevelSunset(), 90 * MINUTE_MILLIS);
        }

        /// <summary>
        /// This method returns <em>tzais</em> (nightfall) based on the opinion of
        /// the Magen Avraham that the time to walk the distance of a Mil in the
        /// Ramba"ms opinion is 2/5 of an hour (24 minutes) for a total of 120
        /// minutes based on the opinion of <em>Ula</em> who calculated
        /// <em>tzais</em> as 5 Mil after sea level shkiah (sunset). A similar
        /// calculation <see cref="GetTzais26Degrees()"/> uses temporal calculations based
        /// on this time.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetTzais26Degrees()"/>
        /// <seealso cref="GetAlos120()"/>
        public virtual DateTime? GetTzais120()
        {
            return GetTimeOffset(GetSeaLevelSunset(), 120 * MINUTE_MILLIS);
        }

        /// <summary>
        /// Method to return <em>tzais</em> (dusk) calculated using 120 minutes
        /// zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        /// <see cref="AstronomicalCalendar.GetSeaLevelSunset">sea level sunset</see>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos120Zmanis()"/>
        public virtual DateTime? GetTzais120Zmanis()
        {
            long shaahZmanis = GetShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
                return null;

            return GetTimeOffset(GetSeaLevelSunset(), shaahZmanis * 2.0);
        }

        /// <summary>
        /// For information on how this is calculated see the comments on
        /// <see cref="GetAlos16Point1Degrees()"/>
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation
        /// can't be computed such as northern and southern locations even
        /// south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this
        /// calculation, a null will be returned. See detailed explanation on
        /// top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="ZmanimCalendar.GetTzais72"/>
        /// <seealso cref="GetAlos16Point1Degrees">for more information on this calculation.</seealso>
        public virtual DateTime? GetTzais16Point1Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_16_POINT_1);
        }

        /// <summary>
        /// For information on how this is calculated see the comments on
        /// <see cref="GetAlos26Degrees()"/>
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be
        /// computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetTzais120()"/>
        /// <seealso cref="GetAlos26Degrees()"/>
        public virtual DateTime? GetTzais26Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_26_DEGREES);
        }

        /// <summary>
        /// For information on how this is calculated see the comments on
        /// <see cref="GetAlos18Degrees()"/>
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation
        /// can't be computed such as northern and southern locations even
        /// south of the Arctic Circle and north of the Antarctic Circle
        /// where the sun may not reach low enough below the horizon for this
        /// calculation, a null will be returned. See detailed explanation on
        /// top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos18Degrees()"/>
        public virtual DateTime? GetTzais18Degrees()
        {
            return GetSunsetOffsetByDegrees(ASTRONOMICAL_ZENITH);
        }

        /// <summary>
        /// For information on how this is calculated see the comments on
        /// <see cref="GetAlos19Point8Degrees()"/>
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be
        /// computed such as northern and southern locations even south of
        /// the Arctic Circle and north of the Antarctic Circle where the sun
        /// may not reach low enough below the horizon for this calculation,
        /// a null will be returned. See detailed explanation on top of the
        /// <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetTzais90()"/>
        /// <seealso cref="GetAlos19Point8Degrees()"/>
        public virtual DateTime? GetTzais19Point8Degrees()
        {
            return GetSunsetOffsetByDegrees(ZENITH_19_POINT_8);
        }

        /// <summary>
        /// A method to return <em>tzais</em> (dusk) calculated as 96 minutes after
        /// sea level sunset. For information on how this is calculated see the
        /// comments on <see cref="GetAlos96()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> representing the time.
        /// If the calculation can't be computed such as in the Arctic Circle
        /// where there is at least one day a year where the sun does not
        /// rise, and one where it does not set, a null will be returned. See
        /// detailed explanation on top of the <see cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetAlos96()"/>
        public virtual DateTime? GetTzais96()
        {
            return GetTimeOffset(GetSeaLevelSunset(), 96 * MINUTE_MILLIS);
        }

        /// <summary>
        /// A method that returns the local time for fixed <em>chatzos</em>. This
        /// time is noon and midnight adjusted from standard time to account for the
        /// local latitude. The 360° of the globe divided by 24 calculates to
        /// 15° per hour with 4 minutes per degree, so at a longitude of 0 , 15,
        /// 30 etc Chatzos in 12:00 noon. Lakewood, NJ whose longitude is -74.2094 is
        /// 0.7906 away from the closest multiple of 15 at -75°. This is
        /// multiplied by 4 to yeild 3 minutes and 10 seconds for a chatzos of
        /// 11:56:50. This method is not tied to the theoretical 15° timezones,
        /// but will adjust to the actual timezone and <a href="http://en.wikipedia.org/wiki/Daylight_saving_time">Daylight saving
        /// time</a>.
        /// </summary>
        /// <returns>
        /// the DateTime representing the local <em>chatzos</em>
        /// </returns>
        /// <seealso cref="GeoLocation.GetLocalMeanTimeOffset"/>
        public virtual DateTime? GetFixedLocalChatzos()
        {
            return GetTimeOffset(
                GetDateFromTime(
                12.0 - DateWithLocation.Location.TimeZone.UtcOffset(DateWithLocation.Date) / HOUR_MILLIS),
                                 -DateWithLocation.Location.GetLocalMeanTimeOffset(DateWithLocation.Date));
        }

        /// <summary>
        /// A method that returns the latest <em>zman krias shema</em> (time to say
        /// Shema in the morning) calculated as 3 hours before
        /// <see cref="GetFixedLocalChatzos()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman shema.
        /// </returns>
        /// <seealso cref="GetFixedLocalChatzos()"/>
        /// <seealso cref="GetSofZmanTfilaFixedLocal()"/>
        public virtual DateTime? GetSofZmanShmaFixedLocal()
        {
            return GetTimeOffset(GetFixedLocalChatzos(), -180 * MINUTE_MILLIS);
        }

        /// <summary>
        /// This method returns the latest <em>zman tfila</em> (time to say the
        /// morning prayers) calculated as 2 hours before
        /// <see cref="GetFixedLocalChatzos()"/>.
        /// </summary>
        /// <returns>
        /// the <c>DateTime</c> of the latest zman tfila.
        /// </returns>
        /// <seealso cref="GetFixedLocalChatzos()"/>
        /// <seealso cref="GetSofZmanShmaFixedLocal()"/>
        public virtual DateTime? GetSofZmanTfilaFixedLocal()
        {
            return GetTimeOffset(GetFixedLocalChatzos(), -120 * MINUTE_MILLIS);
        }

        /// <summary>
        /// This method returns the latest time one is allowed eating chametz on Erev Pesach according to the opinion of the
        /// <em>GRA</em> and the </em>Baal Hatanya</em>. This time is identical to the {@link #getSofZmanTfilaGRA() Sof zman
        /// tefilah GRA}. This time is 4 hours into the day based on the opinion of the <em>GRA</em> and the </em>Baal
        /// Hatanya</em> that the day is calculated from sunrise to sunset. This returns the time 4 *
        /// <seealso cref="GetShaahZmanisGra()"/> after <seealso cref="GetSeaLevelSunrise() sea level sunrise"/>.
        /// </summary>
        /// <seealso cref="ZmanimCalendar#getShaahZmanisGra"></seealso>
        /// <seealso cref="ZmanimCalendar#getSofZmanTfilaGRA"></seealso>
        /// <returns> the <code>Date</code> one is allowed eating chametz on Erev Pesach. If the calculation can't be computed
        ///         such as in the Arctic Circle where there is at least one day a year where the sun does not rise, and one
        ///         where it does not set, a null will be returned. See detailed explanation on top of the
        ///         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? getSofZmanAchilasChametzGRA()
        {
            return GetSofZmanTfilaGRA();
        }

        /// <summary>
        /// This method returns the latest time one is allowed eating chametz on Erev Pesach according to the opinion of the
        /// <em>MGA</em> based on <em>alos</em> being <seealso cref="GetAlos72() 72"/> minutes before <seealso cref="GetSunrise() sunrise"/>.
        /// This time is identical to the <seealso cref="GetSofZmanTfilaMGA72Minutes() Sof zman tefilah MGA 72 minutes"/>. This time
        /// is 4 <em><seealso cref="GetShaahZmanisMGA() shaos zmaniyos"/></em> (temporal hours) after <seealso cref="GetAlos72() dawn"/> based
        /// on the opinion of the <em>MGA</em> that the day is calculated from a <seealso cref="GetAlos72() dawn"/> of 72 minutes
        /// before sunrise to <seealso cref="GetTzais72() nightfall"/> of 72 minutes after sunset. This returns the time of 4 *
        /// <seealso cref="GetShaahZmanisMGA()"/> after <seealso cref="GetAlos72() dawn"/>.
        /// </summary>
        /// <returns> the <code>Date</code> of the latest time of eating chametz. If the calculation can't be computed such as
        ///         in the Arctic Circle where there is at least one day a year where the sun does not rise, and one where it
        ///         does not set), a null will be returned. See detailed explanation on top of the
        ///         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        /// <seealso cref="GetShaahZmanisMGA"></seealso>
        /// <seealso cref="GetAlos72"></seealso>
        /// <seealso cref="GetSofZmanTfilaMGA72Minutes"></seealso>
        public virtual DateTime? GetSofZmanAchilasChametzMGA72Minutes()
        {
            return GetSofZmanTfilaMGA72Minutes();
        }

        /// <summary>
        /// This method returns the latest time one is allowed eating chametz on Erev Pesach according to the opinion of the
        /// <em>MGA</em> based on <em>alos</em> being <seealso cref="GetAlos16Point1Degrees() 16.1&deg;"/> before
        /// <seealso cref="GetSunrise() sunrise"/>. This time is 4 <em><seealso cref="GetShaahZmanis16Point1Degrees() shaos zmaniyos"/></em>
        /// (solar hours) after <seealso cref="GetAlos16Point1Degrees() dawn"/> based on the opinion of the <em>MGA</em> that the day
        /// is calculated from dawn to nightfall with both being 16.1&deg; below sunrise or sunset. This returns the time of
        /// 4 <seealso cref="GetShaahZmanis16Point1Degrees()"/> after <seealso cref="GetAlos16Point1Degrees() dawn"/>.
        /// </summary>
        /// <returns> the <code>Date</code> of the latest time of eating chametz. If the calculation can't be computed such as
        ///         northern and southern locations even south of the Arctic Circle and north of the Antarctic Circle where
        ///         the sun may not reach low enough below the horizon for this calculation, a null will be returned. See
        ///         detailed explanation on top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis16Point1Degrees"></seealso>
        /// <seealso cref="GetAlos16Point1Degrees"></seealso>
        /// <seealso cref="GetSofZmanTfilaMGA16Point1Degrees"></seealso>
        public virtual DateTime? GetSofZmanAchilasChametzMGA16Point1Degrees()
        {
            return GetSofZmanTfilaMGA16Point1Degrees();
        }

        /// <summary>
        /// This method returns the latest time for burning chametz on Erev Pesach according to the opinion of the
        /// <em>GRA</em> and the </em>Baal Hatanya</em>. This time is 5 hours into the day based on the opinion of the
        /// <em>GRA</em> and the </em>Baal Hatanya</em> that the day is calculated from sunrise to sunset. This returns the
        /// time 5 * <seealso cref="GetShaahZmanisGra()"/> after <seealso cref="GetSeaLevelSunrise() sea level sunrise"/>.
        /// </summary>
        /// <seealso cref="ZmanimCalendar#getShaahZmanisGra"></seealso>
        /// <returns> the <code>Date</code> of the latest time for burning chametz on Erev Pesach. If the calculation can't be
        ///         computed such as in the Arctic Circle where there is at least one day a year where the sun does not rise,
        ///         and one where it does not set, a null will be returned. See detailed explanation on top of the
        ///         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetSofZmanBiurChametzGRA()
        {
            return GetTimeOffset(GetSeaLevelSunrise(), GetShaahZmanisGra() * 5);
        }

        /// <summary>
        /// This method returns the latest time for burning chametz on Erev Pesach according to the opinion of the
        /// <em>MGA</em> based on <em>alos</em> being <seealso cref="GetAlos72() 72"/> minutes before <seealso cref="GetSunrise() sunrise"/>.
        /// This time is 5 <em><seealso cref="GetShaahZmanisMGA() shaos zmaniyos"/></em> (temporal hours) after {@link #getAlos72()
        /// dawn} based on the opinion of the <em>MGA</em> that the day is calculated from a <seealso cref="GetAlos72() dawn"/> of 72
        /// minutes before sunrise to <seealso cref="GetTzais72() nightfall"/> of 72 minutes after sunset. This returns the time of 5
        /// * <seealso cref="GetShaahZmanisMGA"/> after <seealso cref="GetAlos72() dawn"/>.
        /// </summary>
        /// <returns> the <code>Date</code> of the latest time for burning chametz on Erev Pesach. If the calculation can't be
        ///         computed such as in the Arctic Circle where there is at least one day a year where the sun does not rise,
        ///         and one where it does not set), a null will be returned. See detailed explanation on top of the
        ///         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        /// <seealso cref="GetShaahZmanisMGA"></seealso>
        /// <seealso cref="GetAlos72()"></seealso>
        public virtual DateTime? GetSofZmanBiurChametzMGA72Minutes()
        {
            return GetTimeOffset(GetAlos72(), GetShaahZmanisMGA() * 5);
        }

        /// <summary>
        /// This method returns the latest time for burning <em>chametz</em> on <em>Erev Pesach</em> according to the opinion
        /// of the <em>MGA</em> based on <em>alos</em> being <seealso cref="GetAlos16Point1Degrees() 16.1&deg;"/> before
        /// <seealso cref="GetSunrise() sunrise"/>. This time is 5 <em><seealso cref="GetShaahZmanis16Point1Degrees() shaos zmaniyos"/></em>
        /// (solar hours) after <seealso cref="GetAlos16Point1Degrees() dawn"/> based on the opinion of the <em>MGA</em> that the day
        /// is calculated from dawn to nightfall with both being 16.1&deg; below sunrise or sunset. This returns the time of
        /// 5 <seealso cref="GetShaahZmanis16Point1Degrees()"/> after <seealso cref="GetAlos16Point1Degrees() dawn"/>.
        /// </summary>
        /// <returns> the <code>Date</code> of the latest time for burning chametz on Erev Pesach. If the calculation can't be
        ///         computed such as northern and southern locations even south of the Arctic Circle and north of the
        ///         Antarctic Circle where the sun may not reach low enough below the horizon for this calculation, a null
        ///         will be returned. See detailed explanation on top of the <seealso cref="AstronomicalCalendar"/> documentation.
        /// </returns>
        /// <seealso cref="GetShaahZmanis16Point1Degrees"></seealso>
        /// <seealso cref="GetAlos16Point1Degrees"></seealso>
        public virtual DateTime? GetSofZmanBiurChametzMGA16Point1Degrees()
        {
            return GetTimeOffset(GetAlos16Point1Degrees(), GetShaahZmanis16Point1Degrees() * 5);
        }

        /// <summary>
        /// A method that returns "solar" midnight, or the time when the sun is at its <a
        /// href="http://en.wikipedia.org/wiki/Nadir">nadir</a>. <br/>
        /// <br/>
        /// <b>Note:</b> this method is experimental and might be removed.
        /// </summary>
        /// <returns> the <code>Date</code> of Solar Midnight (chatzos layla). If the calculation can't be computed such as in
        ///         the Arctic Circle where there is at least one day a year where the sun does not rise, and one where it
        ///         does not set, a null will be returned. See detailed explanation on top of the
        ///         <seealso cref="AstronomicalCalendar"/> documentation. </returns>
        public virtual DateTime? GetSolarMidnight()
        {
            ZmanimCalendar clonedCal = (ZmanimCalendar)MemberwiseClone();
            DateWithLocation.Date = DateWithLocation.Date.AddDays(1);
            DateTime? sunset = GetSeaLevelSunset();
            DateTime? sunrise = clonedCal.GetSeaLevelSunrise();
            return GetTimeOffset(sunset, GetTemporalHour(sunset, sunrise) * 6);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int num = 0x11;
            num = (0x25 * num) + base.GetTemporalHour().GetHashCode();
            num += (0x25 * num) + DateWithLocation.GetHashCode();
            num += (0x25 * num) + DateWithLocation.Location.GetHashCode();
            return (num + ((0x25 * num) + AstronomicalCalculator.GetHashCode()));
        }
    }
}