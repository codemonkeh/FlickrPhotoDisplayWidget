using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Lambda.Services
{
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
                    File.Delete(localFilename);

                client.DownloadFile(uri, localFilename);
            }
        }
    }
}
