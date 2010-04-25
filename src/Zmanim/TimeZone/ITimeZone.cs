namespace Zmanim.TimeZone
{
    ///<summary>
    /// Provides the most basic useage of a TimeZone.
    ///</summary>
    public interface ITimeZone
    {
        bool IsDaylightSavingTime { get; }
        int UtcOffset { get; }
        //int RawOffset { get; }

        //int GMTOffset { get; }
        //int DSTOffset { get; }

        string Name { get; }
        string DisplayName { get; }
    }


    public class TimeZone : ITimeZone
    {
        public TimeZone(int utcOffset)
        {
            UtcOffset = utcOffset;
        }

        public bool IsDaylightSavingTime { get; private set; }
        public int UtcOffset { get; private set; }
        public string Name { get { return "Offset"; } }
        public string DisplayName { get { return "Offset"; } }
    }
}