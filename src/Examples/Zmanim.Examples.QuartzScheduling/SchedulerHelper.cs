using System;
using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using Quartz;
using Zmanim.Extensions;
using TimeZone = java.util.TimeZone;

namespace Zmanim.Examples.QuartzScheduling
{
    public class SchedulerHelper
    {
        public static string LocationName = "Brooklyn, NY";
        public static double Latitude = 40.618851; //Brooklyn, NY
        public static double Longitude = -73.985921; //Brooklyn, NY
        public static double Elevation = 0; //optional elevation

        public static void ScheduleZmanJob(IScheduler scheduler)
        {
            DateTime date = GetNextZmanDateTime();

            var jobDetail = new JobDetail("ZmanimJob", null, typeof(TweetZmanimJob));

            var trigger = new SimpleTrigger("Zman", date.AddMinutes(-30), null, 0, TimeSpan.Zero);

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        public static DateTime GetNextZmanDateTime() {
            var date = GetZman(DateTime.Now);
            if (date < DateTime.UtcNow)
                date = GetZman(DateTime.Now.AddDays(1));

            return date;
        }

        public static DateTime GetZman(DateTime date)
        {
            var timeZone = TimeZone.getTimeZone("America/New_York");
            var location = new GeoLocation(LocationName, Latitude, Longitude, Elevation, timeZone);
            var czc = new ComplexZmanimCalendar(location);

            czc.setCalendar(new GregorianCalendar(date.Year, date.Month - 1, date.Day));

            return czc.getSunset().ToDateTime().ToUniversalTime();
        }
    }
}