using OvlascenoLiceService.Models.OtherServices;
using System;
using System.Threading.Tasks;

namespace OvlascenoLiceService.ServiceCalls.Mocks
{
    /// <summary>
    /// Mock klasa za dobijanje podataka o drzavi
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceCallDrzavaMock<T> : IServiceCall<T>
    {
        /// <summary>
        /// Metoda za slanje get zahteva za drzavu - mock
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> SendGetRequestAsync(string url, string token)
        {
            var drzava = new DrzavaDto
            {
                NazivDrzave = "Austrija"
            };

            return await Task.FromResult((T)Convert.ChangeType(drzava, typeof(T)));
        }
    }
}