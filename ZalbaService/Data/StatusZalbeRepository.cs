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
    public class StatusZalbeRepository : IStatusZalbeRepository
    {
        private readonly ZalbaContext _context;
        private readonly IMapper _mapper;

        public StatusZalbeRepository(ZalbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StatusZalbe>> GetAllStatusesZalbe(string nazivStatusaZalbe = null)
        {
            return await _context.StatusZalbe
                .Where(sz => (nazivStatusaZalbe == null || sz.NazivStatusaZalbe == nazivStatusaZalbe))
                .ToListAsync();
        }

        public async Task<StatusZalbe> GetStatusZalbeById(Guid statusZalbeId)
        {
            return await _context.StatusZalbe.FirstOrDefaultAsync(sz => sz.StatusZalbeId == statusZalbeId);
        }

        public async Task<StatusZalbe> CreateStatusZalbe(StatusZalbe statusZalbe)
        {
            _context.StatusZalbe.Add(statusZalbe);
            await _context.SaveChangesAsync();

            return statusZalbe;
        }

        public async Task DeleteStatusZalbe(Guid statusZalbeId)
        {
            var statusZalbe = await GetStatusZalbeById(statusZalbeId);

            _context.StatusZalbe.Remove(statusZalbe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusZalbe(StatusZalbe statusZalbe)
        {
            await _context.SaveChangesAsync();
        }
    }
}
