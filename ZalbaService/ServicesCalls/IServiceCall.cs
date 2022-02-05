using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.ServicesCalls
{
    /// <summary>
    /// Interfejs za komunikaciju sa drugim servisima
    /// </summary>
    /// <typeparam name="T">Generic</typeparam>
    public interface IServiceCall<T>
    {
        /// <summary>
        /// Metoda za slanje get zahteva
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<T> SendGetRequestAsync(string url);
    }
}
