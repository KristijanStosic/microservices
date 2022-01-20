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
    public class TipZalbeRepository : ITipZalbeRepository
    {
        private readonly ZalbaContext _context;
        private readonly IMapper _mapper;

        public TipZalbeRepository(ZalbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TipZalbe>> GetAllTipoviZalbe(string nazivTipaZalbe = null)
        {
            return await _context.TipZalbe
                .Where(tz => (nazivTipaZalbe == null || tz.NazivTipaZalbe == nazivTipaZalbe))
                .ToListAsync();
        }

        public async Task<TipZalbe> GetTipZalbeById(Guid tipZalbeId)
        {
            return await _context.TipZalbe.FirstOrDefaultAsync(tz => tz.TipZalbeId == tipZalbeId);
        }

        public async Task<TipZalbe> CreateTipZalbe(TipZalbe tipZalbe)
        {
            _context.TipZalbe.Add(tipZalbe);
            await _context.SaveChangesAsync();

            return tipZalbe;
        }

        public async Task DeleteTipZalbe(Guid tipZalbeId)
        {
            var tipZalbe = await GetTipZalbeById(tipZalbeId);

            _context.TipZalbe.Remove(tipZalbe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTipZalbe(TipZalbe tipZalbe)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsValidTipZalbe(string nazivTipaZalbe)
        {
            // proverava se unos istog naziva tipa zalbe

            // pokusaj unosa 123 i ako nemam u bazi on vrati Count 0 i true, u suprotnom Count = n i false
            var listaTipovaZalbi = await _context.TipZalbe.Where(tz => tz.NazivTipaZalbe == nazivTipaZalbe).ToListAsync();

            return listaTipovaZalbi.Count == 0;
        }
    }
}
