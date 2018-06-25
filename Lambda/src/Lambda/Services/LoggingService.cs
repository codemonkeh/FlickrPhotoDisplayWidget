using System;

namespace Lambda.Services
{
    /// <summary>
    /// Simple logger wrapper. All messages to the console will be recorded in CloudWatch logs
    /// </summary>
    public class LoggingService : ILoggingService
    {
        public void Log(string format, params object[] args)
        {
            Console.WriteLine($"INFO: {string.Format(format, args)}");
        }

        public void LogError(string message, Exception ex = null)
        {
            Console.WriteLine($"ERROR: {message} { ex?.Message?? string.Empty}");
        }
    }
}
