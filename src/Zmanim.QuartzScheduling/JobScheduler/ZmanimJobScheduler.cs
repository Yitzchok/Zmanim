using System;
using java.util;
using Quartz;
using Zmanim.Extensions;
using Zmanim.QuartzScheduling.Configuration;
using Zmanim.Scheduling;

namespace Zmanim.QuartzScheduling.JobScheduler
{
    public class ZmanimJobScheduler : IJobScheduler
    {
        public string Name { get; set; }
        public DateTime DateToStart { get; set; }
        public ReminderService ReminderService { get; set; }
        public Account Account { get; set; }

        public ZmanimJobScheduler(string name, DateTime dateToStart, ReminderService reminderService, Account account)
        {
            Name = name;
            DateToStart = dateToStart;
            ReminderService = reminderService;
            Account = account;
        }

        public void Schedule(IScheduler scheduler)
        {
            var jobDetail = new JobDetail(Name, null, SchedulerHelper.GetJobType(ReminderService.JobToRun));
            jobDetail.JobDataMap["ReminderService"] = ReminderService;
            jobDetail.JobDataMap["Account"] = Account;
            jobDetail.JobDataMap["Location"] = ReminderService.Location;

            var trigger = new ZmanimTrigger(Name, 
                                            ReminderService.Location, z => ((Date)SchedulerHelper.GetMethodInfo(ReminderService.ZmanName).Invoke(z, null)).ToDateTime()
                                                                               .AddSeconds(ReminderService.AddSeconds));

            scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}