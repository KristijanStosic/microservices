using KupacService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data.Interfaces
{
    public interface IPrioritetRepository
    {
        Task<List<Prioritet>> GetPrioritet(string opis = null);
        Task<Prioritet> GetPrioritetById(Guid prioritetId);
        Task<Prioritet> CreatePrioritet(Prioritet prioritet);
        Task DeletePrioritet(Guid prioritetId);
        Task SaveChangesAsync();
    }
}
