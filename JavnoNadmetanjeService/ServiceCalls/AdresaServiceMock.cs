using System.Threading.Tasks;
using JavnoNadmetanjeService.Models.Other;

namespace JavnoNadmetanjeService.ServiceCalls
{
    public class AdresaServiceMock : IAdresaService
    {
        public async Task<AdresaDto> GetAdresaDto(string url)
        {
            var adresa = new AdresaDto
            {
                Ulica = "Bulevar Oslobodjenja",
                Broj = "50",
                Mesto = "Novi Sad",
                PostanskiBroj = "21000"
            };

            return await Task.FromResult(adresa);
        }

    }
}
