using System;
using net.sourceforge.zmanim;
using Quartz;
using Zmanim.QuartzScheduling.Configuration;
using Zmanim.Extensions;

namespace Zmanim.QuartzScheduling
{
    /// <summary>
    /// This Calendar will include Shabbos.
    /// </summary>
    public class ShabbosCalendar : ICalendar
    {
        private readonly ZmanimLocationProperties locationProperties;

        public ShabbosCalendar(ZmanimLocationProperties locationProperties)
        {
            this.locationProperties = locationProperties;
        }

        public bool IsTimeIncluded(DateTime timeUtc)
        {
            var calendar = SchedulerHelper.GetComplexZmanimCalendar(locationProperties, timeUtc);
            bool isShabbos = false;

            if (timeUtc.DayOfWeek == DayOfWeek.Friday)
                isShabbos = timeUtc > calendar.getCandelLighting().ToDateTime().ToUniversalTime();
            if (timeUtc.DayOfWeek == DayOfWeek.Saturday)
                isShabbos = timeUtc <= calendar.getTzais().ToDateTime().ToUniversalTime();

            return isShabbos;
        }

        public DateTime GetNextIncludedTimeUtc(DateTime timeUtc)
        {
            ComplexZmanimCalendar complexZmanimCalendar = null;
            if (timeUtc.DayOfWeek == DayOfWeek.Friday)
                complexZmanimCalendar = SchedulerHelper.GetComplexZmanimCalendar(locationProperties, timeUtc.AddDays(1));
            else if (timeUtc.DayOfWeek == DayOfWeek.Saturday)
                complexZmanimCalendar = SchedulerHelper.GetComplexZmanimCalendar(locationProperties, timeUtc);

            return complexZmanimCalendar == null ? DateTime.UtcNow : complexZmanimCalendar.getTzais().ToDateTime().ToUniversalTime();
        }

        public string Description { get; set; }
        public ICalendar CalendarBase { get; set; }
    }
}