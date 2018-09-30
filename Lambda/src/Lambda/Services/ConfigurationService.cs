using System;
using System.Collections.Generic;

namespace Lambda.Services
{
    public interface IConfigurationService
    {
        string FlickrApiKey { get; }
        string FlickrApiSecret { get; }
        string FlickrUserId { get; }
        string S3Bucket { get; }
        string AwsRegion { get; }
        int ResizeToWidth { get; }
        string TempFolder { get; }
    }

    public class ConfigurationService : IConfigurationService
    {
        private ILoggingService _log;        
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
        public virtual string AwsRegion => GetConfiguredValue(ENV_AWS_REGION);
        public virtual int ResizeToWidth => GetConfiguredIntValue(ENV_RESIZE_TO_WIDTH);
        public string TempFolder => "/tmp";

        private IList<string> MandatoryKeys => new[]
        {
            nameof(FlickrUserId), nameof(FlickrApiKey), nameof(FlickrApiSecret), nameof(S3Bucket), nameof(AwsRegion),
            nameof(ResizeToWidth)
        };

        public ConfigurationService(ILoggingService loggingService)
        {
            _log = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public virtual string GetConfiguredValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Environment.GetEnvironmentVariable(key);
        }

        public virtual int GetConfiguredIntValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var value = Environment.GetEnvironmentVariable(key);
            if (value != null && int.TryParse(value, out int result))
                return result;

            throw new Exception($"No configuration found for '{key}'");
        }

        //todo: implement assertion method
        //public bool AssertConfigurationIsValid()
        //{
        //    if (!MandatoryKeys.Any()) _log.LogDebug(() => "No mandatory keys specified.");

        //    MandatoryKeys.ToList().ForEach(k => 
        //    {
        //        if (string.IsNullOrWhiteSpace(GetConfiguredValue(k)))
        //            _log.Log($"Invalid configuration, '{k}' is not configured.");
        //    });
        //}
    }

    public class ConfigurationValidationException : Exception
    {
        public ConfigurationValidationException(string message) : base(message)
        {
        }
    }
}
