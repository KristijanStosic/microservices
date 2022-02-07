using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Services
{
    public interface IServiceCall<T>
    {
        Task<T> SendGetRequestAsync(string url, string token);
    }
}
