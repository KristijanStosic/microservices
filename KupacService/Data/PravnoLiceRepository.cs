using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Entities.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data
{
    public class PravnoLiceRepository : IPravnoLiceRepository
    {
        private readonly KupacContext _context;

        public PravnoLiceRepository(KupacContext context)
        {
            this._context = context;
        }

        public async Task<PravnoLice> CreatePravnoLice(PravnoLice pravnoLice)
        {
            await _context.AddAsync<PravnoLice>(pravnoLice);
            return pravnoLice;        }

        public async Task DeletePravnoLice(Guid kupacId)
        {
            var pravnoLice = await GetPravnoLiceById(kupacId);
            _context.Remove(pravnoLice);
        }

        public async Task<List<PravnoLice>> GetPravnoLice(string naziv = null, string maticniBroj = null)
        {
            return await _context.PravnaLica.Where(p => (naziv == null || p.Naziv == naziv) && 
            (maticniBroj == null || p.MaticniBroj == maticniBroj)).Include(p => p.Prioriteti).Include(ko => ko.KontaktOsoba).ToListAsync();
        }

        public async Task<PravnoLice> GetPravnoLiceById(Guid kupacId)
        {
            return await _context.PravnaLica.Include(p => p.Prioriteti).Include(ko => ko.KontaktOsoba).FirstOrDefaultAsync<PravnoLice>(p => p.KupacId == kupacId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
