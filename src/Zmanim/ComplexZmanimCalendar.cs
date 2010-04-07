namespace net.sourceforge.zmanim
{
    using java.util;
    using net.sourceforge.zmanim.util;

    /// <summary>
    /// This class extends ZmanimCalendar and provides many more zmanim than
    /// available in the ZmanimCalendar. The basis for most zmanim in this class are
    /// from the <em>sefer</em> <b>Yisroel Vehazmanim</b> by <b>Rabbi Yisroel Dovid
    /// Harfenes</b>. <br />
    /// For an example of the number of different <em>zmanim</em> made available by
    /// this class, there are methods to return 12 different calculations for
    /// <em>alos</em> (dawn) available in this class. The real power of this API is
    /// the ease in calculating <em>zmanim</em> that are not part of the API. The
    /// methods for doing <em>zmanim</em> calculations not present in this or it's
    /// superclass the <seealso cref="ZmanimCalendar"/> are contained in the
    /// <seealso cref="AstronomicalCalendar"/>, the base class of the calendars in our API
    /// since they are generic methods for calculating time based on degrees or time
    /// before or after <seealso cref="#getSunrise sunrise"/> and <seealso cref="#getSunset sunset"/> and
    /// are of interest for calculation beyond <em>zmanim</em> calculations. Here are
    /// some examples: <br />
    /// First create the Calendar for the location you would like to calculate:
    /// 
    /// <pre>
    /// String locationName = &quot;Lakewood, NJ&quot;
    /// double latitude = 40.0828; //Lakewood, NJ
    /// double longitude = -74.2094; //Lakewood, NJ
    /// double elevation = 0;
    /// //the String parameter in getTimeZone() has to be a valid timezone listed in <seealso cref="java.util.TimeZone#getAvailableIDs()"/>
    /// TimeZone timeZone = TimeZone.getTimeZone(&quot;America/New_York&quot;);
    /// GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    /// 		elevation, timeZone);
    /// ComplexZmanimCalendar czc = new ComplexZmanimCalendar(location);
    /// </pre>
    /// 
    /// Note: For locations such as Israel where the beginning and end of daylight
    /// savings time can fluctuate from year to year create a
    /// <seealso cref="java.util.SimpleTimeZone"/> with the known start and end of DST. <br />
    /// To get alos calculated as 14&deg; below the horizon (as calculated in the
    /// calendars published in Montreal) use:
    /// 
    /// <pre>
    /// Date alos14 = czc.getSunriseOffsetByDegrees(14);
    /// </pre>
    /// 
    /// To get <em>mincha gedola</em> calculated based on the MGA using a <em>shaah
    /// zmanis</em> based on the day starting 16.1&deg; below the horizon (and ending
    /// 16.1&deg; after sunset the following calculation can be used:
    /// 
    /// <pre>
    /// Date minchaGedola = czc.getTimeOffset(czc.getAlos16point1Degrees(), czc
    /// 		.getShaahZmanis16Point1Degrees() * 6.5);
    /// </pre>
    /// 
    /// A little more complex example would be calculating <em>plag hamincha</em>
    /// based on a shaah zmanis that was not present in this class. While a drop more
    /// complex it is still rather easy. For example if you wanted to calculate
    /// <em>plag</em> based on the day starting 12&deg; before sunrise and ending
    /// 12&deg; after sunset as calculated in the calendars in Manchester, England
    /// (there is nothing that would prevent your calculating the day using sunrise
    /// and sunset offsets that are not identical degrees, but this would lead to
    /// chatzos being a time other than the <seealso cref="#getSunTransit() solar transit"/>
    /// (solar midday)). The steps involved would be to first calculate the
    /// <em>shaah zmanis</em> and than use that time in milliseconds to calculate
    /// 10.75 hours after sunrise starting at 12&deg; before sunset
    /// 
    /// <pre>
    /// long shaahZmanis = czc.getTemporalHour(czc.getSunriseOffsetByDegrees(12), czc
    /// 		.getSunsetOffsetByDegrees(12));
    /// Date plag = getTimeOffset(czc.getSunriseOffsetByDegrees(12),
    /// 		shaahZmanis * 10.75);
    /// </pre>
    /// 
    /// <h2>Disclaimer:</h2> While I did my best to get accurate results please do
    /// not rely on these zmanim for <em>halacha lemaaseh</em>
    /// </summary>
    /// <remarks>
    /// @author &copy; Eliyahu Hershfeld 2004 - 2010
    /// @version 1.2
    /// </remarks>
    public class ComplexZmanimCalendar : ZmanimCalendar
    {
        private const long serialVersionUID = 1;
 
        ///	 <summary>
        /// The zenith of 3.7&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This calculation is used for calculating <em>tzais</em>
        ///	(nightfall) according to some opinions. This calculation is based on the
        ///	opinion of the Geonim that <em>tzais</em> is the time it takes to walk
        ///	3/4 of a Mil at 18 minutes a Mil, or 13.5 minutes after sunset. The sun
        ///	is 3.7&deg below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/> at this time
        ///	in Jerusalem on March 16, about 4 days before the equinox, the day that a
        ///	solar hour is one hour.
        ///	
        ///	TODO: AT see #getTzaisGeonim3Point7Degrees() </summary>
        protected internal const double ZENITH_3_POINT_7 = GEOMETRIC_ZENITH + 3.7;

        ///	 <summary>
        /// The zenith of 5.95&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This calculation is used for calculating <em>tzais</em>
        ///	(nightfall) according to some opinions. This calculation is based on the
        ///	position of the sun 24 minutes after sunset in Jerusalem on March 16,
        ///	about 4 days before the equinox, the day that a solar hour is one hour,
        ///	which calculates to 5.95&deg; below {@link #GEOMETRIC_ZENITH geometric
        ///	zenith}
        ///	 </summary>
        ///	<seealso cref="#getTzaisGeonim5Point95Degrees()"/>
        protected internal const double ZENITH_5_POINT_95 = GEOMETRIC_ZENITH + 5.95;

        ///	 <summary>
        /// The zenith of 7.083&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This is often referred to as 7&deg;5' or 7&deg; and 5 minutes.
        ///	This calculation is used for calculating <em>alos</em> (dawn) and
        ///	<em>tzais</em> (nightfall) according to some opinions. This calculation
        ///	is based on the position of the sun 30 minutes after sunset in Jerusalem
        ///	on March 16, about 4 days before the equinox, the day that a solar hour
        ///	is one hour, which calculates to 7.0833333&deg; below
        ///	<seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>. This is time some opinions
        ///	consider dark enough for 3 stars to be visible. This is the opinion of
        ///	the Shu"t Melamed Leho'il, Shu"t Binyan Tziyon, Tenuvas Sadeh and very
        ///	close to the time of the Mekor Chesed on the Sefer chasidim.
        ///	 </summary>
        ///	<seealso cref="#getTzaisGeonim7Point083Degrees()"/>
        ///	<seealso cref="#getBainHasmashosRT13Point5MinutesBefore7Point083Degrees()"/>
        protected internal const double ZENITH_7_POINT_083 = GEOMETRIC_ZENITH + 7 + (5 / 60);
  
        ///	 <summary>
        /// The zenith of 10.2&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This calculation is used for calculating <em>misheyakir</em>
        ///	according to some opinions. This calculation is based on the position of
        ///	the sun 45 minutes before <seealso cref="#getSunrise sunrise"/> in Jerusalem on
        ///	March 16, about 4 days before the equinox, the day that a solar hour is
        ///	one hour which calculates to 10.2&deg; below {@link #GEOMETRIC_ZENITH
        ///	geometric zenith}
        ///	 </summary>
        ///	<seealso cref="#getMisheyakir10Point2Degrees()"/>
        protected internal const double ZENITH_10_POINT_2 = GEOMETRIC_ZENITH + 10.2;
 
        ///	 <summary>
        /// The zenith of 11&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This calculation is used for calculating <em>misheyakir</em>
        ///	according to some opinions. This calculation is based on the position of
        ///	the sun 48 minutes before <seealso cref="#getSunrise sunrise"/> in Jerusalem on
        ///	March 16, about 4 days before the equinox, the day that a solar hour is
        ///	one hour which calculates to 11&deg; below {@link #GEOMETRIC_ZENITH
        ///	geometric zenith}
        ///	 </summary>
        ///	<seealso cref="#getMisheyakir11Degrees()"/>
        protected internal const double ZENITH_11_DEGREES = GEOMETRIC_ZENITH + 11;
 
        ///	 <summary>
        /// The zenith of 11.5&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This calculation is used for calculating <em>misheyakir</em>
        ///	according to some opinions. This calculation is based on the position of
        ///	the sun 52 minutes before <seealso cref="#getSunrise sunrise"/> in Jerusalem on
        ///	March 16, about 4 days before the equinox, the day that a solar hour is
        ///	one hour which calculates to 11.5&deg; below {@link #GEOMETRIC_ZENITH
        ///	geometric zenith}
        ///	 </summary>
        ///	<seealso cref="#getMisheyakir11Point5Degrees()"/>
        protected internal const double ZENITH_11_POINT_5 = GEOMETRIC_ZENITH + 11.5;

        ///	 <summary>
        /// The zenith of 13&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This calculation is used for calculating
        ///	<em>Rabainu Tam's bain hashmashos</em> according to some opinions. <br/>
        ///	<br/>
        ///	<b>FIXME:</b> See comments on <seealso cref="#getBainHasmashosRT13Degrees"/>. This
        ///	should be changed to 13.2477 after confirmation.
        ///	 </summary>
        ///	<seealso cref="#getBainHasmashosRT13Degrees"/>
        protected internal const double ZENITH_13_DEGREES = GEOMETRIC_ZENITH + 13;

        ///	 <summary>
        /// The zenith of 19.8&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This calculation is used for calculating <em>alos</em> (dawn)
        ///	and <em>tzais</em> (nightfall) according to some opinions. This
        ///	calculation is based on the position of the sun 90 minutes after sunset
        ///	in Jerusalem on March 16, about 4 days before the equinox, the day that a
        ///	solar hour is one hour which calculates to 19.8&deg; below
        ///	<seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	 </summary>
        ///	<seealso cref="#getTzais19Point8Degrees()"/>
        ///	<seealso cref="#getAlos19Point8Degrees()"/>
        ///	<seealso cref="#getAlos90()"/>
        ///	<seealso cref="#getTzais90()"/>
        protected internal const double ZENITH_19_POINT_8 = GEOMETRIC_ZENITH + 19.8;
 
        ///	 <summary>
        /// The zenith of 26&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	(90&deg;). This calculation is used for calculating <em>alos</em> (dawn)
        ///	and <em>tzais</em> (nightfall) according to some opinions. This
        ///	calculation is based on the position of the sun {@link #getAlos120() 120
        ///	minutes} after sunset in Jerusalem on March 16, about 4 days before the
        ///	equinox, the day that a solar hour is one hour which calculates to
        ///	26&deg; below <seealso cref="#GEOMETRIC_ZENITH geometric zenith"/>
        ///	 </summary>
        ///	<seealso cref="#getAlos26Degrees()"/>
        ///	<seealso cref="#getTzais26Degrees()"/>
        ///	<seealso cref="#getAlos120()"/>
        ///	<seealso cref="#getTzais120()"/>
        protected internal const double ZENITH_26_DEGREES = GEOMETRIC_ZENITH + 26.0;

        private double ateretTorahSunsetOffset = 40;

        ///	 <summary>
        /// Default constructor will set a default <seealso cref="GeoLocation#GeoLocation()"/>,
        ///	a default {@link AstronomicalCalculator#getDefault()
        ///	AstronomicalCalculator} and default the calendar to the current date.
        ///	 </summary>
        ///	<seealso cref="AstronomicalCalendar#AstronomicalCalendar()"/>
        public ComplexZmanimCalendar()
        {
            this.ateretTorahSunsetOffset = 40.0;
        }

        public ComplexZmanimCalendar(GeoLocation location)
            : base(location)
        {
            this.ateretTorahSunsetOffset = 40.0;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is ComplexZmanimCalendar))
            {
                return false;
            }
            ComplexZmanimCalendar calendar = (ComplexZmanimCalendar)obj;
            return ((this.getCalendar().equals(calendar.getCalendar()) && this.getGeoLocation().Equals(calendar.getGeoLocation())) && java.lang.Object.instancehelper_equals(this.getAstronomicalCalculator(), calendar.getAstronomicalCalculator()));
        }

        public virtual Date getAlos120()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)(-7200000L));
        }

        public virtual Date getAlos120Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)(num * -2L));
        }

        public virtual Date getAlos16Point1Degrees()
        {
            return this.getSunriseOffsetByDegrees(106.1);
        }

        public virtual Date getAlos18Degrees()
        {
            return this.getSunriseOffsetByDegrees(108.0);
        }

        public virtual Date getAlos19Point8Degrees()
        {
            return this.getSunriseOffsetByDegrees(109.8);
        }

        public virtual Date getAlos26Degrees()
        {
            return this.getSunriseOffsetByDegrees(116.0);
        }

        ///	 <summary> * Method to return <em>alos</em> (dawn) calculated using 60 minutes before
        ///	 * <seealso cref="#getSeaLevelSunrise() sea level sunrise"/> on the time to walk the
        ///	 * distance of 4 <em>Mil</em> at 15 minutes a <em>Mil</em> (the opinion of
        ///	 * the Chavas Yair. See the Divray Malkiel). This is based on the opinion of
        ///	 * most <em>Rishonim</em> who stated that the time of the <em>Neshef</em>
        ///	 * (time between dawn and sunrise) does not vary by the time of year or
        ///	 * location but purely depends on the time it takes to walk the distance of
        ///	 * 4 <em>Mil</em>.
        ///	 *  </summary>
        ///	 * <returns> the <code>Date</code> representing the time. </returns>
        public virtual Date getAlos60()
        {
            return getTimeOffset(getSeaLevelSunrise(), -60 * MINUTE_MILLIS);
        }

        ///	 <summary> * Method to return <em>alos</em> (dawn) calculated using 72 minutes
        ///	 * <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/10th
        ///	 * of the day before sea level sunrise. This is based on an 18 minute
        ///	 * <em>Mil</em> so the time for 4 <em>Mil</em> is 72 minutes which is 1/10th
        ///	 * of a day (12 * 60 = 720) based on the day starting at
        ///	 * <seealso cref="#getSeaLevelSunrise() sea level sunrise"/> and ending at
        ///	 * <seealso cref="#getSeaLevelSunset() sea level sunset"/>. The actual alculation is
        ///	 * <seealso cref="#getSeaLevelSunrise()"/>- ( <seealso cref="#getShaahZmanisGra()"/> * 1.2).
        ///	 * This calculation is used in the calendars published by
        ///	 * <em>Hisachdus Harabanim D'Artzos Habris Ve'Kanada</em>
        ///	 *  </summary>
        ///	 * <returns> the <code>Date</code> representing the time. </returns>
        ///	 * <seealso cref= #getShaahZmanisGra() </seealso>
        public virtual Date getAlos72Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunrise(), (long)(shaahZmanis * -1.2));
        }

        public virtual Date getAlos90()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)(-5400000L));
        }

        ///	 <summary> * Method to return <em>alos</em> (dawn) calculated using 90 minutes
        ///	 * <em>zmaniyos</em>( <em>GR"A</em> and the <em>Baal Hatanya</em>) or 1/8th
        ///	 * of the day before sea level sunrise. This is based on a 22.5 minute
        ///	 * <em>Mil</em> so the time for 4 <em>Mil</em> is 90 minutes which is 1/8th
        ///	 * of a day (12 * 60 = 720) /8 =90 based on the day starting at
        ///	 * <seealso cref="#getSunrise() sunrise"/> and ending at <seealso cref="#getSunset() sunset"/>.
        ///	 * The actual calculation is <seealso cref="#getSunrise()"/> - (
        ///	 * <seealso cref="#getShaahZmanisGra()"/> * 1.5).
        ///	 *  </summary>
        ///	 * <returns> the <code>Date</code> representing the time. </returns>
        ///	 * <seealso cref= #getShaahZmanisGra() </seealso>
        public virtual Date getAlos90Zmanis()
        {
            long shaahZmanis = getShaahZmanisGra();
            if (shaahZmanis == long.MinValue)
            {
                return null;
            }
            return getTimeOffset(getSeaLevelSunrise(), (long)(shaahZmanis * -1.5));
        }

        ///	 <summary> * Method to return <em>alos</em> (dawn) calculated using 96 minutes before
        ///	 * <seealso cref="#getSeaLevelSunrise() sea level sunrise"/> based on the time to walk
        ///	 * the distance of 4 <em>Mil</em> at 24 minutes a <em>Mil</em>. This is
        ///	 * based on the opinion of most <em>Rishonim</em> who stated that the time
        ///	 * of the <em>Neshef</em> (time between dawn and sunrise) does not vary by
        ///	 * the time of year or location but purely depends on the time it takes to
        ///	 * walk the distance of 4 <em>Mil</em>.
        ///	 *  </summary>
        ///	 * <returns> the <code>Date</code> representing the time. </returns>
        public virtual Date getAlos96()
        {
            return getTimeOffset(getSeaLevelSunrise(), -96 * MINUTE_MILLIS);
        }

        public virtual Date getAlos96Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)num * -1.6);
        }

        public virtual double getAteretTorahSunsetOffset()
        {
            return this.ateretTorahSunsetOffset;
        }

        public virtual Date getBainHasmashosRT13Degrees()
        {
            return this.getSunsetOffsetByDegrees(103.0);
        }

        public virtual Date getBainHasmashosRT13Point5MinutesBefore7Point083Degrees()
        {
            return this.getTimeOffset(this.getSunsetOffsetByDegrees(97.0), (double)-810000.0);
        }

        public virtual Date getBainHasmashosRT2Stars()
        {
            Date date = this.getAlos19Point8Degrees();
            Date date2 = this.getSeaLevelSunrise();
            if ((date != null) && (date2 != null))
            {
                return this.getTimeOffset(this.getSeaLevelSunset(), (double)((date2.getTime() - date.getTime()) * 0.27777777777777779));
            }
            return null;
        }

        ///	 <summary>
        /// This method returns Bain Hashmashos of Rabainu Tam calculated as a 58.5
        ///	minute offset after sunset. Bain hashmashos is 3/4 of a mil before tzais
        ///	or 3 1/4 mil after sunset. With a mil calculated as 18 minutes, 3.25 * 18
        ///	= 58.5 minutes.
        ///	 </summary>
        ///	<returns> the <code>Date</code> of 58.5 minutes after sunset
        ///	 </returns>
        public virtual Date getBainHasmashosRT58Point5Minutes()
        {
            return getTimeOffset(getSeaLevelSunset(), 58.5 * MINUTE_MILLIS);
        }

        ///	<summary>
        /// A method that returns the local time for fixed <em>chatzos</em>. This
        ///	time is noon and midnight adjusted from standard time to account for the
        ///	local latitude. The 360&deg; of the globe divided by 24 calculates to
        ///	15&deg; per hour with 4 minutes per degree, so at a longitude of 0 , 15,
        ///	30 etc Chatzos in 12:00 noon. Lakewood, NJ whose longitude is -74.2094 is
        ///	0.7906 away from the closest multiple of 15 at -75&deg;. This is
        ///	multiplied by 4 to yeild 3 minutes and 10 seconds for a chatzos of
        ///	11:56:50. This method is not tied to the theoretical 15&deg; timezones,
        ///	but will adjust to the actual timezone and <a
        ///	href="http://en.wikipedia.org/wiki/Daylight_saving_time">Daylight saving
        ///	time</a>.
        ///	</summary>
        ///	<returns> the Date representing the local <em>chatzos</em> </returns>
        ///	<seealso cref="GeoLocation#getLocalMeanTimeOffset()"/>
        public virtual Date getFixedLocalChatzos()
        {
            return getTimeOffset(getDateFromTime(12.0 - getGeoLocation().getTimeZone().getRawOffset() / HOUR_MILLIS), -getGeoLocation().getLocalMeanTimeOffset());
        }

        public virtual Date getMinchaGedola16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double)(this.getShaahZmanis16Point1Degrees() * 6.5));
        }

        public virtual Date getMinchaGedola30Minutes()
        {
            return this.getTimeOffset(this.getChatzos(), (long)0x1b7740L);
        }

        public virtual Date getMinchaGedola72Minutes()
        {
            return this.getTimeOffset(this.getAlos72(), (double)(this.getShaahZmanis72Minutes() * 6.5));
        }

        public virtual Date getMinchaGedolaAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (double)(this.getShaahZmanisAteretTorah() * 6.5));
        }

        public virtual Date getMinchaGedolaGreaterThan30()
        {
            if ((this.getMinchaGedola30Minutes() == null) || (this.getMinchaGedola() == null))
            {
                return null;
            }
            return ((this.getMinchaGedola30Minutes().compareTo(this.getMinchaGedola()) <= 0) ? this.getMinchaGedola() : this.getMinchaGedola30Minutes());
        }

        public virtual Date getMinchaKetana16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double)(this.getShaahZmanis16Point1Degrees() * 9.5));
        }

        public virtual Date getMinchaKetana72Minutes()
        {
            return this.getTimeOffset(this.getAlos72(), (double)(this.getShaahZmanis72Minutes() * 9.5));
        }

        public virtual Date getMinchaKetanaAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (double)(this.getShaahZmanisAteretTorah() * 9.5));
        }

        public virtual Date getMisheyakir10Point2Degrees()
        {
            return this.getSunriseOffsetByDegrees(100.2);
        }

        public virtual Date getMisheyakir11Degrees()
        {
            return this.getSunriseOffsetByDegrees(101.0);
        }

        public virtual Date getMisheyakir11Point5Degrees()
        {
            return this.getSunriseOffsetByDegrees(101.5);
        }

        public virtual Date getPlagAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            long num = this.getTemporalHour(this.getAlos16Point1Degrees(), this.getTzaisGeonim7Point083Degrees());
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double)(num * 10.75));
        }

        public virtual Date getPlagAlosToSunset()
        {
            long num = this.getTemporalHour(this.getAlos16Point1Degrees(), this.getSeaLevelSunset());
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double)(num * 10.75));
        }

        ///	 <summary> * This method returns the time of <em>plag hamincha</em>. This is
        ///	 * calculated as 10.75 hours after <seealso cref="#getAlos72() dawn"/>. The formula
        ///	 * used is:<br/>
        ///	 * 10.75 <seealso cref="#getShaahZmanis72Minutes()"/> after <seealso cref="#getAlos72()"/>.
        ///	 *  </summary>
        ///	 * <returns> the <code>Date</code> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha120Minutes()
        {
            return getTimeOffset(getAlos120(), getShaahZmanis120Minutes() * 10.75);
        }

        ///	 <summary> * This method returns the time of <em>plag hamincha</em>. This is
        ///	 * calculated as 10.75 hours after <seealso cref="#getAlos96Zmanis() dawn"/>. The
        ///	 * formula used is:<br/>
        ///	 * 10.75 * <seealso cref="#getShaahZmanis96MinutesZmanis()"/> after
        ///	 * <seealso cref="#getAlos96Zmanis() dawn"/>.
        ///	 *  </summary>
        ///	 * <returns> the <code>Date</code> of the time of <em>plag hamincha</em>. </returns>
        public virtual Date getPlagHamincha120MinutesZmanis()
        {
            return getTimeOffset(getAlos120Zmanis(), getShaahZmanis120MinutesZmanis() * 10.75);
        }

        public virtual Date getPlagHamincha16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double)(this.getShaahZmanis16Point1Degrees() * 10.75));
        }

        public virtual Date getPlagHamincha18Degrees()
        {
            return this.getTimeOffset(this.getAlos18Degrees(), (double)(this.getShaahZmanis18Degrees() * 10.75));
        }

        public virtual Date getPlagHamincha19Point8Degrees()
        {
            return this.getTimeOffset(this.getAlos19Point8Degrees(), (double)(this.getShaahZmanis19Point8Degrees() * 10.75));
        }

        public virtual Date getPlagHamincha26Degrees()
        {
            return this.getTimeOffset(this.getAlos26Degrees(), (double)(this.getShaahZmanis26Degrees() * 10.75));
        }

        public virtual Date getPlagHamincha60Minutes()
        {
            return this.getTimeOffset(this.getAlos60(), (double)(this.getShaahZmanis60Minutes() * 10.75));
        }

        public virtual Date getPlagHamincha72Minutes()
        {
            return this.getTimeOffset(this.getAlos72(), (double)(this.getShaahZmanis72Minutes() * 10.75));
        }

        public virtual Date getPlagHamincha72MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (double)(this.getShaahZmanis72MinutesZmanis() * 10.75));
        }

        public virtual Date getPlagHamincha90Minutes()
        {
            return this.getTimeOffset(this.getAlos90(), (double)(this.getShaahZmanis90Minutes() * 10.75));
        }

        public virtual Date getPlagHamincha90MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos90Zmanis(), (double)(this.getShaahZmanis90MinutesZmanis() * 10.75));
        }

        public virtual Date getPlagHamincha96Minutes()
        {
            return this.getTimeOffset(this.getAlos96(), (double)(this.getShaahZmanis96Minutes() * 10.75));
        }

        public virtual Date getPlagHamincha96MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos96Zmanis(), (double)(this.getShaahZmanis96MinutesZmanis() * 10.75));
        }

        public virtual Date getPlagHaminchaAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (double)(this.getShaahZmanisAteretTorah() * 10.75));
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///	 * a dip of 120 minutes. This calculation divides the day based on the
        ///	 * opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ///	 * calculation is 120 minutes before sunrise and dusk is 120 minutes after
        ///	 * sunset. This day is split into 12 equal parts with each part being a
        ///	 * <em>shaah zmanis</em>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis120Minutes()
        {
            return getTemporalHour(getAlos120(), getTzais120());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///	 * opinion of the MGA based on <em>alos</em> being
        ///	 * <seealso cref="#getAlos120Zmanis() 120"/> minutes <em>zmaniyos</em> before
        ///	 * <seealso cref="#getSunrise() sunrise"/>. This calculation divides the day based on
        ///	 * the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ///	 * for this calculation is 120 minutes <em>zmaniyos</em> before sunrise and
        ///	 * dusk is 120 minutes <em>zmaniyos</em> after sunset. This day is split
        ///	 * into 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ///	 * identical to 1/6th of the day from <seealso cref="#getSunrise() sunrise"/> to
        ///	 * <seealso cref="#getSunset() sunset"/>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        ///	 * <seealso cref= #getAlos120Zmanis() </seealso>
        ///	 * <seealso cref= #getTzais120Zmanis() </seealso>
        public virtual long getShaahZmanis120MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos120Zmanis(), this.getTzais120Zmanis());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///	 * a dip of 16.1&deg;. This calculation divides the day based on the opinion
        ///	 * that the day runs from dawn to dusk. Dawn for this calculation is when
        ///	 * the sun is 16.1&deg; below the eastern geometric horizon before sunrise
        ///	 * and dusk is when the sun is 16.1&deg; below the western geometric horizon
        ///	 * after sunset. This day is split into 12 equal parts with each part being
        ///	 * a <em>shaah zmanis</em>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        ///	 * <seealso cref= #getAlos16Point1Degrees() </seealso>
        ///	 * <seealso cref= #getTzais16Point1Degrees() </seealso>
        ///	 * <seealso cref= #getSofZmanShmaMGA16Point1Degrees() </seealso>
        ///	 * <seealso cref= #getSofZmanTfilaMGA16Point1Degrees() </seealso>
        ///	 * <seealso cref= #getMinchaGedola16Point1Degrees() </seealso>
        ///	 * <seealso cref= #getMinchaKetana16Point1Degrees() </seealso>
        ///	 * <seealso cref= #getPlagHamincha16Point1Degrees() </seealso>
        public virtual long getShaahZmanis16Point1Degrees()
        {
            return getTemporalHour(getAlos16Point1Degrees(), getTzais16Point1Degrees());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///	 * a 18&deg; dip. This calculation divides the day based on the opinion of
        ///	 * the MGA that the day runs from dawn to dusk. Dawn for this calculation is
        ///	 * when the sun is 18&deg; below the eastern geometric horizon before
        ///	 * sunrise. Dusk for this is when the sun is 18&deg; below the western
        ///	 * geometric horizon after sunset. This day is split into 12 equal parts
        ///	 * with each part being a <em>shaah zmanis</em>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis18Degrees()
        {
            return getTemporalHour(getAlos18Degrees(), getTzais18Degrees());
        }

        ///	 <summary>
        /// Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///	a 19.8&deg; dip. This calculation divides the day based on the opinion of
        ///	the MGA that the day runs from dawn to dusk. Dawn for this calculation is
        ///	when the sun is 19.8&deg; below the eastern geometric horizon before
        ///	sunrise. Dusk for this is when the sun is 19.8&deg; below the western
        ///	geometric horizon after sunset. This day is split into 12 equal parts
        ///	with each part being a <em>shaah zmanis</em>.
        ///	 </summary>
        ///	<returns> the <code>long</code> millisecond length of a
        ///	        <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis19Point8Degrees()
        {
            return getTemporalHour(getAlos19Point8Degrees(), getTzais19Point8Degrees());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///	 * a dip of 26&deg;. This calculation divides the day based on the opinion
        ///	 * of the MGA that the day runs from dawn to dusk. Dawn for this calculation
        ///	 * is when the sun is <seealso cref="#getAlos26Degrees() 26&deg;"/> below the eastern
        ///	 * geometric horizon before sunrise. Dusk for this is when the sun is
        ///	 * <seealso cref="#getTzais26Degrees() 26&deg;"/> below the western geometric horizon
        ///	 * after sunset. This day is split into 12 equal parts with each part being
        ///	 * a <em>shaah zmanis</em>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis26Degrees()
        {
            return getTemporalHour(getAlos26Degrees(), getTzais26Degrees());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (solar hour) according to the
        ///	 * opinion of the MGA. This calculation divides the day based on the opinion
        ///	 * of the <em>MGA</em> that the day runs from dawn to dusk. Dawn for this
        ///	 * calculation is 60 minutes before sunrise and dusk is 60 minutes after
        ///	 * sunset. This day is split into 12 equal parts with each part being a
        ///	 * <em>shaah zmanis</em>. Alternate mothods of calculating a
        ///	 * <em>shaah zmanis</em> are available in the subclass
        ///	 * <seealso cref="ComplexZmanimCalendar"/>
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis60Minutes()
        {
            return getTemporalHour(getAlos60(), getTzais60());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (solar hour) according to the
        ///	 * opinion of the MGA. This calculation divides the day based on the opinion
        ///	 * of the <em>MGA</em> that the day runs from dawn to dusk. Dawn for this
        ///	 * calculation is 72 minutes before sunrise and dusk is 72 minutes after
        ///	 * sunset. This day is split into 12 equal parts with each part being a
        ///	 * <em>shaah zmanis</em>. Alternate mothods of calculating a
        ///	 * <em>shaah zmanis</em> are available in the subclass
        ///	 * <seealso cref="ComplexZmanimCalendar"/>
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis72Minutes()
        {
            return getShaahZmanisMGA();
        }
        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///	 * opinion of the MGA based on <em>alos</em> being
        ///	 * <seealso cref="#getAlos72Zmanis() 72"/> minutes <em>zmaniyos</em> before
        ///	 * <seealso cref="#getSunrise() sunrise"/>. This calculation divides the day based on
        ///	 * the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ///	 * for this calculation is 72 minutes <em>zmaniyos</em> before sunrise and
        ///	 * dusk is 72 minutes <em>zmaniyos</em> after sunset. This day is split into
        ///	 * 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ///	 * identical to 1/10th of the day from <seealso cref="#getSunrise() sunrise"/> to
        ///	 * <seealso cref="#getSunset() sunset"/>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        ///	 * <seealso cref= #getAlos72Zmanis() </seealso>
        ///	 * <seealso cref= #getTzais72Zmanis() </seealso>
        public virtual long getShaahZmanis72MinutesZmanis()
        {
            return getTemporalHour(getAlos72Zmanis(), getTzais72Zmanis());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///	 * a dip of 90 minutes. This calculation divides the day based on the
        ///	 * opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ///	 * calculation is 90 minutes before sunrise and dusk is 90 minutes after
        ///	 * sunset. This day is split into 12 equal parts with each part being a
        ///	 * <em>shaah zmanis</em>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis90Minutes()
        {
            return getTemporalHour(getAlos90(), getTzais90());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///	 * opinion of the MGA based on <em>alos</em> being
        ///	 * <seealso cref="#getAlos90Zmanis() 90"/> minutes <em>zmaniyos</em> before
        ///	 * <seealso cref="#getSunrise() sunrise"/>. This calculation divides the day based on
        ///	 * the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ///	 * for this calculation is 90 minutes <em>zmaniyos</em> before sunrise and
        ///	 * dusk is 90 minutes <em>zmaniyos</em> after sunset. This day is split into
        ///	 * 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ///	 * identical to 1/8th of the day from <seealso cref="#getSunrise() sunrise"/> to
        ///	 * <seealso cref="#getSunset() sunset"/>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        ///	 * <seealso cref= #getAlos90Zmanis() </seealso>
        ///	 * <seealso cref= #getTzais90Zmanis() </seealso>
        public virtual long getShaahZmanis90MinutesZmanis()
        {
            return getTemporalHour(getAlos90Zmanis(), getTzais90Zmanis());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) calculated using
        ///	 * a dip of 96 minutes. This calculation divides the day based on the
        ///	 * opinion of the MGA that the day runs from dawn to dusk. Dawn for this
        ///	 * calculation is 96 minutes before sunrise and dusk is 96 minutes after
        ///	 * sunset. This day is split into 12 equal parts with each part being a
        ///	 * <em>shaah zmanis</em>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        public virtual long getShaahZmanis96Minutes()
        {
            return getTemporalHour(getAlos96(), getTzais96());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///	 * opinion of the MGA based on <em>alos</em> being
        ///	 * <seealso cref="#getAlos96Zmanis() 96"/> minutes <em>zmaniyos</em> before
        ///	 * <seealso cref="#getSunrise() sunrise"/>. This calculation divides the day based on
        ///	 * the opinion of the <em>MGA</em> that the day runs from dawn to dusk. Dawn
        ///	 * for this calculation is 96 minutes <em>zmaniyos</em> before sunrise and
        ///	 * dusk is 96 minutes <em>zmaniyos</em> after sunset. This day is split into
        ///	 * 12 equal parts with each part being a <em>shaah zmanis</em>. This is
        ///	 * identical to 1/7.5th of the day from <seealso cref="#getSunrise() sunrise"/> to
        ///	 * <seealso cref="#getSunset() sunset"/>.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        ///	 * <seealso cref= #getAlos96Zmanis() </seealso>
        ///	 * <seealso cref= #getTzais96Zmanis() </seealso>
        public virtual long getShaahZmanis96MinutesZmanis()
        {
            return getTemporalHour(getAlos96Zmanis(), getTzais96Zmanis());
        }

        ///	 <summary> * Method to return a <em>shaah zmanis</em> (temporal hour) according to the
        ///	 * opinion of the Chacham Yosef Harari-Raful of Yeshivat Ateret Torah
        ///	 * calculated with <em>alos</em> being 1/10th of sunrise to sunset day, or
        ///	 * <seealso cref="#getAlos72Zmanis() 72"/> minutes <em>zmaniyos</em> of such a day
        ///	 * before <seealso cref="#getSunrise() sunrise"/>, and tzais is usually calculated as
        ///	 * <seealso cref="#getTzaisAteretTorah() 40 minutes"/> after {@link #getSunset()
        ///	 * sunset}. This day is split into 12 equal parts with each part being a
        ///	 * <em>shaah zmanis</em>. Note that with this system, chatzos (mid-day) will
        ///	 * not be the point that the sun is {@link #getSunTransit() halfway across
        ///	 * the sky}.
        ///	 *  </summary>
        ///	 * <returns> the <code>long</code> millisecond length of a
        ///	 *         <em>shaah zmanis</em>. </returns>
        ///	 * <seealso cref= #getAlos72Zmanis() </seealso>
        ///	 * <seealso cref= #getTzaisAteretTorah() </seealso>
        ///	 * <seealso cref= #getAteretTorahSunsetOffset() </seealso>
        ///	 * <seealso cref= #setAteretTorahSunsetOffset(double) </seealso>
        public virtual long getShaahZmanisAteretTorah()
        {
            return getTemporalHour(getAlos72Zmanis(), getTzaisAteretTorah());
        }

        public virtual Date getSofZmanShma3HoursBeforeChatzos()
        {
            return this.getTimeOffset(this.getChatzos(), (long)(-10800000L));
        }

        public virtual Date getSofZmanShmaAlos16Point1ToSunset()
        {
            long num = this.getTemporalHour(this.getAlos16Point1Degrees(), this.getSeaLevelSunset());
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (long)(num * 3L));
        }

        public virtual Date getSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            long num = this.getTemporalHour(this.getAlos16Point1Degrees(), this.getTzaisGeonim7Point083Degrees());
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (long)(num * 3L));
        }

        public virtual Date getSofZmanShmaAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (long)(this.getShaahZmanisAteretTorah() * 3L));
        }

        public virtual Date getSofZmanShmaFixedLocal()
        {
            return this.getTimeOffset(this.getFixedLocalChatzos(), (long)(-10800000L));
        }

        public virtual Date getSofZmanShmaKolEliyahu()
        {
            Date time = this.getFixedLocalChatzos();
            if ((time == null) || (this.getSunrise() == null))
            {
                return null;
            }
            long num = (time.getTime() - this.getSeaLevelSunrise().getTime()) / 2L;
            return this.getTimeOffset(time, -num);
        }

        public virtual Date getSofZmanShmaMGA120Minutes()
        {
            return this.getTimeOffset(this.getAlos120(), (long)(this.getShaahZmanis120Minutes() * 3L));
        }

        public virtual Date getSofZmanShmaMGA16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (long)(this.getShaahZmanis16Point1Degrees() * 3L));
        }

        public virtual Date getSofZmanShmaMGA19Point8Degrees()
        {
            return this.getTimeOffset(this.getAlos19Point8Degrees(), (long)(this.getShaahZmanis19Point8Degrees() * 3L));
        }

        public virtual Date getSofZmanShmaMGA72Minutes()
        {
            return this.getSofZmanShmaMGA();
        }

        public virtual Date getSofZmanShmaMGA72MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (long)(this.getShaahZmanis72MinutesZmanis() * 3L));
        }

        public virtual Date getSofZmanShmaMGA90Minutes()
        {
            return this.getTimeOffset(this.getAlos90(), (long)(this.getShaahZmanis90Minutes() * 3L));
        }

        public virtual Date getSofZmanShmaMGA90MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos90Zmanis(), (long)(this.getShaahZmanis90MinutesZmanis() * 3L));
        }

        public virtual Date getSofZmanShmaMGA96Minutes()
        {
            return this.getTimeOffset(this.getAlos96(), (long)(this.getShaahZmanis96Minutes() * 3L));
        }

        public virtual Date getSofZmanShmaMGA96MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos96Zmanis(), (long)(this.getShaahZmanis96MinutesZmanis() * 3L));
        }

        public virtual Date getSofZmanTfila2HoursBeforeChatzos()
        {
            return this.getTimeOffset(this.getChatzos(), (long)(-7200000L));
        }

        public virtual Date getSofZmanTfilaFixedLocal()
        {
            return this.getTimeOffset(this.getFixedLocalChatzos(), (long)(-7200000L));
        }

        public virtual Date getSofZmanTfilahAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (long)(this.getShaahZmanisAteretTorah() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA120Minutes()
        {
            return this.getTimeOffset(this.getAlos120(), (long)(this.getShaahZmanis120Minutes() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (long)(this.getShaahZmanis16Point1Degrees() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA19Point8Degrees()
        {
            return this.getTimeOffset(this.getAlos19Point8Degrees(), (long)(this.getShaahZmanis19Point8Degrees() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA72Minutes()
        {
            return this.getSofZmanTfilaMGA();
        }

        public virtual Date getSofZmanTfilaMGA72MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (long)(this.getShaahZmanis72MinutesZmanis() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA90Minutes()
        {
            return this.getTimeOffset(this.getAlos90(), (long)(this.getShaahZmanis90Minutes() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA90MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos90Zmanis(), (long)(this.getShaahZmanis90MinutesZmanis() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA96Minutes()
        {
            return this.getTimeOffset(this.getAlos96(), (long)(this.getShaahZmanis96Minutes() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA96MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos96Zmanis(), (long)(this.getShaahZmanis96MinutesZmanis() * 4L));
        }

        public virtual Date getTzais120()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long)0x6ddd00L);
        }

        public virtual Date getTzais120Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunset(), (double)(num * 2.0));
        }

        public virtual Date getTzais16Point1Degrees()
        {
            return this.getSunsetOffsetByDegrees(106.1);
        }

        public virtual Date getTzais18Degrees()
        {
            return this.getSunsetOffsetByDegrees(108.0);
        }

        public virtual Date getTzais19Point8Degrees()
        {
            return this.getSunsetOffsetByDegrees(109.8);
        }

        public virtual Date getTzais26Degrees()
        {
            return this.getSunsetOffsetByDegrees(116.0);
        }

        public virtual Date getTzais60()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long)0x36ee80L);
        }

        public virtual Date getTzais72Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunset(), (double)(num * 1.2));
        }

        public virtual Date getTzais90()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long)0x5265c0L);
        }

        public virtual Date getTzais90Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunset(), (double)(num * 1.5));
        }

        public virtual Date getTzais96()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long)0x57e400L);
        }

        public virtual Date getTzais96Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunset(), (double)(num * 1.6));
        }

        public virtual Date getTzaisAteretTorah()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (double)(this.getAteretTorahSunsetOffset() * 60000.0));
        }

        public virtual Date getTzaisGeonim5Point95Degrees()
        {
            return this.getSunsetOffsetByDegrees(95.95);
        }

        public virtual Date getTzaisGeonim7Point083Degrees()
        {
            return this.getSunsetOffsetByDegrees(97.0);
        }

        public virtual Date getTzaisGeonim8Point5Degrees()
        {
            return this.getSunsetOffsetByDegrees(98.5);
        }

        public override int GetHashCode()
        {
            int num = 0x11;
            num = (0x25 * num) + java.lang.Object.instancehelper_hashCode(base.getTemporalHour());
            num += (0x25 * num) + this.getCalendar().hashCode();
            num += (0x25 * num) + this.getGeoLocation().GetHashCode();
            return (num + ((0x25 * num) + java.lang.Object.instancehelper_hashCode(this.getAstronomicalCalculator())));
        }

        public virtual void setAteretTorahSunsetOffset(double ateretTorahSunsetOffset)
        {
            this.ateretTorahSunsetOffset = ateretTorahSunsetOffset;
        }
    }
}

