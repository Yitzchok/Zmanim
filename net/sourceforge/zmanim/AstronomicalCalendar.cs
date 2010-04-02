namespace net.sourceforge.zmanim
{
    using IKVM.Attributes;
    using IKVM.Runtime;
    using java.lang;
    using java.math;
    using java.util;
    using net.sourceforge.zmanim.util;
    using System;
    using System.Runtime.CompilerServices;

    public class AstronomicalCalendar : ICloneable
    {
        public const double ASTRONOMICAL_ZENITH = 108.0;
        private AstronomicalCalculator astronomicalCalculator;
        private Calendar calendar;
        public const double CIVIL_ZENITH = 96.0;
        private GeoLocation geoLocation;
        public const double GEOMETRIC_ZENITH = 90.0;
        internal const long HOUR_MILLIS = 0x36ee80L;
        internal const long MINUTE_MILLIS = 0xea60L;
        public const double NAUTICAL_ZENITH = 102.0;
        private const long serialVersionUID = 1L;

        public AstronomicalCalendar()
            : this(new GeoLocation())
        {
        }

        public AstronomicalCalendar(GeoLocation geoLocation)
        {
            this.setCalendar(Calendar.getInstance(geoLocation.getTimeZone()));
            this.setGeoLocation(geoLocation);
            this.setAstronomicalCalculator(AstronomicalCalculator.getDefault());
        }

        public override object clone()
        {
            AstronomicalCalendar calendar = null;
            try
            {
                calendar = (AstronomicalCalendar)base.clone();
            }
            catch (CloneNotSupportedException)
            {
                System.@out.print("Required by the compiler. Should never be reached since we implement clone()");
            }
            calendar.setGeoLocation((GeoLocation)this.getGeoLocation().clone());
            calendar.setCalendar((Calendar)this.getCalendar().clone());
            calendar.setAstronomicalCalculator((AstronomicalCalculator)this.getAstronomicalCalculator().clone());
            return calendar;
        }

        public override bool equals(object @object)
        {
            if (this == @object)
            {
                return true;
            }
            if (!(@object is AstronomicalCalendar))
            {
                return false;
            }
            AstronomicalCalendar calendar = (AstronomicalCalendar)@object;
            return ((this.getCalendar().equals(calendar.getCalendar()) && this.getGeoLocation().equals(calendar.getGeoLocation())) && java.lang.Object.instancehelper_equals(this.getAstronomicalCalculator(), calendar.getAstronomicalCalculator()));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x92, 0x79, 0x71, 0x67, 0x68, 0x89 })]
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

        public virtual AstronomicalCalculator getAstronomicalCalculator()
        {
            return this.astronomicalCalculator;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0xd6)]
        public virtual Date getBeginAstronomicalTwilight()
        {
            return this.getSunriseOffsetByDegrees(108.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0xbc)]
        public virtual Date getBeginCivilTwilight()
        {
            return this.getSunriseOffsetByDegrees(96.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0xc9)]
        public virtual Date getBeginNauticalTwilight()
        {
            return this.getSunriseOffsetByDegrees(102.0);
        }

        public virtual Calendar getCalendar()
        {
            return this.calendar;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 
            0xa1, 0xd6, 0x6c, 130, 0x6b, 0x99, 0x66, 0x66, 0x73, 0x73, 0xd3, 0x88, 0x68, 0x76, 0x68, 0x76, 
            0x88, 0x69, 0x69, 0x69, 0x79
         })]
        protected internal virtual Date getDateFromTime(double time)
        {
            if (java.lang.Double.isNaN(time))
            {
                return null;
            }
            time = this.getOffsetTime(time);
            time = (time + 240.0) % 24.0;
            GregorianCalendar calendar = new GregorianCalendar();
            calendar.clear();
            calendar.set(1, this.getCalendar().get(1));
            calendar.set(2, this.getCalendar().get(2));
            calendar.set(5, this.getCalendar().get(5));
            int num = ByteCodeHelper.d2i(time);
            time -= num;
            double d = time * 60.0;
            time = d;
            int num2 = ByteCodeHelper.d2i(d);
            time -= num2;
            double num4 = time * 60.0;
            time = num4;
            int num3 = ByteCodeHelper.d2i(num4);
            time -= num3;
            calendar.set(11, num);
            calendar.set(12, num2);
            calendar.set(13, num3);
            calendar.set(14, ByteCodeHelper.d2i(time * 1000.0));
            return calendar.getTime();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)330)]
        public virtual Date getEndAstronomicalTwilight()
        {
            return this.getSunsetOffsetByDegrees(108.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x130)]
        public virtual Date getEndCivilTwilight()
        {
            return this.getSunsetOffsetByDegrees(96.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x13d)]
        public virtual Date getEndNauticalTwilight()
        {
            return this.getSunsetOffsetByDegrees(102.0);
        }

        public virtual GeoLocation getGeoLocation()
        {
            return this.geoLocation;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0x90, 0x9c, 0x86, 0x9a, 0x66, 0xba })]
        private double getOffsetTime(double num1)
        {
            int num = (int)this.getCalendar().getTimeZone().inDaylightTime(this.getCalendar().getTime());
            double num2 = 0f;
            double num3 = ((long)this.getCalendar().getTimeZone().getRawOffset()) / 0x36ee80L;
            if (num != 0)
            {
                num2 = ((long)this.getCalendar().getTimeZone().getDSTSavings()) / 0x36ee80L;
            }
            return ((num1 + num3) + num2);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 120, 0x71, 0x6b, 130 })]
        public virtual Date getSeaLevelSunrise()
        {
            double v = this.getUTCSeaLevelSunrise(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0xab, 0x71, 0x6b, 130 })]
        public virtual Date getSeaLevelSunset()
        {
            double v = this.getUTCSeaLevelSunset(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSeaLevelSunrise());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x61, 0x71, 0x6b, 130 })]
        public virtual Date getSunrise()
        {
            double v = this.getUTCSunrise(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 7, 0x6a, 0x6b, 130 })]
        public virtual Date getSunriseOffsetByDegrees(double offsetZenith)
        {
            double v = this.getUTCSunrise(offsetZenith);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa2, 0, 0x67, 0x9a, 0x67, 0x8b, 0x77, 0x68, 0x99 })]
        public virtual double getSunriseSolarDipFromOffset(double minutes)
        {
            Date date = this.getSeaLevelSunrise();
            Date date2 = this.getTimeOffset(this.getSeaLevelSunrise(), -(minutes * 60000.0));
            BigDecimal num = new BigDecimal(0);
            BigDecimal augend = new BigDecimal("0.0001");
            while ((date == null) || (date.getTime() > date2.getTime()))
            {
                num = num.add(augend);
                date = this.getSunriseOffsetByDegrees(90.0 + num.doubleValue());
            }
            return num.doubleValue();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x7c, 0x71, 0x6b, 130 })]
        public virtual Date getSunset()
        {
            double v = this.getUTCSunset(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSunrise());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0x1b, 0x6a, 0x6b, 130 })]
        public virtual Date getSunsetOffsetByDegrees(double offsetZenith)
        {
            double v = this.getUTCSunset(offsetZenith);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSunriseOffsetByDegrees(offsetZenith));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa2, 0x1a, 0x67, 0x99, 0x67, 0x8b, 0x77, 0x68, 0x99 })]
        public virtual double getSunsetSolarDipFromOffset(double minutes)
        {
            Date date = this.getSeaLevelSunset();
            Date date2 = this.getTimeOffset(this.getSeaLevelSunset(), (double)(minutes * 60000.0));
            BigDecimal num = new BigDecimal(0);
            BigDecimal augend = new BigDecimal("0.0001");
            while ((date == null) || (date.getTime() < date2.getTime()))
            {
                num = num.add(augend);
                date = this.getSunsetOffsetByDegrees(90.0 + num.doubleValue());
            }
            return num.doubleValue();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x23c)]
        public virtual Date getSunTransit()
        {
            return this.getTimeOffset(this.getSunrise(), (long)(this.getTemporalHour() * 6L));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x218)]
        public virtual long getTemporalHour()
        {
            return this.getTemporalHour(this.getSunrise(), this.getSunset());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0xb7, 0x6c, 0x8a })]
        public virtual long getTemporalHour(Date sunrise, Date sunset)
        {
            if ((sunrise != null) && (sunset != null))
            {
                return (long)(((ulong)(sunset.getTime() - sunrise.getTime())) / 12L);
            }
            return -9223372036854775808L;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x159)]
        public virtual Date getTimeOffset(Date time, double offset)
        {
            return this.getTimeOffset(time, ByteCodeHelper.d2l(offset));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0xf5, 0x75, 130 })]
        public virtual Date getTimeOffset(Date time, long offset)
        {
            if ((time == null) || (offset == -9223372036854775808L))
            {
                return null;
            }
            Date.__<clinit>();
            return new Date(time.getTime() + offset);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x1cf)]
        public virtual double getUTCSeaLevelSunrise(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunrise(this, zenith, false);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x1f5)]
        public virtual double getUTCSeaLevelSunset(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunset(this, zenith, false);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x1b9)]
        public virtual double getUTCSunrise(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunrise(this, zenith, true);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x1df)]
        public virtual double getUTCSunset(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunset(this, zenith, true);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa2, 70, 0x63, 0x71, 0x73, 0x73, 0x73 })]
        public override int hashCode()
        {
            int num = 0x11;
            num = (0x25 * num) + java.lang.Object.instancehelper_hashCode(base.getClass());
            num += (0x25 * num) + this.getCalendar().hashCode();
            num += (0x25 * num) + this.getGeoLocation().hashCode();
            return (num + ((0x25 * num) + java.lang.Object.instancehelper_hashCode(this.getAstronomicalCalculator())));
        }

        [HideFromJava]
        public static implicit operator Cloneable(AstronomicalCalendar calendar1)
        {
            Cloneable cloneable;
            cloneable.__<ref> = calendar1;
            return cloneable;
        }

        public virtual void setAstronomicalCalculator(AstronomicalCalculator astronomicalCalculator)
        {
            this.astronomicalCalculator = astronomicalCalculator;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa2, 0x91, 0x67, 0xab, 150 })]
        public virtual void setCalendar(Calendar calendar)
        {
            this.calendar = calendar;
            if (this.getGeoLocation() != null)
            {
                this.getCalendar().setTimeZone(this.getGeoLocation().getTimeZone());
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa2, 0x5f, 0xa7, 0x71 })]
        public virtual void setGeoLocation(GeoLocation geoLocation)
        {
            this.geoLocation = geoLocation;
            this.getCalendar().setTimeZone(geoLocation.getTimeZone());
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort)0x2a2)]
        public override string toString()
        {
            return ZmanimFormatter.toXML(this);
        }
    }
}

