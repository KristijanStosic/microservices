using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Entities.DataContext;
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

        public KupacRepository(IPravnoLiceRepository pravnoLiceRepository,IFizickoLiceRepository fizickoLiceRepository)
        {
            this._pravnoLiceRepository = pravnoLiceRepository;
            this._fizickoLiceRepository = fizickoLiceRepository;
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

        public async Task<Kupac> GetKupacById(Guid kupacId)
        {
            Kupac kupac = await _fizickoLiceRepository.GetFizickoLiceById(kupacId);
            if (kupac == null)
                kupac = await _pravnoLiceRepository.GetPravnoLiceById(kupacId);

            return kupac;
        }

    }
}
