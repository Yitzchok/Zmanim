using System;
using System.Collections.Generic;
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
            var applicationSettings = SettingProvider.LoadApplicationSettings();
            if (applicationSettings == null)
            {
                SettingProvider.Save(new ApplicationSettings
                {
                    Accounts = new List<Account> { new Account() },
                    Services = new List<ReminderService> { new ReminderService() }
                });

                Environment.Exit(1);
            }

            scheduler = new StdSchedulerFactory().GetScheduler();

            foreach (var service in applicationSettings.Services)
            {
                SchedulerHelper.ScheduleZmanJob(scheduler, service,
                              applicationSettings.Accounts.Where(a => a.Id == service.AccountId).First()
                    );
            }
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