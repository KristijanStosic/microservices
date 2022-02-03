using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UgovorOZakupu.Services.Logger
{
    public interface ILoggerService
    {
        Task Log(LogLevel level, string method, string message, Exception exception = null);
    }
}