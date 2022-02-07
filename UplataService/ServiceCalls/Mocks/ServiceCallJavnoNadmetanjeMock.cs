using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UplataService.Model.Services;

namespace UplataService.ServiceCalls.Mocks
{
    public class ServiceCallJavnoNadmetanjeMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url, string token)
        {
            var javnoNadmetanje = new JavnoNadmetanjeDto
            {
                PocetnaCenaHektar = 200,
                VisinaDopuneDepozita = 100,
                PeriodZakupa = 3,
                IzlicitiranaCena = 22,
                BrojUcesnika = 11,
                Krug = 3,
                Izuzeto = false,
                Status = "Status Quo",
                Tip = "Tip 1"
            };
            return await Task.FromResult((T)Convert.ChangeType(javnoNadmetanje, typeof(T)));
        }

    }
}
