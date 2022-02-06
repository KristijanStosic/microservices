using ParcelaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IOdvodnjavanjeRepository
    {
        Task<List<Odvodnjavanje>> GetAllOdvodnjavanje(string opisOdvodnjavanja = null);
        Task<Odvodnjavanje> GetOdvodnjavanjeById(Guid odvodnjavanjeId);
        Task<Odvodnjavanje> CreateOdvodnjavanje(Odvodnjavanje odvodnjavanje);
        Task DeleteOdvodnjavanje(Guid odvodnjavanjeId);
        Task SaveChangesAsync();
    }
}
