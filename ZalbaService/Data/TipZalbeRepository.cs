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
    /// <summary>
    /// Repozitorijum za tip žalbe
    /// </summary>
    public class TipZalbeRepository : ITipZalbeRepository
    {
        private readonly ZalbaContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Konstruktor repoa tipa žalbe
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="mapper">AutoMapper</param>
        public TipZalbeRepository(ZalbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Dobijanje podataka o svim tipovima žalbi
        /// </summary>
        /// <returns></returns>
        public async Task<List<TipZalbe>> GetAllTipoviZalbe(string nazivTipaZalbe = null)
        {
            return await _context.TipZalbe
                .Where(tz => (nazivTipaZalbe == null || tz.NazivTipaZalbe == nazivTipaZalbe))
                .ToListAsync();
        }

        /// <summary>
        /// Dobijanje tipa žalbe po id-u
        /// </summary>
        /// <param name="tipZalbeId">Id tipa žalbe</param>
        /// <returns></returns>
        public async Task<TipZalbe> GetTipZalbeById(Guid tipZalbeId)
        {
            return await _context.TipZalbe.FirstOrDefaultAsync(tz => tz.TipZalbeId == tipZalbeId);
        }

        /// <summary>
        /// Kreiranje tipa žalbe
        /// </summary>
        /// <param name="tipZalbe">Objekat tipa žalbe</param>
        /// <returns></returns>
        public async Task<TipZalbe> CreateTipZalbe(TipZalbe tipZalbe)
        {
            _context.TipZalbe.Add(tipZalbe);
            await _context.SaveChangesAsync();

            return tipZalbe;
        }

        /// <summary>
        /// Brisanje tipa žalbe
        /// </summary>
        /// <param name="tipZalbeId">Id tipa žalbe</param>
        /// <returns></returns>
        public async Task DeleteTipZalbe(Guid tipZalbeId)
        {
            var tipZalbe = await GetTipZalbeById(tipZalbeId);

            _context.TipZalbe.Remove(tipZalbe);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cuvanje izmena u bazi podataka
        /// </summary>
        /// <returns></returns>
        public async Task UpdateTipZalbe(TipZalbe tipZalbe)
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Provera da li dati podatak postoji u bazi, ako postoji vrati false, ako ne onda true
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsValidTipZalbe(string nazivTipaZalbe)
        {
            // proverava se unos istog naziva tipa zalbe

            // pokusaj unosa 123 i ako nemam u bazi on vrati Count 0 i true, u suprotnom Count = n i false
            var listaTipovaZalbi = await _context.TipZalbe.Where(tz => tz.NazivTipaZalbe == nazivTipaZalbe).ToListAsync();

            return listaTipovaZalbi.Count == 0;
        }
    }
}
