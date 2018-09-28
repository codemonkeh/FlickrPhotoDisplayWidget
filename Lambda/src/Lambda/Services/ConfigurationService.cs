using System;
using System.Collections.Generic;
using System.Linq;

namespace Lambda.Services
{
    public interface IConfigurationService
    {
        string FlickrApiKey { get; }
        string FlickrApiSecret { get; }
        string FlickrUserId { get; }
        string S3Bucket { get; }
        string AwsRegion { get; }
    }

    public class ConfigurationService : IConfigurationService
    {
        private ILoggingService _log;
        private const string AWS_REGION = "AwsRegion";
        private const string ENV_S3_BUCKET = "S3Bucket";
        private const string ENV_FLICKR_API_KEY = "FlickrApiKey";
        private const string ENV_FLICKR_API_SECRET = "FlickrApiSecret";
        private const string ENV_FLICKR_USER_ID = "FlickrUserId";

        public virtual string FlickrApiKey => GetConfiguredValue(ENV_FLICKR_API_KEY);
        public virtual string FlickrApiSecret => GetConfiguredValue(ENV_FLICKR_API_SECRET);
        public virtual string FlickrUserId => GetConfiguredValue(ENV_FLICKR_USER_ID);
        public virtual string S3Bucket => GetConfiguredValue(ENV_S3_BUCKET);
        public virtual string AwsRegion => GetConfiguredValue(AWS_REGION);

        private IList<string> MandatoryKeys => new[] {FlickrUserId, FlickrApiKey, FlickrApiSecret};

        public ConfigurationService(ILoggingService loggingService)
        {
            _log = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        public virtual string GetConfiguredValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return Environment.GetEnvironmentVariable(key);
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
