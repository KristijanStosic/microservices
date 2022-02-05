using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UgovorOZakupu.Services
{
    public class Service<T> : IService<T>
    {
        private HttpClient _http;

        public Service()
        {
            _http = new HttpClient();
        }

        public async Task<T> SendGetRequest(string uri)
        {
            var response = await _http.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(content))
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(content);
            }

            return default;
        }

        public async Task<T> SendPostRequest<TPayload>(string uri, TPayload payload)
        {
            var response = await _http.PostAsJsonAsync(uri, payload);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(content))
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(content);
            }

            return default;
        }
    }
}