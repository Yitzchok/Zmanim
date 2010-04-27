using System;
using TimeZone = java.util.TimeZone;

namespace net.sourceforge.zmanim
{
    public interface ICalendar : ICloneable
    {
        DateTime Date { get; set; }
        TimeZone TimeZone { get; set; }
        long getTimeInMillis();
    }

    public class DefaultCalendar : ICalendar
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        public DateTime Date { get; set; }
        public TimeZone TimeZone { get; set; }

        public long getTimeInMillis()
        {
            return Date.ToFileTime();
        }
    }
}