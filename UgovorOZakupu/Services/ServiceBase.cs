using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace UgovorOZakupu.Services
{
    public abstract class ServiceBase
    {
        private string _url;
        private HttpClient _http;

        protected ServiceBase(string url)
        {
            _url = url;
            _http = new HttpClient();
        }

        public async Task SendPostRequest<T>(T payload)
        {
            await _http.PostAsJsonAsync(_url, payload);
        }
    }
}