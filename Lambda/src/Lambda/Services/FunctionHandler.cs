using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using ImageMagick;

namespace Lambda.Services
{
    public interface IFunctionHandler
    {
        Task Handle(ILambdaContext context);
    }

    public class FunctionHandler : IFunctionHandler
    {
        private readonly IFlickrService _flickrService;
        private readonly IDownloadService _downloadService;
        private readonly IS3FileService _s3FileService;
        private readonly ILoggingService _logger;
        private readonly IConfigurationService _configurationService;


        public FunctionHandler(ILoggingService logger, IConfigurationService configurationService, IFlickrService flickrService, IDownloadService downloadService, IS3FileService s3FileService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _flickrService = flickrService ?? throw new ArgumentNullException(nameof(flickrService));
            _downloadService = downloadService ?? throw new ArgumentNullException(nameof(downloadService));
            _s3FileService = s3FileService;
        }

        /// <summary>
        /// Main entry point for the lambda
        /// </summary>
        /// <param name="context"></param>
        public async Task Handle(ILambdaContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            //todo: supply / validate configuration
            var apiKey = _configurationService.FlickrApiKey;
            var apiSecret = _configurationService.FlickrApiSecret;
            var userId = _configurationService.FlickrUserId;
            var bucketName = _configurationService.S3Bucket;
            var s3Region = _configurationService.AwsRegion;

            // download file from flickr
            var url = await _flickrService.GetLastUploadedPhotoUrl(apiKey, apiSecret, userId, PhotoSize.Small);
            if (!string.IsNullOrWhiteSpace(url))
            {
                const string TEMP_FOLDER = "/tmp";
                var localPath = $"{TEMP_FOLDER}/lastPhoto.jpg";
                _downloadService.DownloadFile(url, localPath);

                //todo: size?
                const int MAX_WIDTH = 150; //todo: set proper size
                
                _logger.Log($"Resizing image...");                        
                var file = new FileInfo(localPath);
                var newFilename = $"{TEMP_FOLDER}/resizedPhoto_w{MAX_WIDTH}.jpg";
                MagickAnyCPU.CacheDirectory = "/tmp";
                using (var image = new MagickImage(file))
                {
                    var factor = image.Width / MAX_WIDTH;
                    int newWidth = image.Width * factor;
                    int newHeight = image.Height * factor;
                    image.Resize(newWidth, newHeight);
                    image.Write(newFilename);
                }                
                _logger.Log($"Resized image saved to \"{newFilename}\".");

                // copy file to S3 bucket

                var key = $"images/flickrLatest.jpg";
                await _s3FileService.UploadFile(newFilename, bucketName, key, s3Region);
            }
        }
    }
}
