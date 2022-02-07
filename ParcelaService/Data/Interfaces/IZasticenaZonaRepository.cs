using ParcelaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IZasticenaZonaRepository
    {
        Task<List<ZasticenaZona>> GetAllZasticenaZona(string brojZasticeneZone = null);
        Task<ZasticenaZona> GetZasticenaZonaById(Guid zasticenaZonaId);
        Task<ZasticenaZona> CreateZasticenaZona(ZasticenaZona zasticenaZona);
        Task DeleteZasticenaZona(Guid zasticenaZonaId);
        Task SaveChangesAsync();
    }
}
