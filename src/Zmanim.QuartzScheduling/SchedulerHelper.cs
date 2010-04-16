using System;
using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using Quartz;
using Quartz.Impl.Calendar;
using Zmanim.Examples.QuartzScheduling.Configuration;
using Zmanim.Examples.QuartzScheduling.Jobs;
using Zmanim.Extensions;
using TimeZone = java.util.TimeZone;
using System.Linq;

namespace Zmanim.Examples.QuartzScheduling
{
    public class ReminderServiceJobDetail : JobDetail
    {
        public ReminderServiceJobDetail() { }
        public ReminderServiceJobDetail(string name, Type jobType) : base(name, jobType) { }
        public ReminderServiceJobDetail(string name, string group, Type jobType) : base(name, group, jobType) { }
        public ReminderServiceJobDetail(string name, string group, Type jobType, bool isVolatile, bool isDurable, bool requestsRecovery) : base(name, group, jobType, isVolatile, isDurable, requestsRecovery) { }
        public ReminderService ReminderService { get; set; }
        public Account Account { get; set; }
    }

    public class SchedulerHelper
    {
        public static void ScheduleZmanJob(IScheduler scheduler, ReminderService reminderService, Account account)
        {
            DateTime date = GetNextZmanDateTime(reminderService.LocationProperties, reminderService.JobToRun);

            var jobDetail = new ReminderServiceJobDetail(reminderService.ZmanName, null, typeof(TweetZmanimJob))
                                {
                                    ReminderService = reminderService,
                                    Account = account
                                };


            var trigger = new SimpleTrigger(reminderService.ZmanName,
                date.AddMinutes(reminderService.AddSeconds), null, 0, TimeSpan.Zero);

            if (reminderService.SkipFriday)
            {
                if (!scheduler.GetCalendarNames().Contains("Friday"))
                    scheduler.AddCalendar("Friday", SetupCalendar(DayOfWeek.Friday), false, false);

                trigger.CalendarName = "Friday";
            }
            else if (reminderService.SkipShabbos)
            {
                if (!scheduler.GetCalendarNames().Contains("Saturday"))
                    scheduler.AddCalendar("Saturday", SetupCalendar(DayOfWeek.Saturday), false, false);

                trigger.CalendarName = "Saturday";
            }

            scheduler.ScheduleJob(jobDetail, trigger);
        }


        private static WeeklyCalendar SetupCalendar(DayOfWeek dayOfWeek)
        {
            var cal = new WeeklyCalendar();
            cal.SetDayExcluded(dayOfWeek, true);
            return cal;
        }

        public static DateTime GetNextZmanDateTime(
            ZmanimLocationProperties locationProperties, string methodName)
        {
            var date = GetZman(DateTime.Now, locationProperties, methodName);
            if (date < DateTime.UtcNow)
                date = GetZman(DateTime.Now.AddDays(1), locationProperties, methodName);

            return date;
        }

        public static DateTime GetZman(
            DateTime date, ZmanimLocationProperties locationProperties, string methodName)
        {
            var timeZone = TimeZone.getTimeZone(locationProperties.TimeZone);
            var location = new GeoLocation(locationProperties.LocationName,
                locationProperties.Latitude, locationProperties.Longitude,
                locationProperties.Elevation, timeZone);

            var czc = new ComplexZmanimCalendar(location);

            czc.setCalendar(new GregorianCalendar(date.Year, date.Month - 1, date.Day));

            return czc.getSunset().ToDateTime().ToUniversalTime();
        }
    }
}