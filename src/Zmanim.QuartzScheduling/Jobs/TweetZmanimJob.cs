using System;
using Quartz;
using TweetSharp.Fluent;
using TweetSharp.Model;
using Zmanim.QuartzScheduling.Properties;

namespace Zmanim.QuartzScheduling.Jobs
{
    public class TweetZmanimJob : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            var reminderServiceJobDetail = (ReminderServiceJobDetail) context.JobDetail;

            var simpleTrigger = ((SimpleTrigger) context.Trigger);
            simpleTrigger.StartTimeUtc =
                SchedulerHelper.GetZman(DateTime.Now.AddDays(1),
                                        reminderServiceJobDetail.ReminderService.LocationProperties,
                                        reminderServiceJobDetail.ReminderService.JobToRun).AddMinutes(-30);

            DateTime zmanSunset = SchedulerHelper.GetZman(DateTime.Now,
                                                          reminderServiceJobDetail.ReminderService.LocationProperties,
                                                          reminderServiceJobDetail.ReminderService.JobToRun)
                .ToLocalTime();

            TwitterResult twitter = FluentTwitter.CreateRequest()
                .AuthenticateAs(Settings.Default.TWITTER_USERNAME, Settings.Default.TWITTER_PASSWORD)
                .Statuses().Update(
                    string.Format(
                        reminderServiceJobDetail.ReminderService.JobOptions["Message"],
                        zmanSunset.ToShortTimeString())
                )
                .AsJson().Request();
        }
    }
}