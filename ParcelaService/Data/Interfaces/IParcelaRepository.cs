using ParcelaService.Entities;
using ParcelaService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data.Interfaces
{
    public interface IParcelaRepository
    {
        Task<List<Parcela>> GetAllParcela();
        Task<Parcela> GetParcelaById(Guid parcelaId);
        Task<ParcelaConfirmation> CreateParcela(Parcela parcela);
        Task DeleteParcela(Guid parcelaId);
        Task SaveChangesAsync();

    }
}
