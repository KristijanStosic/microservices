﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UgovorOZakupu.Services.Service;

namespace UgovorOZakupu.Services
{
    public class Service<T> : IService<T>
    {
        private readonly HttpClient _http;
        private readonly string _url;

        public Service(string url)
        {
            _url = url;
            _http = new HttpClient();
        }

        public async Task<T> SendGetRequest(string token, string uri = "")
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync(uri == string.Empty ? _url : $"{_url}/{uri}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(content)) return default;

                return JsonConvert.DeserializeObject<T>(content);
            }

            return default;
        }

        public async Task<T> SendPostRequest<TPayload>(TPayload payload, string uri = "")
        {
            var response = await _http.PostAsJsonAsync(uri == string.Empty ? _url : $"{_url}/{uri}", payload);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(content)) return default;

                return JsonConvert.DeserializeObject<T>(content);
            }

            return default;
        }
    }
}