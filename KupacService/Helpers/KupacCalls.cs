using AutoMapper;
using KupacService.Entities;
using KupacService.Model.Kupac;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Helpers
{
    public class KupacCalls : IKupacCalls
    {
        private readonly IServiceCall<AdresaDto> _adresaServiceCall;
        private readonly IServiceCall<OvlascenoLiceDto> _ovlascenoLiceServiceCall;
        private readonly IServiceCall<UplataDto> _uplataServiceCall;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public KupacCalls(IServiceCall<AdresaDto> adresaServiceCall,IServiceCall<OvlascenoLiceDto> ovlascenoLiceServiceCall,
            IServiceCall<UplataDto> uplataServiceCall,IMapper mapper,IConfiguration configuration)
        {
            this._adresaServiceCall = adresaServiceCall;
            this._ovlascenoLiceServiceCall = ovlascenoLiceServiceCall;
            this._uplataServiceCall = uplataServiceCall;
            this._mapper = mapper;
            this._configuration = configuration;
        }
        public async Task<KupacOtherServicesDto> GetKupacDtoWithOtherServicesInfo(Kupac kupac)
        {
            KupacOtherServicesDto otherServicesDto = new KupacOtherServicesDto();

            if (kupac.AdresaId != null)
            {
                string adresaUrl = _configuration["Services:AdresaService"];
                var adresaDto = await _adresaServiceCall.SendGetRequestAsync(adresaUrl + kupac.AdresaId);
                if (adresaDto != null)
                    otherServicesDto.Adresa = adresaDto;
            }

            if (kupac.OvlascenaLica != null)
            {
                string ovlascenoLiceUrl = _configuration["Services:OvlascenoLiceService"];
                otherServicesDto.OvlascenaLica = new List<OvlascenoLiceDto>();
                foreach (Guid ovlascenoLiceId in kupac.OvlascenaLica)
                {
                    var ovlascenoLiceDto = await _ovlascenoLiceServiceCall.SendGetRequestAsync(ovlascenoLiceUrl + ovlascenoLiceId);
                    if (ovlascenoLiceDto != null)
                        otherServicesDto.OvlascenaLica.Add(ovlascenoLiceDto);
                }
            }

            if(kupac.Uplate != null)
            {
                string uplataUrl = _configuration["Services:UplataService"];
                otherServicesDto.Uplate = new List<UplataDto>();
                foreach(Guid uplataId in kupac.Uplate)
                {
                    var uplataDto = await _uplataServiceCall.SendGetRequestAsync(uplataUrl + uplataId);
                    if (uplataDto != null)
                        otherServicesDto.Uplate.Add(uplataDto);
                }
            }


            return otherServicesDto;
        }
    }
}
