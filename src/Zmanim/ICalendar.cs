using System;
using TimeZone = java.util.TimeZone;

namespace net.sourceforge.zmanim
{
    public interface ICalendar : ICloneable
    {
        DateTime getTime();
        void setTime(DateTime date);
        TimeZone getTimeZone();
        void setTimeZone(TimeZone timeZone);
        long getTimeInMillis();
    }

    public class DefaultCalendar : ICalendar
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        public DateTime getTime()
        {
            return Date;
        }

        public void setTime(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; set; }
        public TimeZone TimeZone { get; set; }

        public TimeZone getTimeZone()
        {
            return TimeZone;
        }

        public void setTimeZone(TimeZone timeZone)
        {
            TimeZone = timeZone;
        }

        public long getTimeInMillis()
        {
            return Date.ToFileTime();
        }
    }
}