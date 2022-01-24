using JavnoNadmetanjeService.Models.Other;
using System;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls
{
    public interface IAdresaService
    {
        Task<AdresaDto> GetAdresaDto(Guid adresaId);
    }
}
