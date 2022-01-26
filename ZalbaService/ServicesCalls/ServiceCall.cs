using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ZalbaService.Models.Exceptions;

namespace ZalbaService.ServicesCalls
{
    public class ServiceCall<T> : IServiceCall<T>
    {
        public async Task<T> SendGetRequestAsync(string url)
        {
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

                return JsonConvert.DeserializeObject<T>(content);
            }

            throw new ServiceCallException("Desio se problem pri komunikaciji sa drugim mikroservisom");
        }
    }
}
