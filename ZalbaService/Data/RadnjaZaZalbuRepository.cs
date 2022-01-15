using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Entities.DataContext;

namespace ZalbaService.Data
{
    public class RadnjaZaZalbuRepository : IRadnjaZaZalbuRepository
    {
        private readonly ZalbaContext _context;
        private readonly IMapper _mapper;

        public RadnjaZaZalbuRepository(ZalbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RadnjaZaZalbu>> GetAllRadnjeZaZalbu(string nazivRadnjeZaZalbu = null)
        {
            return await _context.RadnjaZaZalbu
                .Where(rz => (nazivRadnjeZaZalbu == null || rz.NazivRadnjeZaZalbu == nazivRadnjeZaZalbu))
                .ToListAsync();
        }

        public async Task<RadnjaZaZalbu> GetRadnjaZaZalbuById(Guid radnjaZaZalbuId)
        {
            return await _context.RadnjaZaZalbu.FirstOrDefaultAsync(rz => rz.RadnjaZaZalbuId == radnjaZaZalbuId);
        }

        public async Task<RadnjaZaZalbu> CreateRadnjaZaZalbu(RadnjaZaZalbu radnjaZaZalbu)
        {
            _context.RadnjaZaZalbu.Add(radnjaZaZalbu);
            await _context.SaveChangesAsync();

            return radnjaZaZalbu;
        }

        public async Task DeleteRadnjaZaZalbu(Guid radnjaZaZalbuId)
        {
            var radnjaZaZalbu = await GetRadnjaZaZalbuById(radnjaZaZalbuId);

            _context.RadnjaZaZalbu.Remove(radnjaZaZalbu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRadnjaZaZalbu(RadnjaZaZalbu radnjaZaZalbu)
        {
            await _context.SaveChangesAsync();
        }
    }
}
