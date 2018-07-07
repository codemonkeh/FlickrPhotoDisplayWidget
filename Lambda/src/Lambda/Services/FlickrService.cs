using System;
using System.Linq;
using System.Threading.Tasks;
using FlickrNet;

namespace Lambda.Services
{
    public enum PhotoSize
    {
        Large = 0,        
        Medium,
        Small,
        Original
    }

    // Need to install FlickrNetCore from myget.org
    public class FlickrService : IFlickrService
    {
        private readonly ILoggingService _logger;

        public FlickrService(ILoggingService logger)
        {
            _logger = logger;
        }

        public async Task<string> GetLastUploadedPhotoUrl(string apiKey, string apiSecret, string userId, PhotoSize size = PhotoSize.Large)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            if (apiSecret == null) throw new ArgumentNullException(nameof(apiSecret));
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            var service = new Flickr(apiKey, apiSecret);

            var options = new PhotoSearchOptions(userId)
            {
                PerPage = 1,
                Extras = PhotoSearchExtras.DateUploaded
            };

            _logger.LogDebug(() => $"Using Flickr user id: '{userId}'");
            _logger.Log($"Searching for latest photo ...");
            var photos = await service.PhotosSearchAsync(options);
            _logger.Log($"{photos.Count} photos found.");

            if (photos.Any())
            {                
                var photo = photos[0];
                _logger.Log("Photo Id={0} Title=\"{1}\", Uploaded on {2}, URL={3}", photo.PhotoId, photo.Title,
                    photo.DateUploaded, photo.Medium640Url);

                switch (size)
                {
                    case PhotoSize.Original:
                        return photo.OriginalUrl;
                    case PhotoSize.Medium:
                        return photo.MediumUrl;
                    case PhotoSize.Small:
                        return photo.SmallUrl;
                    default:
                        // you may not be able to access the original version of another user's photo
                        return photo.LargeUrl;
                }
            }

            _logger.Log("No photos found.");
            return null;
        }
    }
}