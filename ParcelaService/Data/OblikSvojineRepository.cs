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
    public class OblikSvojineRepository : IOblikSvojineRepository
    {
        public readonly ParcelaContext _context;

        public OblikSvojineRepository(ParcelaContext context)
        {
            _context = context;
        }

        public async Task<OblikSvojine> CreateOblikSvojine(OblikSvojine oblikSvojine)
        {
            await _context.OblikSvojine.AddAsync(oblikSvojine);

            return oblikSvojine;
        }

        public async Task DeleteOblikSvojine(Guid oblikSvojineId)
        {
            var oblikSvojine = await GetOblikSvojineById(oblikSvojineId);

            _context.OblikSvojine.Remove(oblikSvojine);
        }

        public async Task<List<OblikSvojine>> GetAllOblikSvojine(string opisOblikaSvojine = null)
        {
            return await _context.OblikSvojine
                .Where(os => (opisOblikaSvojine == null || os.OpisOblikaSvojine == opisOblikaSvojine))
                .ToListAsync();
        }

        public async Task<OblikSvojine> GetOblikSvojineById(Guid oblikSvojineId)
        {
            return await _context.OblikSvojine.FirstOrDefaultAsync(os => os.OblikSvojineId == oblikSvojineId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
