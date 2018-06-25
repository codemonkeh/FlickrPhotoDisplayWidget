using System.Threading.Tasks;

namespace Lambda.Services
{
    public interface IFlickrService
    {
        Task<string> GetLastUploadedPhotoUrl(string userId, PhotoSize size = PhotoSize.Large);
    }
}