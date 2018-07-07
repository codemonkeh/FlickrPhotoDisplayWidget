using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Lambda.Services;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Lambda
{
    public class Function
    {
        public static ServiceProvider Container { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(ILambdaContext context)
        {
            try
            {                
                // configure DI
                RegisterServices();

                var serviceScopeFactory = Container.GetRequiredService<IServiceScopeFactory>();
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    //todo: verify configuration?

                    var processor = scope.ServiceProvider.GetService<IFunctionHandler>();
                    await processor.Handle(context);
                }                
            }
            catch (Exception ex)
            {       
                Console.WriteLine("Unhandled exception");
                // rethrow it, full contextual information will be recorded
                throw;
            }            
        }

        private void RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IConfigurationService, ConfigurationService>();
            serviceCollection.AddScoped<IFunctionHandler, FunctionHandler>();
            serviceCollection.AddScoped<IS3FileService, S3FileService>();
            serviceCollection.AddScoped<IDownloadService, DownloadService>();
            serviceCollection.AddScoped<IFlickrService, FlickrService>();
            serviceCollection.AddScoped<ILoggingService, LoggingService>();
            Container = serviceCollection.BuildServiceProvider();
        }
    }
}
