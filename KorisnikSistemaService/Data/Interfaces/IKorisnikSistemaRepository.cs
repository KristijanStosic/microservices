using KorisnikSistemaService.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KorisnikSistemaService.Data.Interfaces
{
    public interface IKorisnikSistemaRepository
    {
        Task<List<KorisnikSistema>> GetAllKorisnikSistema();
        Task<KorisnikSistema> GetKorisnikSistemaById(Guid korisnikSistemaId);
        Task<KorisnikSistema> GetKorisnikSistemaByKorisnickoIme(string korisnickoIme);
        Task<KorisnikSistema> CreateKorisnikSistema(KorisnikSistema korisnikSistema);
        Task DeleteKorisnikSistema(Guid korisnikSistemaId);
        Task SaveChangesAsync();
    }
}
