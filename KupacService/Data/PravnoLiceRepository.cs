using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Entities.DataContext;
using KupacService.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data
{
    public class PravnoLiceRepository : IPravnoLiceRepository
    {
        private readonly KupacContext _context;

        public PravnoLiceRepository(KupacContext context)
        {
            this._context = context;
        }

        public async Task<PravnoLice> CreatePravnoLice(PravnoLice pravnoLice)
        {
            await _context.AddAsync<PravnoLice>(pravnoLice);

            var kupacOvlascenaLicaList = new List<KupacOvlascenoLice>();
            foreach (var ovlascenoLiceId in pravnoLice.OvlascenaLica)
            {
                var kupacOvlascenoLice = new KupacOvlascenoLice()
                {
                    KupacId = pravnoLice.KupacId,
                    OvlascenoLiceId = ovlascenoLiceId
                };
                kupacOvlascenaLicaList.Add(kupacOvlascenoLice);


            }
            await _context.kupacOvlascenoLice.AddRangeAsync(kupacOvlascenaLicaList);

            var kupacUplataList = new List<KupacUplata>();
            foreach (var uplataId in pravnoLice.Uplate)
            {
                var kupacUplata = new KupacUplata()
                {
                    KupacId = pravnoLice.KupacId,
                    UplataId = uplataId
                };
                kupacUplataList.Add(kupacUplata);
            }

            await _context.kupacUplata.AddRangeAsync(kupacUplataList);


            return pravnoLice;        
        }

        public async Task UpdateManyToManyTables(PravnoLice pravnoLice)
        {

            var oldOvlascenaLica = await _context.kupacOvlascenoLice.Where(ko => ko.KupacId == pravnoLice.KupacId).ToListAsync();
            _context.kupacOvlascenoLice.RemoveRange(oldOvlascenaLica);

            List<Guid> ovlascenaLica = pravnoLice.OvlascenaLica;
            var kupacOvlascenaLicaList = new List<KupacOvlascenoLice>();
            foreach (var ovlascenoLiceId in ovlascenaLica)
            {
                var kupacOvlascenoLice = new KupacOvlascenoLice()
                {
                    KupacId = pravnoLice.KupacId,
                    OvlascenoLiceId = ovlascenoLiceId
                };
                kupacOvlascenaLicaList.Add(kupacOvlascenoLice);


            }
            await _context.kupacOvlascenoLice.AddRangeAsync(kupacOvlascenaLicaList);


            var oldUplate = await _context.kupacUplata.Where(ku => ku.KupacId == pravnoLice.KupacId).ToListAsync();
            _context.kupacUplata.RemoveRange(oldUplate);

            List<Guid> uplate = pravnoLice.Uplate;
            var kupacUplataList = new List<KupacUplata>();
            foreach (var uplataId in uplate)
            {
                var kupacUplata = new KupacUplata()
                {
                    KupacId = pravnoLice.KupacId,
                    UplataId = uplataId
                };
                kupacUplataList.Add(kupacUplata);
            }

            await _context.kupacUplata.AddRangeAsync(kupacUplataList);


        }

        public async Task DeletePravnoLice(Guid kupacId)
        {
            var pravnoLice = await GetPravnoLiceById(kupacId);
            _context.Remove(pravnoLice);
        }

        public async Task<List<PravnoLice>> GetPravnoLice(string naziv = null, string maticniBroj = null)
        {
            var pravnaLica = await _context.PravnaLica.Where(p => (naziv == null || p.Naziv == naziv) && 
            (maticniBroj == null || p.MaticniBroj == maticniBroj)).Include(p => p.Prioriteti).Include(ko => ko.KontaktOsoba).ToListAsync();

            foreach (var pravnoLice in pravnaLica)
            {
                pravnoLice.OvlascenaLica = await _context.kupacOvlascenoLice.Where(ko => ko.KupacId == pravnoLice.KupacId).Select(o => o.OvlascenoLiceId).ToListAsync();
                pravnoLice.Uplate = await _context.kupacUplata.Where(ku => ku.KupacId == pravnoLice.KupacId).Select(u => u.UplataId).ToListAsync();

            }

            return pravnaLica;
        }

        public async Task<PravnoLice> GetPravnoLiceById(Guid kupacId)
        {
            var pravnoLice = await _context.PravnaLica.Include(p => p.Prioriteti).Include(ko => ko.KontaktOsoba).FirstOrDefaultAsync<PravnoLice>(p => p.KupacId == kupacId);

            pravnoLice.OvlascenaLica = await _context.kupacOvlascenoLice.Where(ko => ko.KupacId == pravnoLice.KupacId).Select(o => o.OvlascenoLiceId).ToListAsync();
            pravnoLice.Uplate = await _context.kupacUplata.Where(ku => ku.KupacId == pravnoLice.KupacId).Select(u => u.UplataId).ToListAsync();
            return pravnoLice;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
