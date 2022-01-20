using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Entities.Confirmations;

namespace ZalbaService.Data.Interfaces
{
    public interface IZalbaRepository
    {
        Task<List<Zalba>> GetAllZalbe();
        Task<Zalba> GetZalbaById(Guid zalbaId);
        Task<ZalbaConfirmation> CreateZalba(Zalba zalba);
        Task DeleteZalba(Guid zalbaId);
        Task UpdateZalba(Zalba zalba);
        Task<bool> IsValidZalba(Zalba zalba);
    }
}
