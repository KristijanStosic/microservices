using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.Kupac;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Controllers
{
    [ApiController]
    [Route("api/kupac")]
    public class KupcaController : ControllerBase
    {
        private readonly IKupacRepository _kupacRepository;
 
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IServiceCall<AdresaDto> _adresaServiceCall;

        public KupcaController(IKupacRepository kupacRepository,IMapper mapper, IConfiguration configuration, IServiceCall<AdresaDto> adresaServiceCall)
        {
            this._kupacRepository = kupacRepository;
            this._mapper = mapper;
            this._configuration = configuration;
            this._adresaServiceCall = adresaServiceCall;
        }

        [HttpGet]
        public async Task<ActionResult<List<KupacDto>>> GetKupci()
        {
            var kupci = await _kupacRepository.GetKupci();
            if(kupci == null && kupci.Count == 0)
            {
                return NoContent();
            }


            List<KupacDto> kupciDto = new List<KupacDto>();
            string adresaurl = _configuration["Services:AdresaService"];
            foreach (var kupac in kupci)
            {
                KupacDto kupacDto = _mapper.Map<KupacDto>(kupac);

                if (kupac.AdresaId != null)
                {
                    var adresaDto = await _adresaServiceCall.SendGetRequestAsync(adresaurl + "adresa/" + kupac.AdresaId);
                    if (adresaDto != null)
                        kupacDto.Adresa = adresaDto;
                }
                kupciDto.Add(kupacDto);
            }

            return Ok(kupciDto);
        }

        [HttpGet("{kupacId}")]
        public async Task<ActionResult<KupacDto>> GetKupacById(Guid kupacId)
        {
            var kupac = await _kupacRepository.GetKupacById(kupacId);

            if(kupac == null)
            {
                return NotFound();
            }

            KupacDto kupacDto = _mapper.Map<KupacDto>(kupac);

            if (kupac.AdresaId != null)
            {
                string adresaurl = _configuration["Services:AdresaService"];
                var adresaDto = await _adresaServiceCall.SendGetRequestAsync(adresaurl + "adresa/" + kupac.AdresaId);
                if (adresaDto != null)
                    kupacDto.Adresa = adresaDto;
            }

            return Ok(kupacDto);
        }


        

    }
}
