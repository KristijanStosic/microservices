using AdresaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Data.Interfaces
{
    public interface IAdresaRepository
    {
        Task<List<Adresa>> GetAdrese(string ulica = null,string mesto = null,string postanskiBroj = null);
        Task<Adresa> GetAdresaById(Guid adresaId);
        Task<Adresa> CreateAdresa(Adresa adresa);
        Task DeleteAdresa(Guid adresaId);
        Task SaveChangesAsync();
    }
}
