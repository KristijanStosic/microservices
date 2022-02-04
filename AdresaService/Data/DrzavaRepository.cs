using AdresaService.Data.Interfaces;
using AdresaService.Entities;
using AdresaService.Entities.DataContext;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AdresaService.Data
{
    public class DrzavaRepository : IDrzavaRepository
    {
        private readonly AdresaContext _context;


        public DrzavaRepository(AdresaContext context)
        {
            this._context = context;
        }



        public async Task<Drzava> CreateDrzava(Drzava drzava)
        {
            await _context.Drzave.AddAsync(drzava);
            return drzava;
        }

        public async Task DeleteDrzava(Guid drzavaId)
        {
            var drzava = await GetDrzavaById(drzavaId);
            _context.Remove(drzava);
            
        }

        public async Task<List<Drzava>> GetAllDrzava(string nazivDrzave = null)
        {
            return await _context.Drzave.Where(d => (nazivDrzave == null || d.NazivDrzave.Contains(nazivDrzave))).ToListAsync<Drzava>();
        }

        public async Task<Drzava> GetDrzavaById(Guid drzavaId)
        {
            return await _context.Drzave.FirstOrDefaultAsync<Drzava>(d => d.DrzavaId == drzavaId);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
