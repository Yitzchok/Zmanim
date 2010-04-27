using System;

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
}