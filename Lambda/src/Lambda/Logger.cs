using System;

namespace Lambda
{
    public interface ILogger
    {        
        void Log(string format, params object[] args);
        void LogError(string message, Exception ex = null);
    }

    /// <summary>
    /// Simple logger wrapper. All messages to the console will be recorded in CloudWatch logs
    /// </summary>
    public class Logger : ILogger
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
