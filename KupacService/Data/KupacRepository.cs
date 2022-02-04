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
    public class KupacRepository : IKupacRepository
    {
        private readonly IPravnoLiceRepository _pravnoLiceRepository;
        private readonly IFizickoLiceRepository _fizickoLiceRepository;
        private readonly KupacContext _context;

        public KupacRepository(IPravnoLiceRepository pravnoLiceRepository,IFizickoLiceRepository fizickoLiceRepository,KupacContext context)
        {
            this._pravnoLiceRepository = pravnoLiceRepository;
            this._fizickoLiceRepository = fizickoLiceRepository;
            this._context = context;
        }

       
        public async Task<List<Kupac>> GetKupci()
        {
            List<Kupac> kupci = new List<Kupac>();

            List<PravnoLice> pravnaLica = new List<PravnoLice>();
            List<FizickoLice> fizickaLica = new List<FizickoLice>();

            pravnaLica = await _pravnoLiceRepository.GetPravnoLice();
            if (pravnaLica != null && pravnaLica.Count>0)
                kupci.AddRange(pravnaLica);

            fizickaLica = await _fizickoLiceRepository.GetFizickoLice();
            if(fizickaLica != null && fizickaLica.Count>0)
                kupci.AddRange(fizickaLica);

            return kupci;
        }


        public async Task<List<KupacOvlascenoLice>> GetKupacOvlascenoLiceByOvlascenoLiceId(Guid ovlascenoLiceId)
        {
            return await _context.kupacOvlascenoLice.Where(ko => ko.OvlascenoLiceId == ovlascenoLiceId).ToListAsync<KupacOvlascenoLice>();
        }
        public async Task<Kupac> GetKupacById(Guid kupacId)
        {
            Kupac kupac = await _fizickoLiceRepository.GetFizickoLiceById(kupacId);
            if (kupac == null)
                kupac = await _pravnoLiceRepository.GetPravnoLiceById(kupacId);

            return kupac;
        }

    }
}
