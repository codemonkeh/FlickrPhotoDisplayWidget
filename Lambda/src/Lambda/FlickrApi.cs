using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Lambda
{
    public class FlickrApi
    {
        private readonly ILogger _logger;
        private string _apiKey;
        private string _apiSecret;

        public FlickrApi(ILogger logger, string apiKey, string apiSecret)
        {
            _logger = logger;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public async Task GetLatestPhotoUrl(string userId)
        {
            var service = new Flickr(_apiKey, _apiSecret);

            var options = new PhotoSearchOptions(userId)
            {
                PerPage = 1,
                Extras = PhotoSearchExtras.DateUploaded
            };
            var photos = await service.PhotosSearchAsync(options);

            foreach (Photo photo in photos)
            {
                _logger.Log("Photo ID={0} Title=\"{1}\", Uploaded on {2}, URL={3}", photo.PhotoId, photo.Title, photo.DateUploaded, photo.Medium640Url);

                //using (var client = new WebClient())
                //{
                //    const string FILENAME = "photo.jpg";
                //    var outputFile = $".\\{FILENAME}";
                //    if (File.Exists(outputFile)) File.Delete(outputFile);

                //    client.DownloadFile(new Uri(photo.Medium640Url), outputFile);
                //}
            }
        }
    }
}