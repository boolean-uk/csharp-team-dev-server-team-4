namespace exercise.wwwapi.Configuration
{
    public class StagingConfigurationSettings : IConfigurationSettings
    {
        private readonly IConfiguration _configuration;

        public StagingConfigurationSettings()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("StagingAppSettings.json").Build();
        }

        public string GetValue(string key)
        {
            return _configuration.GetValue<string>(key)!;
        }
    }
}