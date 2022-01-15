using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;

namespace ZalbaService.Data
{
    public interface IRadnjaZaZalbu
    {
        Task<List<RadnjaZaZalbu>> GetAllRadnjeZaZalbu(string nazivRadnjeZaZalbu = null);
        Task<RadnjaZaZalbu> GetRadnjaZaZalbuById(Guid radnjaZaZalbuId);
        Task<RadnjaZaZalbu> CreateRadnjaZaZalbu(RadnjaZaZalbu radnjaZaZalbu);
        Task DeleteRadnjaZaZalbu(Guid radnjaZaZalbuId);
        Task UpdateRadnjaZaZalbu(RadnjaZaZalbu radnjaZaZalbu);
    }
}
