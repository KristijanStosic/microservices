using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;

namespace ZalbaService.Data
{
    public interface ITipZalbeRepository
    {
        Task<List<TipZalbe>> GetAllTipoviZalbe(string nazivTipaZalbe = null);
        Task<TipZalbe> GetTipZalbeById(Guid tipZalbeId);
        Task<TipZalbe> CreateTipZalbe(TipZalbe tipZalbe);
        Task DeleteTipZalbe(Guid tipZalbeId);
        Task UpdateTipZalbe(TipZalbe tipZalbe);
    }
}
