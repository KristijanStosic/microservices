using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace UgovorOZakupu.Services
{
    public abstract class Service<T>
    {
        private HttpClient _http;

        protected Service()
        {
            _http = new HttpClient();
        }

        public async Task SendPostRequest(string url, T payload)
        {
            await _http.PostAsJsonAsync(url, payload);
        }
    }
}