using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Lambda
{
    public class Function
    {
        public ILogger Logger { get; set; } = new Logger();

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(ILambdaContext context)
        {
            try
            {
                //context.ClientContext.Environment[]

                // verify configuration

                // download file from flickr
                
                //using (var client = new WebClient())
                //{
                //    const string FILENAME = "photo.jpg";
                //    var outputFile = $".\\{FILENAME}";
                //    if (File.Exists(outputFile)) File.Delete(outputFile);

                //    client.DownloadFile(new Uri(photo.Medium640Url), outputFile);
                //}

                // resize the file, possibly to multiple different sizes
                // copy file to S3 bucket
            }
            catch (Exception ex)
            {
                Logger.LogError("Unhandled exception", ex);

                // rethrow it, full contextual information will be recorded
                throw;
            }            
        }
    }
}
