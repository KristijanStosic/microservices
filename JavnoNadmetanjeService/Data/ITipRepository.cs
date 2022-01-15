using JavnoNadmetanjeService.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data
{
    public interface ITipRepository
    {
        Task<List<Tip>> GetAllTip(string nazivTipa = null);
        Task<Tip> GetTipById(Guid tipId);
        Task<Tip> CreateTip(Tip tip);
        Task DeleteTip(Guid tipId);
        Task SaveChangesAsync();
    }
}
