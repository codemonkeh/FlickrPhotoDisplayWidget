using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Lambda.Tests
{
    public class FlickrApiTest
    {
        private string _flickrApiKey;
        private string _flickrApiSecret;
        private const string FILENAME_SECRETS = "secrets.json";

        public FlickrApiTest()
        {
            // store api information in a secret json file that will not be checked into source control
            // the Lambda will use environmental configuration to store this data
            var json = File.ReadAllText(FILENAME_SECRETS);
            var secretsType = new { flickrApiKey = "", flickrApiSecret = "" };
            var secrets = JsonConvert.DeserializeAnonymousType(json, secretsType);
            _flickrApiKey = secrets.flickrApiKey;
            _flickrApiSecret = secrets.flickrApiSecret;
        }

        [Fact]
        public async Task GetLastUploadedPhotoUrl_FunctionTest_ShouldReturnFileUrl()
        {
            //arrange
            var userId = "christianfroehlich";
            var logger = new Mock<ILogger>();
            var target = new FlickrApiService(logger.Object, _flickrApiKey, _flickrApiSecret);

            //act
            var url = await target.GetLastUploadedPhotoUrl(userId);

            //assert
            // will throw an exception if there is no user
            Assert.NotNull(url);
        }
    }
}
