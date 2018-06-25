using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon;

namespace Lambda.Services
{
    // need to install nuget packages:
    // * AWSSDK.Extensions.NETCore.Setup (and perhaps AWSSDK.S3?)
    public class S3FileService : IS3FileService
    {
        private readonly ILogger _logger;

        public S3FileService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task UploadFile(string filePath, string bucketName, string keyName, RegionEndpoint endpoint)
        {
            //var bucketRegion = RegionEndpoint.APSoutheast2; //Sydney
            //var s3Client = new AmazonS3Client(bucketRegion);
            //var fileTransferUtility = new TransferUtility(s3Client);
            //await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
        }
    }
}
