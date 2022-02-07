using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Helpers
{
    public interface IJavnoNadmetanjeCalls
    {
        Task<JavnoNadmetanjeDto> GetJavnoNadmetanjeDtoWithOtherServicesInfo(JavnoNadmetanje javnoNadmetanje, string token);
    }
}
