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
    public class KlasaRepository : IKlasaRepository
    {
        private readonly ParcelaContext _context;

        public KlasaRepository(ParcelaContext context)
        {
            _context = context;
        }

        public async Task<Klasa> CreateKlasa(Klasa klasa)
        {
            await _context.Klasa.AddAsync(klasa);

            return klasa;
        }

        public async Task DeteleKlasa(Guid klasaId)
        {
            var klasa = await GetKlasaById(klasaId);

            _context.Klasa.Remove(klasa);
        }

        public async Task<List<Klasa>> GetAllKlasa(string KlasaNaziv = null)
        {
            return await _context.Klasa
                .Where(k => (KlasaNaziv == null || k.KlasaNaziv == KlasaNaziv))
                .ToListAsync();
        }

        public async Task<Klasa> GetKlasaById(Guid klasaId)
        {
            return await _context.Klasa.FirstOrDefaultAsync(k => k.KlasaId == klasaId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
