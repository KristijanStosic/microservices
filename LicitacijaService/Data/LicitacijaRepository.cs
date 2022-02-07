using AutoMapper;
using LicitacijaService.Data.Interfaces;
using LicitacijaService.Entities;
using LicitacijaService.Entities.Confirmations;
using LicitacijaService.Entities.DataContext;
using LicitacijaService.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Data
{
    public class LicitacijaRepository : ILicitacijaRepository
    {
        private readonly LicitacijaContext _context;
        private readonly IMapper _mapper;

        public LicitacijaRepository(LicitacijaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LicitacijaConfirmation> CreateLicitacija(Licitacija licitacija)
        {
            var kreiranaLicitacija = await _context.Licitacija.AddAsync(licitacija);


            List<Guid> javnaNadmetanja = licitacija.JavnaNadmetanja;
            foreach(var nadmetanje in javnaNadmetanja)
            {
                var licitacija_javnoNadmetanje = new LicitacijaJavnoNadmetanje
                {
                    LicitacijaId = licitacija.LicitacijaId,
                    JavnoNadmetanjeId = nadmetanje
                };
                await _context.LicitacijaJavnoNadmetanje.AddAsync(licitacija_javnoNadmetanje);
            }


            return _mapper.Map<LicitacijaConfirmation>(kreiranaLicitacija.Entity);
        }

        public async Task DeleteLicitacija(Guid licitacijaId)
        {
            var licitacija = await GetLicitacijaById(licitacijaId);

            _context.Licitacija.Remove(licitacija);
        }

        public async Task<List<Licitacija>> GetAllLicitacija()
        {
            var licitacije = await _context.Licitacija.ToListAsync();
            foreach(var licitacija in licitacije)
            {
                licitacija.JavnaNadmetanja = await _context.LicitacijaJavnoNadmetanje.Where(ljm => ljm.LicitacijaId == licitacija.LicitacijaId).Select(jn => jn.JavnoNadmetanjeId).ToListAsync();
            }
            return licitacije;
        }

        public async Task<Licitacija> GetLicitacijaById(Guid licitacijaId)
        {
            var licitacija = await _context.Licitacija.FirstOrDefaultAsync(l => l.LicitacijaId == licitacijaId);

            licitacija.JavnaNadmetanja = new List<Guid>();

            licitacija.JavnaNadmetanja = await _context.LicitacijaJavnoNadmetanje.Where(ljm => ljm.LicitacijaId == licitacija.LicitacijaId).Select(jn => jn.JavnoNadmetanjeId).ToListAsync();

            return licitacija;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Licitacija> UpdateLicitacija(Licitacija licitacija)
        {
            //ManyToMany tabela
            var stariPodaciJavnoNadmetanje = await _context.LicitacijaJavnoNadmetanje.Where(ljn => ljn.LicitacijaId == licitacija.LicitacijaId).ToListAsync();
            _context.LicitacijaJavnoNadmetanje.RemoveRange(stariPodaciJavnoNadmetanje);

            List<Guid> javnaNadmetanja = licitacija.JavnaNadmetanja;
            foreach(var javnoNadmetanje in javnaNadmetanja)
            {
                var licitacija_javnoNadmetanje = new LicitacijaJavnoNadmetanje
                {
                    LicitacijaId = licitacija.LicitacijaId,
                    JavnoNadmetanjeId = javnoNadmetanje
                };
                await _context.LicitacijaJavnoNadmetanje.AddAsync(licitacija_javnoNadmetanje);
            }
            return licitacija;
        }
        
    }
}
