using System.Threading.Tasks;

namespace Lambda.Services
{
    public interface IFlickrService
    {
        Task<string> GetLastUploadedPhotoUrl(string apiKey, string apiSecret, string userId, PhotoSize size = PhotoSize.Large);
    }
}