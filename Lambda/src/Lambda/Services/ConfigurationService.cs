using System;

namespace Lambda.Services
{
    public interface IConfigurationService
    {
        string FlickrApiKey { get; }
        string FlickrApiSecret { get; }
        string FlickrUserId { get; }
        string S3Bucket { get; }
        string AwsRegion { get; }        
        string TempFolder { get; }
        string S3UploadKey { get; }
        int ResizeToWidth { get; }
    }

    public class ConfigurationService : IConfigurationService
    {
        private ILoggingService _log;
        private const string ENV_S3_UPLOAD_KEY = "S3UploadKey";
        private const string ENV_AWS_REGION = "AwsRegion";
        private const string ENV_S3_BUCKET = "S3Bucket";
        private const string ENV_FLICKR_API_KEY = "FlickrApiKey";
        private const string ENV_FLICKR_API_SECRET = "FlickrApiSecret";
        private const string ENV_FLICKR_USER_ID = "FlickrUserId";
        private const string ENV_RESIZE_TO_WIDTH = "ImageResizeWidth";
        
        public virtual string FlickrApiKey => GetConfiguredValue(ENV_FLICKR_API_KEY);
        public virtual string FlickrApiSecret => GetConfiguredValue(ENV_FLICKR_API_SECRET);
        public virtual string FlickrUserId => GetConfiguredValue(ENV_FLICKR_USER_ID);
        public virtual string S3Bucket => GetConfiguredValue(ENV_S3_BUCKET);
        public virtual string S3UploadKey => GetConfiguredValue(ENV_S3_UPLOAD_KEY);
        public virtual string AwsRegion => GetConfiguredValue(ENV_AWS_REGION);
        public virtual int ResizeToWidth => GetConfiguredIntValue(ENV_RESIZE_TO_WIDTH);        
        public string TempFolder => "/tmp";

        public ConfigurationService(ILoggingService loggingService)
        {
            _log = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public virtual string GetConfiguredValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var value = Environment.GetEnvironmentVariable(key);
            return value ?? throw new ConfigurationException($"No configuration specified for '{key}'");
        }

        public virtual int GetConfiguredIntValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var value = Environment.GetEnvironmentVariable(key);
            if (value != null && int.TryParse(value, out int result))
                return result;

            throw new ConfigurationException($"No configuration or non-integer specified for '{key}'");
        }

        public virtual bool? GetConfiguredBoolValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var value = Environment.GetEnvironmentVariable(key);

            if (value != null && bool.TryParse(value, out bool parsedValue))
                return parsedValue;

            return null;
        }
    }

    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message)
        {
        }
    }
}
