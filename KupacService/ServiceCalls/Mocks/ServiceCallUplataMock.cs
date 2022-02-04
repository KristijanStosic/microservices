using KupacService.Model.OtherServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.ServiceCalls.Mocks
{
    public class ServiceCallUplataMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url)
        {
            var uplata = new UplataDto
            {
                BrojRacuna = "123124124",
                PozivNaBroj = "41234",
                SvrhaUplate = "Uplata",
                Iznos = 1000.00,
                Datum = DateTime.Now
            };

            return await Task.FromResult((T)Convert.ChangeType(uplata, typeof(T)));
        }
    }
}
