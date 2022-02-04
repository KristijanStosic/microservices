using ParcelaService.Entities;
using ParcelaService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IDeoParceleRepository
    {
        Task<List<DeoParcele>> GetAllDeoParcele();
        Task<DeoParcele> GetDeoParceleById(Guid deoParceleId);
        Task<DeoParceleConfirmation> CreateDeoParcele(DeoParcele deoParcele);
        Task DeleteDeoParcele(Guid parcelaId);
        Task SaveChangesAsync();
    }
}
