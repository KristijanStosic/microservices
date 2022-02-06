using KorisnikSistemaService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KorisnikSistemaService.Data.Interfaces
{
    public interface ITipKorisnikaRepository
    {
        Task<List<TipKorisnika>> GetAllTipKorisnika(string nazivTipaKorisnika = null);
        Task<TipKorisnika> GetTipKorisnikaById(Guid tipKorisnikaId);
        Task<TipKorisnika> CreateTipKorisnika(TipKorisnika tipKorisnika);
        Task DeleteTipKorisnika(Guid tipKorisnikaId);
        Task SaveChangesAsync();
    }
}
