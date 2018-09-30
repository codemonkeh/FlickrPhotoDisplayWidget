using System;
using System.IO;
using System.Net;

namespace Lambda.Services
{
    public interface IDownloadService
    {
        void DownloadFile(string url, string localFilename);
        void DownloadFile(Uri uri, string localFilename);
    }

    public class DownloadService : IDownloadService
    {
        private readonly ILoggingService _logger;

        public DownloadService(ILoggingService logger)
        {
            _logger = logger;
        }

        public void DownloadFile(string url, string localFilename)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (localFilename == null) throw new ArgumentNullException(nameof(localFilename));

            DownloadFile(new Uri(url), localFilename);
        }

        public void DownloadFile(Uri uri, string localFilename)
        {            
            using (var client = new WebClient())
            {                
                if (File.Exists(localFilename))
                {
                    _logger.LogDebug(() => $"'{localFilename}' exists, attempting to delete...");
                    File.Delete(localFilename);
                    _logger.LogDebug(() => $"'{localFilename}' deleted.");
                }

                _logger.LogDebug(() => $"Attempting to download from url '{uri.AbsoluteUri}' to file '{localFilename}'");
                client.DownloadFile(uri, localFilename);
                _logger.Log($"Downloaded from url '{uri.AbsoluteUri}' to file '{localFilename}'");
            }
        }
    }
}
