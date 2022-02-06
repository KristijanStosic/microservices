﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.ServiceCalls
{
    public interface IServiceCall<T>
    {
        Task<T> SendGetRequestAsync(string url);
    }
}
