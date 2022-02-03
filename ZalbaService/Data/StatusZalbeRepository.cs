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
    /// Repozitorijum za status žalbe
    /// </summary>
    public class StatusZalbeRepository : IStatusZalbeRepository
    {
        private readonly ZalbaContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Konstruktor repoa statusa žalbe
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="mapper">AutoMapper</param>
        public StatusZalbeRepository(ZalbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Dobijanje podataka o svim statusima žalbe
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatusZalbe>> GetAllStatusesZalbe(string nazivStatusaZalbe = null)
        {
            return await _context.StatusZalbe
                .Where(sz => (nazivStatusaZalbe == null || sz.NazivStatusaZalbe == nazivStatusaZalbe))
                .ToListAsync();
        }

        /// <summary>
        /// Dobijanje statusa žalbe po id-u
        /// </summary>
        /// <param name="statusZalbeId">Id statusa žalbe</param>
        /// <returns></returns>
        public async Task<StatusZalbe> GetStatusZalbeById(Guid statusZalbeId)
        {
            return await _context.StatusZalbe.FirstOrDefaultAsync(sz => sz.StatusZalbeId == statusZalbeId);
        }

        /// <summary>
        /// Kreiranje statusa žalbe
        /// </summary>
        /// <param name="statusZalbe">Objekat statusa žalbe</param>
        /// <returns></returns>
        public async Task<StatusZalbe> CreateStatusZalbe(StatusZalbe statusZalbe)
        {
            _context.StatusZalbe.Add(statusZalbe);
            await _context.SaveChangesAsync();

            return statusZalbe;
        }

        /// <summary>
        /// Brisanje statusa žalbe
        /// </summary>
        /// <param name="statusZalbeId">Id statusa žalbe</param>
        /// <returns></returns>
        public async Task DeleteStatusZalbe(Guid statusZalbeId)
        {
            var statusZalbe = await GetStatusZalbeById(statusZalbeId);

            _context.StatusZalbe.Remove(statusZalbe);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cuvanje izmena u bazi podataka
        /// </summary>
        /// <returns></returns>
        public async Task UpdateStatusZalbe(StatusZalbe statusZalbe)
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Provera da li dati podatak postoji u bazi, ako postoji vrati false, ako ne onda true
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsValidStatusZalbe(string nazivStatusaZalbe)
        {
            // proverava se unos istog naziva tipa zalbe

            // pokusaj unosa 123 i ako nemam u bazi on vrati Count 0 i true, u suprotnom Count = n i false
            var listaStatusaZalbi = await _context.StatusZalbe.Where(sz => sz.NazivStatusaZalbe == nazivStatusaZalbe).ToListAsync();

            return listaStatusaZalbi.Count == 0;
        }
    }
}
