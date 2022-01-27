using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OvlascenoLiceService.Data.Interfaces;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using OvlascenoLiceService.Entities.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OvlascenoLiceService.Data
{
    /// <summary>
    /// Repozitorijum za ovlasceno lice
    /// </summary>
    public class OvlascenoLiceRepository : IOvlascenoLiceRepository
    {
        private readonly OvlascenoLiceContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Konstruktor repoa ovlasceno lice
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="mapper">AutoMapper</param>
        public OvlascenoLiceRepository(OvlascenoLiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Dobijanje podataka o svim ovlascenim licima
        /// </summary>
        /// <returns></returns>
        public async Task<List<OvlascenoLice>> GetAllOvlascenoLice(string ime = null, string prezime = null)
        {
            return await _context.OvlascenoLice.Include(b => b.BrojeviTabli).Where(o => (ime == null || o.Ime == ime) &&
                                                        (prezime == null || o.Prezime == prezime)).ToListAsync();
        }
        /// <summary>
        /// Dobijanje ovlascenog lica po id-u
        /// </summary>
        /// <param name="ovlascenoLiceId">Id ovlascenog lica</param>
        /// <returns></returns>
        public async Task<OvlascenoLice> GetOvlascenoLiceById(Guid ovlascenoLiceId)
        {
            return await _context.OvlascenoLice.Include(b => b.BrojeviTabli).FirstOrDefaultAsync(o => o.OvlascenoLiceId == ovlascenoLiceId);
        }
        /// <summary>
        /// Kreiranje ovlascenog lica
        /// </summary>
        /// <param name="ovlascenoLice">Objekat ovlasceno lice</param>
        /// <returns></returns>
        public async Task<OvlascenoLiceConfirmation> CreateOvlascenoLice(OvlascenoLice ovlascenoLice)
        {
            var kreiranoOvlascenoLice = await _context.OvlascenoLice.AddAsync(ovlascenoLice);

            return _mapper.Map<OvlascenoLiceConfirmation>(kreiranoOvlascenoLice.Entity);
        }
        /// <summary>
        /// Brisanje ovlascenog lica
        /// </summary>
        /// <param name="ovlascenoLiceId">Id ovlascenog lica</param>
        /// <returns></returns>
        public async Task DeleteOvlascenoLice(Guid ovlascenoLiceId)
        {
            var ovlascenoLice = await GetOvlascenoLiceById(ovlascenoLiceId);

            _context.OvlascenoLice.Remove(ovlascenoLice);
        }
        /// <summary>
        /// Cuvanje izmena u bazi podataka
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
