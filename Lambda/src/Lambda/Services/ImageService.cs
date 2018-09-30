using System.IO;
using ImageMagick;

namespace Lambda.Services
{
    public interface IImageService
    {
        string ResizeImage(string input, int resizeWidth);
    }

    // Need to install
    //  * Magick.NET-Q8-AnyCPU
    public class ImageService : IImageService
    {
        private ILoggingService _logger;
        private readonly IConfigurationService _configurationService;

        public ImageService(ILoggingService logger, IConfigurationService configurationService)
        {
            _logger = logger;
            _configurationService = configurationService;
        }

        public string ResizeImage(string input, int resizeWidth)
        {
            _logger.Log($"Resizing image...");
            var tempFolder = _configurationService.TempFolder;
            var file = new FileInfo(input);
            var newFilename = $"{tempFolder}/resizedPhoto_w{resizeWidth}.jpg";
            if (File.Exists(newFilename)) File.Delete(newFilename);

            MagickAnyCPU.CacheDirectory = tempFolder;
            using (var image = new MagickImage(file))
            {
                var factor = image.Width / resizeWidth;
                int newWidth = image.Width * factor;
                int newHeight = image.Height * factor;
                image.Resize(newWidth, newHeight);
                image.Write(newFilename);
            }
            _logger.Log($"Resized image saved to \"{newFilename}\".");

            return newFilename;
        }
    }
}
