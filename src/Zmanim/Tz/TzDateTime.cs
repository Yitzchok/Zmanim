 using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace PublicDomain
{
    /// <summary>
    /// Wraps DateTime to provide time zone information
    /// with an <see cref="PublicDomain.TzTimeZone" /> from
    /// the Olson tz database.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = GlobalConstants.PublicDomainNamespace)]
    [SoapType(Namespace = GlobalConstants.PublicDomainNamespace)]
    public class TzDateTime : IComparable, IFormattable, IComparable<TzDateTime>, IEquatable<TzDateTime>, IConvertible
    {
        /// <summary>
        /// Represents the string: +00:00
        /// </summary>
        public const string UtcOffsetModifier = "+00:00";

        private DateTime m_dateTimeUtc;
        private TzTimeZone m_timeZone;

        /// <summary>
        /// Represents the largest possible value of PublicDomain.TzDateTime. This field is read-only.
        /// </summary>
        public static readonly TzDateTime MaxValue;
        
        /// <summary>
        ///     Represents the smallest possible value of PublicDomain.TzDateTime. This field is
        ///     read-only.
        /// </summary>
        public static readonly TzDateTime MinValue;

        /// <summary>
        /// 
        /// </summary>
        static TzDateTime()
        {
            MinValue = new TzDateTime(DateTime.MinValue.Ticks, TzTimeZone.ZoneUTC);
            MaxValue = new TzDateTime(DateTime.MaxValue.Ticks, TzTimeZone.ZoneUTC);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicDomain.TzDateTime"/> class.
        /// </summary>
        public TzDateTime()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        public TzDateTime(DateTime time)
            : this(time, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="forceTimeAsUtc">if set to <c>true</c> force time as utc.</param>
        public TzDateTime(DateTime time, bool forceTimeAsUtc)
            : this(forceTimeAsUtc ? DateTimeUtlities.CloneDateTimeAsUTC(time) : time)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzDateTime"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="timeZone">The time zone.</param>
        public TzDateTime(DateTime time, TzTimeZone timeZone)
        {
            if (timeZone == null && (time.Kind == DateTimeKind.Local || TzTimeZone.TreatUnspecifiedKindAsLocal && time.Kind == DateTimeKind.Unspecified))
            {
                throw new ArgumentException("A date/time with DateTimeKind Local or Unspecified must be initialized with a time zone. Otherwise, the date/time is ambiguous. You must explicitly provide a time zone for predictable results, for example, you may provide the local time zone of the computer running this program.", "timeZone");
            }
            m_timeZone = timeZone;
            SetDateTime(time);
        }

        /// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to a specified
        ///     number of ticks.
        /// </summary>
        /// <param name="ticks">A date and time expressed in 100-nanosecond units.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">ticks is less than PublicDomain.TzDateTime.MinValue or greater than PublicDomain.TzDateTime.MaxValue.</exception>
        public TzDateTime(long ticks)
            : this(ticks, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to a specified
        /// number of ticks.
        /// </summary>
        /// <param name="ticks">A date and time expressed in 100-nanosecond units.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">ticks is less than PublicDomain.TzDateTime.MinValue or greater than PublicDomain.TzDateTime.MaxValue.</exception>
        public TzDateTime(long ticks, TzTimeZone timeZone)
            : this(new DateTime(ticks), timeZone)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to a specified
        ///     number of ticks and to Coordinated Universal Time (UTC) or local time.
        /// </summary>
        /// <param name="ticks">A date and time expressed in 100-nanosecond units.</param>
        /// <param name="kind">
        ///     One of the PublicDomain.TzDateTimeKind values that indicates whether ticks specifies
        ///     a local time, Coordinated Universal Time (UTC), or neither.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">ticks is less than PublicDomain.TzDateTime.MinValue or greater than PublicDomain.TzDateTime.MaxValue.</exception>
        /// <exception cref="System.ArgumentException">kind is not one of the PublicDomain.TzDateTimeKind values.</exception>
        public TzDateTime(long ticks, DateTimeKind kind)
            : this(ticks, kind, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to a specified
        /// number of ticks and to Coordinated Universal Time (UTC) or local time.
        /// </summary>
        /// <param name="ticks">A date and time expressed in 100-nanosecond units.</param>
        /// <param name="kind">One of the PublicDomain.TzDateTimeKind values that indicates whether ticks specifies
        /// a local time, Coordinated Universal Time (UTC), or neither.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">ticks is less than PublicDomain.TzDateTime.MinValue or greater than PublicDomain.TzDateTime.MaxValue.</exception>
        /// <exception cref="System.ArgumentException">kind is not one of the PublicDomain.TzDateTimeKind values.</exception>
        public TzDateTime(long ticks, DateTimeKind kind, TzTimeZone timeZone)
            : this(new DateTime(ticks, kind), timeZone)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, and day.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        ///     than 12.-or- day is less than 1 or greater than the number of days in month.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     The specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or
        ///     more than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime(int year, int month, int day)
            : this(year, month, day, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, and day.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        /// than 12.-or- day is less than 1 or greater than the number of days in month.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or
        /// more than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime(int year, int month, int day, TzTimeZone timeZone)
            : this(new DateTime(year, month, day), timeZone)
        {
        }

        /*/// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, and day for the specified calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this PublicDomain.TzDateTime.</param>
        /// <exception cref="System.ArgumentException">
        ///     Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is not in the range supported by calendar.-or- month is less than 1
        ///     or greater than the number of months in calendar.-or- day is less than 1
        ///     or greater than the number of days in month.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">calendar is null.</exception>
        public TzDateTime(int year, int month, int day, Calendar calendar)
            : this(year, month, day, calendar, null)
        {
        }*/

        /*/// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, and day for the specified calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this PublicDomain.TzDateTime.</param>
        /// <exception cref="System.ArgumentException">
        /// Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        /// than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is not in the range supported by calendar.-or- month is less than 1
        /// or greater than the number of months in calendar.-or- day is less than 1
        /// or greater than the number of days in month.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">calendar is null.</exception>
        public TzDateTime(int year, int month, int day, Calendar calendar)
            : this(new DateTime(year, month, day, calendar), timeZone)
        {
        }*/

        /// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, day, hour, minute, and second.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <exception cref="System.ArgumentException">
        ///     Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is less than 1 or greater than 9999. -or- month is less than 1 or greater
        ///     than 12. -or- day is less than 1 or greater than the number of days in month.-or-
        ///     hour is less than 0 or greater than 23. -or- minute is less than 0 or greater
        ///     than 59. -or- second is less than 0 or greater than 59.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, day, hour, minute, and second.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentException">
        /// Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        /// than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999. -or- month is less than 1 or greater
        /// than 12. -or- day is less than 1 or greater than the number of days in month.-or-
        /// hour is less than 0 or greater than 23. -or- minute is less than 0 or greater
        /// than 59. -or- second is less than 0 or greater than 59.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, TzTimeZone timeZone)
            : this(new DateTime(year, month, day, hour, minute, second), timeZone)
        {
        }

        /*/// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, day, hour, minute, and second for the specified calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this PublicDomain.TzDateTime.</param>
        /// <exception cref="System.ArgumentException">
        ///     Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">calendar is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is not in the range supported by calendar.-or- month is less than 1
        ///     or greater than the number of months in calendar.-or- day is less than 1
        ///     or greater than the number of days in month.-or- hour is less than 0 or greater
        ///     than 23 -or- minute is less than 0 or greater than 59. -or- second is less
        ///     than 0 or greater than 59.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar)
            : this(year, month, day, hour, minute, second, calendar, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, day, hour, minute, and second for the specified calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this PublicDomain.TzDateTime.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentException">
        /// Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        /// than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">calendar is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is not in the range supported by calendar.-or- month is less than 1
        /// or greater than the number of months in calendar.-or- day is less than 1
        /// or greater than the number of days in month.-or- hour is less than 0 or greater
        /// than 23 -or- minute is less than 0 or greater than 59. -or- second is less
        /// than 0 or greater than 59.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar, TzTimeZone timeZone)
            : this(new DateTime(year, month, day, hour, minute, second, calendar), timeZone)
        {
        }*/

        /// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, day, hour, minute, second, and Coordinated Universal Time (UTC)
        ///     or local time.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="kind">
        ///     One of the PublicDomain.TzDateTimeKind values that indicates whether year, month,
        ///     day, hour, minute and second specify a local time, Coordinated Universal
        ///     Time (UTC), or neither.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     The specified time parameters evaluate to less than PublicDomain.TzDateTime.MinValue
        ///     or more than PublicDomain.TzDateTime.MaxValue. -or-kind is not one of the PublicDomain.TzDateTimeKind
        ///     values.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is less than 1 or greater than 9999. -or- month is less than 1 or greater
        ///     than 12. -or- day is less than 1 or greater than the number of days in month.-or-
        ///     hour is less than 0 or greater than 23. -or- minute is less than 0 or greater
        ///     than 59. -or- second is less than 0 or greater than 59.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
            : this(year, month, day, hour, minute, second, kind, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, day, hour, minute, second, and Coordinated Universal Time (UTC)
        /// or local time.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="kind">One of the PublicDomain.TzDateTimeKind values that indicates whether year, month,
        /// day, hour, minute and second specify a local time, Coordinated Universal
        /// Time (UTC), or neither.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentException">
        /// The specified time parameters evaluate to less than PublicDomain.TzDateTime.MinValue
        /// or more than PublicDomain.TzDateTime.MaxValue. -or-kind is not one of the PublicDomain.TzDateTimeKind
        /// values.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999. -or- month is less than 1 or greater
        /// than 12. -or- day is less than 1 or greater than the number of days in month.-or-
        /// hour is less than 0 or greater than 23. -or- minute is less than 0 or greater
        /// than 59. -or- second is less than 0 or greater than 59.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind, TzTimeZone timeZone)
            : this(new DateTime(year, month, day, hour, minute, second, kind), timeZone)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, day, hour, minute, second, and millisecond.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        ///     than 12.-or- day is less than 1 or greater than the number of days in month.-or-
        ///     hour is less than 0 or greater than 23.-or- minute is less than 0 or greater
        ///     than 59.-or- second is less than 0 or greater than 59.-or- millisecond is
        ///     less than 0 or greater than 999.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
            : this(year, month, day, hour, minute, second, millisecond, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, day, hour, minute, second, and millisecond.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        /// than 12.-or- day is less than 1 or greater than the number of days in month.-or-
        /// hour is less than 0 or greater than 23.-or- minute is less than 0 or greater
        /// than 59.-or- second is less than 0 or greater than 59.-or- millisecond is
        /// less than 0 or greater than 999.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        /// than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, TzTimeZone timeZone)
            : this(new DateTime(year, month, day, hour, minute, second, millisecond), timeZone)
        {
        }

        /*/// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, day, hour, minute, second, and millisecond for the specified
        ///     calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this PublicDomain.TzDateTime.</param>
        /// <exception cref="System.ArgumentException">
        ///     Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">calendar is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is not in the range supported by calendar.-or- month is less than 1
        ///     or greater than the number of months in calendar.-or- day is less than 1
        ///     or greater than the number of days in month.-or- hour is less than 0 or greater
        ///     than 23.-or- minute is less than 0 or greater than 59.-or- second is less
        ///     than 0 or greater than 59.-or- millisecond is less than 0 or greater than
        ///     999.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar)
            : this(year, month, day, hour, minute, second, millisecond, calendar, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, day, hour, minute, second, and millisecond for the specified
        /// calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this PublicDomain.TzDateTime.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentException">
        /// Specified parameters evaluate to less than PublicDomain.TzDateTime.MinValue or more
        /// than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">calendar is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is not in the range supported by calendar.-or- month is less than 1
        /// or greater than the number of months in calendar.-or- day is less than 1
        /// or greater than the number of days in month.-or- hour is less than 0 or greater
        /// than 23.-or- minute is less than 0 or greater than 59.-or- second is less
        /// than 0 or greater than 59.-or- millisecond is less than 0 or greater than
        /// 999.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, TzTimeZone timeZone)
            : this(new DateTime(year, month, day, hour, minute, second, millisecond, calendar), timeZone)
        {
        }*/

        /// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, day, hour, minute, second, millisecond, and Coordinated Universal
        ///     Time (UTC) or local time.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="kind">
        ///     One of the PublicDomain.TzDateTimeKind values that indicates whether year, month,
        ///     day, hour, minute, second, and millisecond specify a local time, Coordinated
        ///     Universal Time (UTC), or neither.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        ///     than 12.-or- day is less than 1 or greater than the number of days in month.-or-
        ///     hour is less than 0 or greater than 23.-or- minute is less than 0 or greater
        ///     than 59.-or- second is less than 0 or greater than 59.-or- millisecond is
        ///     less than 0 or greater than 999.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        ///     The specified time parameters evaluate to less than PublicDomain.TzDateTime.MinValue
        ///     or more than PublicDomain.TzDateTime.MaxValue. -or-kind is not one of the PublicDomain.TzDateTimeKind
        ///     values.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
            : this(year, month, day, hour, minute, second, millisecond, kind, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, day, hour, minute, second, millisecond, and Coordinated Universal
        /// Time (UTC) or local time.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="kind">One of the PublicDomain.TzDateTimeKind values that indicates whether year, month,
        /// day, hour, minute, second, and millisecond specify a local time, Coordinated
        /// Universal Time (UTC), or neither.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is less than 1 or greater than 9999.-or- month is less than 1 or greater
        /// than 12.-or- day is less than 1 or greater than the number of days in month.-or-
        /// hour is less than 0 or greater than 23.-or- minute is less than 0 or greater
        /// than 59.-or- second is less than 0 or greater than 59.-or- millisecond is
        /// less than 0 or greater than 999.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified time parameters evaluate to less than PublicDomain.TzDateTime.MinValue
        /// or more than PublicDomain.TzDateTime.MaxValue. -or-kind is not one of the PublicDomain.TzDateTimeKind
        /// values.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind, TzTimeZone timeZone)
            : this(new DateTime(year, month, day, hour, minute, second, millisecond, kind), timeZone)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        ///     year, month, day, hour, minute, second, millisecond, and Coordinated Universal
        ///     Time (UTC) or local time for the specified calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this PublicDomain.TzDateTime.</param>
        /// <param name="kind">
        ///     One of the PublicDomain.TzDateTimeKind values that indicates whether year, month,
        ///     day, hour, minute, second, and millisecond specify a local time, Coordinated
        ///     Universal Time (UTC), or neither.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///     The specified time parameters evaluate to less than PublicDomain.TzDateTime.MinValue
        ///     or more than PublicDomain.TzDateTime.MaxValue. -or-kind is not one of the PublicDomain.TzDateTimeKind
        ///     values.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">calendar is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     year is not in the range supported by calendar.-or- month is less than 1
        ///     or greater than the number of months in calendar.-or- day is less than 1
        ///     or greater than the number of days in month.-or- hour is less than 0 or greater
        ///     than 23.-or- minute is less than 0 or greater than 59.-or- second is less
        ///     than 0 or greater than 59.-or- millisecond is less than 0 or greater than
        ///     999.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind)
            : this(year, month, day, hour, minute, second, millisecond, calendar, kind, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PublicDomain.TzDateTime structure to the specified
        /// year, month, day, hour, minute, second, millisecond, and Coordinated Universal
        /// Time (UTC) or local time for the specified calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in month).</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        /// <param name="calendar">The System.Globalization.Calendar that applies to this PublicDomain.TzDateTime.</param>
        /// <param name="kind">One of the PublicDomain.TzDateTimeKind values that indicates whether year, month,
        /// day, hour, minute, second, and millisecond specify a local time, Coordinated
        /// Universal Time (UTC), or neither.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <exception cref="System.ArgumentException">
        /// The specified time parameters evaluate to less than PublicDomain.TzDateTime.MinValue
        /// or more than PublicDomain.TzDateTime.MaxValue. -or-kind is not one of the PublicDomain.TzDateTimeKind
        /// values.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">calendar is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// year is not in the range supported by calendar.-or- month is less than 1
        /// or greater than the number of months in calendar.-or- day is less than 1
        /// or greater than the number of days in month.-or- hour is less than 0 or greater
        /// than 23.-or- minute is less than 0 or greater than 59.-or- second is less
        /// than 0 or greater than 59.-or- millisecond is less than 0 or greater than
        /// 999.
        /// </exception>
        public TzDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind, TzTimeZone timeZone)
            : this(new DateTime(year, month, day, hour, minute, second, millisecond, calendar, kind), timeZone)
        {
        }

        /// <summary>
        /// Sets the date time.
        /// </summary>
        /// <param name="time">The time.</param>
        private void SetDateTime(DateTime time)
        {
            switch (time.Kind)
            {
                case DateTimeKind.Local:
                    ThrowIfNullTimeZone();
                    m_dateTimeUtc = m_timeZone.ToUniversalTime(time);
                    break;
                case DateTimeKind.Unspecified:
                    if (TzTimeZone.TreatUnspecifiedKindAsLocal)
                    {
                        ThrowIfNullTimeZone();
                        m_dateTimeUtc = m_timeZone.ToUniversalTime(time);
                        break;
                    }
                    else
                    {
                        throw new ArgumentException("unspecified kind");
                    }
                case DateTimeKind.Utc:
                    m_dateTimeUtc = time;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the System.DateTime which represents the UTC version of the date and time.
        /// </summary>
        /// <value>The System.DateTime which represents the UTC version of the date and time.</value>
        public DateTime DateTimeUtc
        {
            get
            {
                return m_dateTimeUtc;
            }
            set
            {
                SetDateTime(value);
            }
        }

        /// <summary>
        /// Gets the System.DateTime which represents the local version of the date and time
        /// with the specified PublicDomain.TzTimeZone. If no time zone has been 
        /// specified, an exception is thrown.
        /// </summary>
        /// <value>The System.DateTime which represents the local version of the date and time
        /// with the specified PublicDomain.TzTimeZone. If no time zone has been 
        /// specified, an exception is thrown.</value>
        [XmlIgnore]
        [SoapIgnore]
        public DateTime DateTimeLocal
        {
            get
            {
                ThrowIfNullTimeZone();
                return m_timeZone.ToLocalTime(m_dateTimeUtc);
            }
            set
            {
                SetDateTime(value);
            }
        }

        private void ThrowIfNullTimeZone()
        {
            if (m_timeZone == null)
            {
                throw new Exception("Time zone not specified to retrieve local date.");
            }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public TzTimeZone TimeZone
        {
            get
            {
                return m_timeZone;
            }
            set
            {
                m_timeZone = value;
            }
        }

        /// <summary>
        /// Gets the UTC offset of the date/time.
        /// </summary>
        /// <value>Gets the UTC offset of the date/time</value>
        public TimeSpan UtcOffset
        {
            get
            {
                ThrowIfNullTimeZone();
                return TimeZone.GetUtcOffset(m_dateTimeUtc);
            }
        }

        /// <summary>
        /// Returns a new instance with the saved time zone, but
        /// with the hours, minutes, and seconds set to 0.
        /// </summary>
        /// <returns></returns>
        public TzDateTime GetDate()
        {
            return new TzDateTime(DateTimeUtc.Date, TimeZone);
        }

        /// <summary>
        /// Returns a new instance with the saved time zone, but
        /// with the hours, minutes, and seconds set to 0.
        /// </summary>
        /// <returns></returns>
        public TzDateTime GetDateLocal()
        {
            return new TzDateTime(DateTimeLocal.Date, TimeZone);
        }

        /// <summary>
        /// Gets the date local.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="tz">The tz.</param>
        /// <returns></returns>
        public static DateTime GetDateLocal(TzDateTime dt, TzTimeZone tz)
        {
            return tz.ToLocalTime(dt.DateTimeUtc);
        }

        /// <summary>
        ///     Subtracts a specified date and time from another specified date and time,
        ///     yielding a time interval.
        /// </summary>
        /// <param name="d1">A PublicDomain.TzDateTime (the minuend).</param>
        /// <param name="d2">A PublicDomain.TzDateTime (the subtrahend).</param>
        /// <returns>
        ///     A System.TimeSpan that is the time interval between d1 and d2; that is, d1
        ///     minus d2.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public static TimeSpan operator -(TzDateTime d1, TzDateTime d2)
        {
            return d1.Subtract(d2);
        }

        /// <summary>
        ///     Subtracts a specified time interval from a specified date and time, yielding
        ///     a new date and time.
        /// </summary>
        /// <param name="d">A PublicDomain.TzDateTime.</param>
        /// <param name="t">A System.TimeSpan.</param>
        /// <returns>A PublicDomain.TzDateTime whose value is the value of d minus the value of t.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public static TzDateTime operator -(TzDateTime d, TimeSpan t)
        {
            return d.Subtract(t);
        }

        /// <summary>
        /// Determines whether two specified instances of PublicDomain.TzDateTime are not equal.
        /// </summary>
        /// <param name="d1">A PublicDomain.TzDateTime.</param>
        /// <param name="d2">A PublicDomain.TzDateTime.</param>
        /// <returns>true if d1 and d2 do not represent the same date and time; otherwise, false.</returns>
        public static bool operator !=(TzDateTime d1, TzDateTime d2)
        {
            return !(d1 == d2);
        }

        /// <summary>
        ///     Adds a specified time interval to a specified date and time, yielding a new
        ///     date and time.
        /// </summary>
        /// <param name="d">A PublicDomain.TzDateTime.</param>
        /// <param name="t">A System.TimeSpan.</param>
        /// <returns>A PublicDomain.TzDateTime that is the sum of the values of d and t.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public static TzDateTime operator +(TzDateTime d, TimeSpan t)
        {
            return d.Add(t);
        }

        /// <summary>
        ///     Determines whether one specified PublicDomain.TzDateTime is less than another specified
        ///     PublicDomain.TzDateTime.
        /// </summary>
        /// <param name="t1">A PublicDomain.TzDateTime.</param>
        /// <param name="t2">A PublicDomain.TzDateTime.</param>
        /// <returns>true if t1 is less than t2; otherwise, false.</returns>
        public static bool operator <(TzDateTime t1, TzDateTime t2)
        {
            return t1.DateTimeUtc < t2.DateTimeUtc;
        }

        /// <summary>
        ///     Determines whether one specified PublicDomain.TzDateTime is less than or equal to
        ///     another specified PublicDomain.TzDateTime.
        /// </summary>
        /// <param name="t1">A PublicDomain.TzDateTime.</param>
        /// <param name="t2">A PublicDomain.TzDateTime.</param>
        /// <returns>true if t1 is less than or equal to t2; otherwise, false.</returns>
        public static bool operator <=(TzDateTime t1, TzDateTime t2)
        {
            return t1.DateTimeUtc <= t2.DateTimeUtc;
        }

        /// <summary>
        /// Determines whether two specified instances of PublicDomain.TzDateTime are equal.
        /// </summary>
        /// <param name="d1">A PublicDomain.TzDateTime.</param>
        /// <param name="d2">A PublicDomain.TzDateTime.</param>
        /// <returns>true if d1 and d2 represent the same date and time; otherwise, false.</returns>
        public static bool operator ==(TzDateTime d1, TzDateTime d2)
        {
            if (object.Equals(d1, null) && object.Equals(d2, null))
            {
                return true;
            }
            else if (object.Equals(d1, null))
            {
                return false;
            }
            else if (object.Equals(d2, null))
            {
                return false;
            }
            return DateTime.Equals(d1.DateTimeUtc, d2.DateTimeUtc);
        }

        /// <summary>
        /// Determines whether one specified PublicDomain.TzDateTime is greater than another specified PublicDomain.TzDateTime.
        /// </summary>
        /// <param name="t1">A PublicDomain.TzDateTime.</param>
        /// <param name="t2">A PublicDomain.TzDateTime.</param>
        /// <returns>true if t1 is greater than t2; otherwise, false.</returns>
        public static bool operator >(TzDateTime t1, TzDateTime t2)
        {
            return t1.DateTimeUtc > t2.DateTimeUtc;
        }

        /// <summary>
        /// Determines whether one specified PublicDomain.TzDateTime is greater than or equal to another specified PublicDomain.TzDateTime.
        /// </summary>
        /// <param name="t1">A PublicDomain.TzDateTime.</param>
        /// <param name="t2">A PublicDomain.TzDateTime.</param>
        /// <returns>true if t1 is greater than or equal to t2; otherwise, false.</returns>
        public static bool operator >=(TzDateTime t1, TzDateTime t2)
        {
            return t1.DateTimeUtc >= t2.DateTimeUtc;
        }

        /// <summary>
        /// Gets a PublicDomain.TzDateTime object that is set to the current date and time on
        /// this computer, expressed as the local time.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>A PublicDomain.TzDateTime whose value is the current local date and time.</returns>
        public static TzDateTime Now(TzTimeZone timeZone)
        {
            return new TzDateTime(DateTime.UtcNow, timeZone);
        }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>A PublicDomain.TzDateTime set to today's date, with the time component set to 00:00:00.</returns>
        public static TzDateTime Today(TzTimeZone timeZone)
        {
            return new TzDateTime(DateTime.UtcNow.Date, timeZone);
        }

        /// <summary>
        /// Gets a PublicDomain.TzDateTime object that is set to the current date and time on
        /// this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>A PublicDomain.TzDateTime whose value is the current UTC date and time.</returns>
        public static TzDateTime UtcNow(TzTimeZone timeZone)
        {
            return new TzDateTime(DateTime.UtcNow, timeZone);
        }

        /// <summary>
        /// Adds the value of the specified System.TimeSpan to the value of this instance.
        /// </summary>
        /// <param name="value">A System.TimeSpan that contains the interval to add.</param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and the time interval represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime Add(TimeSpan value)
        {
            return new TzDateTime(DateTimeUtc.Add(value), TimeZone);
        }

        /// <summary>
        /// Adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="value">
        ///     A number of whole and fractional days. The value parameter can be negative
        ///     or positive.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and the number of days represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime AddDays(double value)
        {
            return new TzDateTime(DateTimeUtc.AddDays(value), TimeZone);
        }

        /// <summary>
        /// Adds the specified number of hours to the value of this instance.
        /// </summary>
        /// <param name="value">
        ///     A number of whole and fractional hours. The value parameter can be negative
        ///     or positive.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and the number of hours represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime AddHours(double value)
        {
            return new TzDateTime(DateTimeUtc.AddHours(value), TimeZone);
        }

        /// <summary>
        /// Adds the specified number of milliseconds to the value of this instance.
        /// </summary>
        /// <param name="value">
        /// A number of whole and fractional milliseconds. The value parameter can be
        /// negative or positive.</param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and the number of milliseconds represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime AddMilliseconds(double value)
        {
            return new TzDateTime(DateTimeUtc.AddMilliseconds(value), TimeZone);
        }

        /// <summary>
        /// Adds the specified number of minutes to the value of this instance.
        /// </summary>
        /// <param name="value">
        ///     A number of whole and fractional minutes. The value parameter can be negative
        ///     or positive.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and the number of minutes represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime AddMinutes(double value)
        {
            return new TzDateTime(DateTimeUtc.AddMinutes(value), TimeZone);
        }

        /// <summary>
        /// Adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="months">A number of months. The months parameter can be negative or positive.</param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and months.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.-or- months is less than -120,000 or greater
        ///     than 120,000.
        /// </exception>
        public TzDateTime AddMonths(int months)
        {
            return new TzDateTime(DateTimeUtc.AddMonths(months), TimeZone);
        }

        /// <summary>
        /// Adds the specified number of seconds to the value of this instance.
        /// </summary>
        /// <param name="value">
        ///     A number of whole and fractional seconds. The value parameter can be negative
        ///     or positive.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and the number of seconds represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime AddSeconds(double value)
        {
            return new TzDateTime(DateTimeUtc.AddSeconds(value), TimeZone);
        }

        /// <summary>
        /// Adds the specified number of ticks to the value of this instance.
        /// </summary>
        /// <param name="value">
        ///     A number of 100-nanosecond ticks. The value parameter can be positive or
        ///     negative.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and the time represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue or greater
        ///     than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime AddTicks(long value)
        {
            return new TzDateTime(DateTimeUtc.AddTicks(value), TimeZone);
        }

        /// <summary>
        /// Adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
        /// <returns>
        ///     A PublicDomain.TzDateTime whose value is the sum of the date and time represented
        ///     by this instance and the number of years represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     value or the resulting PublicDomain.TzDateTime is less than PublicDomain.TzDateTime.MinValue
        ///     or greater than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public TzDateTime AddYears(int value)
        {
            return new TzDateTime(DateTimeUtc.AddYears(value), TimeZone);
        }

        /// <summary>
        ///     Compares two instances of PublicDomain.TzDateTime and returns an indication of their
        ///     relative values.
        /// </summary>
        /// <param name="t1">The first PublicDomain.TzDateTime.</param>
        /// <param name="t2">The second PublicDomain.TzDateTime.</param>
        /// <returns>
        ///     A signed number indicating the relative values of t1 and t2.Value Type Condition
        ///     Less than zero t1 is less than t2. Zero t1 equals t2. Greater than zero t1
        ///     is greater than t2.
        /// </returns>
        public static int Compare(TzDateTime t1, TzDateTime t2)
        {
            if (t1 == null)
            {
                throw new ArgumentNullException("t1");
            }
            return t1.CompareTo(t2);
        }

        /// <summary>
        ///     Compares this instance to a specified PublicDomain.TzDateTime object and returns
        ///     an indication of their relative values.
        /// </summary>
        /// <param name="value">A PublicDomain.TzDateTime object to compare.</param>
        /// <returns>
        ///     A signed number indicating the relative values of this instance and the value
        ///     parameter.Value Description Less than zero This instance is less than value.
        ///     Zero This instance is equal to value. Greater than zero This instance is
        ///     greater than value.
        /// </returns>
        public int CompareTo(TzDateTime value)
        {
            long me = DateTimeUtc.Ticks;
            long cmp = value.DateTimeUtc.Ticks;
            if (me < cmp)
            {
                return -1;
            }
            else if (me > cmp)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        ///     Compares this instance to a specified object and returns an indication of
        ///     their relative values.
        /// </summary>
        /// <param name="value">A boxed PublicDomain.TzDateTime object to compare, or null.</param>
        /// <returns>
        ///     A signed number indicating the relative values of this instance and value.Value
        ///     Description Less than zero This instance is less than value. Zero This instance
        ///     is equal to value. Greater than zero This instance is greater than value,
        ///     or value is null.
        /// </returns>
        /// <exception cref="System.ArgumentException">value is not a PublicDomain.TzDateTime.</exception>
        public int CompareTo(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (value is DateTime)
            {
                return CompareTo((DateTime)value);
            }

            TzDateTime tzd = value as TzDateTime;
            if (tzd == null)
            {
                throw new ArgumentException("obj is not a " + typeof(TzDateTime).FullName);
            }
            return DateTimeUtc.CompareTo(tzd.DateTimeUtc);
        }

        /// <summary>
        /// Returns the number of days in the specified month and year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month (a number ranging from 1 to 12).</param>
        /// <returns>
        ///     The number of days in month for the specified year.For example, if month
        ///     equals 2 for February, the return value is 28 or 29 depending upon whether
        ///     year is a leap year.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     month is less than 1 or greater than 12.-or-year is less than 1 or greater
        ///     than 9999.
        /// </exception>
        public static int DaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to the specified
        ///     PublicDomain.TzDateTime instance.
        /// </summary>
        /// <param name="value">A PublicDomain.TzDateTime instance to compare to this instance.</param>
        /// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
        public bool Equals(TzDateTime value)
        {
            return DateTimeUtc.Equals(value.DateTimeUtc);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified
        ///     object.
        /// </summary>
        /// <param name="value">An object to compare to this instance.</param>
        /// <returns>
        ///     true if value is an instance of PublicDomain.TzDateTime and equals the value of this
        ///     instance; otherwise, false.
        /// </returns>
        public override bool Equals(object value)
        {
            TzDateTime y = value as TzDateTime;
            if (y != null)
            {
                return DateTimeUtc.Equals(y.DateTimeUtc);
            }
            return base.Equals(value);
        }

        /// <summary>
        /// Returns a value indicating whether two instances of PublicDomain.TzDateTime are equal.
        /// </summary>
        /// <param name="t1">The first PublicDomain.TzDateTime instance.</param>
        /// <param name="t2">The second PublicDomain.TzDateTime instance.</param>
        /// <returns>true if the two PublicDomain.TzDateTime values are equal; otherwise, false.</returns>
        public static bool Equals(TzDateTime t1, TzDateTime t2)
        {
            return t1 == t2;
        }

        /// <summary>
        /// Deserializes a 64-bit binary value and recreates an original serialized PublicDomain.TzDateTime
        /// object.
        /// </summary>
        /// <param name="dateData">A 64-bit signed integer that encodes the PublicDomain.TzDateTime.Kind property in
        /// a 2-bit field and the PublicDomain.TzDateTime.Ticks property in a 62-bit field.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime object that is equivalent to the PublicDomain.TzDateTime object
        /// that was serialized by the PublicDomain.TzDateTime.ToBinary() method.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// dateData is less than PublicDomain.TzDateTime.MinValue or greater than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public static TzDateTime FromBinary(long dateData, TzTimeZone timeZone)
        {
            return new TzDateTime(DateTime.FromBinary(dateData), timeZone);
        }

        /// <summary>
        /// Converts the specified Windows file time to an equivalent local time.
        /// </summary>
        /// <param name="fileTime">A Windows file time expressed in ticks.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime object that represents a local time equivalent to the date
        /// and time represented by the fileTime parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// fileTime is less than 0 or represents a time greater than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public static TzDateTime FromFileTime(long fileTime, TzTimeZone timeZone)
        {
            return new TzDateTime(DateTime.FromFileTime(fileTime), timeZone);
        }

        /// <summary>
        /// Converts the specified Windows file time to an equivalent UTC time.
        /// </summary>
        /// <param name="fileTime">A Windows file time expressed in ticks.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime object that represents a UTC time equivalent to the date
        /// and time represented by the fileTime parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// fileTime is less than 0 or represents a time greater than PublicDomain.TzDateTime.MaxValue.
        /// </exception>
        public static TzDateTime FromFileTimeUtc(long fileTime, TzTimeZone timeZone)
        {
            return new TzDateTime(DateTime.FromFileTimeUtc(fileTime), timeZone);
        }

        /// <summary>
        /// Returns a PublicDomain.TzDateTime equivalent to the specified OLE Automation Date.
        /// </summary>
        /// <param name="d">An OLE Automation Date value.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime that represents the same date and time as d.
        /// </returns>
        /// <exception cref="System.ArgumentException">The date is not a valid OLE Automation Date value.</exception>
        public static TzDateTime FromOADate(double d, TzTimeZone timeZone)
        {
            return new TzDateTime(DateTime.FromOADate(d), timeZone);
        }

        /// <summary>
        ///     Converts the value of this instance to all the string representations supported
        ///     by the standard PublicDomain.TzDateTime format specifiers.
        /// </summary>
        /// <returns>
        ///     A string array where each element is the representation of the value of this
        ///     instance formatted with one of the standard PublicDomain.TzDateTime formatting specifiers.
        /// </returns>
        public string[] GetDateTimeFormats()
        {
            return DateTimeUtc.GetDateTimeFormats();
        }

        /// <summary>
        ///     Converts the value of this instance to all the string representations supported
        ///     by the specified standard PublicDomain.TzDateTime format specifier.
        /// </summary>
        /// <param name="format">A Unicode character containing a format specifier.</param>
        /// <returns>
        ///     A string array where each element is the representation of the value of this
        ///     instance formatted with the format standard PublicDomain.TzDateTime formatting specifier.
        /// </returns>
        public string[] GetDateTimeFormats(char format)
        {
            return DateTimeUtc.GetDateTimeFormats(format);
        }

        /// <summary>
        ///     Converts the value of this instance to all the string representations supported
        ///     by the standard PublicDomain.TzDateTime format specifiers and the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An System.IFormatProvider that supplies culture-specific formatting information
        ///     about this instance.
        /// </param>
        /// <returns>
        ///     A string array where each element is the representation of the value of this
        ///     instance formatted with one of the standard PublicDomain.TzDateTime formatting specifiers.
        /// </returns>
        public string[] GetDateTimeFormats(IFormatProvider provider)
        {
            return DateTimeUtc.GetDateTimeFormats(provider);
        }

        /// <summary>
        ///     Converts the value of this instance to all the string representations supported
        ///     by the specified standard PublicDomain.TzDateTime format specifier and culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="format">A Unicode character containing a format specifier.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider that supplies culture-specific formatting information
        ///     about this instance.
        /// </param>
        /// <returns>
        ///     A string array where each element is the representation of the value of this
        ///     instance formatted with one of the standard PublicDomain.TzDateTime formatting specifiers.
        /// </returns>
        public string[] GetDateTimeFormats(char format, IFormatProvider provider)
        {
            return DateTimeUtc.GetDateTimeFormats(format, provider);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return DateTimeUtc.GetHashCode();
        }

        /// <summary>
        /// Returns the System.TypeCode for value type PublicDomain.TzDateTime.
        /// </summary>
        /// <returns>The enumerated constant, System.TypeCode.DateTime.</returns>
        public TypeCode GetTypeCode()
        {
            return TypeCode.DateTime;
        }

        /// <summary>
        ///     Indicates whether this instance of PublicDomain.TzDateTime is within the Daylight
        ///     Saving Time range for the current time zone.
        /// </summary>
        /// <returns>
        ///     true if PublicDomain.TzDateTime.Kind is PublicDomain.TzDateTimeKind.Local or PublicDomain.TzDateTimeKind.Unspecified
        ///     and the value of this instance of PublicDomain.TzDateTime is within the Daylight
        ///     Saving Time range for the current time zone. false if PublicDomain.TzDateTime.Kind
        ///     is PublicDomain.TzDateTimeKind.Utc.
        /// </returns>
        public bool IsDaylightSavingTime()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an indication whether the specified year is a leap year.
        /// </summary>
        /// <param name="year">A 4-digit year.</param>
        /// <returns>true if year is a leap year; otherwise, false.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">year is less than 1 or greater than 9999.</exception>
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <returns>A PublicDomain.TzDateTime equivalent to the date and time contained in s.</returns>
        /// <exception cref="System.ArgumentNullException">s is null.</exception>
        /// <exception cref="System.FormatException">s does not contain a valid string representation of a date and time.</exception>
        public static TzDateTime Parse(string s)
        {
            return ParseTz(s, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime equivalent to the date and time contained in s.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">s is null.</exception>
        /// <exception cref="System.FormatException">s does not contain a valid string representation of a date and time.</exception>
        public static TzDateTime ParseTz(string s, TzTimeZone timeZone)
        {
            return ParseTz(s, null, timeZone);
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent using the specified culture-specific format information.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider that supplies culture-specific format information
        ///     about s.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        ///     by provider.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">s is null.</exception>
        /// <exception cref="System.FormatException">s does not contain a valid string representation of a date and time.</exception>
        public static TzDateTime Parse(string s, IFormatProvider provider)
        {
            return ParseTz(s, provider, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent using the specified culture-specific format information.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific format information
        /// about s.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        /// by provider.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">s is null.</exception>
        /// <exception cref="System.FormatException">s does not contain a valid string representation of a date and time.</exception>
        public static TzDateTime ParseTz(string s, IFormatProvider provider, TzTimeZone timeZone)
        {
            return ParseTz(s, provider, DateTimeStyles.None, timeZone);
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent using the specified culture-specific format information and formatting
        ///     style.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider that supplies culture-specific formatting information
        ///     about s.
        /// </param>
        /// <param name="styles">
        ///     A bitwise combination of System.Globalization.DateTimeStyles values that
        ///     indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        ///     by provider and styles.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">s is null.</exception>
        /// <exception cref="System.FormatException">s does not contain a valid string representation of a date and time.</exception>
        /// <exception cref="System.ArgumentException">
        ///     styles contains an invalid combination of System.Globalization.DateTimeStyles
        ///     values. For example, both System.Globalization.DateTimeStyles.AssumeLocal
        ///     and System.Globalization.DateTimeStyles.AssumeUniversal.
        /// </exception>
        public static TzDateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles)
        {
            return ParseTz(s, provider, styles, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent using the specified culture-specific format information and formatting
        /// style.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific formatting information
        /// about s.</param>
        /// <param name="styles">A bitwise combination of System.Globalization.DateTimeStyles values that
        /// indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        /// by provider and styles.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">s is null.</exception>
        /// <exception cref="System.FormatException">s does not contain a valid string representation of a date and time.</exception>
        /// <exception cref="System.ArgumentException">
        /// styles contains an invalid combination of System.Globalization.DateTimeStyles
        /// values. For example, both System.Globalization.DateTimeStyles.AssumeLocal
        /// and System.Globalization.DateTimeStyles.AssumeUniversal.
        /// </exception>
        public static TzDateTime ParseTz(string s, IFormatProvider provider, DateTimeStyles styles, TzTimeZone timeZone)
        {
            if (s == null)
            {
                throw new ArgumentNullException("input");
            }

            s = s.Trim();

            // First, see if this is an ISO 8601 date
            TzDateTime result;
            if (!Iso8601.TryParse(s, timeZone, out result))
            {
                DateTime? dt = null;

                // If it's not ISO 8601, we use the normal DateTime.Parse
                // method; however, we also check if there is a time zone
                // designator on the end
                if (s.Length >= UtcOffsetModifier.Length)
                {
                    // There may be a time zone designator
                    if (s[s.Length - UtcOffsetModifier.Length] == '+' ||
                        s[s.Length - UtcOffsetModifier.Length] == '-')
                    {
                        // Looks like there is a time zone designator
                        char modifier = s[s.Length - UtcOffsetModifier.Length];
                        string[] pieces = StringUtilities.SplitAroundLastIndexOfAny(s, '+', '-');
                        s = pieces[0];
                        
                        TzTimeZone offsetTimeZone = TzTimeZone.GetTimeZoneByOffset(modifier + pieces[1]);

                        // If it's UTC, then create a UTC DateTime
                        if (offsetTimeZone.Equals(TzTimeZone.ZoneUTC))
                        {
                            dt = DateTime.SpecifyKind(DateTime.Parse(s), DateTimeKind.Utc);
                        }
                        else
                        {
                            // Convert it to UTC
                            dt = offsetTimeZone.ToUniversalTime(DateTime.SpecifyKind(DateTime.Parse(s), DateTimeKind.Local));
                        }

                        if (timeZone == null)
                        {
                            timeZone = offsetTimeZone;
                        }
                    }
                }

                if (dt == null)
                {
                    dt = DateTime.Parse(s);
                }

                result = new TzDateTime(dt.Value, timeZone);
            }
            return result;
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent using the specified format and culture-specific format information.
        ///     The format of the string representation must match the specified format exactly.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="format">The expected format of s.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider that supplies culture-specific format information
        ///     about s.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        ///     by format and provider.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">s or format is null.</exception>
        /// <exception cref="System.FormatException">
        ///     s or format is an empty string. -or- s does not contain a date and time that
        ///     corresponds to the pattern specified in format.
        /// </exception>
        public static TzDateTime ParseExact(string s, string format, IFormatProvider provider)
        {
            return ParseExactTz(s, format, provider, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent using the specified format and culture-specific format information.
        /// The format of the string representation must match the specified format exactly.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="format">The expected format of s.</param>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific format information
        /// about s.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        /// by format and provider.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">s or format is null.</exception>
        /// <exception cref="System.FormatException">
        /// s or format is an empty string. -or- s does not contain a date and time that
        /// corresponds to the pattern specified in format.
        /// </exception>
        public static TzDateTime ParseExactTz(string s, string format, IFormatProvider provider, TzTimeZone timeZone)
        {
            return ParseExactTz(s, format, provider, timeZone);
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent using the specified format, culture-specific format information,
        ///     and style. The format of the string representation must match the specified
        ///     format exactly.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="format">The expected format of s.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider that supplies culture-specific formatting information
        ///     about s.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of System.Globalization.DateTimeStyles values that
        ///     indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        ///     by format, provider, and style.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     style contains an invalid combination of System.Globalization.DateTimeStyles
        ///     values. For example, both System.Globalization.DateTimeStyles.AssumeLocal
        ///     and System.Globalization.DateTimeStyles.AssumeUniversal.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">s or format is null.</exception>
        /// <exception cref="System.FormatException">
        ///     s or format is an empty string. -or- s does not contain a date and time that
        ///     corresponds to the pattern specified in format.
        /// </exception>
        public static TzDateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        {
            return ParseExactTz(s, format, provider, style, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent using the specified format, culture-specific format information,
        /// and style. The format of the string representation must match the specified
        /// format exactly.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="format">The expected format of s.</param>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific formatting information
        /// about s.</param>
        /// <param name="style">A bitwise combination of System.Globalization.DateTimeStyles values that
        /// indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        /// by format, provider, and style.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// style contains an invalid combination of System.Globalization.DateTimeStyles
        /// values. For example, both System.Globalization.DateTimeStyles.AssumeLocal
        /// and System.Globalization.DateTimeStyles.AssumeUniversal.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">s or format is null.</exception>
        /// <exception cref="System.FormatException">
        /// s or format is an empty string. -or- s does not contain a date and time that
        /// corresponds to the pattern specified in format.
        /// </exception>
        public static TzDateTime ParseExactTz(string s, string format, IFormatProvider provider, DateTimeStyles style, TzTimeZone timeZone)
        {
            return ParseExactTz(s, format, provider, style, timeZone);
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent using the specified array of formats, culture-specific format
        ///     information, and style. The format of the string representation must match
        ///     at least one of the specified formats exactly.
        /// </summary>
        /// <param name="s">A string containing one or more dates and times to convert.</param>
        /// <param name="formats">An array of expected formats of s.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider that supplies culture-specific format information
        ///     about s.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of System.Globalization.DateTimeStyles values that
        ///     indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.
        /// </param>
        /// <returns>
        ///     A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        ///     by formats, provider, and style.
        /// </returns>
        /// <exception cref="System.FormatException">
        ///     s is an empty string. -or- an element of formats is an empty string. -or-
        ///     s does not contain a date and time that corresponds to any element of formats.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">s or formats is null.</exception>
        /// <exception cref="System.ArgumentException">
        ///     style contains an invalid combination of System.Globalization.DateTimeStyles
        ///     values. For example, both System.Globalization.DateTimeStyles.AssumeLocal
        ///     and System.Globalization.DateTimeStyles.AssumeUniversal.
        /// </exception>
        public static TzDateTime ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            return ParseExactTz(s, formats, provider, style, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent using the specified array of formats, culture-specific format
        /// information, and style. The format of the string representation must match
        /// at least one of the specified formats exactly.
        /// </summary>
        /// <param name="s">A string containing one or more dates and times to convert.</param>
        /// <param name="formats">An array of expected formats of s.</param>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific format information
        /// about s.</param>
        /// <param name="style">A bitwise combination of System.Globalization.DateTimeStyles values that
        /// indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// A PublicDomain.TzDateTime equivalent to the date and time contained in s as specified
        /// by formats, provider, and style.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// s is an empty string. -or- an element of formats is an empty string. -or-
        /// s does not contain a date and time that corresponds to any element of formats.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">s or formats is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// style contains an invalid combination of System.Globalization.DateTimeStyles
        /// values. For example, both System.Globalization.DateTimeStyles.AssumeLocal
        /// and System.Globalization.DateTimeStyles.AssumeUniversal.
        /// </exception>
        public static TzDateTime ParseExactTz(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, TzTimeZone timeZone)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Subtracts the specified date and time from this instance.
        /// </summary>
        /// <param name="value">An instance of PublicDomain.TzDateTime.</param>
        /// <returns>
        ///     A System.TimeSpan interval equal to the date and time represented by this
        ///     instance minus the date and time represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The result is less than PublicDomain.TzDateTime.MinValue or greater than PublicDomain.TzDateTime.MaxValue.</exception>
        public TimeSpan Subtract(TzDateTime value)
        {
            return DateTimeUtc - value.DateTimeUtc;
        }

        /// <summary>
        /// Subtracts the specified duration from this instance.
        /// </summary>
        /// <param name="value">An instance of System.TimeSpan.</param>
        /// <returns>
        ///     A PublicDomain.TzDateTime equal to the date and time represented by this instance
        ///     minus the time interval represented by value.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The result is less than PublicDomain.TzDateTime.MinValue or greater than PublicDomain.TzDateTime.MaxValue.</exception>
        public TzDateTime Subtract(TimeSpan value)
        {
            return new TzDateTime(DateTimeUtc.Subtract(value), TimeZone);
        }

        /// <summary>
        ///     Serializes the current PublicDomain.TzDateTime object to a 64-bit binary value that
        ///     subsequently can be used to recreate the PublicDomain.TzDateTime object.
        /// </summary>
        /// <returns>
        ///     A 64-bit signed integer that encodes the PublicDomain.TzDateTime.Kind and PublicDomain.TzDateTime.Ticks
        ///     properties.
        /// </returns>
        public long ToBinary()
        {
            return DateTimeUtc.ToBinary();
        }

        /// <summary>
        ///     Converts the value of the current PublicDomain.TzDateTime object to a Windows file
        ///     time.
        /// </summary>
        /// <returns>
        ///     The value of the current PublicDomain.TzDateTime object expressed as a Windows file
        ///     time.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting file time would represent a date and time before 12:00 midnight
        ///     January 1, 1601 C.E. UTC.
        /// </exception>
        public long ToFileTime()
        {
            return DateTimeLocal.ToFileTime();
        }

        /// <summary>
        ///     Converts the value of the current PublicDomain.TzDateTime object to a Windows file
        ///     time.
        /// </summary>
        /// <returns>
        ///     The value of the current PublicDomain.TzDateTime object expressed as a Windows file
        ///     time.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     The resulting file time would represent a date and time before 12:00 midnight
        ///     January 1, 1601 C.E. UTC.
        /// </exception>
        public long ToFileTimeUtc()
        {
            return DateTimeUtc.ToFileTimeUtc();
        }

        /// <summary>
        ///     Converts the value of this instance to its equivalent long date string representation.
        /// </summary>
        /// <returns>
        ///     A string containing the name of the day of the week, the name of the month,
        ///     the numeric day of the month, and the year equivalent to the date value of
        ///     this instance.
        /// </returns>
        public string ToLongDateString()
        {
            return DateTimeUtc.ToLongDateString();
        }

        /// <summary>
        ///     Converts the value of this instance to its equivalent long time string representation.
        /// </summary>
        /// <returns>
        ///     A string containing the name of the day of the week, the name of the month,
        ///     the numeric day of the hours, minutes, and seconds equivalent to the time
        ///     value of this instance.
        /// </returns>
        public string ToLongTimeString()
        {
            return DateTimeUtc.ToLongTimeString();
        }

        /// <summary>
        ///     Converts the value of this instance to the equivalent OLE Automation date.
        /// </summary>
        /// <returns>
        ///     A double-precision floating-point number that contains an OLE Automation
        ///     date equivalent to the value of this instance.
        /// </returns>
        /// <exception cref="System.OverflowException">The value of this instance cannot be represented as an OLE Automation Date.</exception>
        public double ToOADate()
        {
            return DateTimeUtc.ToOADate();
        }

        /// <summary>
        ///     Converts the value of this instance to its equivalent short date string representation.
        /// </summary>
        /// <returns>
        ///     A string containing the numeric month, the numeric day of the month, and
        ///     the year equivalent to the date value of this instance.
        /// </returns>
        public string ToShortDateString()
        {
            return DateTimeUtc.ToShortDateString();
        }

        /// <summary>
        ///     Converts the value of this instance to its equivalent short time string representation.
        /// </summary>
        /// <returns>
        ///     A string containing the name of the day of the week, the name of the month,
        ///     the numeric day of the hours, minutes, and seconds equivalent to the time
        ///     value of this instance.
        /// </returns>
        public string ToShortTimeString()
        {
            return DateTimeUtc.ToShortTimeString();
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of value of this instance.
        /// </returns>
        public override string ToString()
        {
            return DateTimeUtc.ToString() + UtcOffsetModifier;
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation
        /// using the specified culture-specific format information.
        /// </summary>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A string representation of value of this instance as specified by provider.
        /// </returns>
        public string ToString(IFormatProvider provider)
        {
            return DateTimeUtc.ToString(provider) + UtcOffsetModifier;
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation
        /// using the specified format.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <returns>
        /// A string representation of value of this instance as specified by format.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// The length of format is 1, and it is not one of the format specifier characters
        /// defined for System.Globalization.DateTimeFormatInfo.-or- format does not
        /// contain a valid custom format pattern.
        /// </exception>
        public string ToString(string format)
        {
            return DateTimeUtc.ToString(format) + UtcOffsetModifier;
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation
        /// using the specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A string representation of value of this instance as specified by format
        /// and provider.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// The length of format is 1, and it is not one of the format specifier characters
        /// defined for System.Globalization.DateTimeFormatInfo.-or- format does not
        /// contain a valid custom format pattern.
        /// </exception>
        public string ToString(string format, IFormatProvider provider)
        {
            return DateTimeUtc.ToString(format, provider) + UtcOffsetModifier;
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>
        /// A string representation of value of this instance.
        /// </returns>
        public string ToStringLocal()
        {
            return DateTimeLocal.ToString();
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation
        /// using the specified culture-specific format information.
        /// </summary>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A string representation of value of this instance as specified by provider.
        /// </returns>
        public string ToStringLocal(IFormatProvider provider)
        {
            return DateTimeLocal.ToString(provider);
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation
        /// using the specified format.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <returns>
        /// A string representation of value of this instance as specified by format.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// The length of format is 1, and it is not one of the format specifier characters
        /// defined for System.Globalization.DateTimeFormatInfo.-or- format does not
        /// contain a valid custom format pattern.
        /// </exception>
        public string ToStringLocal(string format)
        {
            return DateTimeLocal.ToString(format);
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation
        /// using the specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <param name="provider">An System.IFormatProvider that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A string representation of value of this instance as specified by format
        /// and provider.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// The length of format is 1, and it is not one of the format specifier characters
        /// defined for System.Globalization.DateTimeFormatInfo.-or- format does not
        /// contain a valid custom format pattern.
        /// </exception>
        public string ToStringLocal(string format, IFormatProvider provider)
        {
            return DateTimeLocal.ToString(format, provider);
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="result">
        ///     When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        ///     the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        ///     if the conversion failed. The conversion fails if the s parameter is null,
        ///     or does not contain a valid string representation of a date and time. This
        ///     parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     true if the s parameter was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParse(string s, out TzDateTime result)
        {
            return TryParseTz(s, out result, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="result">When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        /// the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        /// if the conversion failed. The conversion fails if the s parameter is null,
        /// or does not contain a valid string representation of a date and time. This
        /// parameter is passed uninitialized.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// true if the s parameter was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParseTz(string s, out TzDateTime result, TzTimeZone timeZone)
        {
            return TryParseTz(s, null, DateTimeStyles.None, out result, timeZone);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="styles">The styles.</param>
        /// <param name="result">When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        /// the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        /// if the conversion failed. The conversion fails if the s parameter is null,
        /// or does not contain a valid string representation of a date and time. This
        /// parameter is passed uninitialized.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// true if the s parameter was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParseTz(string s, DateTimeStyles styles, out TzDateTime result, TzTimeZone timeZone)
        {
            return TryParseTz(s, null, styles, out result, timeZone);
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent using the specified culture-specific format information and formatting
        ///     style.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider object that supplies culture-specific formatting
        ///     information about s.
        /// </param>
        /// <param name="styles">
        ///     A bitwise combination of System.Globalization.DateTimeStyles values that
        ///     indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        ///     the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        ///     if the conversion failed. The conversion fails if the s parameter is null,
        ///     or does not contain a valid string representation of a date and time. This
        ///     parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     true if the s parameter was converted successfully; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     styles is not a valid System.Globalization.DateTimeStyles value.-or-styles
        ///     contains an invalid combination of System.Globalization.DateTimeStyles values
        ///     (for example, both System.Globalization.DateTimeStyles.AssumeLocal and System.Globalization.DateTimeStyles.AssumeUniversal).
        /// </exception>
        public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles styles, out TzDateTime result)
        {
            return TryParseTz(s, provider, styles, out result, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent using the specified culture-specific format information and formatting
        /// style.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="provider">An System.IFormatProvider object that supplies culture-specific formatting
        /// information about s.</param>
        /// <param name="styles">A bitwise combination of System.Globalization.DateTimeStyles values that
        /// indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <param name="result">When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        /// the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        /// if the conversion failed. The conversion fails if the s parameter is null,
        /// or does not contain a valid string representation of a date and time. This
        /// parameter is passed uninitialized.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// true if the s parameter was converted successfully; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// styles is not a valid System.Globalization.DateTimeStyles value.-or-styles
        /// contains an invalid combination of System.Globalization.DateTimeStyles values
        /// (for example, both System.Globalization.DateTimeStyles.AssumeLocal and System.Globalization.DateTimeStyles.AssumeUniversal).
        /// </exception>
        public static bool TryParseTz(string s, IFormatProvider provider, DateTimeStyles styles, out TzDateTime result, TzTimeZone timeZone)
        {
            result = null;
            try
            {
                result = ParseTz(s, provider, styles, timeZone);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent using the specified format, culture-specific format information,
        ///     and style. The format of the string representation must match the specified
        ///     format exactly.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="format">The expected format of s.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider object that supplies culture-specific formatting
        ///     information about s.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of one or more System.Globalization.DateTimeStyles
        ///     values that indicate the permitted format of s.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        ///     the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        ///     if the conversion failed. The conversion fails if either the s or format
        ///     parameter is null, is an empty string, or does not contain a date and time
        ///     that correspond to the pattern specified in format. This parameter is passed
        ///     uninitialized.
        /// </param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="System.ArgumentException">
        ///     styles is not a valid System.Globalization.DateTimeStyles value.-or-styles
        ///     contains an invalid combination of System.Globalization.DateTimeStyles values
        ///     (for example, both System.Globalization.DateTimeStyles.AssumeLocal and System.Globalization.DateTimeStyles.AssumeUniversal).
        /// </exception>
        public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style, out TzDateTime result)
        {
            return TryParseExactTz(s, format, provider, DateTimeStyles.None, out result, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent using the specified format, culture-specific format information,
        /// and style. The format of the string representation must match the specified
        /// format exactly.
        /// </summary>
        /// <param name="s">A string containing a date and time to convert.</param>
        /// <param name="format">The expected format of s.</param>
        /// <param name="provider">An System.IFormatProvider object that supplies culture-specific formatting
        /// information about s.</param>
        /// <param name="style">A bitwise combination of one or more System.Globalization.DateTimeStyles
        /// values that indicate the permitted format of s.</param>
        /// <param name="result">When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        /// the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        /// if the conversion failed. The conversion fails if either the s or format
        /// parameter is null, is an empty string, or does not contain a date and time
        /// that correspond to the pattern specified in format. This parameter is passed
        /// uninitialized.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// true if s was converted successfully; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// styles is not a valid System.Globalization.DateTimeStyles value.-or-styles
        /// contains an invalid combination of System.Globalization.DateTimeStyles values
        /// (for example, both System.Globalization.DateTimeStyles.AssumeLocal and System.Globalization.DateTimeStyles.AssumeUniversal).
        /// </exception>
        public static bool TryParseExactTz(string s, string format, IFormatProvider provider, DateTimeStyles style, out TzDateTime result, TzTimeZone timeZone)
        {
            return TryParseExactTz(s, new string[] { format }, provider, style, out result, timeZone);
        }

        /// <summary>
        ///     Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        ///     equivalent using the specified array of formats, culture-specific format
        ///     information, and style. The format of the string representation must match
        ///     at least one of the specified formats exactly.
        /// </summary>
        /// <param name="s">A string containing one or more dates and times to convert.</param>
        /// <param name="formats">An array of expected formats of s.</param>
        /// <param name="provider">
        ///     An System.IFormatProvider object that supplies culture-specific format information
        ///     about s.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of System.Globalization.DateTimeStyles values that
        ///     indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.
        /// </param>
        /// <param name="result">
        ///     When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        ///     the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        ///     if the conversion failed. The conversion fails if s or formats is null, s
        ///     or an element of formats is an empty string, or the format of s is not exactly
        ///     as specified by at least one of the format patterns in formats. This parameter
        ///     is passed uninitialized.
        /// </param>
        /// <returns>true if the s parameter was converted successfully; otherwise, false.</returns>
        /// <exception cref="System.ArgumentException">
        ///     styles is not a valid System.Globalization.DateTimeStyles value.-or-styles
        ///     contains an invalid combination of System.Globalization.DateTimeStyles values
        ///     (for example, both System.Globalization.DateTimeStyles.AssumeLocal and System.Globalization.DateTimeStyles.AssumeUniversal).
        /// </exception>
        public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out TzDateTime result)
        {
            return TryParseExactTz(s, formats, provider, style, out result, null);
        }

        /// <summary>
        /// Converts the specified string representation of a date and time to its PublicDomain.TzDateTime
        /// equivalent using the specified array of formats, culture-specific format
        /// information, and style. The format of the string representation must match
        /// at least one of the specified formats exactly.
        /// </summary>
        /// <param name="s">A string containing one or more dates and times to convert.</param>
        /// <param name="formats">An array of expected formats of s.</param>
        /// <param name="provider">An System.IFormatProvider object that supplies culture-specific format information
        /// about s.</param>
        /// <param name="style">A bitwise combination of System.Globalization.DateTimeStyles values that
        /// indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
        /// <param name="result">When this method returns, contains the PublicDomain.TzDateTime value equivalent to
        /// the date and time contained in s, if the conversion succeeded, or PublicDomain.TzDateTime.MinValue
        /// if the conversion failed. The conversion fails if s or formats is null, s
        /// or an element of formats is an empty string, or the format of s is not exactly
        /// as specified by at least one of the format patterns in formats. This parameter
        /// is passed uninitialized.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>
        /// true if the s parameter was converted successfully; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// styles is not a valid System.Globalization.DateTimeStyles value.-or-styles
        /// contains an invalid combination of System.Globalization.DateTimeStyles values
        /// (for example, both System.Globalization.DateTimeStyles.AssumeLocal and System.Globalization.DateTimeStyles.AssumeUniversal).
        /// </exception>
        public static bool TryParseExactTz(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out TzDateTime result, TzTimeZone timeZone)
        {
            result = null;
            try
            {
                result = ParseExactTz(s, formats, provider, style, timeZone);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public TzDateTime Clone()
        {
            return (TzDateTime)MemberwiseClone();
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A Boolean value equivalent to the value of this instance.
        /// </returns>
        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException("ToBoolean");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException("ToByte");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A Unicode character equivalent to the value of this instance.
        /// </returns>
        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException("ToChar");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"></see> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A <see cref="T:System.DateTime"></see> instance equivalent to the value of this instance.
        /// </returns>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return DateTimeUtc;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"></see> number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A <see cref="T:System.Decimal"></see> number equivalent to the value of this instance.
        /// </returns>
        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException("ToDecimal");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException("ToDouble");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("ToInt16");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("ToInt32");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("ToInt64");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        [CLSCompliant(false)]
        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException("ToSByte");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException("ToSingle");
        }

        /// <summary>
        /// Converts the value of this instance to an <see cref="T:System.Object"></see> of the specified <see cref="T:System.Type"></see> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="conversionType">The <see cref="T:System.Type"></see> to which the value of this instance is converted.</param>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> instance of type conversionType whose value is equivalent to the value of this instance.
        /// </returns>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new InvalidCastException("ToType");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        [CLSCompliant(false)]
        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("ToUInt16");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        [CLSCompliant(false)]
        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("ToUInt32");
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"></see> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        [CLSCompliant(false)]
        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("ToUInt64");
        }

        /// <summary>
        /// From the unix timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns></returns>
        public static TzDateTime FromUnixTimestamp(double timestamp, TzTimeZone timeZone)
        {
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            return new TzDateTime(dateTime.AddSeconds(timestamp), timeZone);
        }
    }
}
