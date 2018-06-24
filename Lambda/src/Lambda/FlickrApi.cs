using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FlickrNet;

namespace Lambda
{
    public enum PhotoSize
    {
        Large = 0,        
        Medium,
        Small,
        Original
    }

    //todo: add integration tests
    //todo: store api details externally (secrets?)
    public class FlickrApi
    {
        private readonly ILogger _logger;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public FlickrApi(ILogger logger, string apiKey, string apiSecret)
        {
            _logger = logger;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public async Task<string> GetLastUploadedPhotoUrl(string userId, PhotoSize size = PhotoSize.Large)
        {
            var service = new Flickr(_apiKey, _apiSecret);

            var options = new PhotoSearchOptions(userId)
            {
                PerPage = 1,
                Extras = PhotoSearchExtras.DateUploaded
            };
            var photos = await service.PhotosSearchAsync(options);

            if (photos.Any())
            {
                _logger.Log($"{photos.Count} photos found.");

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