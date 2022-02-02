using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Entities.DataContext;
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
            if(fizickoLice.Prioriteti!= null && fizickoLice.Prioriteti.Count > 0)
            {
                List<Prioritet> prioriteti = new List<Prioritet>();

                foreach(Prioritet prioritet in fizickoLice.Prioriteti)
                {
                    Prioritet tempPrioritet = await _kupacContext.Prioriteti.FirstOrDefaultAsync<Prioritet>(p => p.PrioritetId == prioritet.PrioritetId);
                    if(tempPrioritet == null)
                    {
                        continue;
                    }
                    prioriteti.Add(tempPrioritet);
                }

                fizickoLice.Prioriteti = prioriteti;
            }

            await _kupacContext.FizickaLica.AddAsync(fizickoLice);
            return fizickoLice;
        }

        public async Task DeleteFizickoLice(Guid kupacId)
        {
            var fizickoLice = await GetFizickoLiceById(kupacId);
     
            _kupacContext.FizickaLica.Remove(fizickoLice);
        }

        public async Task<List<FizickoLice>> GetFizickoLice(string ime = null, string prezime = null, string brojRacuna = null)
        {
            return await   _kupacContext.FizickaLica.Where(f => (ime == null || f.Ime == ime) && 
            (prezime == null || f.Prezime == prezime) && 
            (brojRacuna == null || f.BrojRacuna == brojRacuna)).Include(p => p.Prioriteti).ToListAsync<FizickoLice>();
        }

        public async Task<FizickoLice> GetFizickoLiceById(Guid kupacId)
        {
            return await _kupacContext.FizickaLica.Include(p => p.Prioriteti).FirstOrDefaultAsync(f => f.KupacId == kupacId);
        }

        public async Task SaveChangesAsync()
        {
            await _kupacContext.SaveChangesAsync();
        }
    }
}
