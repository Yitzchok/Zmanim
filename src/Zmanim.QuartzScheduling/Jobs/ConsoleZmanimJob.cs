using System;
using Quartz;
using Zmanim.QuartzScheduling.Configuration;
using Zmanim.Scheduling;

namespace Zmanim.QuartzScheduling.Jobs
{
    public class ConsoleZmanimJob : INextRun, IJob
    {
        public void Execute(JobExecutionContext context)
        {
            var reminderService = context.Get("ReminderService") as ReminderService;
            
            var zmanSunset = SchedulerHelper.GetZman(DateTime.UtcNow,
                                                        reminderService.Location,
                                                        reminderService.ZmanName)
                                                        .ToLocalTime();

            Console.WriteLine(string.Format(
                    reminderService.JobOptions["Message"],
                    zmanSunset.ToShortTimeString())
                );
        }

        public DateTime RunNextJobAt()
        {
            return DateTime.UtcNow.AddDays(1);
        }
    }
}