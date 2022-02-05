using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LicnostService.Services
{
    public interface ILoggerService
    {
        Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null);
    }
}
