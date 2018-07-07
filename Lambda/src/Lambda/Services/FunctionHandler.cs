using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly ILoggingService _logger;
        private readonly IConfigurationService _configurationService;


        public FunctionHandler(ILoggingService logger, IConfigurationService configurationService, IFlickrService flickrService, IDownloadService downloadService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _flickrService = flickrService ?? throw new ArgumentNullException(nameof(flickrService));
            _downloadService = downloadService ?? throw new ArgumentNullException(nameof(downloadService));
        }

        /// <summary>
        /// Main entry point for the lambda
        /// </summary>
        /// <param name="context"></param>
        public async Task Handle(ILambdaContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            //todo: supply / validate configuration
            //var endPoint = "ap-southeast-2";

            var apiKey = _configurationService.FlickrApiKey;
            var apiSecret = _configurationService.FlickrApiSecret;
            var userId = _configurationService.FlickrUserId;            

            // download file from flickr
            var url = await _flickrService.GetLastUploadedPhotoUrl(apiKey, apiSecret, userId, PhotoSize.Small);
            if (!string.IsNullOrWhiteSpace(url))
            {
                var localFilename = $"/tmp/lastPhoto.jpg";
                _downloadService.DownloadFile(url, localFilename);

                //todo: 
                // resize the file, possibly to multiple different sizes
                // copy file to S3 bucket
            }

            //context.ClientContext.Environment[]
        }
    }
}
