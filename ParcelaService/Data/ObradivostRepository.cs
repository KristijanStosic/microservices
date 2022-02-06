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
    public class ObradivostRepository : IObradivostRepository
    {
        public readonly ParcelaContext _context;

        public ObradivostRepository(ParcelaContext context)
        {
            _context = context;
        }

        public async Task<Obradivost> CreateObradivost(Obradivost obradivost)
        {
            await _context.Obradivost.AddAsync(obradivost);

            return obradivost;
        }

        public async Task DeleteObradivost(Guid obradivostId)
        {
            var obradivost = await GetObradivostById(obradivostId);

            _context.Obradivost.Remove(obradivost);
        }

        public async Task<List<Obradivost>> GetAllObradivost(string opisObradivosti = null)
        {
            return await _context.Obradivost
                .Where(o => (opisObradivosti == null || o.OpisObradivosti == opisObradivosti))
                .ToListAsync();
        }

        public async Task<Obradivost> GetObradivostById(Guid obradivostId)
        {
            return await _context.Obradivost.FirstOrDefaultAsync(o => o.ObradivostId == obradivostId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
