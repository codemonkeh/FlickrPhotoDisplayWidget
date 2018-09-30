using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

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
        private readonly IImageService _imageService;
        private readonly ILoggingService _logger;
        private readonly IConfigurationService _configurationService;


        public FunctionHandler(ILoggingService logger, IConfigurationService configurationService, IFlickrService flickrService, 
                               IDownloadService downloadService, IS3FileService s3FileService, IImageService imageService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _flickrService = flickrService ?? throw new ArgumentNullException(nameof(flickrService));
            _downloadService = downloadService ?? throw new ArgumentNullException(nameof(downloadService));
            _s3FileService = s3FileService ?? throw new ArgumentNullException(nameof(s3FileService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        /// <summary>
        /// Main entry point for the lambda
        /// </summary>
        /// <param name="context"></param>
        public async Task Handle(ILambdaContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var apiKey = _configurationService.FlickrApiKey;
            var apiSecret = _configurationService.FlickrApiSecret;
            var userId = _configurationService.FlickrUserId;
            var bucketName = _configurationService.S3Bucket;
            var s3Region = _configurationService.AwsRegion;
            var resizeToWidth = _configurationService.ResizeToWidth;

            // download file from flickr
            var url = await _flickrService.GetLastUploadedPhotoUrl(apiKey, apiSecret, userId, PhotoSize.Small);
            if (!string.IsNullOrWhiteSpace(url))
            {                
                var downloadedFile = _downloadService.DownloadFile(url);
                var resizedFile = _imageService.ResizeImage(downloadedFile, resizeToWidth);

                var key = _configurationService.S3UploadKey;
                await _s3FileService.UploadFile(resizedFile, bucketName, key, s3Region);
            }
        }
    }
}
