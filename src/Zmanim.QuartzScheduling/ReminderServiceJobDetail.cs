using System;
using Quartz;
using Zmanim.QuartzScheduling.Configuration;

namespace Zmanim.QuartzScheduling
{
    public class ReminderServiceJobDetail : JobDetail
    {
        public ReminderServiceJobDetail() {}
        public ReminderServiceJobDetail(string name, Type jobType) : base(name, jobType) {}
        public ReminderServiceJobDetail(string name, string group, Type jobType) : base(name, group, jobType) {}
        public ReminderServiceJobDetail(string name, string group, Type jobType, bool isVolatile, bool isDurable, bool requestsRecovery)
            : base(name, group, jobType, isVolatile, isDurable, requestsRecovery) {}
        public ReminderService ReminderService { get; set; }
        public Account Account { get; set; }
    }
}