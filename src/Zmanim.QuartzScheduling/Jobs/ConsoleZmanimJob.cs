using System;
using Quartz;

namespace Zmanim.QuartzScheduling.Jobs
{
    public class ConsoleZmanimJob : IZmanJob
    {
        public void Execute(JobExecutionContext context)
        {
            var reminderServiceJobDetail = (ReminderServiceJobDetail)context.JobDetail;

            var zmanSunset = SchedulerHelper.GetZman(DateTime.UtcNow,
                                                        reminderServiceJobDetail.ReminderService.LocationProperties,
                                                        reminderServiceJobDetail.ReminderService.ZmanName)
                                                        .ToLocalTime();

            Console.WriteLine(string.Format(
                    reminderServiceJobDetail.ReminderService.JobOptions["Message"],
                    zmanSunset.ToShortTimeString())
                );
        }

        public DateTime RunNextJobAt()
        {
            return DateTime.UtcNow.AddDays(1);
        }
    }
}