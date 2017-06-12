using System;
using Zmanim.Extensions;

namespace Zmanim.TimeZone
{
    ///<summary>
    /// TimeZone based on the Gmt offset (this is very limited)
    ///</summary>
    public class OffsetTimeZone : ITimeZone
    {
        private readonly TimeSpan offsetFromGmt;

        ///<summary>
        ///</summary>
        ///<param name="hoursOffsetFromGmt">The amount of hours from gmt.</param>
        public OffsetTimeZone(int hoursOffsetFromGmt)
            : this(new TimeSpan(hoursOffsetFromGmt, 0, 0))
        { }

        ///<summary>
        ///</summary>
        ///<param name="offsetFromGmt">TimeSpan from Gmt</param>
        public OffsetTimeZone(TimeSpan offsetFromGmt)
        {
            this.offsetFromGmt = offsetFromGmt;
        }

        public int UtcOffset(DateTime dateTime)
        {
            return (int)offsetFromGmt.TotalMilliseconds;
        }

        public bool IsDaylightSavingTime(DateTime dateTime)
        {
            return false;
        }

        public string GetId()
        {
            return "Offset";
        }

        public string GetDisplayName()
        {
            return GetId();
        }

        public int GetOffset(long timeFromEpoch)
        {
            return UtcOffset(timeFromEpoch.ToDateTime());
        }
    }
}