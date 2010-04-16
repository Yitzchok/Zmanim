using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using Quartz;
using Quartz.Impl.Calendar;
using Zmanim.Extensions;
using Zmanim.QuartzScheduling.Configuration;
using TimeZone = java.util.TimeZone;

namespace Zmanim.QuartzScheduling
{
    public class SchedulerHelper
    {
        public static void ReScheduleZmanJob(string lastTriggerName, DateTime date, IScheduler scheduler, ReminderService reminderService, Account account)
        {
            if (lastTriggerName.EndsWith("__1"))
                ScheduleZmanJob(reminderService.Name, date, scheduler, reminderService, account);
            else
                ScheduleZmanJob(reminderService.Name + "__1", date, scheduler, reminderService, account);
        }

        public static void ScheduleZmanJob(IScheduler scheduler, ReminderService reminderService, Account account)
        {
            ScheduleZmanJob(reminderService.Name, DateTime.UtcNow, scheduler, reminderService, account);
        }

        public static void ScheduleZmanJob(string name, DateTime dateToStart, IScheduler scheduler, ReminderService reminderService, Account account)
        {
            DateTime date = GetNextZmanDateTime(dateToStart,
                reminderService.LocationProperties,
                reminderService.ZmanName,
                reminderService.SkipIfPassedRunBeforeZmanSeconds ? reminderService.AddSeconds : 0);

            var jobDetail = new ReminderServiceJobDetail(name, null,
               GetJobType(reminderService.JobToRun))
                                {
                                    ReminderService = reminderService,
                                    Account = account
                                };

            var trigger = new SimpleTrigger(name, date.AddSeconds(reminderService.AddSeconds), date, 0, TimeSpan.Zero);
            scheduler.AddGlobalTriggerListener(new ShabbosTriggerListener());

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        private static Type GetJobType(string jobName)
        {
            return Assembly.GetExecutingAssembly()
                .GetType("Zmanim.QuartzScheduling.Jobs." + jobName, true, true);
        }

        private static WeeklyCalendar SetupCalendar(DayOfWeek dayOfWeek)
        {
            var cal = new WeeklyCalendar();
            cal.SetDayExcluded(dayOfWeek, true);
            return cal;
        }

        public static DateTime GetNextZmanDateTime(DateTime dateToStart,
            ZmanimLocationProperties locationProperties, string methodName, double addSeconds)
        {
            DateTime date = GetZman(dateToStart, locationProperties, methodName);

            if (date.AddSeconds(addSeconds) < DateTime.UtcNow)
                date = GetZman(dateToStart.AddDays(1), locationProperties, methodName);

            return date;
        }

        public static DateTime GetZman(
            DateTime date, ZmanimLocationProperties locationProperties, string methodName)
        {
            ComplexZmanimCalendar czc = GetComplexZmanimCalendar(locationProperties, date);

            return GetZmanMethodFormName(methodName, czc).ToUniversalTime();
        }

        public static ComplexZmanimCalendar GetComplexZmanimCalendar(ZmanimLocationProperties locationProperties, DateTime date)
        {
            TimeZone timeZone = TimeZone.getTimeZone(locationProperties.TimeZone);
            var location = new GeoLocation(locationProperties.LocationName,
                                           locationProperties.Latitude, locationProperties.Longitude,
                                           locationProperties.Elevation, timeZone);

            var czc = new ComplexZmanimCalendar(location);

            czc.setCalendar(new GregorianCalendar(date.Year, date.Month - 1, date.Day));
            return czc;
        }

        private static IEnumerable<MethodInfo> ZmainMethods;
        private static DateTime GetZmanMethodFormName(string methodName, ComplexZmanimCalendar zmanimCalendar)
        {
            if (ZmainMethods == null)
            {
                var dateType = typeof(Date);
                /*var longType = typeof(long);*/

                ZmainMethods = typeof(ComplexZmanimCalendar).GetMethods()
                    .Where(m => (m.ReturnType == dateType /*|| m.ReturnType == longType*/)
                                && m.Name.StartsWith("get")
                                && m.IsPublic
                                && m.GetParameters().Count() == 0).ToList();
            }

            var methodInfo = ZmainMethods
                .Where(z => z.Name.ToLowerInvariant() == "get" + methodName.ToLowerInvariant()).First();

            return ((Date)methodInfo.Invoke(zmanimCalendar, null)).ToDateTime();
        }
    }
}