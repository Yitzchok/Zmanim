using System.Collections.Generic;

namespace Zmanim.QuartzScheduling.Configuration
{
    public class ReminderService
    {
        public ReminderService()
        {
            LocationProperties = new ZmanimLocationProperties();
            JobOptions = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string AccountId { get; set; }
        public string JobToRun { get; set; }
        public ZmanimLocationProperties LocationProperties { get; set; }
        public string ZmanName { get; set; }
        public double AddSeconds { get; set; }
        public bool SkipIfPassedRunBeforeZmanSeconds { get; set; }
        public bool SkipFriday { get; set; }
        public bool SkipShabbos { get; set; }
        public IDictionary<string, string> JobOptions { get; set; }
    }
}