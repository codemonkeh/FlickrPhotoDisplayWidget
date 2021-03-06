﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lambda.Services;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Lambda.Tests
{
    public class FlickrApiServiceTest
    {
        private string _flickrApiKey;
        private string _flickrApiSecret;
        private const string FILENAME_SECRETS = "secrets.json";

        public FlickrApiServiceTest()
        {
            // store api information in a secret json file that will not be checked into source control
            // the Lambda will use environmental configuration to store this data
            var json = File.ReadAllText(FILENAME_SECRETS);
            var secretsType = new { flickrApiKey = "", flickrApiSecret = "" };
            var secrets = JsonConvert.DeserializeAnonymousType(json, secretsType);
            _flickrApiKey = secrets.flickrApiKey;
            _flickrApiSecret = secrets.flickrApiSecret;
        }

        /* Unit Tests */
        //todo...

        /* Integration Tests */
        [Fact]
        public async Task GetLastUploadedPhotoUrl_FunctionTest_ShouldReturnFileUrl()
        {
            //arrange
            var userId = "christianfroehlich";
            var logger = new Mock<ILoggingService>();
            var target = new FlickrService(logger.Object);

            //act
            var url = await target.GetLastUploadedPhotoUrl(_flickrApiKey, _flickrApiSecret, userId);

            //assert
            // will throw an exception if there is no user
            Assert.NotNull(url);
        }
    }
}
