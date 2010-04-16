using System;
using Quartz;

namespace Zmanim.QuartzScheduling.Jobs
{
    public class ConsoleZmanimJob : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            var reminderServiceJobDetail = (ReminderServiceJobDetail)context.JobDetail;

            var simpleTrigger = ((SimpleTrigger)context.Trigger);
            simpleTrigger.StartTimeUtc =
                SchedulerHelper.GetZman(DateTime.Now.AddDays(1),
                                        reminderServiceJobDetail.ReminderService.LocationProperties,
                                        reminderServiceJobDetail.ReminderService.ZmanName).AddMinutes(-30);

            var zmanSunset = SchedulerHelper.GetZman(DateTime.Now,
                                                        reminderServiceJobDetail.ReminderService.LocationProperties,
                                                        reminderServiceJobDetail.ReminderService.ZmanName)
                                                        .ToLocalTime();

            Console.WriteLine(string.Format(
                    reminderServiceJobDetail.ReminderService.JobOptions["Message"],
                    zmanSunset.ToShortTimeString())
                );
        }
    }
}