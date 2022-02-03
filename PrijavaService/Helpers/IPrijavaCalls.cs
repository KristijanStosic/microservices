using PrijavaService.Entities;
using PrijavaService.Models.Prijava;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Helpers
{
    public interface IPrijavaCalls
    {
        Task<PrijavaDto> GetPrijvaDtoWithOtherServicesInfo(Prijava prijava);
    }
}
