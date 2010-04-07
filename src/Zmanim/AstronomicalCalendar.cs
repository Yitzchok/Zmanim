namespace net.sourceforge.zmanim
{
    using java.math;
    using java.util;
    using net.sourceforge.zmanim.util;
    using System;

    /// <summary>
    /// A Java calendar that calculates astronomical time calculations such as
    /// {@link #getSunrise() sunrise} and {@link #getSunset() sunset} times. This
    /// class contains a Calendar <see cref="getCalendar" /> and can therefore use the
    /// standard Calendar functionality to change dates etc. The calculation engine
    /// used to calculate the astronomical times can be changed to a different
    /// implementation by implementing the <see cref="AstronomicalCalculator"/> and setting
    /// it with the {@link #setAstronomicalCalculator(AstronomicalCalculator)}. A
    /// number of different implementations are included in the util package <br />
    /// <b>Note:</b> There are times when the algorithms can't calculate proper
    /// values for sunrise and sunset. This is usually caused by trying to calculate
    /// times for areas either very far North or South, where sunrise / sunset never
    /// happen on that date. This is common when calculating twilight with a deep dip
    /// below the horizon for locations as south of the North Pole as London in the
    /// northern hemisphere. When the calculations encounter this condition a null
    /// will be returned when a <code>{@link java.util.Date}</code> is expected and
    /// {@link Double#NaN} when a double is expected. The reason that
    /// <code>Exception</code>s are not thrown in these cases is because the lack
    /// of a rise/set are not exceptions, but expected in many parts of the world.
    /// 
    /// Here is a simple example of how to use the API to calculate sunrise: <br />
    /// First create the Calendar for the location you would like to calculate:
    /// <example>
    /// String locationName = &quot;Lakewood, NJ&quot;;
    /// double latitude = 40.0828; //Lakewood, NJ
    /// double longitude = -74.2094; //Lakewood, NJ
    /// double elevation = 20; // optional elevation correction in Meters
    /// //the String parameter in getTimeZone() has to be a valid timezone listed in {@link java.util.TimeZone#getAvailableIDs()}
    /// TimeZone timeZone = TimeZone.getTimeZone(&quot;America/New_York&quot;);
    /// GeoLocation location = new GeoLocation(locationName, latitude, longitude,
    /// 		elevation, timeZone);
    /// AstronomicalCalendar ac = new AstronomicalCalendar(location);
    /// </example>
    /// To get the time of sunrise, first set the date (if not set, the date will
    /// default to today):
    /// <example>
    /// ac.getCalendar().set(Calendar.MONTH, Calendar.FEBRUARY);
    /// ac.getCalendar().set(Calendar.DAY_OF_MONTH, 8);
    /// Date sunrise = ac.getSunrise();
    /// </example>
    /// @author &copy; Eliyahu Hershfeld 2004 - 2010
    /// @version 1.2
    /// </summary>
    public class AstronomicalCalendar : ICloneable
    {
        private const long serialVersionUID = 1;

        ///<summary>
        /// 90&deg; below the vertical. Used for certain calculations.<br />
        /// <b>Note </b>: it is important to note the distinction between this zenith
        /// and the <see cref="adjustZenith"/> used
        /// for some solar calculations. This 90 zenith is only used because some
        /// calculations in some subclasses are historically calculated as an offset
        /// in reference to 90.
        /// </summary>
        public const double GEOMETRIC_ZENITH = 90.0;

        /// <summary>
        /// Sun's zenith at astronomical twilight (108&deg;).
        /// </summary>
        public const double ASTRONOMICAL_ZENITH = 108.0;


        /// <summary>
        /// Default value for Sun's zenith and true rise/set Zenith (used in this
        /// class and subclasses) is the angle that the center of the Sun makes to a
        /// line perpendicular to the Earth's surface. If the Sun were a point and
        /// the Earth were without an atmosphere, true sunset and sunrise would
        /// correspond to a 90&deg; zenith. Because the Sun is not a point, and
        /// because the atmosphere refracts light, this 90&deg; zenith does not, in
        /// fact, correspond to true sunset or sunrise, instead the center of the
        /// Sun's disk must lie just below the horizon for the upper edge to be
        /// obscured. This means that a zenith of just above 90&deg; must be used.
        /// The Sun subtends an angle of 16 minutes of arc (this can be changed via
        /// the {@link #setSunRadius(double)} method , and atmospheric refraction
        /// accounts for 34 minutes or so (this can be changed via the
        /// {@link #setRefraction(double)} method), giving a total of 50 arcminutes.
        /// The total value for ZENITH is 90+(5/6) or 90.8333333&deg; for true
        /// sunrise/sunset.
        /// </summary>
        //public static double ZENITH = GEOMETRIC_ZENITH + 5.0 / 6.0;

        /// <summary>
        /// Sun's zenith at civil twilight (96&deg;).
        /// </summary>
        public const double CIVIL_ZENITH = 96.0;


        /// <summary>
        /// constant for milliseconds in an hour (3,600,000)
        /// </summary>
        internal const long HOUR_MILLIS = 0x36ee80L;

        /// <summary>
        /// constant for milliseconds in a minute (60,000)
        /// </summary>
        internal const long MINUTE_MILLIS = 0xea60L;

        /// <summary>
        /// Sun's zenith at nautical twilight (102&deg;).
        /// </summary>
        public const double NAUTICAL_ZENITH = 102.0;

        /// <summary>
        /// The Java Calendar encapsulated by this class to track the current date
        /// used by the class
        /// </summary>
        private AstronomicalCalculator astronomicalCalculator;
        private Calendar calendar;
        private GeoLocation geoLocation;
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

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is AstronomicalCalendar))
            {
                return false;
            }
            AstronomicalCalendar calendar = (AstronomicalCalendar)obj;
            return ((this.getCalendar().equals(calendar.getCalendar()) && this.getGeoLocation().Equals(calendar.getGeoLocation())) && java.lang.Object.instancehelper_equals(this.getAstronomicalCalculator(), calendar.getAstronomicalCalculator()));
        }

        /// <summary>
        /// The getSunrise method Returns a <code>Date</code> representing the
        /// sunrise time. The zenith used for the calculation uses
        /// {@link #GEOMETRIC_ZENITH geometric zenith} of 90&deg;. This is adjusted
        /// by the {@link AstronomicalCalculator} that adds approximately 50/60 of a
        /// degree to account for 34 archminutes of refraction and 16 archminutes for
        /// the sun's radius for a total of
        /// {@link AstronomicalCalculator#adjustZenith 90.83333&deg;}. See
        /// documentation for the specific implementation of the
        /// {@link AstronomicalCalculator} that you are using.
        /// <see cref="adjustZenith"/>
        /// </summary>
        /// <returns>
        /// the <code>Date</code> representing the exact sunrise time. If
        /// the calculation can not be computed null will be returned.</returns>
        public virtual Date getSunrise()
        {
            double v = this.getUTCSunrise(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

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

        public virtual Date getBeginAstronomicalTwilight()
        {
            return this.getSunriseOffsetByDegrees(108.0);
        }

        public virtual Date getBeginCivilTwilight()
        {
            return this.getSunriseOffsetByDegrees(96.0);
        }

        public virtual Date getBeginNauticalTwilight()
        {
            return this.getSunriseOffsetByDegrees(102.0);
        }

        public virtual Calendar getCalendar()
        {
            return this.calendar;
        }

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
            int num = (int)time;
            time -= num;
            double d = time * 60.0;
            time = d;
            int num2 = (int)d;
            time -= num2;
            double num4 = time * 60.0;
            time = num4;
            int num3 = (int)num4;
            time -= num3;
            calendar.set(11, num);
            calendar.set(12, num2);
            calendar.set(13, num3);
            calendar.set(14, (int)(time * 1000.0));
            return calendar.getTime();
        }

        public virtual Date getEndAstronomicalTwilight()
        {
            return this.getSunsetOffsetByDegrees(108.0);
        }

        public virtual Date getEndCivilTwilight()
        {
            return this.getSunsetOffsetByDegrees(96.0);
        }

        public virtual Date getEndNauticalTwilight()
        {
            return this.getSunsetOffsetByDegrees(102.0);
        }

        public virtual GeoLocation getGeoLocation()
        {
            return this.geoLocation;
        }

        private double getOffsetTime(double num1)
        {
            bool isInDaylightTime = this.getCalendar().getTimeZone().inDaylightTime(this.getCalendar().getTime());
            double num2 = 0f;
            double num3 = ((long)this.getCalendar().getTimeZone().getRawOffset()) / 0x36ee80L;
            if (isInDaylightTime)
            {
                num2 = ((long)this.getCalendar().getTimeZone().getDSTSavings()) / 0x36ee80L;
            }
            return ((num1 + num3) + num2);
        }

        public virtual Date getSeaLevelSunrise()
        {
            double v = this.getUTCSeaLevelSunrise(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

        public virtual Date getSeaLevelSunset()
        {
            double v = this.getUTCSeaLevelSunset(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSeaLevelSunrise());
        }

        public virtual Date getSunriseOffsetByDegrees(double offsetZenith)
        {
            double v = this.getUTCSunrise(offsetZenith);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getDateFromTime(v);
        }

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

        public virtual Date getSunset()
        {
            double v = this.getUTCSunset(90.0);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSunrise());
        }

        public virtual Date getSunsetOffsetByDegrees(double offsetZenith)
        {
            double v = this.getUTCSunset(offsetZenith);
            if (java.lang.Double.isNaN(v))
            {
                return null;
            }
            return this.getAdjustedSunsetDate(this.getDateFromTime(v), this.getSunriseOffsetByDegrees(offsetZenith));
        }

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

        public virtual Date getSunTransit()
        {
            return this.getTimeOffset(this.getSunrise(), (long)(this.getTemporalHour() * 6L));
        }

        public virtual long getTemporalHour()
        {
            return this.getTemporalHour(this.getSunrise(), this.getSunset());
        }

        public virtual long getTemporalHour(Date sunrise, Date sunset)
        {
            if ((sunrise != null) && (sunset != null))
            {
                return (long)(((ulong)(sunset.getTime() - sunrise.getTime())) / 12L);
            }
            return -9223372036854775808L;
        }

        public virtual Date getTimeOffset(Date time, double offset)
        {
            return this.getTimeOffset(time, (long)offset);
        }

        public virtual Date getTimeOffset(Date time, long offset)
        {
            if ((time == null) || (offset == -9223372036854775808L))
            {
                return null;
            }
            return new Date(time.getTime() + offset);
        }

        public virtual double getUTCSeaLevelSunrise(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunrise(this, zenith, false);
        }

        public virtual double getUTCSeaLevelSunset(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunset(this, zenith, false);
        }

        public virtual double getUTCSunrise(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunrise(this, zenith, true);
        }

        public virtual double getUTCSunset(double zenith)
        {
            return this.getAstronomicalCalculator().getUTCSunset(this, zenith, true);
        }

        public override int GetHashCode()
        {
            int num = 0x11;
            num = (0x25 * num) + java.lang.Object.instancehelper_hashCode(base.GetType());
            num += (0x25 * num) + this.getCalendar().hashCode();
            num += (0x25 * num) + this.getGeoLocation().GetHashCode();
            return (num + ((0x25 * num) + java.lang.Object.instancehelper_hashCode(this.getAstronomicalCalculator())));
        }

        public virtual void setAstronomicalCalculator(AstronomicalCalculator astronomicalCalculator)
        {
            this.astronomicalCalculator = astronomicalCalculator;
        }

        public virtual void setCalendar(Calendar calendar)
        {
            this.calendar = calendar;
            if (this.getGeoLocation() != null)
            {
                this.getCalendar().setTimeZone(this.getGeoLocation().getTimeZone());
            }
        }

        public virtual void setGeoLocation(GeoLocation geoLocation)
        {
            this.geoLocation = geoLocation;
            this.getCalendar().setTimeZone(geoLocation.getTimeZone());
        }

        public object Clone()
        {
            /*
             * 
             * 
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
             * 
             * */
            return this.MemberwiseClone();
        }
    }
}

