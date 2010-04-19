using System;
using Quartz;
using TweetSharp.Fluent;
using TweetSharp.Model;
using Zmanim.QuartzScheduling.Configuration;
using Zmanim.Scheduling;

namespace Zmanim.QuartzScheduling.Jobs
{
    public class TweetZmanimJob : INextRun, IJob
    {
        public void Execute(JobExecutionContext context)
        {
            var reminderService = context.Get("ReminderService") as ReminderService;
            var account = context.Get("Account") as Account;

            DateTime zmanSunset = SchedulerHelper.GetZman(DateTime.UtcNow,
                                                          reminderService.Location,
                                                          reminderService.ZmanName)
                .ToLocalTime();

            TwitterResult twitter = FluentTwitter.CreateRequest()
                .AuthenticateAs(account.UserName, account.Password)
                .Statuses().Update(
                    string.Format(
                        reminderService.JobOptions["Message"],
                        zmanSunset.ToShortTimeString())
                )
                .AsJson().Request();
        }

        public DateTime RunNextJobAt()
        {
            return DateTime.UtcNow.AddDays(1);
        }
    }
}