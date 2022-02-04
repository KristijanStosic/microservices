using KupacService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data.Interfaces
{
    public interface IPravnoLiceRepository
    {
        Task<List<PravnoLice>> GetPravnoLice(string naziv = null, string maticniBroj = null);
        Task<PravnoLice> GetPravnoLiceById(Guid kupacId);
        Task<PravnoLice> CreatePravnoLice(PravnoLice pravnoLice);
        Task DeletePravnoLice(Guid kupacId);
        Task SaveChangesAsync();
        Task UpdateKupacOvlascenoLice(PravnoLice pravnoLice);
    }
}
