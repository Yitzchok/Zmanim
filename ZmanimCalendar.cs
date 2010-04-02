namespace net.sourceforge.zmanim
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using net.sourceforge.zmanim.util;
    using System;
    using System.Runtime.CompilerServices;

    public class ZmanimCalendar : AstronomicalCalendar
    {
        private double candleLightingOffset;
        private const long serialVersionUID = 1L;
        protected internal const double ZENITH_16_POINT_1 = 106.1;
        protected internal const double ZENITH_8_POINT_5 = 98.5;

        public ZmanimCalendar()
        {
            this.candleLightingOffset = 18.0;
        }

        public ZmanimCalendar(GeoLocation location) : base(location)
        {
            this.candleLightingOffset = 18.0;
        }

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
            ZmanimCalendar calendar = (ZmanimCalendar) obj;
            return ((this.getCalendar().equals(calendar.getCalendar()) && this.getGeoLocation().equals(calendar.getGeoLocation())) && java.lang.Object.instancehelper_equals(this.getAstronomicalCalculator(), calendar.getAstronomicalCalculator()));
        }

        public virtual Date getAlos72()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long) (-4320000L));
        }

        public virtual Date getAlosHashachar()
        {
            return this.getSunriseOffsetByDegrees(106.1);
        }

        public virtual Date getCandelLighting()
        {
            return this.getTimeOffset(this.getSunset(), (double) (-this.getCandleLightingOffset() * 60000.0));
        }

        public virtual double getCandleLightingOffset()
        {
            return this.candleLightingOffset;
        }

        public virtual Date getChatzos()
        {
            return this.getSunTransit();
        }

        public virtual Date getMinchaGedola()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (double) (this.getShaahZmanisGra() * 6.5));
        }

        public virtual Date getMinchaKetana()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (double) (this.getShaahZmanisGra() * 9.5));
        }

        public virtual Date getPlagHamincha()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (double) (this.getShaahZmanisGra() * 10.75));
        }

        public virtual long getShaahZmanisGra()
        {
            return this.getTemporalHour(this.getSeaLevelSunrise(), this.getSeaLevelSunset());
        }

        public virtual long getShaahZmanisMGA()
        {
            return this.getTemporalHour(this.getAlos72(), this.getTzais72());
        }

        public virtual Date getSofZmanShmaGRA()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long) (this.getShaahZmanisGra() * 3L));
        }

        public virtual Date getSofZmanShmaMGA()
        {
            return this.getTimeOffset(this.getAlos72(), (long) (this.getShaahZmanisMGA() * 3L));
        }

        public virtual Date getSofZmanTfilaGRA()
        {
            return this.getTimeOffset(this.getSeaLevelSunrise(), (long) (this.getShaahZmanisGra() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA()
        {
            return this.getTimeOffset(this.getAlos72(), (long) (this.getShaahZmanisMGA() * 4L));
        }

       public virtual Date getSolarMidnight()
        {
            ZmanimCalendar calendar = (ZmanimCalendar) this.clone();
            calendar.getCalendar().add(5, 1);
            Date time = this.getSunset();
            Date sunset = calendar.getSunrise();
            return this.getTimeOffset(time, (long) (this.getTemporalHour(time, sunset) * 6L));
        }

        public virtual Date getTzais()
        {
            return this.getSunsetOffsetByDegrees(98.5);
        }

        public virtual Date getTzais72()
        {
            return this.getTimeOffset(this.getSeaLevelSunset(), (long) 0x41eb00L);
        }

        public override int GetHashCode()
        {
            int num = 0x11;
            num = (0x25 * num) + java.lang.Object.instancehelper_hashCode(base.GetType());
            num += (0x25 * num) + this.getCalendar().hashCode();
            num += (0x25 * num) + this.getGeoLocation().GetHashCode();
            return (num + ((0x25 * num) + java.lang.Object.instancehelper_hashCode(this.getAstronomicalCalculator())));
        }

        public virtual void setCandleLightingOffset(double candleLightingOffset)
        {
            this.candleLightingOffset = candleLightingOffset;
        }
    }
}

