using KupacService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data.Interfaces
{
    public interface IFizickoLiceRepository
    {
        /*  
        Task<List<KontaktOsoba>> GetKontaktOsoba(string ime = null, string prezime = null);
        Task<KontaktOsoba> GetKontaktOsobaById(Guid kontaktOsobaId);
        Task<KontaktOsoba> CreateKontaktOsoba(KontaktOsoba kontaktOsoba);
        Task DeleteKontaktOsoba(Guid KontaktOsobaId);
        Task SaveChangesAsync();
        */

        Task<List<FizickoLice>> GetFizickoLice(string ime = null,string prezime = null,string brojRacuna = null);
        Task<FizickoLice> GetFizickoLiceById(Guid kupacId);
        Task<FizickoLice> CreateFizickoLice(FizickoLice fizickoLice);
        Task DeleteFizickoLice(Guid kupacId);
        Task SaveChangesAsync();
    }
}
