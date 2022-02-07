using ParcelaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IKulturaRepository
    {
        Task<List<Kultura>> GetAllKultura(string nazivKulture = null);
        Task<Kultura> GetKulturaById(Guid kulturaId);
        Task<Kultura> CreateKultura(Kultura kultura);
        Task DeleteKultura(Guid kulturaId);
        Task SaveChangesAsync();
    }
}
