using Microsoft.EntityFrameworkCore;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Entities.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data
{
    public class OdvodnjavanjeRepository : IOdvodnjavanjeRepository
    {
        public readonly ParcelaContext _context;

        public OdvodnjavanjeRepository(ParcelaContext context)
        {
            _context = context;
        }

        public async Task<Odvodnjavanje> CreateOdvodnjavanje(Odvodnjavanje odvodnjavanje)
        {
            await _context.Odvodnjavanje.AddAsync(odvodnjavanje);

            return odvodnjavanje;
        }

        public async Task DeleteOdvodnjavanje(Guid odvodnjavanjeId)
        {
            var odvodnjavanje = await GetOdvodnjavanjeById(odvodnjavanjeId);

            _context.Odvodnjavanje.Remove(odvodnjavanje);
        }

        public async Task<List<Odvodnjavanje>> GetAllOdvodnjavanje(string opisOdvodnjavanja = null)
        {
            return await _context.Odvodnjavanje
                .Where(o => (opisOdvodnjavanja == null || o.OpisOdvodnjavanja == opisOdvodnjavanja))
                .ToListAsync();
        }

        public async Task<Odvodnjavanje> GetOdvodnjavanjeById(Guid odvodnjavanjeId)
        {
            return await _context.Odvodnjavanje.FirstOrDefaultAsync(o => o.OdvodnjavanjeId == odvodnjavanjeId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
