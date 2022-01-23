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
        private readonly IMapper _mapper;

        public DrzavaRepository(AdresaContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
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

        public async Task<List<Drzava>> GetAllDrzava()
        {
            return await _context.Drzave.ToListAsync<Drzava>();
        }

        public async Task<Drzava> GetDrzavaById(Guid drzavaId)
        {
            return await _context.Drzave.FirstOrDefaultAsync<Drzava>(d => d.DrzavaId == drzavaId);
        }

        public async Task<Drzava> GetDrzavaByName(string nazivDrzave)
        {
            return await _context.Drzave.FirstOrDefaultAsync<Drzava>(d => d.NazivDrzave == nazivDrzave);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
