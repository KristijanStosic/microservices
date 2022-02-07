using JavnoNadmetanjeService.Models.Other;
using System;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls.Mocks
{
    public class ServiceCallAdresaMock<T> : IServiceCall<T>
    {
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
