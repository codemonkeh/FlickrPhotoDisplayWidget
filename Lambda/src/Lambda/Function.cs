using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Lambda
{
    public class Function
    {
        public ILog Logger { get; set; } = new Logger();

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(string input, ILambdaContext context)
        {
            try
            {
                //context.ClientContext.Environment[]

                // verify configuration

                // download file from flickr
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
