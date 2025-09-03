namespace exercise.wwwapi.Configuration
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        IConfiguration _configuration;

        public ConfigurationSettings()
        {
            var env = Environment.GetEnvironmentVariable("environment");
            if (env is "Staging")
            {
                _configuration = new ConfigurationBuilder().AddJsonFile("StagingAppSettings.json").Build();
            }
            else
            {
                _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            }
        }

        public string GetValue(string key)
        {
            return _configuration.GetValue<string>(key)!;
        }
    }
}