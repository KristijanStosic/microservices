using KupacService.Entities;
using KupacService.Entities.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data.Interfaces
{
    public interface IKupacRepository
    {

        Task<List<Kupac>> GetKupci();
        Task<Kupac> GetKupacById(Guid kupacId);
        Task<List<KupacOvlascenoLice>> GetKupceByOvlascenoLiceId(Guid ovlascenoLiceId);
        Task<List<KupacUplata>> GetKupceByUplataId(Guid uplataId);


    }
}
