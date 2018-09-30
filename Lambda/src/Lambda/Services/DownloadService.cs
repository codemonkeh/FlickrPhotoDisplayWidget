using System;
using System.IO;
using System.Net;

namespace Lambda.Services
{
    public interface IDownloadService
    {
        string DownloadFile(string url);
        string DownloadFile(Uri uri);
    }

    public class DownloadService : IDownloadService
    {
        private readonly ILoggingService _logger;
        private readonly IConfigurationService _configurationService;

        public DownloadService(ILoggingService logger, IConfigurationService configurationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        public string DownloadFile(string url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            return DownloadFile(new Uri(url));
        }

        public string DownloadFile(Uri uri)
        {            
            using (var client = new WebClient())
            {
                var localFilename = $"{_configurationService.TempFolder}/download.jpg";
                if (File.Exists(localFilename))
                {
                    _logger.LogDebug(() => $"'{localFilename}' exists, attempting to delete...");
                    File.Delete(localFilename);
                    _logger.LogDebug(() => $"'{localFilename}' deleted.");
                }

                _logger.LogDebug(() => $"Attempting to download from url '{uri.AbsoluteUri}' to file '{localFilename}'");
                client.DownloadFile(uri, localFilename);
                _logger.Log($"Downloaded from url '{uri.AbsoluteUri}' to file '{localFilename}'");
                return localFilename;
            }
        }
    }
}
