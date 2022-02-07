using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;
using JavnoNadmetanjeService.Models.Other;
using JavnoNadmetanjeService.ServiceCalls;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Helpers
{
    public class JavnoNadmetanjeCalls : IJavnoNadmetanjeCalls
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IServiceCall<AdresaDto> _adresaService;
        private readonly IServiceCall<KupacDto> _kupacService;
        private readonly IServiceCall<OvlascenoLiceDto> _ovlascenoLiceService;
        private readonly IServiceCall<DeoParceleDto> _parcelaService;

        public JavnoNadmetanjeCalls(IConfiguration configuration, IMapper mapper, IServiceCall<AdresaDto> adresaService, IServiceCall<KupacDto> kupacService, IServiceCall<OvlascenoLiceDto> ovlascenoLiceService, IServiceCall<DeoParceleDto> parcelaService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _adresaService = adresaService;
            _kupacService = kupacService;
            _ovlascenoLiceService = ovlascenoLiceService;
            _parcelaService = parcelaService;
        }

        public async Task<JavnoNadmetanjeDto> GetJavnoNadmetanjeDtoWithOtherServicesInfo(JavnoNadmetanje javnoNadmetanje, string token)
        {
            //Adresa mikroservis komunikacija 
            string urlAdresa = _configuration["Services:AdresaService"];
            var javnoNadmetanjeDto = _mapper.Map<JavnoNadmetanjeDto>(javnoNadmetanje);
            if (javnoNadmetanje.AdresaId is not null)
            {
                var adresaDto = await _adresaService.SendGetRequestAsync(urlAdresa + javnoNadmetanje.AdresaId, token);
                if (adresaDto is not null)
                    javnoNadmetanjeDto.Adresa = adresaDto.Ulica + " " + adresaDto.Broj + " " + adresaDto.Mesto + ", " + adresaDto.Drzava;
            }

            //Kupac mikroservis komunikacija 
            //Najbolji kupac
            string urlKupac = _configuration["Services:KupacService"];
            if (javnoNadmetanje.KupacId is not null)
            {
                var kupacDto = await _kupacService.SendGetRequestAsync(urlKupac + javnoNadmetanje.KupacId, token);
                if (kupacDto is not null)
                    javnoNadmetanjeDto.Kupac = kupacDto;
            }

            //Svi kupci
            javnoNadmetanjeDto.Kupci = new List<KupacDto>();
            foreach (var kupac in javnoNadmetanje.Kupci)
            {
                var kupacDto = await _kupacService.SendGetRequestAsync(urlKupac + kupac, token);
                if (kupacDto is not null)
                    javnoNadmetanjeDto.Kupci.Add(kupacDto);
            }

            //Ovlasceno lice mikroservis komunikacija 
            string urlOvlascenoLice = _configuration["Services:OvlascenoLiceService"];
            javnoNadmetanjeDto.OvlascenaLica = new List<OvlascenoLiceDto>();
            foreach (var ovlascenoLice in javnoNadmetanje.OvlascenaLica)
            {
                var ovlascenoLiceDto = await _ovlascenoLiceService.SendGetRequestAsync(urlOvlascenoLice + ovlascenoLice, token);
                if (ovlascenoLiceDto is not null)
                    javnoNadmetanjeDto.OvlascenaLica.Add(ovlascenoLiceDto);
            }

            //Parcela mikroservis komunikacija 
            string urlParcela = _configuration["Services:ParcelaService"];
            javnoNadmetanjeDto.DeloviParcele = new List<DeoParceleDto>();
            foreach (var deoParcele in javnoNadmetanje.DeloviParcele)
            {
                var deoParceleDto = await _parcelaService.SendGetRequestAsync(urlParcela + deoParcele, token);
                if (deoParceleDto is not null)
                    javnoNadmetanjeDto.DeloviParcele.Add(deoParceleDto);
            }

            return javnoNadmetanjeDto;
        }
    }
}
