using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LicnostService.Entities;
using LicnostService.Entities.DataContext;
using Microsoft.EntityFrameworkCore;

namespace LicnostService.Data
{
    public class LicnostRepository :ILicnostRepository
    {
        private readonly LicnostContext _context;
        private readonly IMapper _mapper;

        public LicnostRepository(LicnostContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Licnost>> GetAllLicnosti(string imeLicnosti = null)
        {
            return await _context.Licnost
                .Where(l => (imeLicnosti == null || l.Ime == imeLicnosti))
                .ToListAsync();
        }

        public async Task<Licnost> GetLicnostById(Guid licnostId)
        {
            return await _context.Licnost
                .FirstOrDefaultAsync(l => l.LicnostId == licnostId);
        }

        public async Task<Licnost> CreateLicnost(Licnost licnost)
        {
            _context.Licnost.Add(licnost);
            await _context.SaveChangesAsync();

            return licnost;
        }

        public async Task DeleteLicnost(Guid licnostId)
        {
            var licnost = await GetLicnostById(licnostId);

            _context.Licnost.Remove(licnost);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLicnost(Licnost licnost)
        {
            await _context.SaveChangesAsync();
        }
    }
}
