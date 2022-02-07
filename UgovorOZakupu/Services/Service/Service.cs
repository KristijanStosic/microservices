using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;

namespace UgovorOZakupu.Services.Service
{
    public class Service<T> : IService<T>
    {
        private readonly HttpClient _http;

        public Service(string path)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(path)
            };
        }

        public async Task<T> SendGetRequest(string relativePath = "", string token = "")
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = relativePath != string.Empty
                    ? new Uri(_http.BaseAddress!, relativePath)
                    : _http.BaseAddress,
                Headers =
                {
                    Authorization = token != string.Empty
                        ? new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token)
                        : null
                }
            };

            var response = await _http.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();

            return string.IsNullOrEmpty(content) ? default : JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> SendPostRequest<TPayload>(TPayload payload, string relativePath = "", string token = "")
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = relativePath != string.Empty
                    ? new Uri(_http.BaseAddress!, relativePath)
                    : _http.BaseAddress,
                Headers =
                {
                    Authorization = token != string.Empty
                        ? new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token)
                        : null
                },
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8,
                    MediaTypeNames.Application.Json)
            };

            var response = await _http.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();

            return string.IsNullOrEmpty(content) ? default : JsonConvert.DeserializeObject<T>(content);
        }
    }
}