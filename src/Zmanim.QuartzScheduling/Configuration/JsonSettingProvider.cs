using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Zmanim.QuartzScheduling.Configuration
{
    public class JsonSettingProvider : ISettingProvider
    {
        private const string AccountSettingJson = "ApplicationSettings.json";

        #region ISettingProvider Members

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

            string readAllText = File.ReadAllText(AccountSettingJson);

            if (string.IsNullOrEmpty(readAllText))
                return null;

            ApplicationSettings loadAccounts;
            try {
                loadAccounts = JsonConvert.DeserializeObject<ApplicationSettings>(readAllText);
            }
            catch {
                return null;
            }


            return loadAccounts;
        }

        #endregion
    }
}