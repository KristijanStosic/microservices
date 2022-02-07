using PrijavaService.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.ServiceCalls.Mocks
{
    public class ServiceCallKupacMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url, string token)
        {
            var kupac = new KupacDto
            {
                Kupac = "Mladen Mladic",
                Email = "mladen@gmail.com",
                BrojRacuna = "123123123",
                BrojTelefona = "064222222"
            };

            return await Task.FromResult((T)Convert.ChangeType(kupac, typeof(T)));
        }
    }
}
