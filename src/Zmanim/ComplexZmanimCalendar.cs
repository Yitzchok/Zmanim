namespace net.sourceforge.zmanim
{
    using java.util;
    using net.sourceforge.zmanim.util;

    public class ComplexZmanimCalendar : ZmanimCalendar
    {
        private double ateretTorahSunsetOffset;
        private const long serialVersionUID = 1L;
        protected internal const double ZENITH_10_POINT_2 = 100.2;
        protected internal const double ZENITH_11_DEGREES = 101.0;
        protected internal const double ZENITH_11_POINT_5 = 101.5;
        protected internal const double ZENITH_13_DEGREES = 103.0;
        protected internal const double ZENITH_19_POINT_8 = 109.8;
        protected internal const double ZENITH_26_DEGREES = 116.0;
        protected internal const double ZENITH_3_POINT_7 = 93.7;
        protected internal const double ZENITH_5_POINT_95 = 95.95;
        protected internal const double ZENITH_7_POINT_083 = 97.0;

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

        public virtual Date getAlos60()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)(-3600000L));
        }

        public virtual Date getAlos72Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)num * -1.2);
        }

        public virtual Date getAlos90()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)(-5400000L));
        }

        public virtual Date getAlos90Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)num * -1.5);
        }

        public virtual Date getAlos96()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long)(-5760000L));
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

        public virtual Date getBainHasmashosRT58Point5Minutes()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (double)3510000.0);
        }

        public virtual Date getFixedLocalChatzos()
        {
            return this.getTimeOffset(this.getDateFromTime(12.0 - (((long)this.getGeoLocation().getTimeZone().getRawOffset()) / 0x36ee80L)), -this.getGeoLocation().getLocalMeanTimeOffset());
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

        public virtual Date getPlagHamincha120Minutes()
        {
            return this.getTimeOffset(this.getAlos120(), (double)(this.getShaahZmanis120Minutes() * 10.75));
        }

        public virtual Date getPlagHamincha120MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos120Zmanis(), (double)(this.getShaahZmanis120MinutesZmanis() * 10.75));
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

        public virtual long getShaahZmanis120Minutes()
        {
            return this.getTemporalHour(this.getAlos120(), this.getTzais120());
        }

        public virtual long getShaahZmanis120MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos120Zmanis(), this.getTzais120Zmanis());
        }

        public virtual long getShaahZmanis16Point1Degrees()
        {
            return this.getTemporalHour(this.getAlos16Point1Degrees(), this.getTzais16Point1Degrees());
        }

        public virtual long getShaahZmanis18Degrees()
        {
            return this.getTemporalHour(this.getAlos18Degrees(), this.getTzais18Degrees());
        }

        public virtual long getShaahZmanis19Point8Degrees()
        {
            return this.getTemporalHour(this.getAlos19Point8Degrees(), this.getTzais19Point8Degrees());
        }

        public virtual long getShaahZmanis26Degrees()
        {
            return this.getTemporalHour(this.getAlos26Degrees(), this.getTzais26Degrees());
        }

        public virtual long getShaahZmanis60Minutes()
        {
            return this.getTemporalHour(this.getAlos60(), this.getTzais60());
        }

        public virtual long getShaahZmanis72Minutes()
        {
            return this.getShaahZmanisMGA();
        }

        public virtual long getShaahZmanis72MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos72Zmanis(), this.getTzais72Zmanis());
        }

        public virtual long getShaahZmanis90Minutes()
        {
            return this.getTemporalHour(this.getAlos90(), this.getTzais90());
        }

        public virtual long getShaahZmanis90MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos90Zmanis(), this.getTzais90Zmanis());
        }

        public virtual long getShaahZmanis96Minutes()
        {
            return this.getTemporalHour(this.getAlos96(), this.getTzais96());
        }

        public virtual long getShaahZmanis96MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos96Zmanis(), this.getTzais96Zmanis());
        }

        public virtual long getShaahZmanisAteretTorah()
        {
            return this.getTemporalHour(this.getAlos72Zmanis(), this.getTzaisAteretTorah());
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

