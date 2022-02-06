using JavnoNadmetanjeService.Models.Other;
using System;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls.Mocks
{
    public class ServiceCallDeoParceleMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url)
        {
            var deoParcele = new DeoParceleDto
            {
                BrojParcele = "2345",
                RbDela = 1,
                KatastarskaOpstina = "Palic",
                Klasa = "I",
                Kultura = "Njive",
                Obradivost = "Obradivo",
                Odvodnjavanje = "I",
                PovrsinaDela = 25,
                ZasticenaZona = "2"
            };

            return await Task.FromResult((T)Convert.ChangeType(deoParcele, typeof(T)));
        }
    }
}
