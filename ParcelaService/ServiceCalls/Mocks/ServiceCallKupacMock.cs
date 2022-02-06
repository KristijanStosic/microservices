using ParcelaService.Models.OtherServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.ServiceCalls.Mocks
{
    public class ServiceCallKupacMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url)
        {
            var kupac = new KupacDto
            {
                Kupac = "Milana Milanovic",
                Email = "milana@gmail.com",
                BrojRacuna = "23212345",
                BrojTelefona1 = "065145125"
            };

            return await Task.FromResult((T)Convert.ChangeType(kupac, typeof(T)));
        }
    }
}