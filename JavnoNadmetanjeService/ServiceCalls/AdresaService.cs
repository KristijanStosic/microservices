using JavnoNadmetanjeService.Models.Other;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls
{
    public class AdresaService : IAdresaService
    {
        public async Task<AdresaDto> GetAdresaDto(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Accept", "application/json");
                //request.Headers.Add("Authorization", authToken);

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

                throw new Exception("Desio se problem pri komunikaciji sa drugim mikroservisom");
            }
        }
    }
}
