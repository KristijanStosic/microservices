using KupacService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data.Interfaces
{
    public interface IFizickoLiceRepository
    {

        Task<List<FizickoLice>> GetFizickoLice(string ime = null,string prezime = null,string brojRacuna = null);
        Task<FizickoLice> GetFizickoLiceById(Guid kupacId);
        Task<FizickoLice> GetFizickoLiceInfoById(Guid kupacId);
        Task<FizickoLice> CreateFizickoLice(FizickoLice fizickoLice);
        Task DeleteFizickoLice(Guid kupacId);
        Task SaveChangesAsync();
        Task UpdateManyToManyTables(FizickoLice fizickoLice);

    }
}
