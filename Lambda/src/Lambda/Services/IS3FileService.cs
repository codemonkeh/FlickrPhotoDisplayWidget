using System.Threading.Tasks;

namespace Lambda.Services
{
    public interface IS3FileService
    {
        Task UploadFile(string filePath, string bucketName, string keyName, string regionSystemName);
    }
}