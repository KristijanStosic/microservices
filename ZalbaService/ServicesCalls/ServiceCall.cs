﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ZalbaService.ServicesCalls
{
    /// <summary>
    /// Genericka klasa za komunikaciju sa drugim servisima
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceCall<T> : IServiceCall<T>
    {
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor klase service call
        /// </summary>
        /// <param name="loggerService"></param>
        public ServiceCall(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        /// <summary>
        /// Metoda za slanje get zahteva
        /// </summary>
        /// <param name="url">Url putanja ka drugom servisu</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<T> SendGetRequestAsync(string url, string token)
        {
            try
            {
                using var httpClient = new HttpClient();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Accept", "application/json");

                var response = await httpClient.SendAsync(request);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "SendGetRequestAsync", $"Greška prilikom komunikacije sa drugim servisom iz servisa Zalba. Ciljani url: {url}", e);
                return default;
            }
        }
    }
}
