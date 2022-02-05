using ParcelaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IOblikSvojineRepository
    {
        Task<List<OblikSvojine>> GetAllOblikSvojine(string opisOblikaSvojine = null);
        Task<OblikSvojine> GetOblikSvojineById(Guid oblikSvojineId);
        Task<OblikSvojine> CreateOblikSvojine(OblikSvojine oblikSvojine);
        Task DeleteOblikSvojine(Guid oblikSvojineId);
        Task SaveChangesAsync();
    }
}
