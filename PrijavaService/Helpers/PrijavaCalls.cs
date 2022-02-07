using AutoMapper;
using Microsoft.Extensions.Configuration;
using PrijavaService.Entities;
using PrijavaService.Models.Other;
using PrijavaService.Models.Prijava;
using PrijavaService.ServiceCalls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrijavaService.Helpers
{
    public class PrijavaCalls : IPrijavaCalls
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IServiceCall<JavnoNadmetanjeDto> _javnoNadmetanjeService;
        private readonly IServiceCall<KupacDto> _kupacService;

        public PrijavaCalls(IConfiguration configuration, IMapper mapper, IServiceCall<JavnoNadmetanjeDto> javnoNadmetanjeService,IServiceCall<KupacDto> kupacService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _javnoNadmetanjeService = javnoNadmetanjeService;
            _kupacService = kupacService;
        }

        public async Task<PrijavaDto> GetPrijvaDtoWithOtherServicesInfo(Prijava prijava, string token)
        {
            var prijavaDto = _mapper.Map<PrijavaDto>(prijava);

            // Kupac mikroservis
            string urlKupac = _configuration["Services:KupacService"];
            if (prijava.KupacId is not null)
            {
                var kupacDto = await _kupacService.SendGetRequestAsync(urlKupac + prijava.KupacId, token);
                if (kupacDto is not null)
                    prijavaDto.Kupac = kupacDto;
            }

            // Javno nadmetanje mikroservis 
            string urlJavnoNadmetanje = _configuration["Services:JavnoNadmetanjeService"];
            prijavaDto.JavnoNadmetanje = new List<JavnoNadmetanjeDto>();
            foreach(var javnoNadmetanje in prijava.JavnoNadmetanje)
            {
                var javnoNadmetanjeDto = await _javnoNadmetanjeService.SendGetRequestAsync(urlJavnoNadmetanje + javnoNadmetanje, token);
                if(javnoNadmetanjeDto is not null)
                {
                    prijavaDto.JavnoNadmetanje.Add(javnoNadmetanjeDto);
                }
            }

            return prijavaDto;
        }

    }
}
