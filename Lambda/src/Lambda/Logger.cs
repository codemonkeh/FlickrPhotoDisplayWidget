using System;

namespace Lambda
{
    public interface ILog
    {
        void Log(string message);
        void LogError(string message, Exception ex = null);
    }

    /// <summary>
    /// Simple logger wrapper. All messages to the console will be recorded in CloudWatch logs
    /// </summary>
    public class Logger : ILog
    {
        public void Log(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }

        public void LogError(string message, Exception ex = null)
        {
            Console.WriteLine($"ERROR: {message} { ex?.Message?? string.Empty}");
        }
    }
}
