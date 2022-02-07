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
    public class FizickoLiceRepository : IFizickoLiceRepository
    {
        private readonly KupacContext _kupacContext;

        public FizickoLiceRepository(KupacContext context)
        {
            this._kupacContext = context;
        }
        public async Task<FizickoLice> CreateFizickoLice(FizickoLice fizickoLice)
        {

            await _kupacContext.FizickaLica.AddAsync(fizickoLice);

            var kupacOvlascenaLicaList = new List<KupacOvlascenoLice>();
            foreach (var ovlascenoLiceId in fizickoLice.OvlascenaLica)
            {
                var kupacOvlascenoLice = new KupacOvlascenoLice()
                {
                    KupacId = fizickoLice.KupacId,
                    OvlascenoLiceId = ovlascenoLiceId
                };
                kupacOvlascenaLicaList.Add(kupacOvlascenoLice);


            }
            await _kupacContext.kupacOvlascenoLice.AddRangeAsync(kupacOvlascenaLicaList);


            var kupacUplataList = new List<KupacUplata>();
            foreach (var uplataId in fizickoLice.Uplate)
            {
                var kupacUplata = new KupacUplata()
                {
                    KupacId = fizickoLice.KupacId,
                    UplataId = uplataId
                };
                kupacUplataList.Add(kupacUplata);
            }

            await _kupacContext.kupacUplata.AddRangeAsync(kupacUplataList);



            return fizickoLice;
        }

        public async Task UpdateManyToManyTables(FizickoLice fizickoLice)
        {

            var oldOvlascenaLica = await _kupacContext.kupacOvlascenoLice.Where(ko => ko.KupacId == fizickoLice.KupacId).ToListAsync();
            _kupacContext.kupacOvlascenoLice.RemoveRange(oldOvlascenaLica);

            List<Guid> ovlascenaLica = fizickoLice.OvlascenaLica;
            var kupacOvlascenaLicaList = new List<KupacOvlascenoLice>();
            foreach(var ovlascenoLiceId in ovlascenaLica)
            {
                var kupacOvlascenoLice = new KupacOvlascenoLice()
                {
                    KupacId = fizickoLice.KupacId,
                    OvlascenoLiceId = ovlascenoLiceId
                };
                kupacOvlascenaLicaList.Add(kupacOvlascenoLice);

              
            }
            await _kupacContext.kupacOvlascenoLice.AddRangeAsync(kupacOvlascenaLicaList);

            var oldUplate = await _kupacContext.kupacUplata.Where(ku => ku.KupacId == fizickoLice.KupacId).ToListAsync();
            _kupacContext.kupacUplata.RemoveRange(oldUplate);

            List<Guid> uplate = fizickoLice.Uplate;
            var kupacUplataList = new List<KupacUplata>();
            foreach(var uplataId in uplate)
            {
                var kupacUplata = new KupacUplata() 
                {
                    KupacId = fizickoLice.KupacId,
                    UplataId = uplataId
                };
                kupacUplataList.Add(kupacUplata);
            }

            await _kupacContext.kupacUplata.AddRangeAsync(kupacUplataList);


        }

        public async Task DeleteFizickoLice(Guid kupacId)
        {
            var fizickoLice = await GetFizickoLiceById(kupacId);
     
            _kupacContext.FizickaLica.Remove(fizickoLice);
        }

        public async Task<List<FizickoLice>> GetFizickoLice(string ime = null, string prezime = null, string brojRacuna = null)
        {
            var fizickaLica = await   _kupacContext.FizickaLica.Where(f => (ime == null || f.Ime == ime) && 
            (prezime == null || f.Prezime == prezime) && 
            (brojRacuna == null || f.BrojRacuna == brojRacuna)).Include(p => p.Prioriteti).ToListAsync<FizickoLice>();
            
            foreach(var fizickoLice in fizickaLica)
            {
                fizickoLice.OvlascenaLica = await _kupacContext.kupacOvlascenoLice.Where(ko => ko.KupacId == fizickoLice.KupacId).Select(o => o.OvlascenoLiceId).ToListAsync();
                fizickoLice.Uplate = await _kupacContext.kupacUplata.Where(ku => ku.KupacId == fizickoLice.KupacId).Select(u => u.UplataId).ToListAsync();
            }

            return fizickaLica;
        }

        public async Task<FizickoLice> GetFizickoLiceById(Guid kupacId)
        {
            var fizickoLice = await _kupacContext.FizickaLica.Include(p => p.Prioriteti).FirstOrDefaultAsync(f => f.KupacId == kupacId);
            if (fizickoLice == null)
                return null;
            fizickoLice.OvlascenaLica = await _kupacContext.kupacOvlascenoLice.Where(ko => ko.KupacId == fizickoLice.KupacId).Select(o => o.OvlascenoLiceId).ToListAsync();
            fizickoLice.Uplate = await _kupacContext.kupacUplata.Where(ku => ku.KupacId == fizickoLice.KupacId).Select(u => u.UplataId).ToListAsync();
            return fizickoLice;
        }



        public async Task SaveChangesAsync()
        {
            await _kupacContext.SaveChangesAsync();
        }


        
    }
}
