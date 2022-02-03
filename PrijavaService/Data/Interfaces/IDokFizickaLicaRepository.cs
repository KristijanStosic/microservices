using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Data.Interfaces
{
    public interface IDokFizickaLicaRepository
    {
        Task<List<DokFizickaLica>> GetAllDokFizickaLica();
        Task<DokFizickaLica> GetDokFizickaLicaById(Guid DokFizickaLicaId);
        Task DeleteDokFizickaLica(Guid DokFizickaLicaId);
        Task<DokFizickaLicaConfirmation> CreateDokFizickaLica(DokFizickaLica dokFizickaLica);
        Task SaveChangesAsync();
    }
}
