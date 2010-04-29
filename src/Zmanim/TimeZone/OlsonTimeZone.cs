using System;
using PublicDomain;
using Zmanim.Extensions;

namespace Zmanim.TimeZone
{
    public class OlsonTimeZone : ITimeZone
    {
        public OlsonTimeZone() { }

        public OlsonTimeZone(TzTimeZone timeZone)
        {
            this.TimeZone = timeZone;
        }

        public OlsonTimeZone(string timeZoneName)
        {
            TimeZone = TzTimeZone.GetTimeZone(timeZoneName);
        }

        public TzTimeZone TimeZone { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public int UtcOffset(DateTime dateTime)
        {
            return (int)TimeZone.GetUtcOffset(dateTime).TotalMilliseconds;
        }

        public bool inDaylightTime(DateTime dateTime)
        {
            return TimeZone.IsDaylightSavingTime(dateTime);
        }

        public string getID()
        {
            return getDisplayName();
        }

        public string getDisplayName()
        {
            return TimeZone.StandardName;
        }

        public int getOffset(long timeFromEpoch)
        {
            return UtcOffset(timeFromEpoch.ToDateTime());
        }
    }
}