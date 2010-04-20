using System;
using net.sourceforge.zmanim;
using Quartz;
using Zmanim.Extensions;

namespace Zmanim.Scheduling
{
    /// <summary>
    /// This Calendar will include Shabbos.
    /// </summary>
    public class ShabbosCalendar : ICalendar
    {
        private readonly Location location;

        public ShabbosCalendar(Location location)
        {
            this.location = location;
        }

        public bool IsTimeIncluded(DateTime timeUtc)
        {
            return timeUtc.IsShabbos(location);
        }

        public DateTime GetNextIncludedTimeUtc(DateTime timeUtc)
        {
            ComplexZmanimCalendar complexZmanimCalendar = null;
            if (timeUtc.DayOfWeek == DayOfWeek.Friday)
                complexZmanimCalendar = ZmanimHelper.GetCalendar(location, timeUtc.AddDays(1));
            else if (timeUtc.DayOfWeek == DayOfWeek.Saturday)
                complexZmanimCalendar = ZmanimHelper.GetCalendar(location, timeUtc);

            return complexZmanimCalendar == null ? DateTime.UtcNow : complexZmanimCalendar.getTzais().ToUniversalTime();
        }

        public string Description { get; set; }
        public ICalendar CalendarBase { get; set; }
    }
}