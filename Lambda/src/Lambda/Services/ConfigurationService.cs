using System;

namespace Lambda.Services
{
    public interface IConfigurationService
    {
        string FlickrApiKey { get; }
        string FlickrApiSecret { get; }
        string FlickrUserId { get; }

        string GetConfiguredValue(string key);
    }

    public class ConfigurationService : IConfigurationService
    {
        private const string ENV_FLICKR_API_KEY = "FlickrApiKey";
        private const string ENV_FLICKR_API_SECRET = "FlickrApiSecret";
        private const string ENV_FLICKR_USER_ID = "FlickrUserId";

        public string FlickrApiKey => GetConfiguredValue(ENV_FLICKR_API_KEY);
        public string FlickrApiSecret => GetConfiguredValue(ENV_FLICKR_API_SECRET);
        public string FlickrUserId => GetConfiguredValue(ENV_FLICKR_USER_ID);

        public string GetConfiguredValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Environment.GetEnvironmentVariable(key);
        }
    }
}
