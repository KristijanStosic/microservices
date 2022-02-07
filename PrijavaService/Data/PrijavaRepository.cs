using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrijavaService.Data.Interfaces;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Entities.DataContext;
using PrijavaService.Entities.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Data
{
    public class PrijavaRepository : IPrijavaRepository
    {

        private readonly PrijavaContext _context;
        private readonly IMapper _mapper;

        public PrijavaRepository(PrijavaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PrijavaConfirmation> CreatePrijava(Prijava prijava)
        {
            var kreiranaPrijava = await _context.Prijava.AddAsync(prijava);

            List<Guid> javnaNadmetanja = prijava.JavnoNadmetanje;
            foreach(var javnoNadmetanje in javnaNadmetanja)
            {
                var prijava_javnoNadmetanje = new PrijavaJavnoNadmetanje
                {
                    PrijavaId = prijava.PrijavaId,
                    JavnoNadmetanjeId = javnoNadmetanje
                };
                await _context.PrijavaJavnoNadmetanje.AddAsync(prijava_javnoNadmetanje);
            }

            return _mapper.Map<PrijavaConfirmation>(kreiranaPrijava.Entity);
        }

        public async Task DeletePrijava(Guid PrijavaId)
        {
            var prijava = await GetPrijavaById(PrijavaId);

            _context.Prijava.Remove(prijava);
        }

        public async Task<List<Prijava>> GetAllPrijava()
        {
            var prijave = await _context.Prijava.Include(f => f.DokFizickaLica).Include(p => p.DokPravnaLica).ToListAsync();

            foreach(var prijava in prijave)
            {
                prijava.JavnoNadmetanje = await _context.PrijavaJavnoNadmetanje.Where(pr => pr.PrijavaId == prijava.PrijavaId).Select(j => j.JavnoNadmetanjeId).ToListAsync();
            }

            return prijave;
        }

        public async Task<Prijava> GetPrijavaById(Guid PrijavaId)
        {
            var prijava = await _context.Prijava.Include(f => f.DokFizickaLica).Include(p => p.DokPravnaLica).FirstOrDefaultAsync(pr => pr.PrijavaId == PrijavaId);

            prijava.JavnoNadmetanje = new List<Guid>();

            prijava.JavnoNadmetanje = await _context.PrijavaJavnoNadmetanje.Where(pr => pr.PrijavaId == prijava.PrijavaId).Select(j => j.JavnoNadmetanjeId).ToListAsync();

            return prijava;

        }

        public async Task<bool> IsZatvorenaPrijava(Prijava prijava)
        {
            var res = await GetPrijavaById(prijava.PrijavaId);

            return res.ZatvorenaPonuda;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Prijava> UpdatePrijava(Prijava prijava)
        {
            var stariPodaciJavnoNadmetanje = await _context.PrijavaJavnoNadmetanje.Where(pr => pr.PrijavaId == prijava.PrijavaId).ToListAsync();
            _context.PrijavaJavnoNadmetanje.RemoveRange(stariPodaciJavnoNadmetanje);

            List<Guid> javnaNadmetanja = prijava.JavnoNadmetanje;
            foreach (var javnoNadmetanje in javnaNadmetanja)
            {
                var prijava_javnoNadmetanje = new PrijavaJavnoNadmetanje
                {
                    PrijavaId = prijava.PrijavaId,
                    JavnoNadmetanjeId = javnoNadmetanje
                };

                await _context.PrijavaJavnoNadmetanje.AddAsync(prijava_javnoNadmetanje);
            }

            return prijava;
        }
    }
}
