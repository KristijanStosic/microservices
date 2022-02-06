using LicitacijaService.Models.OtherServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.ServiceCalls.Mocks
{
    public class ServiceCallJavnoNadmetanjeMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url)
        {
            var javnoNadmetanje = new JavnoNadmetanjeDto
            {
                JavnoNadmetanjeId = Guid.Parse("56A7CFF5-CB0A-46B8-BFC1-93DB415FEEB4"),
                PocetnaCenaHektar = 350.50000000,
                VisinaDopuneDepozita = 50,
                PeriodZakupa = 2,
                IzlicitiranaCena = 400,
                BrojUcesnika = 5,
                Krug = 2,
                Izuzeto = false,
            };
            return await Task.FromResult((T)Convert.ChangeType(javnoNadmetanje, typeof(T)));

        }
    }
}
