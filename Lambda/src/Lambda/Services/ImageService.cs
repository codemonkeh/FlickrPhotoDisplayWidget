using System;
using System.IO;
using ImageMagick;

namespace Lambda.Services
{
    public interface IImageService
    {
        string ResizeImage(string inputPath, int resizeWidth);
    }

    // Need to install
    //  * Magick.NET-Q8-AnyCPU
    public class ImageService : IImageService
    {
        private readonly ILoggingService _logger;
        private readonly IConfigurationService _configurationService;

        public ImageService(ILoggingService logger, IConfigurationService configurationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        public string ResizeImage(string inputPath, int resizeWidth)
        {
            _logger.Log($"Resizing image...");
            var tempFolder = _configurationService.TempFolder;            
            var outputPath = $"{tempFolder}/resizedPhoto_w{resizeWidth}.jpg";
            if (File.Exists(outputPath))
            {
                _logger.LogDebug(() => $"'{outputPath}' exists, attempting to delete...");
                File.Delete(outputPath);
                _logger.LogDebug(() => $"'{outputPath}' deleted.");
            }

            MagickAnyCPU.CacheDirectory = tempFolder;
            var fileInfo = new FileInfo(inputPath);
            using (var image = new MagickImage(fileInfo))
            {
                var factor = image.Width / resizeWidth;
                int newWidth = image.Width * factor;
                int newHeight = image.Height * factor;
                image.Resize(newWidth, newHeight);
                image.Write(outputPath);
            }
            _logger.Log($"Resized image saved to \"{outputPath}\".");

            return outputPath;
        }
    }
}
