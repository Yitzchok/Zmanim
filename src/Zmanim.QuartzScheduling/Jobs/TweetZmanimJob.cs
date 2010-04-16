using System;
using Quartz;
using TweetSharp.Fluent;
using TweetSharp.Model;
using Zmanim.QuartzScheduling.Properties;

namespace Zmanim.QuartzScheduling.Jobs
{
    public class TweetZmanimJob : IZmanJob
    {
        public void Execute(JobExecutionContext context)
        {
            var reminderServiceJobDetail = (ReminderServiceJobDetail)context.JobDetail;

            DateTime zmanSunset = SchedulerHelper.GetZman(DateTime.UtcNow,
                                                          reminderServiceJobDetail.ReminderService.LocationProperties,
                                                          reminderServiceJobDetail.ReminderService.ZmanName)
                .ToLocalTime();

            TwitterResult twitter = FluentTwitter.CreateRequest()
                .AuthenticateAs(reminderServiceJobDetail.Account.UserName, reminderServiceJobDetail.Account.Password)
                .Statuses().Update(
                    string.Format(
                        reminderServiceJobDetail.ReminderService.JobOptions["Message"],
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