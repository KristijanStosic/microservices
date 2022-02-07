using LicitacijaService.Entities;
using LicitacijaService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Data.Interfaces
{
    public interface ILicitacijaRepository 
    {
        Task<List<Licitacija>> GetAllLicitacija();
        Task<Licitacija> GetLicitacijaById(Guid licitacijaId);
        Task<LicitacijaConfirmation> CreateLicitacija(Licitacija licitacija);
        Task<Licitacija> UpdateLicitacija(Licitacija licitacija);
        Task DeleteLicitacija(Guid licitacijaId);
        Task SaveChangesAsync();
    }
}
