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
    public class KulturaRepository : IKulturaRepository
    {
        private readonly ParcelaContext _context;

        public KulturaRepository(ParcelaContext context)
        {
            _context = context;
        }

        public async Task<Kultura> CreateKultura(Kultura kultura)
        {
            await _context.Kultura.AddAsync(kultura);

            return kultura;
        }

        public async Task DeleteKultura(Guid kulturaId)
        {
            var kultura = await GetKulturaById(kulturaId);

            _context.Kultura.Remove(kultura);
        }

        public async Task<List<Kultura>> GetAllKultura(string nazivKulture = null)
        {
            return await _context.Kultura
                .Where(k => (nazivKulture == null || k.NazivKulture == nazivKulture))
                .ToListAsync();
        }

        public async Task<Kultura> GetKulturaById(Guid kulturaId)
        {
            return await _context.Kultura.FirstOrDefaultAsync(k => k.KulturaId == kulturaId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
