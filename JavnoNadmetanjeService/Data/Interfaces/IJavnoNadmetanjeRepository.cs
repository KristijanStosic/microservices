using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data.Interfaces
{
    public interface IJavnoNadmetanjeRepository
    {
        Task<List<JavnoNadmetanje>> GetAllJavnoNadmetanje();
        Task<JavnoNadmetanje> GetJavnoNadmetanjeById(Guid javnoNadmetanjeId);
        Task<JavnoNadmetanje> GetJavnoNadmetanjeInfoById(Guid javnoNadmetanjeId);
        Task<JavnoNadmetanjeConfirmation> CreateJavnoNadmetanje(JavnoNadmetanje javnoNadmetanje);
        Task<JavnoNadmetanje> UpdateJavnoNadmetanje(JavnoNadmetanje javnoNadmetanje);
        Task DeleteJavnoNadmetanje(Guid javnoNadmetanjeId);
        Task SaveChangesAsync();
    }
}
