using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace OvlascenoLiceService.ServiceCalls
{
    /// <summary>
    /// Genericka klasa za komunikaciju sa drugim servisima
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceCall<T> : IServiceCall<T>
    {
        /// <summary>
        /// Metoda za slanje get zahteva
        /// </summary>
        /// <param name="url">Url putanja ka drugom servisu</param>
        /// <returns></returns>
        public async Task<T> SendGetRequestAsync(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
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
                throw new Exception("Desio se problem pri komunikaciji sa drugim mikroservisom");
            }
        }
    }
}