using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Lambda.Core;

namespace Lambda.Services
{
    public class FunctionHandler : IFunctionHandler
    {
        public FunctionHandler(ILoggingService logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Main entry point for the lambda
        /// </summary>
        /// <param name="context"></param>
        public void Handle(ILambdaContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            //context.ClientContext.Environment[]

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

            throw new NotImplementedException();
        }
    }
}
