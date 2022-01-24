using AdresaService.Data.Interfaces;
using AdresaService.Entities;
using AdresaService.Entities.DataContext;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Data
{
    public class AdresaRepository : IAdresaRepository
    {
        private readonly AdresaContext _context;
        private readonly IMapper _mapper;

        public AdresaRepository(AdresaContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<Adresa> CreateAdresa(Adresa adresa)
        {
            await _context.Adrese.AddAsync(adresa);
            return adresa;
        }

        public async Task DeleteAdresa(Guid adresaId)
        {
            var adresa = await GetAdresaById(adresaId);
            _context.Remove(adresa);
        }

        public async Task<List<Adresa>> GetAdrese(string ulica = null, string mesto = null, string postanskiBroj = null)
        {
            return await _context.Adrese.Where(a => (ulica == null || a.Ulica == ulica) &&
                                              (mesto == null || a.Mesto == mesto)&&
                                              (postanskiBroj == null || a.PostanskiBroj == postanskiBroj))
                                               .Include(d => d.Drzava).ToListAsync();
        }

        public async Task<Adresa> GetAdresaById(Guid adresaId)
        {
            return await _context.Adrese
                .Include(d => d.Drzava).FirstOrDefaultAsync<Adresa>(a => a.AdresaId == adresaId);
        }

        public async  Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
