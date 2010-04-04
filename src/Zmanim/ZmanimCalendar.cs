using java.lang;

namespace net.sourceforge.zmanim
{
    using java.util;
    using util;

    public class ZmanimCalendar : AstronomicalCalendar
    {
        private double candleLightingOffset;
        private const long serialVersionUID = 1L;
        protected internal const double ZENITH_16_POINT_1 = 106.1;
        protected internal const double ZENITH_8_POINT_5 = 98.5;

        public ZmanimCalendar()
        {
            candleLightingOffset = 18.0;
        }

        public ZmanimCalendar(GeoLocation location)
            : base(location)
        {
            candleLightingOffset = 18.0;
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
            ZmanimCalendar calendar = (ZmanimCalendar)obj;
            return ((getCalendar().equals(calendar.getCalendar()) && getGeoLocation().Equals(calendar.getGeoLocation())) &&
                    Object.instancehelper_equals(getAstronomicalCalculator(), calendar.getAstronomicalCalculator()));
        }

        public virtual Date getAlos72()
        {
            return getTimeOffset(getSeaLevelSunrise(), (-4320000L));
        }

        public virtual Date getAlosHashachar()
        {
            return getSunriseOffsetByDegrees(106.1);
        }

        public virtual Date getCandelLighting()
        {
            var candleLightingOffset = -getCandleLightingOffset() * 60000.0;
            return getTimeOffset(getSunset(), candleLightingOffset);
        }

        public virtual double getCandleLightingOffset()
        {
            return candleLightingOffset;
        }

        public virtual Date getChatzos()
        {
            return getSunTransit();
        }

        public virtual Date getMinchaGedola()
        {
            return getTimeOffset(getSeaLevelSunrise(), (getShaahZmanisGra() * 6.5));
        }

        public virtual Date getMinchaKetana()
        {
            return getTimeOffset(getSeaLevelSunrise(), (getShaahZmanisGra() * 9.5));
        }

        public virtual Date getPlagHamincha()
        {
            return getTimeOffset(getSeaLevelSunrise(), (getShaahZmanisGra() * 10.75));
        }

        public virtual long getShaahZmanisGra()
        {
            return getTemporalHour(getSeaLevelSunrise(), getSeaLevelSunset());
        }

        public virtual long getShaahZmanisMGA()
        {
            return getTemporalHour(getAlos72(), getTzais72());
        }

        public virtual Date getSofZmanShmaGRA()
        {
            return getTimeOffset(getSeaLevelSunrise(), (getShaahZmanisGra() * 3L));
        }

        public virtual Date getSofZmanShmaMGA()
        {
            return getTimeOffset(getAlos72(), (getShaahZmanisMGA() * 3L));
        }

        public virtual Date getSofZmanTfilaGRA()
        {
            return getTimeOffset(getSeaLevelSunrise(), (getShaahZmanisGra() * 4L));
        }

        public virtual Date getSofZmanTfilaMGA()
        {
            return getTimeOffset(getAlos72(), getShaahZmanisMGA() * 4L);
        }

        public virtual Date getSolarMidnight()
        {
            ZmanimCalendar clonedCal = (ZmanimCalendar)Clone();
            clonedCal.getCalendar().add(Calendar.DAY_OF_MONTH, 1);
            Date sunsetTime = getSunset();
            Date sunrise = clonedCal.getSunrise();
            return getTimeOffset(sunsetTime, getTemporalHour(sunsetTime, sunrise) * 6);
        }

        public virtual Date getTzais()
        {
            return getSunsetOffsetByDegrees(98.5);
        }

        public virtual Date getTzais72()
        {
            return getTimeOffset(getSeaLevelSunset(), 0x41eb00L);
        }

        public override int GetHashCode()
        {
            int num = 0x11;
            num = (0x25 * num) + Object.instancehelper_hashCode(base.GetType());
            num += (0x25 * num) + getCalendar().hashCode();
            num += (0x25 * num) + getGeoLocation().GetHashCode();
            return (num + ((0x25 * num) + Object.instancehelper_hashCode(getAstronomicalCalculator())));
        }

        public virtual void setCandleLightingOffset(double candleLightingOffset)
        {
            this.candleLightingOffset = candleLightingOffset;
        }
    }
}