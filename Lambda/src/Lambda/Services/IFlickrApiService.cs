using System.Threading.Tasks;

namespace Lambda
{
    public interface IFlickrApiService
    {
        Task<string> GetLastUploadedPhotoUrl(string userId, PhotoSize size = PhotoSize.Large);
    }
}