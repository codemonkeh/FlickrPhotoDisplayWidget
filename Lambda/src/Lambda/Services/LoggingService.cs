using System;

namespace Lambda.Services
{
    public interface ILoggingService
    {
        void Log(string format, params object[] args);
        void LogDebug(Func<string> loggingFunc);
        void LogError(string message, Exception ex = null);
    }

    /// <summary>
    /// Simple logger wrapper. All messages to the console will be recorded in CloudWatch logs
    /// </summary>
    public class LoggingService : ILoggingService
    {
        //todo: specify this from configuration
        private const bool _debugMode = true;

        public void Log(string format, params object[] args)
        {
            if (format != null) 
                Console.WriteLine($"INFO: {string.Format(format, args)}");
        }

        public void LogDebug(Func<string> loggingFunc)
        {
            if (loggingFunc == null) throw new ArgumentNullException(nameof(loggingFunc));
            
            if (_debugMode) Console.WriteLine($"DEBUG: {loggingFunc()}");
        }

        public void LogError(string message, Exception ex = null)
        {
            if (message != null)
                Console.WriteLine($"ERROR: {message} { ex?.Message?? string.Empty}");
        }
    }
}
