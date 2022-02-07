using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.Etapa;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Helpers
{
    public interface IJavnoNadmetanjeCalls
    {
        Task<JavnoNadmetanjeDto> GetJavnoNadmetanjeDtoWithOtherServicesInfo(JavnoNadmetanje javnoNadmetanje, string token);
        Task EtapaToOcelotQueue(JavnoNadmetanje javnoNadmetanje, EtapaCreationDto etapa, string token);
    }
}
