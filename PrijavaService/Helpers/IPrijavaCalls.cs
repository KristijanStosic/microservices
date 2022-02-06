using PrijavaService.Entities;
using PrijavaService.Models.Prijava;
using System.Threading.Tasks;

namespace PrijavaService.Helpers
{
    public interface IPrijavaCalls
    {
        Task<PrijavaDto> GetPrijvaDtoWithOtherServicesInfo(Prijava prijava);
    }
}
