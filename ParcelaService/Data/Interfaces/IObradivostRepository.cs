using ParcelaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IObradivostRepository
    {
        Task<List<Obradivost>> GetAllObradivost(string opisObradivosti = null);
        Task<Obradivost> GetObradivostById(Guid obradivostId);
        Task<Obradivost> CreateObradivost(Obradivost obradivost);
        Task DeleteObradivost(Guid obradivostId);
        Task SaveChangesAsync();
    }
}
