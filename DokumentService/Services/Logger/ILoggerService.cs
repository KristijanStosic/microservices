using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DokumentService.Services.Logger
{
    public interface ILoggerService
    {
        Task Log(LogLevel level, string method, string message, Exception exception = null);
    }
}