﻿
using KupacService.Model.OtherServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KupacService.ServiceCalls
{
    /// <summary>
    /// Loger servis
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Konstruktor loger servisa
        /// </summary>
        /// <param name="configuration"></param>
        public LoggerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Slanje post zahteva za upis novog log zapisa
        /// </summary>
        /// <param name="level">INFO/DEBUG/WARN/ERROR</param>
        /// <param name="metoda">Naziv metode</param>
        /// <param name="poruka">Tekst poruke</param>
        /// <param name="greska">Exception</param>
        /// <returns></returns>
        public async Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = _configuration["Services:LoggerService"];
                var log = new LogModel
                {
                    Servis = "Ovlasceno lice API",
                    Level = level,
                    Metoda = metoda,
                    Poruka = poruka,
                    Greska = greska
                };

                HttpContent content = new StringContent(JsonConvert.SerializeObject(log));
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                return await Task.FromResult(response.IsSuccessStatusCode);
            }
        }
    }
}
