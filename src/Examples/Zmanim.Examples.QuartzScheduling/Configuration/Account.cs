using System.Collections.Generic;

namespace Zmanim.Examples.QuartzScheduling.Configuration
{
    public class Account
    {
        public Account()
        {
            Options = new Dictionary<string, string>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public IDictionary<string, string> Options { get; set; }
    }
}