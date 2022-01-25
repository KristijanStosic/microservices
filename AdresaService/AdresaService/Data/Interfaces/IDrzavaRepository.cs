using AdresaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Data.Interfaces
{
    public interface IDrzavaRepository
    {

        Task<List<Drzava>> GetAllDrzava(string nazivDrzave = null);
        Task<Drzava> GetDrzavaById(Guid drzavaId);
        Task<Drzava> CreateDrzava(Drzava drzava);
        Task DeleteDrzava(Guid drzavaId);
        Task SaveChangesAsync();
    }
}
