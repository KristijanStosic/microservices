using KorisnikSistemaService.Data.Interfaces;
using KorisnikSistemaService.Entities;
using KorisnikSistemaService.Entities.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KorisnikSistemaService.Data
{
    public class KorisnikSistemaRepository : IKorisnikSistemaRepository
    {

        private readonly KorisnikSistemaContext _context;
        public KorisnikSistemaRepository(KorisnikSistemaContext context)
        {
            _context = context;
        }

        public async Task<KorisnikSistema> CreateKorisnikSistema(KorisnikSistema korisnikSistema)
        {
            await _context.KorisnikSistema.AddAsync(korisnikSistema);

            return korisnikSistema;
        }

        public async Task DeleteKorisnikSistema(Guid korisnikSistemaId)
        {
            var korisnikSistema = await GetKorisnikSistemaById(korisnikSistemaId);

            _context.KorisnikSistema.Remove(korisnikSistema);
        }

        public async Task<List<KorisnikSistema>> GetAllKorisnikSistema()
        {
            var korisniciSistema = await _context.KorisnikSistema
                .Include(t => t.TipKorisnika).ToListAsync();

            return korisniciSistema;

        }

        public async Task<KorisnikSistema> GetKorisnikSistemaById(Guid korisnikSistemaId)
        {
            var korisnikSistema = await _context.KorisnikSistema
                .Include(t => t.TipKorisnika)
                .FirstOrDefaultAsync(ks => ks.KorisnikSistemaId == korisnikSistemaId);

            return korisnikSistema;
        }
        
        public async Task<KorisnikSistema> GetKorisnikSistemaByKorisnickoIme(string korisnickoIme)
        {
            var korisnikSistema = await _context.KorisnikSistema
                .Include(t => t.TipKorisnika)
                .FirstOrDefaultAsync(ks => ks.KorisnickoIme == korisnickoIme);

            return korisnikSistema;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
