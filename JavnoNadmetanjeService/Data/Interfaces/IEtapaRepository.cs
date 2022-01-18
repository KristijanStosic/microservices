using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data.Interfaces
{
    public interface IEtapaRepository
    {
        Task<List<Etapa>> GetAllEtapa();
        Task<Etapa> GetEtapaById(Guid etapaId);
        Task<EtapaConfirmation> CreateEtapa(Etapa etapa);
        Task DeleteEtapa(Guid etapaId);
        Task SaveChangesAsync();
    }
}
