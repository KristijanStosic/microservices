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
    /// <summary>
    /// Repozitorijum za žalbu
    /// </summary>
    public class ZalbaRepository : IZalbaRepository
    {
        private readonly ZalbaContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Konstruktor repoa žalbe
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="mapper">AutoMapper</param>
        public ZalbaRepository(ZalbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Dobijanje podataka o svim žalbama
        /// </summary>
        /// <returns></returns>
        public async Task<List<Zalba>> GetAllZalbe()
        {
            return await _context.Zalba
                .Include(sz => sz.StatusZalbe)
                .Include(tz => tz.TipZalbe)
                .Include(rz => rz.RadnjaZaZalbu)
                .ToListAsync();
        }

        /// <summary>
        /// Dobijanje žalbe po id-u
        /// </summary>
        /// <param name="zalbaId">Id žalbe</param>
        /// <returns></returns>
        public async Task<Zalba> GetZalbaById(Guid zalbaId)
        {
            return await _context.Zalba
                .Include(sz => sz.StatusZalbe)
                .Include(tz => tz.TipZalbe)
                .Include(rz => rz.RadnjaZaZalbu)
                .FirstOrDefaultAsync(z => z.ZalbaId == zalbaId);
        }

        /// <summary>
        /// Kreiranje žalbe
        /// </summary>
        /// <param name="zalba">Objekat žalbe</param>
        /// <returns></returns>
        public async Task<ZalbaConfirmation> CreateZalba(Zalba zalba)
        {
            var kreiranaZalba = await _context.Zalba.AddAsync(zalba);

            await _context.SaveChangesAsync();

            return _mapper.Map<ZalbaConfirmation>(kreiranaZalba.Entity);
        }

        /// <summary>
        /// Brisanje žalbe
        /// </summary>
        /// <param name="zalbaId">Id žalbe</param>
        /// <returns></returns>
        public async Task DeleteZalba(Guid zalbaId)
        {
            var zalba = await GetZalbaById(zalbaId);

            _context.Zalba.Remove(zalba);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Cuvanje izmena u bazi podataka
        /// </summary>
        /// <returns></returns>
        public async Task UpdateZalba(Zalba zalba)
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Provera da li dati podatak postoji u bazi, ako postoji vrati false, ako ne onda true
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsValidZalba(Zalba zalba)
        {
            var zalbe = await _context.Zalba.Where(z => z.BrojResenja == zalba.BrojResenja || z.BrojNadmetanja == zalba.BrojNadmetanja).ToListAsync();

            return zalbe.Count == 0;
        }
    }
}
