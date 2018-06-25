using System;

namespace Lambda.Services
{
    public interface ILoggingService
    {        
        void Log(string format, params object[] args);
        void LogError(string message, Exception ex = null);
    }
}