using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.ServiceCalls
{
    public interface ILoggerService
    {
        Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null);
    }
}
