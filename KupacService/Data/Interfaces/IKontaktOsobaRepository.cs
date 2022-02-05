using KupacService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Data.Interfaces
{
    public interface IKontaktOsobaRepository
    {
        Task<List<KontaktOsoba>> GetKontaktOsoba(string ime = null, string prezime = null);
        Task<KontaktOsoba> GetKontaktOsobaById(Guid kontaktOsobaId);
        Task<KontaktOsoba> CreateKontaktOsoba(KontaktOsoba kontaktOsoba);
        Task DeleteKontaktOsoba(Guid KontaktOsobaId);
        Task SaveChangesAsync();
    }
}
