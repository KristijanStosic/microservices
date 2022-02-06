using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LicnostService.Entities;
using LicnostService.Entities.Confirmations;
using LicnostService.Entities.DataContext;
using Microsoft.EntityFrameworkCore;

namespace LicnostService.Data
{
    public class KomisijaRepository : IKomisijaRepository
    {
        private readonly LicnostContext _context;
        private readonly IMapper _mapper;

        public KomisijaRepository(LicnostContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<List<Komisija>> GetAllKomisije(string nazivKomisije = null)
        {
            return await _context.Komisija
                .Include(p => p.PredsednikKomisije)
                .Include(c => c.ClanoviKomisije)
                 .Where(k => (nazivKomisije == null || k.NazivKomisije == nazivKomisije))
                .ToListAsync();
               
        }


        public async Task<Komisija> GetKomisijeById(Guid komisijaId)
        {
            return await _context.Komisija
                .Include(p => p.PredsednikKomisije)
                .Include(c => c.ClanoviKomisije)
             .FirstOrDefaultAsync(k => k.KomisijaId == komisijaId);
        }

        public async Task<KomisijaConfirmation> CreateKomisija(Komisija komisija)
        {
            var kreiranaKomisija = await _context.Komisija.AddAsync(komisija);

            await _context.SaveChangesAsync();

            return _mapper.Map<KomisijaConfirmation>(kreiranaKomisija.Entity);
        }

        public async Task DeleteKomisija(Guid komisijaId)
        {
            var komisija = await GetKomisijeById(komisijaId);

            _context.Komisija.Remove(komisija);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateKomisija(Komisija komisija)
        {
            await _context.SaveChangesAsync();
        }

    }
}
