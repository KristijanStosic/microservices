using KupacService.Entities;
using KupacService.Model.Kupac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Helpers
{
    public interface IKupacCalls
    {
        Task<KupacOtherServicesDto> GetKupacDtoWithOtherServicesInfo(Kupac kupac, string token);
    }
}
