using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Quartz;
using Quartz.Impl.Calendar;
using Zmanim.QuartzScheduling.Configuration;
using Zmanim.Scheduling;
using Zmanim.TimeZone;
using Zmanim.TzDatebase;
using Zmanim.Utilities;

namespace Zmanim.QuartzScheduling
{
    public class SchedulerHelper
    {

        public static void ScheduleZmanJob(IScheduler scheduler, ReminderService reminderService, Account account)
        {
            ScheduleZmanJob(reminderService.Name, DateTime.UtcNow, scheduler, reminderService, account);
        }

        public static void ScheduleZmanJob(string name, DateTime dateToStart, IScheduler scheduler, ReminderService reminderService, Account account)
        {
            DateTime date = GetNextZmanDateTime(dateToStart,
                reminderService.Location,
                reminderService.ZmanName,
                reminderService.SkipIfPassedRunBeforeZmanSeconds ? reminderService.AddSeconds : 0);

            var jobDetail = new JobDetail(name, null,
                GetJobType(reminderService.JobToRun));
            jobDetail.JobDataMap["ReminderService"] = reminderService;
            jobDetail.JobDataMap["Account"] = account;
            jobDetail.JobDataMap["Location"] = reminderService.Location;

            var trigger = new SimpleTrigger(name, date.AddSeconds(reminderService.AddSeconds), date, 0, TimeSpan.Zero);

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        public static Type GetJobType(string jobName)
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
            Location location, string methodName, double addSeconds)
        {
            DateTime date = GetZman(dateToStart, location, methodName);

            if (date.AddSeconds(addSeconds) < DateTime.UtcNow)
                date = GetZman(dateToStart.AddDays(1), location, methodName);

            return date;
        }

        public static DateTime GetZman(
            DateTime date, Location location, string methodName)
        {
            ComplexZmanimCalendar czc = GetComplexZmanimCalendar(location, date);

            return GetZmanMethodFormName(methodName, czc).ToUniversalTime();
        }

        public static ComplexZmanimCalendar GetComplexZmanimCalendar(Location location, DateTime date)
        {
            ITimeZone timeZone = new OlsonTimeZone(location.TimeZone);
            var geoLocation = new GeoLocation(location.LocationName,
                                           location.Latitude, location.Longitude,
                                           location.Elevation, timeZone);

            return new ComplexZmanimCalendar(new DateTime(date.Year, date.Month, date.Day), geoLocation);
        }

        private static IEnumerable<MethodInfo> ZmainMethods;

        public static DateTime GetZmanMethodFormName(string methodName, ComplexZmanimCalendar zmanimCalendar)
        {
            if (ZmainMethods == null)
            {
                var dateType = typeof(DateTime);
                /*var longType = typeof(long);*/

                ZmainMethods = typeof(ComplexZmanimCalendar).GetMethods()
                    .Where(m => (m.ReturnType == dateType /*|| m.ReturnType == longType*/)
                                && m.Name.StartsWith("get")
                                && m.IsPublic
                                && m.GetParameters().Count() == 0).ToList();
            }

            var methodInfo = ZmainMethods
                .Where(z => z.Name.ToLowerInvariant() == "get" + methodName.ToLowerInvariant()).First();

            return (DateTime)methodInfo.Invoke(zmanimCalendar, null);
        }


        public static MethodInfo GetMethodInfo(string methodName)
        {
            if (ZmainMethods == null)
            {
                var dateType = typeof(DateTime);

                ZmainMethods = typeof(ComplexZmanimCalendar).GetMethods()
                    .Where(m => (m.ReturnType == dateType /*|| m.ReturnType == longType*/)
                                && m.Name.StartsWith("get")
                                && m.IsPublic
                                && m.GetParameters().Count() == 0).ToList();
            }

            var methodInfo = ZmainMethods
                .Where(z => z.Name.ToLowerInvariant() == "get" + methodName.ToLowerInvariant()).First();

            return methodInfo;

            //return ((Date)methodInfo.Invoke(zmanimCalendar, null)).ToDateTime();
        }
    }
}