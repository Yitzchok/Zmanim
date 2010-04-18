using System.Collections.Generic;
using Zmanim.Scheduling;

namespace Zmanim.QuartzScheduling.Configuration
{
    public class ReminderService
    {
        public ReminderService()
        {
            Location = new Location();
            JobOptions = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string AccountId { get; set; }
        public string JobToRun { get; set; }
        public Location Location { get; set; }
        public string ZmanName { get; set; }
        public double AddSeconds { get; set; }
        public bool SkipIfPassedRunBeforeZmanSeconds { get; set; }
        public IDictionary<string, string> JobOptions { get; set; }
    }
}