using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Data.Interfaces;
using ZalbaService.Entities;
using ZalbaService.Entities.Confirmations;
using ZalbaService.Entities.DataContext;

namespace ZalbaService.Data
{
    public class ZalbaRepository : IZalbaRepository
    {
        private readonly ZalbaContext _context;
        private readonly IMapper _mapper;

        public ZalbaRepository(ZalbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Zalba>> GetAllZalbe()
        {
            return await _context.Zalba
                .Include(sz => sz.StatusZalbe)
                .Include(tz => tz.TipZalbe)
                .Include(rz => rz.RadnjaZaZalbu)
                .ToListAsync();
        }

        public async Task<Zalba> GetZalbaById(Guid zalbaId)
        {
            return await _context.Zalba
                .Include(sz => sz.StatusZalbe)
                .Include(tz => tz.TipZalbe)
                .Include(rz => rz.RadnjaZaZalbu)
                .FirstOrDefaultAsync(z => z.ZalbaId == zalbaId);
        }

        public async Task<ZalbaConfirmation> CreateZalba(Zalba zalba)
        {
            var kreiranaZalba = await _context.Zalba.AddAsync(zalba);

            await _context.SaveChangesAsync();

            return _mapper.Map<ZalbaConfirmation>(kreiranaZalba.Entity);
        }

        public async Task DeleteZalba(Guid zalbaId)
        {
            var zalba = await GetZalbaById(zalbaId);

            _context.Zalba.Remove(zalba);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateZalba(Zalba zalba)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsValidZalba(Zalba zalba)
        {
            var zalbe = await _context.Zalba.Where(z => z.BrojResenja == zalba.BrojResenja || z.BrojNadmetanja == zalba.BrojNadmetanja).ToListAsync();

            return zalbe.Count == 0;
        }
    }
}
