using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Entities.ManyToMany;
using JavnoNadmetanjeService.Models.Other;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data
{
    public class JavnoNadmetanjeRepository : IJavnoNadmetanjeRepository
    {
        private readonly JavnoNadmetanjeContext _context;
        private readonly IMapper _mapper;
        public JavnoNadmetanjeRepository(JavnoNadmetanjeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JavnoNadmetanje>> GetAllJavnoNadmetanje()
        {
            var javnaNadmetanja =  await _context.JavnoNadmetanje
                .Include(s => s.Status)
                .Include(t => t.Tip)
                .Include(e => e.Etape)
                .ToListAsync();

            foreach(var javnoNadmetanje in javnaNadmetanja)
            {
                javnoNadmetanje.OvlascenaLica = await _context.JavnoNadmetanjeOvlascenoLice.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanje.JavnoNadmetanjeId).Select(o => o.OvlascenoLiceId).ToListAsync();
                javnoNadmetanje.Kupci = await _context.JavnoNadmetanjeKupac.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanje.JavnoNadmetanjeId).Select(o => o.KupacId).ToListAsync();
                javnoNadmetanje.DeloviParcele = await _context.JavnoNadmetanjeDeoParcele.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanje.JavnoNadmetanjeId).Select(o => o.DeoParceleId).ToListAsync();
            }

            return javnaNadmetanja;
        }

        public async Task<JavnoNadmetanje> GetJavnoNadmetanjeById(Guid javnoNadmetanjeId)
        {
            var javnoNadmetanje = await _context.JavnoNadmetanje
                .Include(s => s.Status)
                .Include(t => t.Tip)
                .Include(e => e.Etape)
                .FirstOrDefaultAsync(jn => jn.JavnoNadmetanjeId == javnoNadmetanjeId);

            if(javnoNadmetanje is not null)
            {
                javnoNadmetanje.OvlascenaLica = new List<Guid>();
                javnoNadmetanje.Kupci = new List<Guid>();
                javnoNadmetanje.DeloviParcele = new List<Guid>();

                javnoNadmetanje.OvlascenaLica = await _context.JavnoNadmetanjeOvlascenoLice.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanjeId).Select(o => o.OvlascenoLiceId).ToListAsync();
                javnoNadmetanje.Kupci = await _context.JavnoNadmetanjeKupac.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanje.JavnoNadmetanjeId).Select(o => o.KupacId).ToListAsync();
                javnoNadmetanje.DeloviParcele = await _context.JavnoNadmetanjeDeoParcele.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanje.JavnoNadmetanjeId).Select(o => o.DeoParceleId).ToListAsync();
            }

            return javnoNadmetanje;
        }

        public async Task<JavnoNadmetanjeConfirmation> CreateJavnoNadmetanje(JavnoNadmetanje javnoNadmetanje)
        {
            if (javnoNadmetanje.IzlicitiranaCena > (javnoNadmetanje.PocetnaCenaHektar * 2))
            {
                javnoNadmetanje.VisinaDopuneDepozita = (int)(javnoNadmetanje.IzlicitiranaCena * 0.5);
            }
            else
            {
                javnoNadmetanje.VisinaDopuneDepozita = (int)(javnoNadmetanje.IzlicitiranaCena * 0.1);
            }

            var kreiranoNadmetanje = await _context.JavnoNadmetanje.AddAsync(javnoNadmetanje);

            //Dodavanje ovlascenih lica u tabelu many-to-many JavnoNadmetanjeOvlascenoLice, JavnoNadmetanjeKupac i JavnoNadmetanjeDeoParcele
            List<Guid> ovlascenaLica = javnoNadmetanje.OvlascenaLica;
            foreach(var ovlascenoLice in ovlascenaLica)
            {
                var javnoNadmetanje_ovlascenoLice = new JavnoNadmetanjeOvlascenoLice
                {
                    JavnoNadmetanjeId = javnoNadmetanje.JavnoNadmetanjeId,
                    OvlascenoLiceId = ovlascenoLice
                };
                await _context.JavnoNadmetanjeOvlascenoLice.AddAsync(javnoNadmetanje_ovlascenoLice);
            }

            List<Guid> kupci = javnoNadmetanje.Kupci;
            foreach (var kupac in kupci)
            {
                var javnoNadmetanje_kupac = new JavnoNadmetanjeKupac
                {
                    JavnoNadmetanjeId = javnoNadmetanje.JavnoNadmetanjeId,
                    KupacId = kupac
                };
                await _context.JavnoNadmetanjeKupac.AddAsync(javnoNadmetanje_kupac);
            }

            List<Guid> delovi = javnoNadmetanje.DeloviParcele;
            foreach (var deo in delovi)
            {
                var javnoNadmetanje_deoParcele = new JavnoNadmetanjeDeoParcele
                {
                    JavnoNadmetanjeId = javnoNadmetanje.JavnoNadmetanjeId,
                    DeoParceleId = deo
                };
                await _context.JavnoNadmetanjeDeoParcele.AddAsync(javnoNadmetanje_deoParcele);
            }

            return _mapper.Map<JavnoNadmetanjeConfirmation>(kreiranoNadmetanje.Entity);
        }

        public async Task<JavnoNadmetanje> UpdateJavnoNadmetanje(JavnoNadmetanje javnoNadmetanje)
        {
            //Izmena ManyToMany tabela
            var stariPodaciOvlascena = await _context.JavnoNadmetanjeOvlascenoLice.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanje.JavnoNadmetanjeId).ToListAsync();
            _context.JavnoNadmetanjeOvlascenoLice.RemoveRange(stariPodaciOvlascena);

            List<Guid> ovlascenaLica = javnoNadmetanje.OvlascenaLica;
            foreach (var ovlascenoLice in ovlascenaLica)
            {
                var javnoNadmetanje_ovlascenoLice = new JavnoNadmetanjeOvlascenoLice
                {
                    JavnoNadmetanjeId = javnoNadmetanje.JavnoNadmetanjeId,
                    OvlascenoLiceId = ovlascenoLice
                };
                await _context.JavnoNadmetanjeOvlascenoLice.AddAsync(javnoNadmetanje_ovlascenoLice);
            }

            var stariPodaciKupci = await _context.JavnoNadmetanjeKupac.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanje.JavnoNadmetanjeId).ToListAsync();
            _context.JavnoNadmetanjeKupac.RemoveRange(stariPodaciKupci);

            List<Guid> kupci = javnoNadmetanje.Kupci;
            foreach (var kupac in kupci)
            {
                var javnoNadmetanje_kupac = new JavnoNadmetanjeKupac
                {
                    JavnoNadmetanjeId = javnoNadmetanje.JavnoNadmetanjeId,
                    KupacId = kupac
                };
                await _context.JavnoNadmetanjeKupac.AddAsync(javnoNadmetanje_kupac);
            }

            var stariPodaciDelovi = await _context.JavnoNadmetanjeDeoParcele.Where(jo => jo.JavnoNadmetanjeId == javnoNadmetanje.JavnoNadmetanjeId).ToListAsync();
            _context.JavnoNadmetanjeDeoParcele.RemoveRange(stariPodaciDelovi);

            List<Guid> delovi = javnoNadmetanje.DeloviParcele;
            foreach (var deo in delovi)
            {
                var javnoNadmetanje_deoParcele = new JavnoNadmetanjeDeoParcele
                {
                    JavnoNadmetanjeId = javnoNadmetanje.JavnoNadmetanjeId,
                    DeoParceleId = deo
                };
                await _context.JavnoNadmetanjeDeoParcele.AddAsync(javnoNadmetanje_deoParcele);
            }

            return javnoNadmetanje;
        }

        public async Task DeleteJavnoNadmetanje(Guid javnoNadmetanjeId)
        {
            var javnoNadmetanje = await GetJavnoNadmetanjeById(javnoNadmetanjeId);

            _context.JavnoNadmetanje.Remove(javnoNadmetanje);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
