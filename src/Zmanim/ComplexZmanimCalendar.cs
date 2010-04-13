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

using java.lang;
using java.util;
using net.sourceforge.zmanim.util;

namespace net.sourceforge.zmanim
{
    /// <summary>
    ///   This class extends ZmanimCalendar and provides many more zmanim than
    ///   available in the ZmanimCalendar. The basis for most zmanim in this class are
    ///   from the <em>sefer</em> <b>Yisroel Vehazmanim</b> by <b>Rabbi Yisroel Dovid
    ///                                                          Harfenes</b>. <br />
    ///   For an example of the number of different <em>zmanim</em> made available by
    ///   this class, there are methods to return 12 different calculations for
    ///   <em>alos</em> (dawn) available in this class. The real power of this API is
    ///   the ease in calculating <em>zmanim</em> that are not part of the API. The
    ///   methods for doing <em>zmanim</em> calculations not present in this or it's
    ///   superclass the <seealso cref = "ZmanimCalendar" /> are contained in the
    ///   <seealso cref = "AstronomicalCalendar" />, the base class of the calendars in our API
    ///   since they are generic methods for calculating time based on degrees or time
    ///   before or after <seealso cref = "getSunris">sunrise"/> and <seealso cref = "getSunset sunset</seealso> and
    ///                     are of interest for calculation beyond <em>zmanim</em> calculations. Here are
    ///                     some examples: <br />
    ///                     First create the Calendar for the location you would like to calculate:
    /// 
    ///                     <pre>
    ///                       String locationName = &quot;Lakewood, NJ&quot;
    ///                       double latitude = 40.0828; //Lakewood, NJ
    ///                       double longitude = -74.2094; //Lakewood, NJ
    ///                       double elevation = 0;
    ///                       //the String parameter in getTimeZone() has to be a valid timezone listed in <seealso cref =  java.util.TimeZone.getAvailableIDs()"/>
    /// TimeZone timeZone = TimeZone.getTimeZone(&quot;America/New_York&quot;);
    /// GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    /// 		elevation, timeZone);
    /// ComplexZmanimCalendar czc = new ComplexZmanimCalendar(location);
    /// </pre>
    /// 
    ///                     Note: For locations such as Israel where the beginning and end of daylight
    ///                     savings time can fluctuate from year to year create a
    ///                     <seealso cref = "java.util.SimpleTimeZone" /> with the known start and end of DST. <br />
    ///                     To get alos calculated as 14° below the horizon (as calculated in the
    ///                     calendars published in Montreal) use:
    /// 
    ///                     <code>
    ///                       Date alos14 = czc.getSunriseOffsetByDegrees(14);
    ///                     </code>
    /// 
    ///                     To get <em>mincha gedola</em> calculated based on the MGA using a <em>shaah
    ///                                                                                         zmanis</em> based on the day starting 16.1° below the horizon (and ending
    ///                     16.1° after sunset the following calculation can be used:
    /// 
    ///                     <code>
    ///                       Date minchaGedola = czc.getTimeOffset(czc.getAlos16point1Degrees(), czc
    ///                       .getShaahZmanis16Point1Degrees() * 6.5);
    ///                     </code>
    /// 
    ///                     A little more complex example would be calculating <em>plag hamincha</em>
    ///                     based on a shaah zmanis that was not present in this class. While a drop more
    ///                     complex it is still rather easy. For example if you wanted to calculate
    ///                     <em>plag</em> based on the day starting 12° before sunrise and ending
    ///                     12° after sunset as calculated in the calendars in Manchester, England
    ///                     (there is nothing that would prevent your calculating the day using sunrise
    ///                     and sunset offsets that are not identical degrees, but this would lead to
    ///                     chatzos being a time other than the <seealso cref = "getSunTransit">solar transit</seealso>
    ///                     (solar midday)). The steps involved would be to first calculate the
    ///                     <em>shaah zmanis</em> and than use that time in milliseconds to calculate
    ///                     10.75 hours after sunrise starting at 12° before sunset
    /// 
    ///                     <code>
    ///                       long shaahZmanis = czc.getTemporalHour(czc.getSunriseOffsetByDegrees(12), czc
    ///                       .getSunsetOffsetByDegrees(12));
    ///                       Date plag = getTimeOffset(czc.getSunriseOffsetByDegrees(12),
    ///                       shaahZmanis * 10.75);
    ///                     </code>
    /// 
    ///                     <h2>Disclaimer:</h2> While I did my best to get accurate results please do
    ///                     not rely on these zmanim for <em>halacha lemaaseh</em>
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class ComplexZmanimCalendar : ZmanimCalendar
    {
        private const long serialVersionUID = 1;

        ///<summary>
        ///  The zenith of 3.7° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  opinion of the Geonim that <em>tzais</em> is the time it takes to walk
        ///  3/4 of a Mil at 18 minutes a Mil, or 13.5 minutes after sunset. The sun
        ///  is 3.7° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso> at this time
        ///  in Jerusalem on March 16, about 4 days before the equinox, the day that a
        ///  solar hour is one hour.
        ///	
        ///  TODO AT <see cref = "getTzaisGeonim3Point7Degrees()" />
        ///</summary>
        protected internal const double ZENITH_3_POINT_7 = GEOMETRIC_ZENITH + 3.7;

        ///<summary>
        ///  The zenith of 5.95° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  position of the sun 24 minutes after sunset in Jerusalem on March 16,
        ///  about 4 days before the equinox, the day that a solar hour is one hour,
        ///  which calculates to 5.95° below <see cref = "GEOMETRIC_ZENITH">geometric zenith</see>
        ///</summary>
        ///<seealso cref = "getTzaisGeonim5Point95Degrees()" />
        protected internal const double ZENITH_5_POINT_95 = GEOMETRIC_ZENITH + 5.95;

        ///<summary>
        ///  The zenith of 7.083° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This is often referred to as 7°5' or 7° and 5 minutes.
        ///  This calculation is used for calculating <em>alos</em> (dawn) and
        ///  <em>tzais</em> (nightfall) according to some opinions. This calculation
        ///  is based on the position of the sun 30 minutes after sunset in Jerusalem
        ///  on March 16, about 4 days before the equinox, the day that a solar hour
        ///  is one hour, which calculates to 7.0833333° below
        ///  <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>. This is time some opinions
        ///  consider dark enough for 3 stars to be visible. This is the opinion of
        ///  the Shu"t Melamed Leho'il, Shu"t Binyan Tziyon, Tenuvas Sadeh and very
        ///  close to the time of the Mekor Chesed on the Sefer chasidim.
        ///</summary>
        ///<seealso cref = "getTzaisGeonim7Point083Degrees()" />
        ///<seealso cref = "getBainHasmashosRT13Point5MinutesBefore7Point083Degrees()" />
        protected internal const double ZENITH_7_POINT_083 = GEOMETRIC_ZENITH + 7 + (5/60);

        ///<summary>
        ///  The zenith of 10.2° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>misheyakir</em>
        ///  according to some opinions. This calculation is based on the position of
        ///  the sun 45 minutes before <seealso cref = "getSunris">sunrise</seealso> in Jerusalem on
        ///  March 16, about 4 days before the equinox, the day that a solar hour is
        ///  one hour which calculates to 10.2° below <see cref = "GEOMETRIC_ZENITH"> geometric zenith</see>
        ///</summary>
        ///<seealso cref = "getMisheyakir10Point2Degrees()" />
        protected internal const double ZENITH_10_POINT_2 = GEOMETRIC_ZENITH + 10.2;

        ///<summary>
        ///  The zenith of 11° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>misheyakir</em>
        ///  according to some opinions. This calculation is based on the position of
        ///  the sun 48 minutes before <seealso cref = "getSunris">sunrise</seealso> in Jerusalem on
        ///  March 16, about 4 days before the equinox, the day that a solar hour is
        ///  one hour which calculates to 11° below <see cref = "GEOMETRIC_ZENITH"> geometric zenith</see>
        ///</summary>
        ///<seealso cref = "getMisheyakir11Degrees()" />
        protected internal const double ZENITH_11_DEGREES = GEOMETRIC_ZENITH + 11;

        ///<summary>
        ///  The zenith of 11.5° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>misheyakir</em>
        ///  according to some opinions. This calculation is based on the position of
        ///  the sun 52 minutes before <seealso cref = "getSunris">sunrise</seealso> in Jerusalem on
        ///  March 16, about 4 days before the equinox, the day that a solar hour is
        ///  one hour which calculates to 11.5° below <see cref = "GEOMETRIC_ZENITH"> geometric zenith</see>
        ///</summary>
        ///<seealso cref = "getMisheyakir11Point5Degrees()" />
        protected internal const double ZENITH_11_POINT_5 = GEOMETRIC_ZENITH + 11.5;

        ///<summary>
        ///  The zenith of 13° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating
        ///  <em>Rabainu Tam's bain hashmashos</em> according to some opinions. <br />
        ///  <br />
        ///  <b>FIXME:</b> See comments on <seealso cref = "getBainHasmashosRT13Degrees" />. This
        ///  should be changed to 13.2477 after confirmation.
        ///</summary>
        ///<seealso cref = "getBainHasmashosRT13Degrees" />
        protected internal const double ZENITH_13_DEGREES = GEOMETRIC_ZENITH + 13;

        ///<summary>
        ///  The zenith of 19.8° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>alos</em> (dawn)
        ///  and <em>tzais</em> (nightfall) according to some opinions. This
        ///  calculation is based on the position of the sun 90 minutes after sunset
        ///  in Jerusalem on March 16, about 4 days before the equinox, the day that a
        ///  solar hour is one hour which calculates to 19.8° below
        ///  <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///</summary>
        ///<seealso cref = "getTzais19Point8Degrees()" />
        ///<seealso cref = "getAlos19Point8Degrees()" />
        ///<seealso cref = "getAlos90()" />
        ///<seealso cref = "getTzais90()" />
        protected internal const double ZENITH_19_POINT_8 = GEOMETRIC_ZENITH + 19.8;

        ///<summary>
        ///  The zenith of 26° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>alos</em> (dawn)
        ///  and <em>tzais</em> (nightfall) according to some opinions. This
        ///  calculation is based on the position of the sun <see cref = "getAlos120()">120 minutes</see>
        ///  after sunset in Jerusalem on March 16, about 4 days before the
        ///  equinox, the day that a solar hour is one hour which calculates to
        ///  26° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///</summary>
        ///<seealso cref = "getAlos26Degrees()" />
        ///<seealso cref = "getTzais26Degrees()" />
        ///<seealso cref = "getAlos120()" />
        ///<seealso cref = "getTzais120()" />
        protected internal const double ZENITH_26_DEGREES = GEOMETRIC_ZENITH + 26.0;

        ///NOTE: Experimental and may not make the final 1.3 cut
        ///<summary>
        ///  The zenith of 4.37° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  position of the sun <see cref = "getTzaisGeonim4Point37Degrees()">16 7/8 minutes</see>
        ///  after sunset (3/4 of a 22.5 minute Mil) in Jerusalem on March
        ///  16, about 4 days before the equinox, the day that a solar hour is one
        ///  hour which calculates to 4.37° below <see cref = "GEOMETRIC_ZENITH"> geometric zenith</see>
        ///</summary>
        ///<seealso cref = "getTzaisGeonim4Point37Degrees()" />
        protected internal const double ZENITH_4_POINT_37 = GEOMETRIC_ZENITH + 4.37;

        ///<summary>
        ///  The zenith of 4.61° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  position of the sun <seealso cref = "getTzaisGeonim4Point37Degrees">18 minutes</seealso>
        ///  after sunset (3/4 of a 24 minute Mil) in Jerusalem on March 16, about 4
        ///  days before the equinox, the day that a solar hour is one hour which
        ///  calculates to 4.61° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///</summary>
        ///<seealso cref = "getTzaisGeonim4Point61Degrees()" />
        protected internal const double ZENITH_4_POINT_61 = GEOMETRIC_ZENITH + 4.61;

        protected internal const double ZENITH_4_POINT_8 = GEOMETRIC_ZENITH + 4.8;

        ///<summary>
        ///  The zenith of 3.65° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). This calculation is used for calculating <em>tzais</em>
        ///  (nightfall) according to some opinions. This calculation is based on the
        ///  position of the sun <seealso cref = "getTzaisGeonim3Point65Degrees">13.5 minutes</seealso>
        ///  after sunset (3/4 of an 18 minute Mil) in Jerusalem on March 16, about 4
        ///  days before the equinox, the day that a solar hour is one hour which
        ///  calculates to 3.65° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///</summary>
        ///<seealso cref = "getTzaisGeonim3Point65Degrees()" />
        protected internal const double ZENITH_3_POINT_65 = GEOMETRIC_ZENITH + 3.65;

        protected internal const double ZENITH_5_POINT_88 = GEOMETRIC_ZENITH + 5.88;

        private double ateretTorahSunsetOffset = 40;

        public ComplexZmanimCalendar(GeoLocation location)
            : base(location)
        {
        }

        ///<summary>
        ///  Default constructor will set a default <seealso cref =  GeoLocation.GeoLocation()"/>,
        ///	a default <see cref =  AstronomicalCalculator.getDefault()"> AstronomicalCalculator</see>
        ///                                           and default the calendar to the current date.
        ///</summary>
        ///<seealso cref =  AstronomicalCalendar.AstronomicalCalendar()"/>
        public ComplexZmanimCalendar()
        {
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///  a 19.8° dip. This calculation divides the day based on the opinion of
        ///  the MGA that the day runs from dawn to dusk. Dawn for this calculation is
        ///  when the sun is 19.8° below the eastern geometric horizon before
        ///  sunrise. Dusk for this is when the sun is 19.8° below the western
        ///  geometric horizon after sunset. This day is split into 12 equal parts
        ///  with each part being a <em>shaah zmanis</em>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis19Point8Degrees()
        {
            return getTemporalHour(getAlos19Point8Degrees(), getTzais19Point8Degrees());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///  a 18° dip. This calculation divides the day based on the opinion of
        ///  the MGA that the day runs from dawn to dusk. Dawn for this calculation is
        ///  when the sun is 18° below the eastern geometric horizon before
        ///  sunrise. Dusk for this is when the sun is 18° below the western
        ///  geometric horizon after sunset. This day is split into 12 equal parts
        ///  with each part being a <em>shaah zmanis</em>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis18Degrees()
        {
            return getTemporalHour(getAlos18Degrees(), getTzais18Degrees());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///  a dip of 26°. This calculation divides the day based on the opinion
        ///  of the MGA that the day runs from dawn to dusk. Dawn for this calculation
        ///  is when the sun is <seealso cref = "getAlos26Degrees">26°</seealso> below the eastern
        ///  geometric horizon before sunrise. Dusk for this is when the sun is
        ///  <seealso cref = "getTzais26Degrees">26°</seealso> below the western geometric horizon
        ///  after sunset. This day is split into 12 equal parts with each part being
        ///  a <em>shaah zmanis</em>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis26Degrees()
        {
            return getTemporalHour(getAlos26Degrees(), getTzais26Degrees());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///  a dip of 16.1°. This calculation divides the day based on the opinion
        ///  that the day runs from dawn to dusk. Dawn for this calculation is when
        ///  the sun is 16.1° below the eastern geometric horizon before sunrise
        ///  and dusk is when the sun is 16.1° below the western geometric horizon
        ///  after sunset. This day is split into 12 equal parts with each part being
        ///  a <em>shaah zmanis</em>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        ///<seealso cref = "getAlos16Point1Degrees()" />
        ///<seealso cref = "getTzais16Point1Degrees()" />
        ///<seealso cref = "getSofZmanShmaMGA16Point1Degrees()" />
        ///<seealso cref = "getSofZmanTfilaMGA16Point1Degrees()" />
        ///<seealso cref = "getMinchaGedola16Point1Degrees()" />
        ///<seealso cref = "getMinchaKetana16Point1Degrees()" />
        ///<seealso cref = "getPlagHamincha16Point1Degrees()" />
        public virtual long getShaahZmanis16Point1Degrees()
        {
            return getTemporalHour(getAlos16Point1Degrees(), getTzais16Point1Degrees());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (solar hour) according to the
        ///  opinion of the MGA. This calculation divides the day based on the opinion
        ///  of the <em>MGA</em> that the day runs from dawn to dusk. Dawn for this
        ///  calculation is 60 minutes before sunrise and dusk is 60 minutes after
        ///  sunset. This day is split into 12 equal parts with each part being a
        ///  <em>shaah zmanis</em>. Alternate mothods of calculating a
        ///  <em>shaah zmanis</em> are available in the subclass
        ///  <seealso cref = "ComplexZmanimCalendar" />
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis60Minutes()
        {
            return getTemporalHour(getAlos60(), getTzais60());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (solar hour) according to the
        ///  opinion of the MGA. This calculation divides the day based on the opinion
        ///  of the <em>MGA</em> that the day runs from dawn to dusk. Dawn for this
        ///  calculation is 72 minutes before sunrise and dusk is 72 minutes after
        ///  sunset. This day is split into 12 equal parts with each part being a
        ///  <em>shaah zmanis</em>. Alternate mothods of calculating a
        ///  <em>shaah zmanis</em> are available in the subclass
        ///  <seealso cref = "ComplexZmanimCalendar" />
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis72Minutes()
        {
            return getShaahZmanisMGA();
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///  opinion of the MGA based on <em>alos</em> being
        ///  <seealso cref = "getAlos72Zmanis">72</seealso> minutes <em>zmaniyos</em> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This calculation divides the day based on
        ///  the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ///  for this calculation is 72 minutes <em>zmaniyos</em> before sunrise and
        ///  dusk is 72 minutes <em>zmaniyos</em> after sunset. This day is split into
        ///  12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ///  identical to 1/10th of the day from <seealso cref = "getSunrise">sunrise</seealso> to
        ///  <seealso cref = "getSunset">sunset</seealso>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        ///<seealso cref = "getAlos72Zmanis()" />
        ///<seealso cref = "getTzais72Zmanis()" />
        public virtual long getShaahZmanis72MinutesZmanis()
        {
            return getTemporalHour(getAlos72Zmanis(), getTzais72Zmanis());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///  a dip of 90 minutes. This calculation divides the day based on the
        ///  opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ///  calculation is 90 minutes before sunrise and dusk is 90 minutes after
        ///  sunset. This day is split into 12 equal parts with each part being a
        ///  <em>shaah zmanis</em>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis90Minutes()
        {
            return getTemporalHour(getAlos90(), getTzais90());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///  opinion of the MGA based on <em>alos</em> being
        ///  <seealso cref = "getAlos90Zmanis">90</seealso> minutes <em>zmaniyos</em> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This calculation divides the day based on
        ///  the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ///  for this calculation is 90 minutes <em>zmaniyos</em> before sunrise and
        ///  dusk is 90 minutes <em>zmaniyos</em> after sunset. This day is split into
        ///  12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ///  identical to 1/8th of the day from <seealso cref = "getSunrise">sunrise</seealso> to
        ///  <seealso cref = "getSunset">sunset</seealso>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        ///<seealso cref = "getAlos90Zmanis()" />
        ///<seealso cref = "getTzais90Zmanis()" />
        public virtual long getShaahZmanis90MinutesZmanis()
        {
            return getTemporalHour(getAlos90Zmanis(), getTzais90Zmanis());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///  opinion of the MGA based on <em>alos</em> being
        ///  <seealso cref = "getAlos96Zmanis">96</seealso> minutes <em>zmaniyos</em> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This calculation divides the day based on
        ///  the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ///  for this calculation is 96 minutes <em>zmaniyos</em> before sunrise and
        ///  dusk is 96 minutes <em>zmaniyos</em> after sunset. This day is split into
        ///  12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ///  identical to 1/7.5th of the day from <seealso cref = "getSunrise">sunrise</seealso> to
        ///  <seealso cref = "getSunset">sunset</seealso>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        ///<seealso cref = "getAlos96Zmanis()" />
        ///<seealso cref = "getTzais96Zmanis()" />
        public virtual long getShaahZmanis96MinutesZmanis()
        {
            return getTemporalHour(getAlos96Zmanis(), getTzais96Zmanis());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///  opinion of the Chacham Yosef Harari-Raful of Yeshivat Ateret Torah
        ///  calculated with <em>alos</em> being 1/10th of sunrise to sunset day, or
        ///  <seealso cref = "getAlos72Zmanis">72</seealso> minutes <em>zmaniyos</em> of such a day
        ///  before <seealso cref = "getSunrise">sunrise</seealso>, and tzais is usually calculated as
        ///  <seealso cref = "getTzaisAteretTorah">40 minutes</seealso> after <see cref = "getSunset()"> sunset</see>
        ///  . This day is split into 12 equal parts with each part being a
        ///  <em>shaah zmanis</em>. Note that with this system, chatzos (mid-day) will
        ///  not be the point that the sun is <see cref = "getSunTransit()">halfway across the sky</see>
        ///  .
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        ///<seealso cref = "getAlos72Zmanis()" />
        ///<seealso cref = "getTzaisAteretTorah()" />
        ///<seealso cref = "getAteretTorahSunsetOffset()" />
        ///<seealso cref = "setAteretTorahSunsetOffset(double)" />
        public virtual long getShaahZmanisAteretTorah()
        {
            return getTemporalHour(getAlos72Zmanis(), getTzaisAteretTorah());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///  a dip of 96 minutes. This calculation divides the day based on the
        ///  opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ///  calculation is 96 minutes before sunrise and dusk is 96 minutes after
        ///  sunset. This day is split into 12 equal parts with each part being a
        ///  <em>shaah zmanis</em>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis96Minutes()
        {
            return getTemporalHour(getAlos96(), getTzais96());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///  a dip of 120 minutes. This calculation divides the day based on the
        ///  opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ///  calculation is 120 minutes before sunrise and dusk is 120 minutes after
        ///  sunset. This day is split into 12 equal parts with each part being a
        ///  <em>shaah zmanis</em>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis120Minutes()
        {
            return getTemporalHour(getAlos120(), getTzais120());
        }

        ///<summary>
        ///  Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///  opinion of the MGA based on <em>alos</em> being
        ///  <seealso cref = "getAlos120Zmanis">120</seealso> minutes <em>zmaniyos</em> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This calculation divides the day based on
        ///  the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ///  for this calculation is 120 minutes <em>zmaniyos</em> before sunrise and
        ///  dusk is 120 minutes <em>zmaniyos</em> after sunset. This day is split
        ///  into 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ///  identical to 1/6th of the day from <seealso cref = "getSunrise">sunrise</seealso> to
        ///  <seealso cref = "getSunset">sunset</seealso>.
        ///</summary>
        ///<returns> the <code>long</code> millisecond length of a
        ///  <em>shaah zmanis</em>. </returns>
        ///<seealso cref = "getAlos120Zmanis()" />
        ///<seealso cref = "getTzais120Zmanis()" />
        public virtual long getShaahZmanis120MinutesZmanis()
        {
            return getTemporalHour(getAlos120Zmanis(), getTzais120Zmanis());
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos120Zmanis">dawn</seealso>. The
        ///  formula used is:<br />
        ///  10.75 * <seealso cref = "getShaahZmanis120MinutesZmanis()" /> after
        ///  <seealso cref = "getAlos120Zmanis">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha120MinutesZmanis()
        {
            return getTimeOffset(getAlos120Zmanis(), getShaahZmanis120MinutesZmanis()*10.75);
        }


        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos120">dawn</seealso>. The formula
        ///  used is:<br />
        ///  10.75 <seealso cref = "getShaahZmanis120Minutes()" /> after <seealso cref = "getAlos120()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha120Minutes()
        {
            return getTimeOffset(getAlos120(), getShaahZmanis120Minutes()*10.75);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 60 minutes before
        ///  <seealso cref = "getSeaLevelSunrise">sea level sunrise</seealso> on the time to walk the
        ///  distance of 4 <em>Mil</em> at 15 minutes a <em>Mil</em> (the opinion of
        ///  the Chavas Yair. See the Divray Malkiel). This is based on the opinion of
        ///  most <em>Rishonim</em> who stated that the time of the <em>Neshef</em>
        ///  (time between dawn and sunrise) does not vary by the time of year or
        ///  location but purely depends on the time it takes to walk the distance of
        ///  4 <em>Mil</em>.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        public virtual Date getAlos60()
        {
            return getTimeOffset(getSeaLevelSunrise(), -60*MINUTE_MILLIS);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 72 minutes
        ///  <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/10th
        ///  of the day before sea level sunrise. This is based on an 18 minute
        ///  <em>Mil</em> so the time for 4 <em>Mil</em> is 72 minutes which is 1/10th
        ///  of a day (12 * 60 = 720) based on the day starting at
        ///  <seealso cref = "getSeaLevelSunrise">sea level sunrise</seealso> and ending at
        ///  <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>. The actual alculation is
        ///  <seealso cref = "getSeaLevelSunrise()" />- ( <seealso cref = "getShaahZmanisGra()" /> * 1.2).
        ///  This calculation is used in the calendars published by
        ///  <em>Hisachdus Harabanim D'Artzos Habris Ve'Kanada</em>
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getShaahZmanisGra()" />
        public virtual Date getAlos72Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunrise(), (long) (shaahZmanis*-1.2));
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 96 minutes before
        ///  <seealso cref = "getSeaLevelSunrise">sea level sunrise</seealso> based on the time to walk
        ///  the distance of 4 <em>Mil</em> at 24 minutes a <em>Mil</em>. This is
        ///  based on the opinion of most <em>Rishonim</em> who stated that the time
        ///  of the <em>Neshef</em> (time between dawn and sunrise) does not vary by
        ///  the time of year or location but purely depends on the time it takes to
        ///  walk the distance of 4 <em>Mil</em>.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        public virtual Date getAlos96()
        {
            return getTimeOffset(getSeaLevelSunrise(), -96*MINUTE_MILLIS);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 90 minutes
        ///  <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/8th
        ///  of the day before sea level sunrise. This is based on a 22.5 minute
        ///  <em>Mil</em> so the time for 4 <em>Mil</em> is 90 minutes which is 1/8th
        ///  of a day (12 * 60 = 720) /8 =90 based on the day starting at
        ///  <see cref = "AstronomicalCalendar.getSunrise()">sunrise</see> and ending at <seealso cref = "getSunset">sunset</seealso>.
        ///  The actual calculation is <seealso cref = "AstronomicalCalendar.getSunrise()" /> - (
        ///  <seealso cref = "getShaahZmanisGra()" /> * 1.5).
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getShaahZmanisGra()" />
        public virtual Date getAlos90Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunrise(), (long) (shaahZmanis*-1.5));
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 90 minutes
        ///  <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/8th
        ///  of the day before sea level sunrise. This is based on a 24 minute
        ///  <em>Mil</em> so the time for 4 <em>Mil</em> is 90 minutes which is
        ///  1/7.5th of a day (12 * 60 = 720) / 7.5 =96 based on the day starting at
        ///  <seealso cref = "AstronomicalCalendar.getSunrise()">sunrise</seealso> and ending at <seealso cref = "getSunset">sunset</seealso>.
        ///  The actual calculation is <seealso cref = "AstronomicalCalendar.getSunrise()" /> - (
        ///  <seealso cref = "getShaahZmanisGra()" /> * 1.6).
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getShaahZmanisGra()" />
        public virtual Date getAlos96Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunrise(), (long) (shaahZmanis*-1.6));
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 90 minutes before
        ///  <seealso cref = "getSeaLevelSunrise">sea level sunrise</seealso> on the time to walk the
        ///  distance of 4 <em>Mil</em> at 22.5 minutes a <em>Mil</em>. This is based
        ///  on the opinion of most <em>Rishonim</em> who stated that the time of the
        ///  <em>Neshef</em> (time between dawn and sunrise) does not vary by the time
        ///  of year or location but purely depends on the time it takes to walk the
        ///  distance of 4 <em>Mil</em>.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        public virtual Date getAlos90()
        {
            return getTimeOffset(getSeaLevelSunrise(), -90*MINUTE_MILLIS);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 120 minutes before
        ///  <seealso cref = "getSeaLevelSunrise">sea level sunrise</seealso> (no adjustment for
        ///  elevation is made) based on the time to walk the distance of 5
        ///  <em>Mil</em>( <em>Ula</em>) at 24 minutes a <em>Mil</em>. This is based
        ///  on the opinion of most <em>Rishonim</em> who stated that the time of the
        ///  <em>Neshef</em> (time between dawn and sunrise) does not vary by the time
        ///  of year or location but purely depends on the time it takes to walk the
        ///  distance of 5 <em>Mil</em>(<em>Ula</em>).
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        public virtual Date getAlos120()
        {
            return getTimeOffset(getSeaLevelSunrise(), -120*MINUTE_MILLIS);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated using 120 minutes
        ///  <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/6th
        ///  of the day before sea level sunrise. This is based on a 24 minute
        ///  <em>Mil</em> so the time for 5 <em>Mil</em> is 120 minutes which is 1/6th
        ///  of a day (12 * 60 = 720) / 6 =120 based on the day starting at
        ///  <seealso cref = "AstronomicalCalendar.getSunrise()">sunrise</seealso> and ending at <seealso cref = "getSunset">sunset</seealso>.
        ///  The actual calculation is <seealso cref = "AstronomicalCalendar.getSunrise()" /> - (
        ///  <seealso cref = "getShaahZmanisGra()" /> * 2).
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getShaahZmanisGra()" />
        public virtual Date getAlos120Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunrise(), shaahZmanis*-2);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated when the sun is
        ///  <seealso cref = "ZENITH_26_DEGREES">26°</seealso> below the eastern geometric horizon
        ///  before sunrise. This calculation is based on the same calculation of
        ///  <seealso cref = "getAlos120">120 minutes</seealso> but uses a degree based calculation
        ///  instead of 120 exact minutes. This calculation is based on the position
        ///  of the sun 120 minutes before sunrise in Jerusalem in the equinox which
        ///  calculates to 26° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> representing <em>alos</em>. </returns>
        ///<seealso cref = "ZENITH_26_DEGREES" />
        ///<seealso cref = "getAlos120()" />
        ///<seealso cref = "getTzais120()" />
        public virtual Date getAlos26Degrees()
        {
            return getSunriseOffsetByDegrees(ZENITH_26_DEGREES);
        }

        ///<summary>
        ///  to return <em>alos</em> (dawn) calculated when the sun is
        ///  <seealso cref = "ASTRONOMICAL_ZENIT">18°</seealso> below the eastern geometric horizon
        ///  before sunrise.
        ///</summary>
        ///<returns> the <c>Date</c> representing <em>alos</em>. </returns>
        ///<seealso cref = "ASTRONOMICAL_ZENITH" />
        public virtual Date getAlos18Degrees()
        {
            return getSunriseOffsetByDegrees(ASTRONOMICAL_ZENITH);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated when the sun is
        ///  <seealso cref = "ZENITH_19_POINT_8">19.8°</seealso> below the eastern geometric horizon
        ///  before sunrise. This calculation is based on the same calculation of
        ///  <seealso cref = "getAlos90">90 minutes</seealso> but uses a degree based calculation
        ///  instead of 90 exact minutes. This calculation is based on the position of
        ///  the sun 90 minutes before sunrise in Jerusalem in the equinox which
        ///  calculates to 19.8° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///</summary>
        ///<returns> the <c>Date</c> representing <em>alos</em>. </returns>
        ///<seealso cref = "ZENITH_19_POINT_8" />
        ///<seealso cref = "getAlos90()" />
        public virtual Date getAlos19Point8Degrees()
        {
            return getSunriseOffsetByDegrees(ZENITH_19_POINT_8);
        }

        ///<summary>
        ///  Method to return <em>alos</em> (dawn) calculated when the sun is
        ///  <seealso cref = "ZENITH_16_POINT_1">16.1°</seealso> below the eastern geometric horizon
        ///  before sunrise. This calculation is based on the same calculation of
        ///  <seealso cref = "getAlos72">72 minutes</seealso> but uses a degree based calculation
        ///  instead of 72 exact minutes. This calculation is based on the position of
        ///  the sun 72 minutes before sunrise in Jerusalem in the equinox which
        ///  calculates to 16.1° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> representing <em>alos</em>. </returns>
        ///<seealso cref = "ZENITH_16_POINT_1" />
        ///<seealso cref = "getAlos72()" />
        public virtual Date getAlos16Point1Degrees()
        {
            return getSunriseOffsetByDegrees(ZENITH_16_POINT_1);
        }

        ///<summary>
        ///  This method returns <em>misheyakir</em> based on the position of the sun
        ///  when it is <seealso cref = "ZENITH_11_DEGREES">11.5°</seealso> below
        ///  <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso> (90°). This calculation is
        ///  used for calculating <em>misheyakir</em> according to some opinions. This
        ///  calculation is based on the position of the sun 52 minutes before
        ///  <seealso cref = "getSunris">sunrise</seealso>in Jerusalem in the equinox which calculates
        ///  to 11.5° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///</summary>
        ///<seealso cref = "ZENITH_11_POINT_5" />
        public virtual Date getMisheyakir11Point5Degrees()
        {
            return getSunriseOffsetByDegrees(ZENITH_11_POINT_5);
        }

        ///<summary>
        ///  This method returns <em>misheyakir</em> based on the position of the sun
        ///  when it is <seealso cref = "ZENITH_11_DEGREES">11°</seealso> below
        ///  <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso> (90°). This calculation is
        ///  used for calculating <em>misheyakir</em> according to some opinions. This
        ///  calculation is based on the position of the sun 48 minutes before
        ///  <seealso cref = "getSunris">sunrise</seealso>in Jerusalem in the equinox which calculates
        ///  to 11° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///</summary>
        ///<seealso cref = "ZENITH_11_DEGREES" />
        public virtual Date getMisheyakir11Degrees()
        {
            return getSunriseOffsetByDegrees(ZENITH_11_DEGREES);
        }

        ///<summary>
        ///  This method returns <em>misheyakir</em> based on the position of the sun
        ///  when it is <seealso cref = "ZENITH_10_POINT_2">10.2°</seealso> below
        ///  <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso> (90°). This calculation is
        ///  used for calculating <em>misheyakir</em> according to some opinions. This
        ///  calculation is based on the position of the sun 45 minutes before
        ///  <seealso cref = "getSunris">sunrise</seealso> in Jerusalem in the equinox which calculates
        ///  to 10.2° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///</summary>
        ///<seealso cref = "ZENITH_10_POINT_2" />
        public virtual Date getMisheyakir10Point2Degrees()
        {
            return getSunriseOffsetByDegrees(ZENITH_10_POINT_2);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos19Point8Degrees()">19.8°</seealso> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 3
        ///  <em><seealso cref = "getShaahZmanis19Point8Degrees">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos19Point8Degrees">dawn</seealso> based on the opinion
        ///  of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        ///  with both being 19.8° below sunrise or sunset. This returns the time
        ///  of 3 <seealso cref = "getShaahZmanis19Point8Degrees()" /> after
        ///  <seealso cref = "getAlos19Point8Degrees">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis19Point8Degrees()" />
        ///<seealso cref = "getAlos19Point8Degrees()" />
        public virtual Date getSofZmanShmaMGA19Point8Degrees()
        {
            return getTimeOffset(getAlos19Point8Degrees(), getShaahZmanis19Point8Degrees()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos16Point1Degrees()">16.1°</seealso> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 3
        ///  <em><seealso cref = "getShaahZmanis16Point1Degrees">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos16Point1Degrees">dawn</seealso> based on the opinion
        ///  of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        ///  with both being 16.1° below sunrise or sunset. This returns the time
        ///  of 3 <seealso cref = "getShaahZmanis16Point1Degrees()" /> after
        ///  <seealso cref = "getAlos16Point1Degrees">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis16Point1Degrees()" />
        ///<seealso cref = "getAlos16Point1Degrees()" />
        public virtual Date getSofZmanShmaMGA16Point1Degrees()
        {
            return getTimeOffset(getAlos16Point1Degrees(), getShaahZmanis16Point1Degrees()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos72">72</seealso> minutes before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 3
        ///  <em><seealso cref = "getShaahZmanis72Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ///  after <seealso cref = "getAlos72">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ///  that the day is calculated from a <seealso cref = "getAlos72">dawn</seealso> of 72 minutes
        ///  before sunrise to <seealso cref = "getTzais72">nightfall</seealso> of 72 minutes after
        ///  sunset. This returns the time of 3 * <seealso cref = "getShaahZmanis72Minutes()" />
        ///  after <seealso cref = "getAlos72">dawn</seealso>. This class returns an identical time to
        ///  <seealso cref = "getSofZmanShmaMGA()" /> and is repeated here for clarity.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis72Minutes()" />
        ///<seealso cref = "getAlos72()" />
        ///<seealso cref = "getSofZmanShmaMGA()" />
        public virtual Date getSofZmanShmaMGA72Minutes()
        {
            return getSofZmanShmaMGA();
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos72Zmanis">72</seealso> minutes
        ///  <em>zmaniyos</em>, or 1/10th of the day before <see cref = "AstronomicalCalendar.getSunrise()"> sunrise</see>
        ///  . This time is 3
        ///  <em><seealso cref = "getShaahZmanis90MinutesZmanis">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos72Zmanis">dawn</seealso> based on the opinion of the
        ///  <em>MG"A</em> that the day is calculated from a
        ///  <seealso cref = "getAlos72Zmanis">dawn</seealso> of 72 minutes <em>zmaniyos</em>, or
        ///  1/10th of the day before <seealso cref = "getSeaLevelSunrise">sea level sunrise</seealso>
        ///  to <seealso cref = "getTzais72Zmanis">nightfall</seealso> of 72 minutes <em>zmaniyos</em>
        ///  after <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>. This returns the
        ///  time of 3 * <seealso cref = "getShaahZmanis72MinutesZmanis()" /> after
        ///  <seealso cref = "getAlos72Zmanis">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis72MinutesZmanis()" />
        ///<seealso cref = "getAlos72Zmanis()" />
        public virtual Date getSofZmanShmaMGA72MinutesZmanis()
        {
            return getTimeOffset(getAlos72Zmanis(), getShaahZmanis72MinutesZmanis()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos90">90</seealso> minutes before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 3
        ///  <em><seealso cref = "getShaahZmanis90Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ///  after <seealso cref = "getAlos90">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ///  that the day is calculated from a <seealso cref = "getAlos90">dawn</seealso> of 90 minutes
        ///  before sunrise to <seealso cref = "getTzais90">nightfall</seealso> of 90 minutes after
        ///  sunset. This returns the time of 3 * <seealso cref = "getShaahZmanis90Minutes()" />
        ///  after <seealso cref = "getAlos90">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis90Minutes()" />
        ///<seealso cref = "getAlos90()" />
        public virtual Date getSofZmanShmaMGA90Minutes()
        {
            return getTimeOffset(getAlos90(), getShaahZmanis90Minutes()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos90Zmanis">90</seealso> minutes
        ///  <em>zmaniyos</em> before <seealso cref = "getSunrise">sunrise</seealso>. This time is 3
        ///  <em><seealso cref = "getShaahZmanis90MinutesZmanis">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos90Zmanis">dawn</seealso> based on the opinion of the
        ///  <em>MG"A</em> that the day is calculated from a
        ///  <seealso cref = "getAlos90Zmanis">dawn</seealso> of 90 minutes <em>zmaniyos</em> before
        ///  sunrise to <seealso cref = "getTzais90Zmanis">nightfall</seealso> of 90 minutes
        ///  <em>zmaniyos</em> after sunset. This returns the time of 3 *
        ///  <seealso cref = "getShaahZmanis90MinutesZmanis()" /> after <see cref = "getAlos90Zmanis()"> dawn</see>
        ///  .
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis90MinutesZmanis()" />
        ///<seealso cref = "getAlos90Zmanis()" />
        public virtual Date getSofZmanShmaMGA90MinutesZmanis()
        {
            return getTimeOffset(getAlos90Zmanis(), getShaahZmanis90MinutesZmanis()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos96">96</seealso> minutes before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 3
        ///  <em><seealso cref = "getShaahZmanis96Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ///  after <seealso cref = "getAlos96">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ///  that the day is calculated from a <seealso cref = "getAlos96">dawn</seealso> of 96 minutes
        ///  before sunrise to <seealso cref = "getTzais96">nightfall</seealso> of 96 minutes after
        ///  sunset. This returns the time of 3 * <seealso cref = "getShaahZmanis96Minutes()" />
        ///  after <seealso cref = "getAlos96">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis96Minutes()" />
        ///<seealso cref = "getAlos96()" />
        public virtual Date getSofZmanShmaMGA96Minutes()
        {
            return getTimeOffset(getAlos96(), getShaahZmanis96Minutes()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos90Zmanis">96</seealso> minutes
        ///  <em>zmaniyos</em> before <seealso cref = "getSunrise">sunrise</seealso>. This time is 3
        ///  <em><seealso cref = "getShaahZmanis96MinutesZmanis">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos96Zmanis">dawn</seealso> based on the opinion of the
        ///  <em>MG"A</em> that the day is calculated from a
        ///  <seealso cref = "getAlos96Zmanis">dawn</seealso> of 96 minutes <em>zmaniyos</em> before
        ///  sunrise to <seealso cref = "getTzais90Zmanis">nightfall</seealso> of 96 minutes
        ///  <em>zmaniyos</em> after sunset. This returns the time of 3 *
        ///  <seealso cref = "getShaahZmanis96MinutesZmanis()" /> after <see cref = "getAlos96Zmanis()"> dawn</see>
        ///  .
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis96MinutesZmanis()" />
        ///<seealso cref = "getAlos96Zmanis()" />
        public virtual Date getSofZmanShmaMGA96MinutesZmanis()
        {
            return getTimeOffset(getAlos96Zmanis(), getShaahZmanis96MinutesZmanis()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) calculated as 3 hours (regular and not zmaniyos)
        ///  before <seealso cref =  ZmanimCalendar.getChatzos()"/>. This is the opinion of the
        ///	<em>Shach</em> in the
        ///           <em>Nekudas Hakesef (Yora Deah 184), Shevus Yaakov, Chasan Sofer</em> and
        ///           others.This returns the time of 3 hours before
        ///           <seealso cref =  ZmanimCalendar.getChatzos()"/>.
        ///	 
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref =  ZmanimCalendar.getChatzos()"/>
        ///	<seealso cref = "getSofZmanTfila2HoursBeforeChatzos()" />
        public virtual Date getSofZmanShma3HoursBeforeChatzos()
        {
            return getTimeOffset(getChatzos(), -180*MINUTE_MILLIS);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos120">120</seealso> minutes or 1/6th of the day
        ///  before <seealso cref = "getSunrise">sunrise</seealso>. This time is 3
        ///  <em><seealso cref = "getShaahZmanis120Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ///  after <seealso cref = "getAlos120">dawn</seealso> based on the opinion of the
        ///  <em>MG"A</em> that the day is calculated from a <see cref = "getAlos120()"> dawn</see>
        ///  of 120 minutes before sunrise to <seealso cref = "getTzais120">nightfall</seealso>
        ///  of 120 minutes after sunset. This returns the time of 3 *
        ///  <seealso cref = "getShaahZmanis120Minutes()" /> after <seealso cref = "getAlos120">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis120Minutes()" />
        ///<seealso cref = "getAlos120()" />
        public virtual Date getSofZmanShmaMGA120Minutes()
        {
            return getTimeOffset(getAlos120(), getShaahZmanis120Minutes()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) based on the opinion that the day starts at
        ///  <em><seealso cref = "getAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        ///  <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>. 3 shaos zmaniyos are
        ///  calculated based on this day and added to
        ///  <seealso cref = "getAlos16Point1Degrees">alos</seealso>to reach this time. This time is 3
        ///  <em>shaos zmaniyos</em> (solar hours) after
        ///  <seealso cref = "getAlos16Point1Degrees">dawn</seealso> based on the opinion that the day
        ///  is calculated from a <seealso cref = "getAlos16Point1Degrees">alos 16.1°</seealso> to
        ///  <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>.<br />
        ///  <b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        ///  midday.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema based on this day. </returns>
        ///<seealso cref = "getAlos16Point1Degrees()" />
        ///<seealso cref = "getSeaLevelSunset()" />
        public virtual Date getSofZmanShmaAlos16Point1ToSunset()
        {
            long shaahZmanis = getTemporalHour(getAlos16Point1Degrees(), getSeaLevelSunset());
            return getTimeOffset(getAlos16Point1Degrees(), shaahZmanis*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) based on the opinion that the day starts at
        ///  <em><seealso cref = "getAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        ///  <seealso cref = "getTzaisGeonim7Point083Degrees">tzais 7.083°</seealso>. 3
        ///  <em>shaos zmaniyos</em> are calculated based on this day and added to
        ///  <seealso cref = "getAlos16Point1Degrees">alos</seealso> to reach this time. This time is 3
        ///  <em>shaos zmaniyos</em> (temporal hours) after
        ///  <seealso cref = "getAlos16Point1Degrees">alos 16.1°</seealso> based on the opinion
        ///  that the day is calculated from a <see cref = "getAlos16Point1Degrees()">alos 16.1°</see>
        ///  to
        ///  <em><seealso cref = "getTzaisGeonim7Point083Degrees">tzais 7.083°</seealso></em>.<br />
        ///  <b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        ///  midday.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema based on this
        ///  calculation. </returns>
        ///<seealso cref = "getAlos16Point1Degrees()" />
        ///<seealso cref = "getTzaisGeonim7Point083Degrees()" />
        public virtual Date getSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            long shaahZmanis = getTemporalHour(getAlos16Point1Degrees(), getTzaisGeonim7Point083Degrees());
            return getTimeOffset(getAlos16Point1Degrees(), shaahZmanis*3);
        }

        ///<summary>
        ///  From the GR"A in Kol Eliyahu on Berachos #173 that states that zman krias
        ///  shema is calculated as half the time from <see cref = "getSeaLevelSunrise()"> sea level sunset</see>
        ///  to fixed local chatzos. The GR"A himself seems to
        ///  contradic this when he stated that zman krias shema is 1/4 of the day
        ///  from sunrise to sunset. See Sarah Lamoed #25 in Yisroel Vehazmanim Vol
        ///  III page 1016.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema based on this
        ///  calculation. </returns>
        ///<seealso cref = "getFixedLocalChatzos()" />
        public virtual Date getSofZmanShmaKolEliyahu()
        {
            Date chatzos = getFixedLocalChatzos();
            if (chatzos == null || getSunrise() == null)
            {
                return null;
            }
            long diff = (chatzos.getTime() - getSeaLevelSunrise().getTime())/2;
            return getTimeOffset(chatzos, -diff);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos19Point8Degrees()">19.8°</seealso> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis19Point8Degrees">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos19Point8Degrees">dawn</seealso> based on the opinion
        ///  of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        ///  with both being 19.8° below sunrise or sunset. This returns the time
        ///  of 4 <seealso cref = "getShaahZmanis19Point8Degrees()" /> after
        ///  <seealso cref = "getAlos19Point8Degrees">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema.
        ///</returns>
        ///<seealso cref = "getShaahZmanis19Point8Degrees()" />
        ///<seealso cref = "getAlos19Point8Degrees()" />
        public virtual Date getSofZmanTfilaMGA19Point8Degrees()
        {
            return getTimeOffset(getAlos19Point8Degrees(), getShaahZmanis19Point8Degrees()*4);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos19Point8Degrees()">16.1°</seealso> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis16Point1Degrees">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos16Point1Degrees">dawn</seealso> based on the opinion
        ///  of the <em>MG"A</em> that the day is calculated from dawn to nightfall
        ///  with both being 16.1° below sunrise or sunset. This returns the time
        ///  of 4 <seealso cref = "getShaahZmanis16Point1Degrees()" /> after
        ///  <seealso cref = "getAlos16Point1Degrees">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema.
        ///</returns>
        ///<seealso cref = "getShaahZmanis16Point1Degrees()" />
        ///<seealso cref = "getAlos16Point1Degrees()" />
        public virtual Date getSofZmanTfilaMGA16Point1Degrees()
        {
            return getTimeOffset(getAlos16Point1Degrees(), getShaahZmanis16Point1Degrees()*4);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos72">72</seealso> minutes before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis72Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ///  after <seealso cref = "getAlos72">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ///  that the day is calculated from a <seealso cref = "getAlos72">dawn</seealso> of 72 minutes
        ///  before sunrise to <seealso cref = "getTzais72">nightfall</seealso> of 72 minutes after
        ///  sunset. This returns the time of 4 * <seealso cref = "getShaahZmanis72Minutes()" />
        ///  after <seealso cref = "getAlos72">dawn</seealso>. This class returns an identical time to
        ///  <seealso cref = "getSofZmanTfilaMGA()" /> and is repeated here for clarity.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman tfila. </returns>
        ///<seealso cref = "getShaahZmanis72Minutes()" />
        ///<seealso cref = "getAlos72()" />
        ///<seealso cref = "getSofZmanShmaMGA()" />
        public virtual Date getSofZmanTfilaMGA72Minutes()
        {
            return getSofZmanTfilaMGA();
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to the morning
        ///  prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        ///  <seealso cref = "getAlos72Zmanis">72</seealso> minutes <em>zmaniyos</em> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis72MinutesZmanis">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos72Zmanis">dawn</seealso> based on the opinion of the
        ///  <em>MG"A</em> that the day is calculated from a
        ///  <seealso cref = "getAlos72Zmanis">dawn</seealso> of 72 minutes <em>zmaniyos</em> before
        ///  sunrise to <seealso cref = "getTzais72Zmanis">nightfall</seealso> of 72 minutes
        ///  <em>zmaniyos</em> after sunset. This returns the time of 4 *
        ///  <seealso cref = "getShaahZmanis72MinutesZmanis()" /> after <see cref = "getAlos72Zmanis()"> dawn</see>
        ///  .
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis72MinutesZmanis()" />
        ///<seealso cref = "getAlos72Zmanis()" />
        public virtual Date getSofZmanTfilaMGA72MinutesZmanis()
        {
            return getTimeOffset(getAlos72Zmanis(), getShaahZmanis72MinutesZmanis()*4);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos90">90</seealso> minutes before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis90Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ///  after <seealso cref = "getAlos90">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ///  that the day is calculated from a <seealso cref = "getAlos90">dawn</seealso> of 90 minutes
        ///  before sunrise to <seealso cref = "getTzais90">nightfall</seealso> of 90 minutes after
        ///  sunset. This returns the time of 4 * <seealso cref = "getShaahZmanis90Minutes()" />
        ///  after <seealso cref = "getAlos90">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman tfila. </returns>
        ///<seealso cref = "getShaahZmanis90Minutes()" />
        ///<seealso cref = "getAlos90()" />
        public virtual Date getSofZmanTfilaMGA90Minutes()
        {
            return getTimeOffset(getAlos90(), getShaahZmanis90Minutes()*4);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to the morning
        ///  prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        ///  <seealso cref = "getAlos90Zmanis">90</seealso> minutes <em>zmaniyos</em> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis90MinutesZmanis">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos90Zmanis">dawn</seealso> based on the opinion of the
        ///  <em>MG"A</em> that the day is calculated from a
        ///  <seealso cref = "getAlos90Zmanis">dawn</seealso> of 90 minutes <em>zmaniyos</em> before
        ///  sunrise to <seealso cref = "getTzais90Zmanis">nightfall</seealso> of 90 minutes
        ///  <em>zmaniyos</em> after sunset. This returns the time of 4 *
        ///  <seealso cref = "getShaahZmanis90MinutesZmanis()" /> after <see cref = "getAlos90Zmanis()"> dawn</see>
        ///  .
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis90MinutesZmanis()" />
        ///<seealso cref = "getAlos90Zmanis()" />
        public virtual Date getSofZmanTfilaMGA90MinutesZmanis()
        {
            return getTimeOffset(getAlos90Zmanis(), getShaahZmanis90MinutesZmanis()*4);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos96">96</seealso> minutes before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis96Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ///  after <seealso cref = "getAlos96">dawn</seealso> based on the opinion of the <em>MG"A</em>
        ///  that the day is calculated from a <seealso cref = "getAlos96">dawn</seealso> of 96 minutes
        ///  before sunrise to <seealso cref = "getTzais96">nightfall</seealso> of 96 minutes after
        ///  sunset. This returns the time of 4 * <seealso cref = "getShaahZmanis96Minutes()" />
        ///  after <seealso cref = "getAlos96">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman tfila. </returns>
        ///<seealso cref = "getShaahZmanis96Minutes()" />
        ///<seealso cref = "getAlos96()" />
        public virtual Date getSofZmanTfilaMGA96Minutes()
        {
            return getTimeOffset(getAlos96(), getShaahZmanis96Minutes()*4);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to the morning
        ///  prayers) in the opinion of the <em>MG"A</em> based on <em>alos</em> being
        ///  <seealso cref = "getAlos96Zmanis">96</seealso> minutes <em>zmaniyos</em> before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis96MinutesZmanis">shaos zmaniyos</seealso></em> (solar
        ///  hours) after <seealso cref = "getAlos96Zmanis">dawn</seealso> based on the opinion of the
        ///  <em>MG"A</em> that the day is calculated from a
        ///  <seealso cref = "getAlos96Zmanis">dawn</seealso> of 96 minutes <em>zmaniyos</em> before
        ///  sunrise to <seealso cref = "getTzais96Zmanis">nightfall</seealso> of 96 minutes
        ///  <em>zmaniyos</em> after sunset. This returns the time of 4 *
        ///  <seealso cref = "getShaahZmanis96MinutesZmanis()" /> after <see cref = "getAlos96Zmanis()"> dawn</see>
        ///  .
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis90MinutesZmanis()" />
        ///<seealso cref = "getAlos90Zmanis()" />
        public virtual Date getSofZmanTfilaMGA96MinutesZmanis()
        {
            return getTimeOffset(getAlos96Zmanis(), getShaahZmanis96MinutesZmanis()*4);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) in the opinion of the <em>MG"A</em> based on
        ///  <em>alos</em> being <seealso cref = "getAlos120">120</seealso> minutes before
        ///  <seealso cref = "getSunrise">sunrise</seealso>. This time is 4
        ///  <em><seealso cref = "getShaahZmanis120Minutes">shaos zmaniyos</seealso></em> (solar hours)
        ///  after <seealso cref = "getAlos120">dawn</seealso> based on the opinion of the
        ///  <em>MG"A</em> that the day is calculated from a <see cref = "getAlos120()"> dawn</see>
        ///  of 120 minutes before sunrise to <seealso cref = "getTzais120">nightfall</seealso>
        ///  of 120 minutes after sunset. This returns the time of 4 *
        ///  <seealso cref = "getShaahZmanis120Minutes()" /> after <seealso cref = "getAlos120">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getShaahZmanis120Minutes()" />
        ///<seealso cref = "getAlos120()" />
        public virtual Date getSofZmanTfilaMGA120Minutes()
        {
            return getTimeOffset(getAlos120(), getShaahZmanis120Minutes()*4);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) calculated as 2 hours befor
        ///  <seealso cref =  ZmanimCalendar.getChatzos()"/>. This is based on the opinions that
        ///	calculate <em>sof zman krias shema</em> as
        ///    <seealso cref = "getSofZmanShma3HoursBeforeChatzos()" />. This returns the time of 2
        ///    hours before <seealso cref =  ZmanimCalendar.getChatzos()"/>.
        ///	 
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref =  ZmanimCalendar.getChatzos()"/>
        ///	<seealso cref = "getSofZmanShma3HoursBeforeChatzos()" />
        public virtual Date getSofZmanTfila2HoursBeforeChatzos()
        {
            return getTimeOffset(getChatzos(), -120*MINUTE_MILLIS);
        }

        ///<summary>
        ///  This method returns mincha gedola calculated as 30 minutes after
        ///  <em><seealso cref = "getChatzos">chatzos</seealso></em> and not 1/2 of a
        ///  <em><seealso cref = "getShaahZmanisGra">shaah zmanis</seealso></em> after
        ///  <em><seealso cref = "getChatzos">chatzos</seealso></em> as calculated by
        ///  <seealso cref = "getMinchaGedola" />. Some use this time to delay the start of mincha
        ///  in the winter when 1/2 of a
        ///  <em><seealso cref = "getShaahZmanisGra">shaah zmanis</seealso></em> is less than 30
        ///  minutes. See <seealso cref = "getMinchaGedolaGreaterThan30()" />for a conveniance
        ///  method that returns the later of the 2 calculations. One should not use
        ///  this time to start <em>mincha</em> before the standard
        ///  <em><seealso cref = "getMinchaGedola">mincha gedola</seealso></em>. See <em>Shulchan Aruch
        ///                                                                            Orach Chayim Siman Raish Lamed Gimel seif alef</em> and the
        ///  <em>Shaar Hatziyon seif katan ches</em>.
        ///</summary>
        ///<returns> the <c>Date</c> of 30 mintes after <em>chatzos</em>. </returns>
        ///<seealso cref = "getMinchaGedola()" />
        ///<seealso cref = "getMinchaGedolaGreaterThan30()" />
        public virtual Date getMinchaGedola30Minutes()
        {
            return getTimeOffset(getChatzos(), MINUTE_MILLIS*30);
        }

        ///<summary>
        ///  This method returns the time of <em>mincha gedola</em> according to the
        ///  Magen Avraham with the day starting 72 minutes before sunrise and ending
        ///  72 minutes after sunset. This is the earliest time to pray
        ///  <em>mincha</em>. For more information on this see the documentation on
        ///  <em><seealso cref = "getMinchaGedola">mincha gedola</seealso></em>. This is calculated as
        ///  6.5 <seealso cref = "getTemporalHour">solar hours</seealso> after alos. The calculation
        ///  used is 6.5 * <seealso cref = "getShaahZmanis72Minutes()" /> after
        ///  <seealso cref = "getAlos72">alos</seealso>.
        ///</summary>
        ///<seealso cref = "getAlos72()()" />
        ///<seealso cref = "getMinchaGedola()" />
        ///<seealso cref = "getMinchaKetana()" />
        ///<seealso cref =  ZmanimCalendar.getMinchaGedola()"/>
        ///	<returns> the <c>Date</c> of the time of mincha gedola. </returns>
        public virtual Date getMinchaGedola72Minutes()
        {
            return getTimeOffset(getAlos72(), getShaahZmanis72Minutes()*6.5);
        }

        ///<summary>
        ///  This method returns the time of <em>mincha gedola</em> according to the
        ///  Magen Avraham with the day starting and ending 16.1° below the
        ///  horizon. This is the earliest time to pray <em>mincha</em>. For more
        ///  information on this see the documentation on
        ///  <em><seealso cref = "getMinchaGedola">mincha gedola</seealso></em>. This is calculated as
        ///  6.5 <seealso cref = "getTemporalHour">solar hours</seealso> after alos. The calculation
        ///  used is 6.5 * <seealso cref = "getShaahZmanis16Point1Degrees()" /> after
        ///  <seealso cref = "getAlos16Point1Degrees">alos</seealso>.
        ///</summary>
        ///<seealso cref = "getShaahZmanis16Point1Degrees()" />
        ///<seealso cref = "getMinchaGedola()" />
        ///<seealso cref = "getMinchaKetana()" />
        ///<returns> the <c>Date</c> of the time of mincha gedola. </returns>
        public virtual Date getMinchaGedola16Point1Degrees()
        {
            return getTimeOffset(getAlos16Point1Degrees(), getShaahZmanis16Point1Degrees()*6.5);
        }

        ///<summary>
        ///  This is a conveniance methd that returns the later of
        ///  <seealso cref = "getMinchaGedola()" /> and <seealso cref = "getMinchaGedola30Minutes()" />. In
        ///  the winter when a <em><seealso cref = "getShaahZmanisGra">shaah zmanis</seealso></em> is
        ///  less than 30 minutes <seealso cref = "getMinchaGedola30Minutes()" /> will be
        ///  returned, otherwise <seealso cref = "getMinchaGedola()" /> will be returned.
        ///</summary>
        ///<returns> the <c>Date</c> of the later of <seealso cref = "getMinchaGedola()" />
        ///  and <seealso cref = "getMinchaGedola30Minutes()" /> </returns>
        public virtual Date getMinchaGedolaGreaterThan30()
        {
            if (getMinchaGedola30Minutes() == null || getMinchaGedola() == null)
                return null;

            return getMinchaGedola30Minutes().compareTo(getMinchaGedola()) > 0
                       ? getMinchaGedola30Minutes()
                       : getMinchaGedola();
        }

        ///<summary>
        ///  This method returns the time of <em>mincha ketana</em> according to the
        ///  Magen Avraham with the day starting and ending 16.1° below the
        ///  horizon. This is the perfered earliest time to pray <em>mincha</em> in
        ///  the opinion of the Ramba"m and others. For more information on this see
        ///  the documentation on <em><seealso cref = "getMinchaGedola">mincha gedola</seealso></em>.
        ///  This is calculated as 9.5 <seealso cref = "getTemporalHour">solar hours</seealso> after
        ///  alos. The calculation used is 9.5 *
        ///  <seealso cref = "getShaahZmanis16Point1Degrees()" /> after
        ///  <seealso cref = "getAlos16Point1Degrees">alos</seealso>.
        ///</summary>
        ///<seealso cref = "getShaahZmanis16Point1Degrees()" />
        ///<seealso cref = "getMinchaGedola()" />
        ///<seealso cref = "getMinchaKetana()" />
        ///<returns> the <c>Date</c> of the time of mincha ketana. </returns>
        public virtual Date getMinchaKetana16Point1Degrees()
        {
            return getTimeOffset(getAlos16Point1Degrees(), getShaahZmanis16Point1Degrees()*9.5);
        }

        ///<summary>
        ///  This method returns the time of <em>mincha ketana</em> according to the
        ///  Magen Avraham with the day starting 72 minutes before sunrise and ending
        ///  72 minutes after sunset. This is the perfered earliest time to pray
        ///  <em>mincha</em> in the opinion of the Ramba"m and others. For more
        ///  information on this see the documentation on
        ///  <em><seealso cref = "getMinchaGedola">mincha gedola</seealso></em>. This is calculated as
        ///  9.5 <seealso cref = "getShaahZmanis72Minutes()" /> after alos. The calculation used
        ///  is 9.5 * <seealso cref = "getShaahZmanis72Minutes()" /> after <see cref = "getAlos72()"> alos</see>
        ///  .
        ///</summary>
        ///<seealso cref = "getShaahZmanis16Point1Degrees()" />
        ///<seealso cref = "getMinchaGedola()" />
        ///<seealso cref = "getMinchaKetana()" />
        ///<returns> the <c>Date</c> of the time of mincha ketana. </returns>
        public virtual Date getMinchaKetana72Minutes()
        {
            return getTimeOffset(getAlos72(), getShaahZmanis72Minutes()*9.5);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos60">dawn</seealso>. The formula
        ///  used is:<br />
        ///  10.75 <seealso cref = "getShaahZmanis60Minutes()" /> after <seealso cref = "getAlos60()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha60Minutes()
        {
            return getTimeOffset(getAlos60(), getShaahZmanis60Minutes()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos72">dawn</seealso>. The formula
        ///  used is:<br />
        ///  10.75 <seealso cref = "getShaahZmanis72Minutes()" /> after <seealso cref = "getAlos72()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha72Minutes()
        {
            return getTimeOffset(getAlos72(), getShaahZmanis72Minutes()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos90">dawn</seealso>. The formula
        ///  used is:<br />
        ///  10.75 <seealso cref = "getShaahZmanis90Minutes()" /> after <seealso cref = "getAlos90()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha90Minutes()
        {
            return getTimeOffset(getAlos90(), getShaahZmanis90Minutes()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos96">dawn</seealso>. The formula
        ///  used is:<br />
        ///  10.75 <seealso cref = "getShaahZmanis96Minutes()" /> after <seealso cref = "getAlos96()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha96Minutes()
        {
            return getTimeOffset(getAlos96(), getShaahZmanis96Minutes()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos96Zmanis">dawn</seealso>. The
        ///  formula used is:<br />
        ///  10.75 * <seealso cref = "getShaahZmanis96MinutesZmanis()" /> after
        ///  <seealso cref = "getAlos96Zmanis">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha96MinutesZmanis()
        {
            return getTimeOffset(getAlos96Zmanis(), getShaahZmanis96MinutesZmanis()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos90Zmanis">dawn</seealso>. The
        ///  formula used is:<br />
        ///  10.75 * <seealso cref = "getShaahZmanis90MinutesZmanis()" /> after
        ///  <seealso cref = "getAlos90Zmanis">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha90MinutesZmanis()
        {
            return getTimeOffset(getAlos90Zmanis(), getShaahZmanis90MinutesZmanis()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em>. This is
        ///  calculated as 10.75 hours after <seealso cref = "getAlos72Zmanis">dawn</seealso>. The
        ///  formula used is:<br />
        ///  10.75 * <seealso cref = "getShaahZmanis72MinutesZmanis()" /> after
        ///  <seealso cref = "getAlos72Zmanis">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha72MinutesZmanis()
        {
            return getTimeOffset(getAlos72Zmanis(), getShaahZmanis72MinutesZmanis()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em> based on the
        ///  opinion that the day starts at
        ///  <em><seealso cref = "getAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        ///  <em><seealso cref = "getTzais16Point1Degrees">tzais 16.1°</seealso></em>. This is
        ///  calculated as 10.75 hours <em>zmaniyos</em> after
        ///  <seealso cref = "getAlos16Point1Degrees">dawn</seealso>. The formula is<br />
        ///  10.75 * <seealso cref = "getShaahZmanis16Point1Degrees()" /> after
        ///  <seealso cref = "getAlos16Point1Degrees()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha16Point1Degrees()
        {
            return getTimeOffset(getAlos16Point1Degrees(), getShaahZmanis16Point1Degrees()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em> based on the
        ///  opinion that the day starts at
        ///  <em><seealso cref = "getAlos19Point8Degrees">alos 19.8°</seealso></em> and ends at
        ///  <em><seealso cref = "getTzais19Point8Degrees">tzais 19.8°</seealso></em>. This is
        ///  calculated as 10.75 hours <em>zmaniyos</em> after
        ///  <seealso cref = "getAlos19Point8Degrees">dawn</seealso>. The formula is<br />
        ///  10.75 * <seealso cref = "getShaahZmanis19Point8Degrees()" /> after
        ///  <seealso cref = "getAlos19Point8Degrees()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha19Point8Degrees()
        {
            return getTimeOffset(getAlos19Point8Degrees(), getShaahZmanis19Point8Degrees()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em> based on the
        ///  opinion that the day starts at
        ///  <em><seealso cref = "getAlos26Degrees">alos 26°</seealso></em> and ends at
        ///  <em><seealso cref = "getTzais26Degrees">tzais 26°</seealso></em>. This is calculated
        ///  as 10.75 hours <em>zmaniyos</em> after <seealso cref = "getAlos26Degrees">dawn</seealso>.
        ///  The formula is<br />
        ///  10.75 * <seealso cref = "getShaahZmanis26Degrees()" /> after
        ///  <seealso cref = "getAlos26Degrees()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha26Degrees()
        {
            return getTimeOffset(getAlos26Degrees(), getShaahZmanis26Degrees()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em> based on the
        ///  opinion that the day starts at
        ///  <em><seealso cref = "getAlos18Degrees">alos 18°</seealso></em> and ends at
        ///  <em><seealso cref = "getTzais18Degrees">tzais 18°</seealso></em>. This is calculated
        ///  as 10.75 hours <em>zmaniyos</em> after <seealso cref = "getAlos18Degrees">dawn</seealso>.
        ///  The formula is<br />
        ///  10.75 * <seealso cref = "getShaahZmanis18Degrees()" /> after
        ///  <seealso cref = "getAlos18Degrees()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha18Degrees()
        {
            return getTimeOffset(getAlos18Degrees(), getShaahZmanis18Degrees()*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em> based on the
        ///  opinion that the day starts at
        ///  <em><seealso cref = "getAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        ///  <seealso cref = "getSunset">sunset</seealso>. 10.75 shaos zmaniyos are calculated based on
        ///  this day and added to <seealso cref = "getAlos16Point1Degrees">alos</seealso> to reach
        ///  this time. This time is 10.75 <em>shaos zmaniyos</em> (temporal hours)
        ///  after <seealso cref = "getAlos16Point1Degrees">dawn</seealso> based on the opinion that
        ///  the day is calculated from a <seealso cref = "getAlos16Point1Degrees">dawn</seealso> of
        ///  16.1 degrees before sunrise to <see cref = "getSeaLevelSunset()">sea level sunset</see>
        ///  . This returns the time of 10.75 * the calculated
        ///  <em>shaah zmanis</em> after <seealso cref = "getAlos16Point1Degrees">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the plag. </returns>
        ///<seealso cref = "getAlos16Point1Degrees()" />
        ///<seealso cref = "getSeaLevelSunset()" />
        public virtual Date getPlagAlosToSunset()
        {
            long shaahZmanis = getTemporalHour(getAlos16Point1Degrees(), getSeaLevelSunset());
            return getTimeOffset(getAlos16Point1Degrees(), shaahZmanis*10.75);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em> based on the
        ///  opinion that the day starts at
        ///  <em><seealso cref = "getAlos16Point1Degrees">alos 16.1°</seealso></em> and ends at
        ///  <seealso cref = "getTzaisGeonim7Point083Degrees">tzais</seealso>. 10.75 shaos zmaniyos are
        ///  calculated based on this day and added to
        ///  <seealso cref = "getAlos16Point1Degrees">alos</seealso> to reach this time. This time is
        ///  10.75 <em>shaos zmaniyos</em> (temporal hours) after
        ///  <seealso cref = "getAlos16Point1Degrees">dawn</seealso> based on the opinion that the day
        ///  is calculated from a <seealso cref = "getAlos16Point1Degrees">dawn</seealso> of 16.1
        ///  degrees before sunrise to <seealso cref = "getTzaisGeonim7Point083Degrees">tzais</seealso>
        ///  . This returns the time of 10.75 * the calculated <em>shaah zmanis</em>
        ///  after <seealso cref = "getAlos16Point1Degrees">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the plag. </returns>
        ///<seealso cref = "getAlos16Point1Degrees()" />
        ///<seealso cref = "getTzaisGeonim7Point083Degrees()" />
        public virtual Date getPlagAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            long shaahZmanis = getTemporalHour(getAlos16Point1Degrees(), getTzaisGeonim7Point083Degrees());
            return getTimeOffset(getAlos16Point1Degrees(), shaahZmanis*10.75);
        }

        ///<summary>
        ///  This method returns Bain Hashmashos of Rabainu Tam calculated as the time
        ///  the sun is 13° below <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso>
        ///  (90°). <br />
        ///  <br />
        ///  <b>FIXME:</b> As per Yisroel Vehazmanim Vol III page 1028 No 50, the
        ///  13° is slightly inaccurate. He lists it as a drop less than 13°.
        ///  Calculations show that is seems to be 13.2477° below the horizon at
        ///  that time. This makes a difference of 1 minute and 10 seconds in
        ///  Jerusalem in the Equinox, and 1 minute 29 seconds in the solstice. for NY
        ///  in the solstice, the difference is 1 minute 56 seconds.
        ///</summary>
        ///<returns> the <c>Date</c> of the sun being 13° below
        ///  <seealso cref = "GEOMETRIC_ZENIT">geometric zenith</seealso> (90°). </returns>
        ///<seealso cref =  #ZENITH_13_DEGREES
        ///</seealso>
        public virtual Date getBainHasmashosRT13Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_13_DEGREES);
        }

        ///<summary>
        ///  This method returns Bain Hashmashos of Rabainu Tam calculated as a 58.5
        ///  minute offset after sunset. Bain hashmashos is 3/4 of a mil before tzais
        ///  or 3 1/4 mil after sunset. With a mil calculated as 18 minutes, 3.25 * 18
        ///  = 58.5 minutes.
        ///</summary>
        ///<returns> the <c>Date</c> of 58.5 minutes after sunset
        ///</returns>
        public virtual Date getBainHasmashosRT58Point5Minutes()
        {
            return getTimeOffset(getSeaLevelSunset(), 58.5*MINUTE_MILLIS);
        }

        ///<summary>
        ///  This method returns the time of <em>bain hashmashos</em> based on the
        ///  calculation of 13.5 minutes (3/4 of an 18 minute mil before shkiah
        ///  calculated as <seealso cref = "getTzaisGeonim7Point083Degrees">7.083°</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the bain hashmashos of Rabainu Tam in
        ///  this calculation. </returns>
        ///<seealso cref = "getTzaisGeonim7Point083Degrees()" />
        public virtual Date getBainHasmashosRT13Point5MinutesBefore7Point083Degrees()
        {
            return getTimeOffset(getSunsetOffsetByDegrees(ZENITH_7_POINT_083), -13.5*MINUTE_MILLIS);
        }

        ///<summary>
        ///  This method returns <em>bain hashmashos</em> of Rabainu Tam calculated in
        ///  the opinion of the Divray Yosef (see Yisrael Vehazmanim) calculated
        ///  5/18th (27.77%) of the time between alos (calculated as 19.8° before
        ///  sunrise) and sunrise. This is added to sunset to arrive at the time for
        ///  bain hashmashos of Rabainu Tam).
        ///</summary>
        ///<returns> the <c>Date</c> of bain hashmashos of Rabainu Tam for this
        ///  calculation. </returns>
        public virtual Date getBainHasmashosRT2Stars()
        {
            Date alos19Point8 = getAlos19Point8Degrees();
            Date sunrise = getSeaLevelSunrise();
            if (alos19Point8 == null || sunrise == null)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunset(), (sunrise.getTime() - alos19Point8.getTime())*(5/18d));
        }

        // public Date getTzaisGeonim3Point7Degrees() {
        // return getSunsetOffsetByDegrees(ZENITH_3_POINT_7);
        // }
        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated at the sun's position at
        ///  <seealso cref = "ZENITH_5_POINT_95">5.95°</seealso> below the western horizon.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time when the sun is
        ///  5.95° below sea level. </returns>
        ///<seealso cref = "ZENITH_5_POINT_95" />
        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated at the sun's position at
        ///  <seealso cref = "ZENITH_5_POINT_95">5.95°</seealso> below the western horizon.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time when the sun is
        ///  5.95° below sea level. </returns>
        ///<seealso cref = "ZENITH_5_POINT_95" />
        public virtual Date getTzaisGeonim5Point95Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_5_POINT_95);
        }

        /// <summary>
        ///   This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///   of the <em>Geonim</em> calculated calculated as 3/4 of a <a href = "http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a> based on an 18 minute Mil, or 13.5 minutes. It is the sun's
        ///   position at <seealso cref = "ZENITH_3_POINT_65">3.65°</seealso> below the western
        ///   horizon. This is a very early zman and should not be relied on without
        ///   Rabbinical guidance.
        /// </summary>
        /// <returns> the <c>Date</c> representing the time when the sun is
        ///   3.65° below sea level. </returns>
        /// <seealso cref = "ZENITH_3_POINT_65" />
        public virtual Date getTzaisGeonim3Point65Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_3_POINT_65);
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated as 3/4 of a <a href = "http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a> based on a 24 minute Mil, or 18 minutes. It is the sun's
        ///  position at <seealso cref = "ZENITH_4_POINT_61">4.61°</seealso> below the western
        ///  horizon. This is a very early zman and should not be relied on without
        ///  Rabbinical guidance.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time when the sun is
        ///  4.61° below sea level. </returns>
        ///<seealso cref = "ZENITH_4_POINT_61" />
        public virtual Date getTzaisGeonim4Point61Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_4_POINT_61);
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated as 3/4 of a <a href = "http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a>, based on a 22.5 minute Mil, or 16 7/8 minutes. It is the sun's
        ///  position at <seealso cref = "ZENITH_4_POINT_37">4.37°</seealso> below the western
        ///  horizon. This is a very early zman and should not be relied on without
        ///  Rabbinical guidance.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time when the sun is
        ///  4.37° below sea level. </returns>
        ///<seealso cref = "ZENITH_4_POINT_37" />
        public virtual Date getTzaisGeonim4Point37Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_4_POINT_37);
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated as 3/4 of a <a href = "http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">Mil</a>. It is based on the Baal Hatanya based on a Mil being 24
        ///  minutes, and is calculated as 18 +2 + 4 for a total of 24 minutes (FIXME:
        ///  additional details needed). It is the sun's position at
        ///  <seealso cref = "ZENITH_5_POINT_88">5.88°</seealso> below the western horizon. This is a
        ///  very early zman and should not be relied on without Rabbinical guidance.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time when the sun is
        ///  5.88° below sea level. </returns>
        ///<seealso cref = "ZENITH_5_POINT_88" />
        public virtual Date getTzaisGeonim5Point88Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_5_POINT_88);
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated as 3/4 of a <a href = "http://en.wikipedia.org/wiki/Biblical_and_Talmudic_units_of_measurement">
        ///                                                  Mil</a>. It is the sun's position at <see cref = "ZENITH_4_POINT_8">4.8°</see>
        ///  below the western horizon based on Rabbi Leo Levi's calculations. (FIXME:
        ///  additional documentation needed) This is the This is a very early zman
        ///  and should not be relied on without Rabbinical guidance.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time when the sun is
        ///  4.8° below sea level. </returns>
        ///<seealso cref = "ZENITH_4_POINT_8" />
        public virtual Date getTzaisGeonim4Point8Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_4_POINT_8);
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated at the sun's position at
        ///  <seealso cref = "ZENITH_7_POINT_083">7.083°</seealso> below the western horizon.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time when the sun is
        ///  7.083° below sea level. </returns>
        ///<seealso cref = "ZENITH_7_POINT_083" />
        public virtual Date getTzaisGeonim7Point083Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_7_POINT_083);
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the <em>Geonim</em> calculated at the sun's position at
        ///  <seealso cref = "ZENITH_8_POINT_5">8.5°</seealso> below the western horizon.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time when the sun is
        ///  8.5° below sea level. </returns>
        ///<seealso cref = "ZENITH_8_POINT_5" />
        public virtual Date getTzaisGeonim8Point5Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_8_POINT_5);
        }

        ///<summary>
        ///  This method returns the <em>tzais</em> (nightfall) based on the opinion
        ///  of the Chavas Yair and Divray Malkiel that the time to walk the distance
        ///  of a Mil is 15 minutes for a total of 60 minutes for 4 mil after
        ///  <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> representing 60 minutes after sea level
        ///  sunset. </returns>
        ///<seealso cref = "getAlos60()" />
        public virtual Date getTzais60()
        {
            return getTimeOffset(getSeaLevelSunset(), 60*MINUTE_MILLIS);
        }

        ///<summary>
        ///  This method returns tzais usually calculated as 40 minutes after sunset.
        ///  Please note that Chacham Yosef Harari-Raful of Yeshivat Ateret Torah who
        ///  uses this time, does so only for calculating various other zmanai hayom
        ///  such as Sof Zman Krias Shema and Plag Hamincha. His calendars do not
        ///  publish a zman for Tzais. It should also be noted that Chacham
        ///  Harari-Raful provided a 25 minute zman for Israel. This API uses 40
        ///  minutes year round in any place on the globe by default. This offset can
        ///  be changed by calling <seealso cref = "setAteretTorahSunsetOffset(double)" />.
        ///</summary>
        ///<returns> the <c>Date</c> representing 40 minutes after sea level
        ///  sunset </returns>
        ///<seealso cref = "getAteretTorahSunsetOffset()" />
        ///<seealso cref = "setAteretTorahSunsetOffset(double)" />
        public virtual Date getTzaisAteretTorah()
        {
            return getTimeOffset(getSeaLevelSunset(), getAteretTorahSunsetOffset()*MINUTE_MILLIS);
        }

        ///<summary>
        ///  Returns the offset in minutes after sunset used to calculate
        ///  <em>tzais</em> for the Ateret Torah zmanim. The defaullt value is 40
        ///  minutes.
        ///</summary>
        ///<returns> the number of minutes after sunset for Tzais. </returns>
        ///<seealso cref = "setAteretTorahSunsetOffset(double)" />
        public virtual double getAteretTorahSunsetOffset()
        {
            return ateretTorahSunsetOffset;
        }

        ///<summary>
        ///  Allows setting the offset in minutes after sunset for the Ateret Torah
        ///  zmanim. The default if unset is 40 minutes. Chacham Yosef Harari-Raful of
        ///  Yeshivat Ateret Torah uses 40 minutes globally with the exception of
        ///  Israel where a 25 minute offset is used. This 25 minute (or any other)
        ///  offset can be overridden by this methd. This offset impacts all Ateret
        ///  Torah methods.
        ///</summary>
        ///<param name = "ateretTorahSunsetOffset">
        ///  the number of minutes after sunset to use as an offset for the
        ///  Ateret Torah <em>tzais</em> </param>
        ///<seealso cref = "getAteretTorahSunsetOffset()" />
        public virtual void setAteretTorahSunsetOffset(double ateretTorahSunsetOffset)
        {
            this.ateretTorahSunsetOffset = ateretTorahSunsetOffset;
        }

        ///<summary>
        ///  This method returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) based on the calculation of Chacham Yosef
        ///  Harari-Raful of Yeshivat Ateret Torah, that the day starts
        ///  <seealso cref = "getAlos72Zmanis">1/10th of the day</seealso> before sunrise and is
        ///  usually calculated as ending <see cref = "getTzaisAteretTorah()">40 minutes after sunset</see>
        ///  . <em>shaos zmaniyos</em> are calculated based on this day
        ///  and added to <seealso cref = "getAlos72Zmanis">alos</seealso> to reach this time. This
        ///  time is 3 <em> <seealso cref = "getShaahZmanisAteretTorah">shaos zmaniyos</seealso></em>
        ///  (temporal hours) after <seealso cref = "getAlos72Zmanis">alos 72 zmaniyos</seealso>.<br />
        ///  <b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        ///  midday.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema based on this
        ///  calculation. </returns>
        ///<seealso cref = "getAlos72Zmanis()" />
        ///<seealso cref = "getTzaisAteretTorah()" />
        ///<seealso cref = "getAteretTorahSunsetOffset()" />
        ///<seealso cref = "setAteretTorahSunsetOffset(double)" />
        ///<seealso cref = "getShaahZmanisAteretTorah()" />
        public virtual Date getSofZmanShmaAteretTorah()
        {
            return getTimeOffset(getAlos72Zmanis(), getShaahZmanisAteretTorah()*3);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) based on the calculation of Chacham Yosef Harari-Raful
        ///  of Yeshivat Ateret Torah, that the day starts <see cref = "getAlos72Zmanis()"> 1/10th of the day</see>
        ///  before sunrise and and is usually calculated as ending
        ///  <seealso cref = "getTzaisAteretTorah">40 minutes after sunset</seealso>.
        ///  <em>shaos zmaniyos</em> are calculated based on this day and added to
        ///  <seealso cref = "getAlos72Zmanis">alos</seealso> to reach this time. This time is 4
        ///  <em><seealso cref = "getShaahZmanisAteretTorah">shaos zmaniyos</seealso></em> (temporal
        ///  hours) after <seealso cref = "getAlos72Zmanis">alos 72 zmaniyos</seealso>.<br />
        ///  <b>Note: </b> Based on this calculation <em>chatzos</em> will not be at
        ///  midday.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema based on this
        ///  calculation. </returns>
        ///<seealso cref = "getAlos72Zmanis()" />
        ///<seealso cref = "getTzaisAteretTorah()" />
        ///<seealso cref = "getShaahZmanisAteretTorah()" />
        ///<seealso cref = "setAteretTorahSunsetOffset(double)" />
        public virtual Date getSofZmanTfilahAteretTorah()
        {
            return getTimeOffset(getAlos72Zmanis(), getShaahZmanisAteretTorah()*4);
        }

        ///<summary>
        ///  This method returns the time of <em>mincha gedola</em> based on the
        ///  calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        ///  the day starts <seealso cref = "getAlos72Zmanis">1/10th of the day</seealso> before
        ///  sunrise and and is usually calculated as ending
        ///  <seealso cref = "getTzaisAteretTorah">40 minutes after sunset</seealso>. This is the
        ///  perfered earliest time to pray <em>mincha</em> in the opinion of the
        ///  Ramba"m and others. For more information on this see the documentation on
        ///  <em><seealso cref = "getMinchaGedola">mincha gedola</seealso></em>. This is calculated as
        ///  6.5 <seealso cref = "getShaahZmanisAteretTorah">solar hours</seealso> after alos. The
        ///  calculation used is 6.5 * <seealso cref = "getShaahZmanisAteretTorah()" /> after
        ///  <seealso cref = "getAlos72Zmanis">alos</seealso>.
        ///</summary>
        ///<seealso cref = "getAlos72Zmanis()" />
        ///<seealso cref = "getTzaisAteretTorah()" />
        ///<seealso cref = "getShaahZmanisAteretTorah()" />
        ///<seealso cref = "getMinchaGedola()" />
        ///<seealso cref = "getMinchaKetanaAteretTorah()" />
        ///<seealso cref =  ZmanimCalendar.getMinchaGedola()"/>
        ///	<returns> the <c>Date</c> of the time of mincha gedola. </returns>
        public virtual Date getMinchaGedolaAteretTorah()
        {
            return getTimeOffset(getAlos72Zmanis(), getShaahZmanisAteretTorah()*6.5);
        }

        ///<summary>
        ///  This method returns the time of <em>mincha ketana</em> based on the
        ///  calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        ///  the day starts <seealso cref = "getAlos72Zmanis">1/10th of the day</seealso> before
        ///  sunrise and and is usually calculated as ending
        ///  <seealso cref = "getTzaisAteretTorah">40 minutes after sunset</seealso>. This is the
        ///  perfered earliest time to pray <em>mincha</em> in the opinion of the
        ///  Ramba"m and others. For more information on this see the documentation on
        ///  <em><seealso cref = "getMinchaGedola">mincha gedola</seealso></em>. This is calculated as
        ///  9.5 <seealso cref = "getShaahZmanisAteretTorah">solar hours</seealso> after
        ///  <seealso cref = "getAlos72Zmanis">alos</seealso>. The calculation used is 9.5 *
        ///  <seealso cref = "getShaahZmanisAteretTorah()" /> after <see cref = "getAlos72Zmanis()"> alos</see>
        ///  .
        ///</summary>
        ///<seealso cref = "getAlos72Zmanis()" />
        ///<seealso cref = "getTzaisAteretTorah()" />
        ///<seealso cref = "getShaahZmanisAteretTorah()" />
        ///<seealso cref = "getMinchaGedola()" />
        ///<seealso cref = "getMinchaKetana()" />
        ///<returns> the <c>Date</c> of the time of mincha ketana. </returns>
        public virtual Date getMinchaKetanaAteretTorah()
        {
            return getTimeOffset(getAlos72Zmanis(), getShaahZmanisAteretTorah()*9.5);
        }

        ///<summary>
        ///  This method returns the time of <em>plag hamincha</em> based on the
        ///  calculation of Chacham Yosef Harari-Raful of Yeshivat Ateret Torah, that
        ///  the day starts <seealso cref = "getAlos72Zmanis">1/10th of the day</seealso> before
        ///  sunrise and and is usually calculated as ending
        ///  <seealso cref = "getTzaisAteretTorah">40 minutes after sunset</seealso>.
        ///  <em>shaos zmaniyos</em> are calculated based on this day and added to
        ///  <seealso cref = "getAlos72Zmanis">alos</seealso> to reach this time. This time is 10.75
        ///  <em><seealso cref = "getShaahZmanisAteretTorah">shaos zmaniyos</seealso></em> (temporal
        ///  hours) after <seealso cref = "getAlos72Zmanis">dawn</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> of the plag. </returns>
        ///<seealso cref = "getAlos72Zmanis()" />
        ///<seealso cref = "getTzaisAteretTorah()" />
        ///<seealso cref = "getShaahZmanisAteretTorah()" />
        public virtual Date getPlagHaminchaAteretTorah()
        {
            return getTimeOffset(getAlos72Zmanis(), getShaahZmanisAteretTorah()*10.75);
        }

        /*
        ///	 <summary> 
        ///	This method returns the time of <em>misheyakir</em> based on the common
        ///	calculation of the Syrian community in NY that the alos is a fixed minute
        ///	offset from day starting <seealso cref="getAlos72Zmanis">1/10th of the day</seealso>
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
        ///	           <seealso cref="getAlos72Zmanis">1/10th of the day</seealso> </param>
        ///	<returns> the <c>Date</c> of misheyakir </returns>
        ///	<seealso cref="getAlos72Zmanis()"/>
        ///	 
        // public Date getMesheyakirAteretTorah(double minutes) {
        // return getTimeOffset(getAlos72Zmanis(), minutes * MINUTE_MILLIS);
        // }
        */

        ///<summary>
        ///  Method to return <em>tzais</em> (dusk) calculated as 72 minutes zmaniyos,
        ///  or 1/10th of the day after <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getAlos72Zmanis()" />
        public virtual Date getTzais72Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunset(), shaahZmanis*1.2);
        }

        ///<summary>
        ///  Method to return <em>tzais</em> (dusk) calculated using 90 minutes
        ///  zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        ///  <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getAlos90Zmanis()" />
        public virtual Date getTzais90Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunset(), shaahZmanis*1.5);
        }

        ///<summary>
        ///  Method to return <em>tzais</em> (dusk) calculated using 96 minutes
        ///  zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        ///  <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getAlos96Zmanis()" />
        public virtual Date getTzais96Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunset(), shaahZmanis*1.6);
        }

        ///<summary>
        ///  Method to return <em>tzais</em> (dusk) calculated as 90 minutes after sea
        ///  level sunset. This method returns <em>tzais</em> (nightfall) based on the
        ///  opinion of the Magen Avraham that the time to walk the distance of a Mil
        ///  in the Ramba"m's opinion is 18 minutes for a total of 90 minutes based on
        ///  the opinion of <em>Ula</em> who calculated <em>tzais</em> as 5 Mil after
        ///  sea level shkiah (sunset). A similar calculation
        ///  <seealso cref = "getTzais19Point8Degrees()" />uses solar position calculations based
        ///  on this time.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getTzais19Point8Degrees()" />
        ///<seealso cref = "getAlos90()" />
        public virtual Date getTzais90()
        {
            return getTimeOffset(getSeaLevelSunset(), 90*MINUTE_MILLIS);
        }

        ///<summary>
        ///  This method returns <em>tzais</em> (nightfall) based on the opinion of
        ///  the Magen Avraham that the time to walk the distance of a Mil in the
        ///  Ramba"ms opinion is 2/5 of an hour (24 minutes) for a total of 120
        ///  minutes based on the opinion of <em>Ula</em> who calculated
        ///  <em>tzais</em> as 5 Mil after sea level shkiah (sunset). A similar
        ///  calculation <seealso cref = "getTzais26Degrees()" /> uses temporal calculations based
        ///  on this time.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getTzais26Degrees()" />
        ///<seealso cref = "getAlos120()" />
        public virtual Date getTzais120()
        {
            return getTimeOffset(getSeaLevelSunset(), 120*MINUTE_MILLIS);
        }

        ///<summary>
        ///  Method to return <em>tzais</em> (dusk) calculated using 120 minutes
        ///  zmaniyos (<em>GR"A</em> and the <em>Baal Hatanya</em>) after
        ///  <seealso cref = "getSeaLevelSunset">sea level sunset</seealso>.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getAlos120Zmanis()" />
        public virtual Date getTzais120Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunset(), shaahZmanis*2.0);
        }

        ///<summary>
        ///  For information on how this is calculated see the comments on
        ///  <seealso cref = "getAlos16Point1Degrees()" />
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getTzais72()" />
        ///<seealso cref = "getAlos16Point1Degrees">for more information on this calculation.</seealso>
        public virtual Date getTzais16Point1Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_16_POINT_1);
        }

        ///<summary>
        ///  For information on how this is calculated see the comments on
        ///  <seealso cref = "getAlos26Degrees()" />
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getTzais120()" />
        ///<seealso cref = "getAlos26Degrees()" />
        public virtual Date getTzais26Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_26_DEGREES);
        }

        ///<summary>
        ///  For information on how this is calculated see the comments on
        ///  <seealso cref = "getAlos18Degrees()" />
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getAlos18Degrees()" />
        public virtual Date getTzais18Degrees()
        {
            return getSunsetOffsetByDegrees(ASTRONOMICAL_ZENITH);
        }

        ///<summary>
        ///  For information on how this is calculated see the comments on
        ///  <seealso cref = "getAlos19Point8Degrees()" />
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getTzais90()" />
        ///<seealso cref = "getAlos19Point8Degrees()" />
        public virtual Date getTzais19Point8Degrees()
        {
            return getSunsetOffsetByDegrees(ZENITH_19_POINT_8);
        }

        ///<summary>
        ///  A method to return <em>tzais</em> (dusk) calculated as 96 minutes after
        ///  sea level sunset. For information on how this is calculated see the
        ///  comments on <seealso cref = "getAlos96()" />.
        ///</summary>
        ///<returns> the <c>Date</c> representing the time. </returns>
        ///<seealso cref = "getAlos96()" />
        public virtual Date getTzais96()
        {
            return getTimeOffset(getSeaLevelSunset(), 96*MINUTE_MILLIS);
        }

        ///<summary>
        ///  A method that returns the local time for fixed <em>chatzos</em>. This
        ///  time is noon and midnight adjusted from standard time to account for the
        ///  local latitude. The 360° of the globe divided by 24 calculates to
        ///  15° per hour with 4 minutes per degree, so at a longitude of 0 , 15,
        ///  30 etc Chatzos in 12:00 noon. Lakewood, NJ whose longitude is -74.2094 is
        ///  0.7906 away from the closest multiple of 15 at -75°. This is
        ///  multiplied by 4 to yeild 3 minutes and 10 seconds for a chatzos of
        ///  11:56:50. This method is not tied to the theoretical 15° timezones,
        ///  but will adjust to the actual timezone and <a href = "http://en.wikipedia.org/wiki/Daylight_saving_time">Daylight saving
        ///                                               time</a>.
        ///</summary>
        ///<returns> the Date representing the local <em>chatzos</em> </returns>
        ///<seealso cref =  GeoLocation.getLocalMeanTimeOffset()"/>
        public virtual Date getFixedLocalChatzos()
        {
            return getTimeOffset(getDateFromTime(12.0 - getGeoLocation().getTimeZone().getRawOffset()/HOUR_MILLIS),
                                 -getGeoLocation().getLocalMeanTimeOffset());
        }

        ///<summary>
        ///  A method that returns the latest <em>zman krias shema</em> (time to say
        ///  Shema in the morning) calculated as 3 hours before
        ///  <seealso cref = "getFixedLocalChatzos()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman shema. </returns>
        ///<seealso cref = "getFixedLocalChatzos()" />
        ///<seealso cref = "getSofZmanTfilaFixedLocal()" />
        public virtual Date getSofZmanShmaFixedLocal()
        {
            return getTimeOffset(getFixedLocalChatzos(), -180*MINUTE_MILLIS);
        }

        ///<summary>
        ///  This method returns the latest <em>zman tfila</em> (time to say the
        ///  morning prayers) calculated as 2 hours before
        ///  <seealso cref = "getFixedLocalChatzos()" />.
        ///</summary>
        ///<returns> the <c>Date</c> of the latest zman tfila. </returns>
        ///<seealso cref = "getFixedLocalChatzos()" />
        ///<seealso cref = "getSofZmanShmaFixedLocal()" />
        public virtual Date getSofZmanTfilaFixedLocal()
        {
            return getTimeOffset(getFixedLocalChatzos(), -120*MINUTE_MILLIS);
        }

        public override int GetHashCode()
        {
            int num = 0x11;
            num = (0x25*num) + Object.instancehelper_hashCode(base.getTemporalHour());
            num += (0x25*num) + getCalendar().hashCode();
            num += (0x25*num) + getGeoLocation().GetHashCode();
            return (num + ((0x25*num) + Object.instancehelper_hashCode(getAstronomicalCalculator())));
        }
    }
}