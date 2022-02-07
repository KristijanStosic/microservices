using KorisnikSistemaService.Data.Interfaces;
using KorisnikSistemaService.Entities;
using KorisnikSistemaService.Entities.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KorisnikSistemaService.Data
{
    public class TipKorisnikaRepository : ITipKorisnikaRepository
    {
        private readonly KorisnikSistemaContext _context;

        public TipKorisnikaRepository(KorisnikSistemaContext context)
        {
            _context = context;
        }
        public async Task<TipKorisnika> CreateTipKorisnika(TipKorisnika tipKorisnika)
        {
            await _context.TipKorisnika.AddAsync(tipKorisnika);

            return tipKorisnika;
        }

        public async Task DeleteTipKorisnika(Guid tipKorisnikaId)
        {
            var tipKorisnika = await GetTipKorisnikaById(tipKorisnikaId);

            _context.TipKorisnika.Remove(tipKorisnika);
        }

        public async Task<List<TipKorisnika>> GetAllTipKorisnika(string nazivTipaKorisnika = null)
        {
            return await _context.TipKorisnika
                .Where(t => (nazivTipaKorisnika == null || t.NazivTipaKorisnika == nazivTipaKorisnika))
                .ToListAsync();
        }

        public async Task<TipKorisnika> GetTipKorisnikaById(Guid tipKorisnikaId)
        {
            return await _context.TipKorisnika.FirstOrDefaultAsync(t => t.TipKorisnikaId == tipKorisnikaId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
