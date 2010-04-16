using System;
using System.Linq;
using Quartz;
using Quartz.Impl;
using Zmanim.QuartzScheduling.Configuration;

namespace Zmanim.QuartzScheduling
{
    public class Scheduler
    {
        private readonly ISettingProvider SettingProvider = new JsonSettingProvider();
        private readonly IScheduler scheduler;

        public Scheduler()
        {
            ApplicationSettings applicationSettings = SettingProvider.LoadApplicationSettings();
            if (applicationSettings == null) {
                SettingProvider.Save(new ApplicationSettings());
                Environment.Exit(1);
            }

            scheduler = new StdSchedulerFactory().GetScheduler();

            foreach (ReminderService service in applicationSettings.Services) {
                SchedulerHelper.ScheduleZmanJob(scheduler, service,
                                                applicationSettings.Accounts.Where(a => a.Id == service.AccountId).First
                                                    ()
                    );
            }

            //scheduler.AddCalendar("Shabbos", SetupCalendar(DayOfWeek.Saturday), true, true);
        }


        public void Start()
        {
            scheduler.Start();
        }

        public void Pause()
        {
            scheduler.Standby();
        }

        public void Stop()
        {
            scheduler.Shutdown();
        }
    }
}