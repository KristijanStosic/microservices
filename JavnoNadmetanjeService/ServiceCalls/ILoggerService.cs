using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls
{
    public interface ILoggerService
    {
        Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null);
    }
}
