using OvlascenoLiceService.Models.OtherServices;
using System;
using System.Threading.Tasks;

namespace OvlascenoLiceService.ServiceCalls.Mocks
{
    /// <summary>
    /// Mock klasa za dobijanje podatka o adresi
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceCallAdresaMock<T> : IServiceCall<T>
    {
        /// <summary>
        /// Metoda za slanje get zahteva - mock
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<T> SendGetRequestAsync(string url, string token)
        {
            var adresa = new AdresaDto
            {
                Ulica = "Bulevar Oslobodjenja",
                Broj = "50",
                Mesto = "Novi Sad",
                PostanskiBroj = "21000",
                Drzava = "Srbija"
            };

            return await Task.FromResult((T)Convert.ChangeType(adresa, typeof(T)));
        }
    }
}