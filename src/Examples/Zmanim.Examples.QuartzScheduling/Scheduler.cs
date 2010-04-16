using System;
using System.Collections.Generic;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using Zmanim.Examples.QuartzScheduling.Configuration;
using System.Linq;

namespace Zmanim.Examples.QuartzScheduling
{
    public class Scheduler
    {
        readonly IScheduler scheduler;
        private readonly ISettingProvider SettingProvider = new JsonSettingProvider();

        public Scheduler()
        {
            var applicationSettings = SettingProvider.LoadApplicationSettings();
            if (applicationSettings == null)
            {
                SettingProvider.Save(new ApplicationSettings());
                Environment.Exit(1);
            }

            scheduler = new StdSchedulerFactory().GetScheduler();

            foreach (var service in applicationSettings.Services)
            {
                SchedulerHelper.ScheduleZmanJob(scheduler, service,
                    applicationSettings.Accounts.Where(a => a.Id == service.AccountId).First()
                    );
            }

            //scheduler.AddCalendar("Shabbos", SetupCalendar(DayOfWeek.Saturday), true, true);
        }


        public void Start() { scheduler.Start(); }

        public void Pause() { scheduler.Standby(); }

        public void Stop() { scheduler.Shutdown(); }

    }
}