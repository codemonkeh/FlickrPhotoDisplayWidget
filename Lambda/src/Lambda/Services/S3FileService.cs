using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace Lambda.Services
{
    public interface IS3FileService
    {
        Task UploadFile(string filePath, string bucketName, string keyName, string regionSystemName);
    }

    // need to install nuget packages:
    // * AWSSDK.S3
    // * AWSSDK.Extensions.NETCore.Setup
    public class S3FileService : IS3FileService
    {
        private readonly ILoggingService _logger;

        public S3FileService(ILoggingService logger)
        {
            _logger = logger;
        }

        public async Task UploadFile(string filePath, string bucketName, string keyName, string regionSystemName)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (bucketName == null) throw new ArgumentNullException(nameof(bucketName));
            if (keyName == null) throw new ArgumentNullException(nameof(keyName));
            if (regionSystemName == null) throw new ArgumentNullException(nameof(regionSystemName));

            var endPoint = RegionEndpoint.GetBySystemName(regionSystemName);
            if (endPoint == null) throw new Exception($"Endpoint with system name \"{regionSystemName}\" could not be resolved.");

            await UploadFile(filePath, bucketName, keyName, endPoint);
        }

        public async Task UploadFile(string filePath, string bucketName, string keyName, RegionEndpoint bucketRegion)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (bucketName == null) throw new ArgumentNullException(nameof(bucketName));
            if (keyName == null) throw new ArgumentNullException(nameof(keyName));
            if (bucketRegion == null) throw new ArgumentNullException(nameof(bucketRegion));

            try
            {
                _logger.Log($"Attempting to upload file to S3 with local path \"{filePath}\" to \"{keyName}\"");

                // the client should probably be static if this function was heavily used
                using (var s3Client = new AmazonS3Client(bucketRegion))
                using (var transferUtility = new TransferUtility(s3Client))
                    await transferUtility.UploadAsync(filePath, bucketName, keyName);

                _logger.Log($"File upload complete.");
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError($"Error uploading file with path \"{filePath}\"", ex);
                throw;
            }            
        }
    }
}
