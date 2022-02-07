using JavnoNadmetanjeService.Models.Other;
using System;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls.Mocks
{
    public class ServiceCallKupacMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url, string token)
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
