﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UplataService.Model.Services
{
    public class LogModel
    {
        public LogLevel Level { get; set; }

        public string Servis { get; set; }

        public string Metoda { get; set; }

        public string Poruka { get; set; }

        public Exception Greska { get; set; }
    }
}
