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
    /// Repozitorijum radnje za žalbu
    /// </summary>
    public class RadnjaZaZalbuRepository : IRadnjaZaZalbuRepository
    {
        private readonly ZalbaContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Konstruktor repoa radnje za žalbu
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="mapper">AutoMapper</param>
        public RadnjaZaZalbuRepository(ZalbaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Dobijanje podataka o svim radnjama za žalbu
        /// </summary>
        /// <returns></returns>
        public async Task<List<RadnjaZaZalbu>> GetAllRadnjeZaZalbu(string nazivRadnjeZaZalbu = null)
        {
            return await _context.RadnjaZaZalbu
                .Where(rz => (nazivRadnjeZaZalbu == null || rz.NazivRadnjeZaZalbu == nazivRadnjeZaZalbu))
                .ToListAsync();
        }

        /// <summary>
        /// Dobijanje radnje za žalbu po id-u
        /// </summary>
        /// <param name="radnjaZaZalbuId">Id radnje za žalbu</param>
        /// <returns></returns>
        public async Task<RadnjaZaZalbu> GetRadnjaZaZalbuById(Guid radnjaZaZalbuId)
        {
            return await _context.RadnjaZaZalbu.FirstOrDefaultAsync(rz => rz.RadnjaZaZalbuId == radnjaZaZalbuId);
        }

        /// <summary>
        /// Kreiranje radnje za žalbu
        /// </summary>
        /// <param name="radnjaZaZalbu">Objekat radnje za žalbu</param>
        /// <returns></returns>
        public async Task<RadnjaZaZalbu> CreateRadnjaZaZalbu(RadnjaZaZalbu radnjaZaZalbu)
        {
            _context.RadnjaZaZalbu.Add(radnjaZaZalbu);
            await _context.SaveChangesAsync();

            return radnjaZaZalbu;
        }

        /// <summary>
        /// Brisanje radnje za žalbu
        /// </summary>
        /// <param name="radnjaZaZalbuId">Id radnje za žalbu</param>
        /// <returns></returns>
        public async Task DeleteRadnjaZaZalbu(Guid radnjaZaZalbuId)
        {
            var radnjaZaZalbu = await GetRadnjaZaZalbuById(radnjaZaZalbuId);

            _context.RadnjaZaZalbu.Remove(radnjaZaZalbu);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cuvanje izmena u bazi podataka
        /// </summary>
        /// <returns></returns>
        public async Task UpdateRadnjaZaZalbu(RadnjaZaZalbu radnjaZaZalbu)
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Provera da li dati podatak postoji u bazi, ako postoji vrati false, ako ne onda true
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsValidRadnjaZaZalbu(string nazivRadnjeZaZalbu)
        {
            // proverava se unos istog naziva tipa zalbe

            // pokusaj unosa 123 i ako nemam u bazi on vrati Count 0 i true, u suprotnom Count = n i false
            var listaRadnjiZaZalbu = await _context.RadnjaZaZalbu.Where(rz => rz.NazivRadnjeZaZalbu == nazivRadnjeZaZalbu).ToListAsync();

            return listaRadnjiZaZalbu.Count == 0;
        }
    }
}
