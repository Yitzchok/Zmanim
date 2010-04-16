using System.Collections.Generic;

namespace Zmanim.Examples.QuartzScheduling.Configuration
{
    public class ApplicationSettings
    {
        public ApplicationSettings()
        {
            Accounts = new List<Account>();
        }

        public IList<Account> Accounts { get; set; }
        public IList<ReminderService> Services { get; set; }
    }
}