﻿using LicitacijaService.Models.OtherServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.ServiceCalls.Mocks
{
    public class LoggerServiceMock : ILoggerService
    {
        public async Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null)
        {
            var log = new LogModel
            {
                Servis = "Licitacija API",
                Level = level,
                Metoda = metoda,
                Poruka = poruka,
                Greska = greska
            };

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(log));

            return await Task.FromResult(true);
        }
    }
}
