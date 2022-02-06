using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrijavaService.Data.Interfaces
{
     public interface IDokPravnaLicaRepository
    {
        Task<List<DokPravnaLica>> GetAllDokPravnaLica();
        Task<DokPravnaLica> GetDokPravnaLicaById(Guid DokPravnaLicaId);
        Task DeleteDokPravnaLica(Guid DokPravnaLicaId);
        Task<DokPravnaLicaConfirmation> CreateDokPravnaLica(DokPravnaLica dokPravnaLica);
        Task SaveChangesAsync();
    }
}
