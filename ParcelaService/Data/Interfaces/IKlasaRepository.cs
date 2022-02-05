using ParcelaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IKlasaRepository
    {
        Task<List<Klasa>> GetAllKlasa(string KlasaNaziv = null);
        Task<Klasa> GetKlasaById(Guid klasaId);
        Task<Klasa> CreateKlasa(Klasa klasa);
        Task DeleteKlasa(Guid klasaId);
        Task SaveChangesAsync();
    }
}
