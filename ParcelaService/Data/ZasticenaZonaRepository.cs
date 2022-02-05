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
    public class ZasticenaZonaRepository : IZasticenaZonaRepository
    {
        public readonly ParcelaContext _context;

        public ZasticenaZonaRepository(ParcelaContext context)
        {
            _context = context;
        }

        public async Task<ZasticenaZona> CreateZasticenaZona(ZasticenaZona zasticenaZona)
        {
            await _context.ZasticenaZona.AddAsync(zasticenaZona);

            return zasticenaZona;
        }

        public async Task DeleteZasticenaZona(Guid zasticenaZonaId)
        {
            var zasticenaZona = await GetZasticenaZonaById(zasticenaZonaId);

            _context.ZasticenaZona.Remove(zasticenaZona);
        }

        public async Task<List<ZasticenaZona>> GetAllZasticenaZona(string brojZasticeneZone = null)
        {
            return await _context.ZasticenaZona
                .Where(zz => (brojZasticeneZone == null || zz.BrojZasticeneZone == brojZasticeneZone))
                .ToListAsync();
        }

        public async Task<ZasticenaZona> GetZasticenaZonaById(Guid zasticenaZonaId)
        {
            return await _context.ZasticenaZona.FirstOrDefaultAsync(zz => zz.ZasticenaZonaId == zasticenaZonaId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
