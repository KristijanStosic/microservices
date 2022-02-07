using AutoMapper;
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
    public class KontaktOsobaRepository : IKontaktOsobaRepository
    {
        private readonly KupacContext _kupacContext;


        public KontaktOsobaRepository(KupacContext kupacContext)
        {
            this._kupacContext = kupacContext;
        }
        public async Task<KontaktOsoba> CreateKontaktOsoba(KontaktOsoba kontaktOsoba)
        {
            await _kupacContext.KontaktOsobe.AddAsync(kontaktOsoba);
            return kontaktOsoba;

        }

        public async Task DeleteKontaktOsoba(Guid KontaktOsobaId)
        {
            var kontaktOsoba = await GetKontaktOsobaById(KontaktOsobaId);
            _kupacContext.KontaktOsobe.Remove(kontaktOsoba);
        }

        public async Task<List<KontaktOsoba>> GetKontaktOsoba(string ime = null, string prezime = null)
        {
            return await _kupacContext.KontaktOsobe.Where(k => (ime == null || k.Ime == ime) && (prezime == null || k.Prezime == prezime)).ToListAsync<KontaktOsoba>();
        }

        public async Task<KontaktOsoba> GetKontaktOsobaById(Guid kontaktOsobaId)
        {
            return await _kupacContext.KontaktOsobe.FirstOrDefaultAsync(k => k.KontaktOsobaId == kontaktOsobaId);
        }

        public async Task SaveChangesAsync()
        {
           await _kupacContext.SaveChangesAsync();
        }
    }
}
