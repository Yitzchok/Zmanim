namespace Zmanim.QuartzScheduling.Configuration
{
    public interface ISettingProvider
    {
        void Save(ApplicationSettings applicationSettings);
        ApplicationSettings LoadApplicationSettings();
    }
}