using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Zmanim.Examples.QuartzScheduling.Configuration
{
    public class JsonSettingProvider : ISettingProvider
    {
        const string AccountSettingJson = "ApplicationSettings.json";
        public void Save(ApplicationSettings applicationSettings)
        {
            File.WriteAllText(AccountSettingJson,
                              JsonConvert.SerializeObject(applicationSettings, Formatting.Indented),
                              Encoding.UTF8
                );
        }

        public ApplicationSettings LoadApplicationSettings()
        {
            if (!File.Exists(AccountSettingJson))
                return null;

            var readAllText = File.ReadAllText(AccountSettingJson);

            if (string.IsNullOrEmpty(readAllText))
                return null;

            ApplicationSettings loadAccounts;
            try
            {
                loadAccounts = JsonConvert.DeserializeObject<ApplicationSettings>(readAllText);
            }
            catch
            {
                return null;
            }


            return loadAccounts;
        }
    }
}