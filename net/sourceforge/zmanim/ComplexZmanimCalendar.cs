namespace net.sourceforge.zmanim
{
    using IKVM.Attributes;
    using IKVM.Runtime;
    using java.lang;
    using java.util;
    using net.sourceforge.zmanim.util;
    using System;
    using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x83, 0xe8, 50, 240, 0x4f })]
        public ComplexZmanimCalendar()
        {
            this.ateretTorahSunsetOffset = 40.0;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 120, 0xe9, 0x3d, 0xd0 })]
        public ComplexZmanimCalendar(GeoLocation location) : base(location)
        {
            this.ateretTorahSunsetOffset = 40.0;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa7, 0xf2, 0x67, 130, 0x6b, 130, 0x87 })]
        public override bool equals(object @object)
        {
            if (this == @object)
            {
                return true;
            }
            if (!(@object is ComplexZmanimCalendar))
            {
                return false;
            }
            ComplexZmanimCalendar calendar = (ComplexZmanimCalendar) @object;
            return ((this.getCalendar().equals(calendar.getCalendar()) && this.getGeoLocation().equals(calendar.getGeoLocation())) && java.lang.Object.instancehelper_equals(this.getAstronomicalCalculator(), calendar.getAstronomicalCalculator()));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x293)]
        public virtual Date getAlos120()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long) (-7200000L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa2, 50, 0x67, 0x6f, 130 })]
        public virtual Date getAlos120Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long) (num * -2L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x2e8)]
        public virtual Date getAlos16Point1Degrees()
        {
            return this.getSunriseOffsetByDegrees(106.1);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 710)]
        public virtual Date getAlos18Degrees()
        {
            return this.getSunriseOffsetByDegrees(108.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x2d7)]
        public virtual Date getAlos19Point8Degrees()
        {
            return this.getSunriseOffsetByDegrees(109.8);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x2ba)]
        public virtual Date getAlos26Degrees()
        {
            return this.getSunriseOffsetByDegrees(116.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x224)]
        public virtual Date getAlos60()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long) (-3600000L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0xc5, 0x67, 0x6f, 130 })]
        public virtual Date getAlos72Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunrise(), ByteCodeHelper.d2l(num * -1.2));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x283)]
        public virtual Date getAlos90()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long) (-5400000L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0xe9, 0x67, 0x6f, 130 })]
        public virtual Date getAlos90Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunrise(), ByteCodeHelper.d2l(num * -1.5));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x24a)]
        public virtual Date getAlos96()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long) (-5760000L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0xfe, 0x67, 0x6f, 130 })]
        public virtual Date getAlos96Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunrise(), ByteCodeHelper.d2l(num * -1.6));
        }

        public virtual double getAteretTorahSunsetOffset()
        {
            return this.ateretTorahSunsetOffset;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x660)]
        public virtual Date getBainHasmashosRT13Degrees()
        {
            return this.getSunsetOffsetByDegrees(103.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x67a)]
        public virtual Date getBainHasmashosRT13Point5MinutesBefore7Point083Degrees()
        {
            return this.getTimeOffset(this.getSunsetOffsetByDegrees(97.0), (double) -810000.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa6, 0x17, 0x67, 0x67, 0x6c, 130 })]
        public virtual Date getBainHasmashosRT2Stars()
        {
            Date date = this.getAlos19Point8Degrees();
            Date date2 = this.getSeaLevelSunrise();
            if ((date != null) && (date2 != null))
            {
                return this.getTimeOffset(this.getSeaLevelSunset(), (double) ((date2.getTime() - date.getTime()) * 0.27777777777777779));
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x66d)]
        public virtual Date getBainHasmashosRT58Point5Minutes()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (double) 3510000.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x841)]
        public virtual Date getFixedLocalChatzos()
        {
            return this.getTimeOffset(this.getDateFromTime(12.0 - (((long) this.getGeoLocation().getTimeZone().getRawOffset()) / 0x36ee80L)), -this.getGeoLocation().getLocalMeanTimeOffset());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x541)]
        public virtual Date getMinchaGedola16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double) (this.getShaahZmanis16Point1Degrees() * 6.5));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x51a)]
        public virtual Date getMinchaGedola30Minutes()
        {
            return this.getTimeOffset(this.getChatzos(), (long) 0x1b7740L);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x52e)]
        public virtual Date getMinchaGedola72Minutes()
        {
            return this.getTimeOffset(this.getAlos72(), (double) (this.getShaahZmanis72Minutes() * 6.5));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x74b)]
        public virtual Date getMinchaGedolaAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (double) (this.getShaahZmanisAteretTorah() * 6.5));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa4, 0xde, 0x76, 130 })]
        public virtual Date getMinchaGedolaGreaterThan30()
        {
            if ((this.getMinchaGedola30Minutes() == null) || (this.getMinchaGedola() == null))
            {
                return null;
            }
            return ((this.getMinchaGedola30Minutes().compareTo(this.getMinchaGedola()) <= 0) ? this.getMinchaGedola() : this.getMinchaGedola30Minutes());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x569)]
        public virtual Date getMinchaKetana16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double) (this.getShaahZmanis16Point1Degrees() * 9.5));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x57e)]
        public virtual Date getMinchaKetana72Minutes()
        {
            return this.getTimeOffset(this.getAlos72(), (double) (this.getShaahZmanis72Minutes() * 9.5));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x765)]
        public virtual Date getMinchaKetanaAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (double) (this.getShaahZmanisAteretTorah() * 9.5));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x315)]
        public virtual Date getMisheyakir10Point2Degrees()
        {
            return this.getSunriseOffsetByDegrees(100.2);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x306)]
        public virtual Date getMisheyakir11Degrees()
        {
            return this.getSunriseOffsetByDegrees(101.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x2f7)]
        public virtual Date getMisheyakir11Point5Degrees()
        {
            return this.getSunriseOffsetByDegrees(101.5);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa5, 0xd7, 0x93 })]
        public virtual Date getPlagAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            long num = this.getTemporalHour(this.getAlos16Point1Degrees(), this.getTzaisGeonim7Point083Degrees());
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double) (num * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa5, 0xbf, 0x93 })]
        public virtual Date getPlagAlosToSunset()
        {
            long num = this.getTemporalHour(this.getAlos16Point1Degrees(), this.getSeaLevelSunset());
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double) (num * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x214)]
        public virtual Date getPlagHamincha120Minutes()
        {
            return this.getTimeOffset(this.getAlos120(), (double) (this.getShaahZmanis120Minutes() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x207)]
        public virtual Date getPlagHamincha120MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos120Zmanis(), (double) (this.getShaahZmanis120MinutesZmanis() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x5e8)]
        public virtual Date getPlagHamincha16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (double) (this.getShaahZmanis16Point1Degrees() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x61b)]
        public virtual Date getPlagHamincha18Degrees()
        {
            return this.getTimeOffset(this.getAlos18Degrees(), (double) (this.getShaahZmanis18Degrees() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x5f9)]
        public virtual Date getPlagHamincha19Point8Degrees()
        {
            return this.getTimeOffset(this.getAlos19Point8Degrees(), (double) (this.getShaahZmanis19Point8Degrees() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x60a)]
        public virtual Date getPlagHamincha26Degrees()
        {
            return this.getTimeOffset(this.getAlos26Degrees(), (double) (this.getShaahZmanis26Degrees() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x58a)]
        public virtual Date getPlagHamincha60Minutes()
        {
            return this.getTimeOffset(this.getAlos60(), (double) (this.getShaahZmanis60Minutes() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x596)]
        public virtual Date getPlagHamincha72Minutes()
        {
            return this.getTimeOffset(this.getAlos72(), (double) (this.getShaahZmanis72Minutes() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x5d7)]
        public virtual Date getPlagHamincha72MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (double) (this.getShaahZmanis72MinutesZmanis() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x5a2)]
        public virtual Date getPlagHamincha90Minutes()
        {
            return this.getTimeOffset(this.getAlos90(), (double) (this.getShaahZmanis90Minutes() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x5c9)]
        public virtual Date getPlagHamincha90MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos90Zmanis(), (double) (this.getShaahZmanis90MinutesZmanis() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x5ae)]
        public virtual Date getPlagHamincha96Minutes()
        {
            return this.getTimeOffset(this.getAlos96(), (double) (this.getShaahZmanis96Minutes() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x5bb)]
        public virtual Date getPlagHamincha96MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos96Zmanis(), (double) (this.getShaahZmanis96MinutesZmanis() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x77a)]
        public virtual Date getPlagHaminchaAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (double) (this.getShaahZmanisAteretTorah() * 10.75));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x1e5)]
        public virtual long getShaahZmanis120Minutes()
        {
            return this.getTemporalHour(this.getAlos120(), this.getTzais120());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x1fa)]
        public virtual long getShaahZmanis120MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos120Zmanis(), this.getTzais120Zmanis());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x13f)]
        public virtual long getShaahZmanis16Point1Degrees()
        {
            return this.getTemporalHour(this.getAlos16Point1Degrees(), this.getTzais16Point1Degrees());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x116)]
        public virtual long getShaahZmanis18Degrees()
        {
            return this.getTemporalHour(this.getAlos18Degrees(), this.getTzais18Degrees());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x105)]
        public virtual long getShaahZmanis19Point8Degrees()
        {
            return this.getTemporalHour(this.getAlos19Point8Degrees(), this.getTzais19Point8Degrees());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x127)]
        public virtual long getShaahZmanis26Degrees()
        {
            return this.getTemporalHour(this.getAlos26Degrees(), this.getTzais26Degrees());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x151)]
        public virtual long getShaahZmanis60Minutes()
        {
            return this.getTemporalHour(this.getAlos60(), this.getTzais60());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x162)]
        public virtual long getShaahZmanis72Minutes()
        {
            return this.getShaahZmanisMGA();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x177)]
        public virtual long getShaahZmanis72MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos72Zmanis(), this.getTzais72Zmanis());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 390)]
        public virtual long getShaahZmanis90Minutes()
        {
            return this.getTemporalHour(this.getAlos90(), this.getTzais90());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x19b)]
        public virtual long getShaahZmanis90MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos90Zmanis(), this.getTzais90Zmanis());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 470)]
        public virtual long getShaahZmanis96Minutes()
        {
            return this.getTemporalHour(this.getAlos96(), this.getTzais96());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x1b0)]
        public virtual long getShaahZmanis96MinutesZmanis()
        {
            return this.getTemporalHour(this.getAlos96Zmanis(), this.getTzais96Zmanis());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x1c7)]
        public virtual long getShaahZmanisAteretTorah()
        {
            return this.getTemporalHour(this.getAlos72Zmanis(), this.getTzaisAteretTorah());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x3d5)]
        public virtual Date getSofZmanShma3HoursBeforeChatzos()
        {
            return this.getTimeOffset(this.getChatzos(), (long) (-10800000L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa3, 0x8d, 0x93 })]
        public virtual Date getSofZmanShmaAlos16Point1ToSunset()
        {
            long num = this.getTemporalHour(this.getAlos16Point1Degrees(), this.getSeaLevelSunset());
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (long) (num * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa3, 0xa7, 0x93 })]
        public virtual Date getSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            long num = this.getTemporalHour(this.getAlos16Point1Degrees(), this.getTzaisGeonim7Point083Degrees());
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (long) (num * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x71a)]
        public virtual Date getSofZmanShmaAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (long) (this.getShaahZmanisAteretTorah() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x850)]
        public virtual Date getSofZmanShmaFixedLocal()
        {
            return this.getTimeOffset(this.getFixedLocalChatzos(), (long) (-10800000L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa3, 0xb9, 0x67, 0x71, 130, 0x76 })]
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

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x3e9)]
        public virtual Date getSofZmanShmaMGA120Minutes()
        {
            return this.getTimeOffset(this.getAlos120(), (long) (this.getShaahZmanis120Minutes() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 830)]
        public virtual Date getSofZmanShmaMGA16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (long) (this.getShaahZmanis16Point1Degrees() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x329)]
        public virtual Date getSofZmanShmaMGA19Point8Degrees()
        {
            return this.getTimeOffset(this.getAlos19Point8Degrees(), (long) (this.getShaahZmanis19Point8Degrees() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x355)]
        public virtual Date getSofZmanShmaMGA72Minutes()
        {
            return this.getSofZmanShmaMGA();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x36d)]
        public virtual Date getSofZmanShmaMGA72MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (long) (this.getShaahZmanis72MinutesZmanis() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x382)]
        public virtual Date getSofZmanShmaMGA90Minutes()
        {
            return this.getTimeOffset(this.getAlos90(), (long) (this.getShaahZmanis90Minutes() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 920)]
        public virtual Date getSofZmanShmaMGA90MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos90Zmanis(), (long) (this.getShaahZmanis90MinutesZmanis() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x3ad)]
        public virtual Date getSofZmanShmaMGA96Minutes()
        {
            return this.getTimeOffset(this.getAlos96(), (long) (this.getShaahZmanis96Minutes() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x3c3)]
        public virtual Date getSofZmanShmaMGA96MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos96Zmanis(), (long) (this.getShaahZmanis96MinutesZmanis() * 3L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x503)]
        public virtual Date getSofZmanTfila2HoursBeforeChatzos()
        {
            return this.getTimeOffset(this.getChatzos(), (long) (-7200000L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x85d)]
        public virtual Date getSofZmanTfilaFixedLocal()
        {
            return this.getTimeOffset(this.getFixedLocalChatzos(), (long) (-7200000L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x732)]
        public virtual Date getSofZmanTfilahAteretTorah()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (long) (this.getShaahZmanisAteretTorah() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x4f3)]
        public virtual Date getSofZmanTfilaMGA120Minutes()
        {
            return this.getTimeOffset(this.getAlos120(), (long) (this.getShaahZmanis120Minutes() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x45b)]
        public virtual Date getSofZmanTfilaMGA16Point1Degrees()
        {
            return this.getTimeOffset(this.getAlos16Point1Degrees(), (long) (this.getShaahZmanis16Point1Degrees() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x445)]
        public virtual Date getSofZmanTfilaMGA19Point8Degrees()
        {
            return this.getTimeOffset(this.getAlos19Point8Degrees(), (long) (this.getShaahZmanis19Point8Degrees() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x472)]
        public virtual Date getSofZmanTfilaMGA72Minutes()
        {
            return this.getSofZmanTfilaMGA();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x488)]
        public virtual Date getSofZmanTfilaMGA72MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos72Zmanis(), (long) (this.getShaahZmanis72MinutesZmanis() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x49d)]
        public virtual Date getSofZmanTfilaMGA90Minutes()
        {
            return this.getTimeOffset(this.getAlos90(), (long) (this.getShaahZmanis90Minutes() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x4b3)]
        public virtual Date getSofZmanTfilaMGA90MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos90Zmanis(), (long) (this.getShaahZmanis90MinutesZmanis() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x4c8)]
        public virtual Date getSofZmanTfilaMGA96Minutes()
        {
            return this.getTimeOffset(this.getAlos96(), (long) (this.getShaahZmanis96Minutes() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x4de)]
        public virtual Date getSofZmanTfilaMGA96MinutesZmanis()
        {
            return this.getTimeOffset(this.getAlos96Zmanis(), (long) (this.getShaahZmanis96MinutesZmanis() * 4L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x7e2)]
        public virtual Date getTzais120()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long) 0x6ddd00L);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa7, 0x7c, 0x67, 0x6f, 130 })]
        public virtual Date getTzais120Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunset(), (double) (num * 2.0));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x7fe)]
        public virtual Date getTzais16Point1Degrees()
        {
            return this.getSunsetOffsetByDegrees(106.1);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x815)]
        public virtual Date getTzais18Degrees()
        {
            return this.getSunsetOffsetByDegrees(108.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x821)]
        public virtual Date getTzais19Point8Degrees()
        {
            return this.getSunsetOffsetByDegrees(109.8);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x80a)]
        public virtual Date getTzais26Degrees()
        {
            return this.getSunsetOffsetByDegrees(116.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x6d0)]
        public virtual Date getTzais60()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long) 0x36ee80L);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa7, 0x29, 0x67, 0x6f, 130 })]
        public virtual Date getTzais72Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunset(), (double) (num * 1.2));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x7d1)]
        public virtual Date getTzais90()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long) 0x5265c0L);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa7, 0x39, 0x67, 0x6f, 130 })]
        public virtual Date getTzais90Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunset(), (double) (num * 1.5));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x82d)]
        public virtual Date getTzais96()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long) 0x57e400L);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa7, 0x49, 0x67, 0x6f, 130 })]
        public virtual Date getTzais96Zmanis()
        {
            long num = this.getShaahZmanisGra();
            if (num == -9223372036854775808L)
            {
                return null;
            }
            return this.getTimeOffset(this.getSeaLevelSunset(), (double) (num * 1.6));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x6e3)]
        public virtual Date getTzaisAteretTorah()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (double) (this.getAteretTorahSunsetOffset() * 60000.0));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x6a8)]
        public virtual Date getTzaisGeonim5Point95Degrees()
        {
            return this.getSunsetOffsetByDegrees(95.95);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x6b5)]
        public virtual Date getTzaisGeonim7Point083Degrees()
        {
            return this.getSunsetOffsetByDegrees(97.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x6c2)]
        public virtual Date getTzaisGeonim8Point5Degrees()
        {
            return this.getSunsetOffsetByDegrees(98.5);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa8, 4, 0x63, 0xb1, 0x73, 0x73, 0x73 })]
        public override int hashCode()
        {
            int num = 0x11;
            num = (0x25 * num) + java.lang.Object.instancehelper_hashCode(base.getClass());
            num += (0x25 * num) + this.getCalendar().hashCode();
            num += (0x25 * num) + this.getGeoLocation().hashCode();
            return (num + ((0x25 * num) + java.lang.Object.instancehelper_hashCode(this.getAstronomicalCalculator())));
        }

        public virtual void setAteretTorahSunsetOffset(double ateretTorahSunsetOffset)
        {
            this.ateretTorahSunsetOffset = ateretTorahSunsetOffset;
        }
    }
}

