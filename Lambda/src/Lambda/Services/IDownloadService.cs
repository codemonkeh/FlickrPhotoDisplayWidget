using System;

namespace Lambda.Services
{
    public interface IDownloadService
    {
        void DownloadFile(string url, string localFilename);
        void DownloadFile(Uri uri, string localFilename);
    }
}