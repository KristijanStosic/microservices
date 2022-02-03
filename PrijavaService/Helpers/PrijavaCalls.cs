using AutoMapper;
using Microsoft.Extensions.Configuration;
using PrijavaService.Entities;
using PrijavaService.Models.Other;
using PrijavaService.Models.Prijava;
using PrijavaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrijavaService.Helpers
{
    public class PrijavaCalls : IPrijavaCalls
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IServiceCall<JavnoNadmetanjeDto> _javnoNadmetanjeService;

        public PrijavaCalls(IConfiguration configuration, IMapper mapper, IServiceCall<JavnoNadmetanjeDto> javnoNadmetanjeService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _javnoNadmetanjeService = javnoNadmetanjeService;
        }

        public async Task<PrijavaDto> GetPrijvaDtoWithOtherServicesInfo(Prijava prijava)
        {
            var prijavaDto = _mapper.Map<PrijavaDto>(prijava);

            string urlJavnoNadmetanje = _configuration["Services:JavnoNadmetanjeService"];
            prijavaDto.JavnoNadmetanje = new List<JavnoNadmetanjeDto>();
            foreach(var javnoNadmetanje in prijava.JavnoNadmetanje)
            {
                var javnoNadmetanjeDto = await _javnoNadmetanjeService.SendGetRequestAsync(urlJavnoNadmetanje + javnoNadmetanje);
                if(javnoNadmetanjeDto is not null)
                {
                    prijavaDto.JavnoNadmetanje.Add(javnoNadmetanjeDto);
                }
            }

            return prijavaDto;
        }

    }
}
