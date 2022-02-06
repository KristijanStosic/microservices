using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrijavaService.Data.Interfaces
{
    public interface IPrijavaRepository
    {
        Task<List<Prijava>> GetAllPrijava();
        Task<Prijava> GetPrijavaById(Guid PrijavaId);
        Task<PrijavaConfirmation> CreatePrijava(Prijava prijava);
        Task<Prijava> UpdatePrijava(Prijava prijava);
        Task DeletePrijava(Guid PrijavaId);
        Task SaveChangesAsync();
        Task<bool> IsZatvorenaPrijava(Prijava prijava);
    }
}
