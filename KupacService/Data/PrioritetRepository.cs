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
    public class PrioritetRepository : IPrioritetRepository
    {
        private readonly KupacContext _context;
        private readonly IMapper _mapper;

        public PrioritetRepository(KupacContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<Prioritet> CreatePrioritet(Prioritet prioritet)
        {
            await _context.Prioriteti.AddAsync(prioritet);
            return prioritet;
        }

        public async Task DeletePrioritet(Guid prioritetId)
        {
            var prioritet = await GetPrioritetById(prioritetId);
            _context.Prioriteti.Remove(prioritet);
        }

        public Task<List<Prioritet>> GetPrioritet(string opis = null)
        {
            return _context.Prioriteti.Where(p => (opis == null || p.Opis.Contains(opis))).ToListAsync();
        }

        public async Task<Prioritet> GetPrioritetById(Guid prioritetId)
        {
            return await _context.Prioriteti.FirstOrDefaultAsync<Prioritet>(p => p.PrioritetId == prioritetId);
        }

       

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
