using KupacService.Model.OtherServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.ServiceCalls.Mocks
{
    public class ServiceCallOvlascenoLiceMock<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url)
        {
            var ovlascenolice = new OvlascenoLiceDto
            {
                OvlascenoLice = "Petar Petrović",
                BrojDokumenta = "0224989800025",
                Stanovanje = "Bulevar Oslobodjenja 50 Novi Sad, Srbija"
            };

            return await Task.FromResult((T)Convert.ChangeType(ovlascenolice, typeof(T)));
        }
    }
}
