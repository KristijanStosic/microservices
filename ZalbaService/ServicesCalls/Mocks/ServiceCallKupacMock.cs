using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Models.Services;

namespace ZalbaService.ServicesCalls
{
    /// <summary>
    /// Mock klasa za dobijanje podatka o kupcu
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceCallKupacMock<T> : IServiceCall<T>
    {
        /// <summary>
        /// Metoda za slanje get zahteva - mock
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<T> SendGetRequestAsync(string url, string token)
        {
            var kupac = new KupacDto
            {
                Kupac = "Bambi DOO",
                Email = "john@email.com",
                BrojRacuna = "155-6984741254580-00",
                BrojTelefona1 = "0609698577"
            };

            return await Task.FromResult((T)Convert.ChangeType(kupac, typeof(T)));
        }
    }
}
