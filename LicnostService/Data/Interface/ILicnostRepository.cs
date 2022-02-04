using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LicnostService.Entities;

namespace LicnostService.Data
{
    public interface ILicnostRepository
    {
        Task<List<Licnost>> GetAllLicnosti(string imeLicnosti = null);
        Task<Licnost> GetLicnostById(Guid licnostId);
        Task<Licnost> CreateLicnost(Licnost licnost);
        Task DeleteLicnost(Guid licnostId);
        Task UpdateLicnost(Licnost licnost);
    }
}
