using System;
using java.util;
using Zmanim.Extensions;

namespace Zmanim.TimeZone
{
    ///<summary>
    /// Provides the most basic useage of a TimeZone.
    ///</summary>
    public interface ITimeZone : ICloneable
    {
        //bool IsDaylightSavingTime { get; }
        int UtcOffset { get; }
        //int RawOffset { get; }

        //int GMTOffset { get; }
        //int DSTOffset { get; }

        string Name { get; }
        string DisplayName { get; }
        int getRawOffset();
        int getDSTSavings();
        bool inDaylightTime(DateTime dateTime);
        string getID();
        string getDisplayName();
        int getOffset(long toFileTime);
    }

    public class JavaTimeZone : ITimeZone
    {
        private java.util.TimeZone timeZone;
        public JavaTimeZone(string name)
        {
            timeZone = java.util.TimeZone.getTimeZone(name);
        }

        //public bool IsDaylightSavingTime
        //{
        //    get { return false; }
        //}

        public JavaTimeZone()
        {
            timeZone = java.util.TimeZone.getDefault();
        }

        public int UtcOffset
        {
            get { return timeZone.getRawOffset(); }
        }

        public string Name
        {
            get { return timeZone.getDisplayName(); }
        }

        public string DisplayName
        {
            get { return getDisplayName(); }
        }

        public int getRawOffset()
        {
            return timeZone.getRawOffset();
        }

        public int getDSTSavings()
        {
            return timeZone.getDSTSavings();
        }

        public bool inDaylightTime(DateTime dateTime)
        {
            return timeZone.inDaylightTime(dateTime.ToDate());
        }

        public string getID()
        {
            return timeZone.getID();
        }

        public string getDisplayName()
        {
            return timeZone.getDisplayName();
        }

        public int getOffset(long toFileTime)
        {
            return timeZone.getOffset(toFileTime);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}