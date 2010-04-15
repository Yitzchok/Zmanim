using System;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;

namespace Zmanim.Examples.QuartzScheduling
{
    public class Scheduler
    {
        readonly IScheduler scheduler;

        public Scheduler()
        {
            var cal = new WeeklyCalendar();
            cal.SetDayExcluded(DayOfWeek.Saturday, true);

            ISchedulerFactory schedFact = new StdSchedulerFactory();
            scheduler = schedFact.GetScheduler();
            scheduler.AddCalendar("Shabbos", cal, true, true);
            SchedulerHelper.ScheduleZmanJob(scheduler);
        }

        public void Start() { scheduler.Start(); }

        public void Pause() { scheduler.Standby(); }

        public void Stop() { scheduler.Shutdown(); }

    }
}