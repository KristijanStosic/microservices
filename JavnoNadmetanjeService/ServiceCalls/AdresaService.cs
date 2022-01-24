using JavnoNadmetanjeService.Models.Exceptions;
using JavnoNadmetanjeService.Models.Other;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls
{
    public class AdresaService : IAdresaService
    {
        private readonly IConfiguration _configuration;

        public AdresaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AdresaDto> GetAdresaDto(Guid adresaId)
        {
            string url = _configuration["Services:AdresaService"] + adresaId;
            using var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<AdresaDto>(content);
            }

            throw new ServiceCallException("Desio se problem pri komunikaciji sa drugim mikroservisom");
        }
    }
}
