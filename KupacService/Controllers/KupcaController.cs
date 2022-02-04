using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Helpers;
using KupacService.Model.Kupac;
using KupacService.Model.ManyToMany;
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
        private readonly IKupacCalls _kupacCalls;

        public KupcaController(IKupacRepository kupacRepository,IMapper mapper, IConfiguration configuration, IServiceCall<AdresaDto> adresaServiceCall,
            IKupacCalls kupacCalls)
        {
            this._kupacRepository = kupacRepository;
            this._mapper = mapper;
            this._configuration = configuration;
            this._adresaServiceCall = adresaServiceCall;
            this._kupacCalls = kupacCalls;
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
        
            foreach (var kupac in kupci)
            {
                KupacDto kupacDto = _mapper.Map<KupacDto>(kupac);
                var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(kupac);
                _mapper.Map(otherServicesDto, kupacDto);
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

            var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(kupac);
            _mapper.Map(otherServicesDto, kupacDto);

            return Ok(kupacDto);
        }
        [HttpGet("uplata/{uplataId}")]
        public async Task<ActionResult<List<KupacManyToManyDto>>> GetKupceByUplataId(Guid uplataId)
        {
            var kupci = await _kupacRepository.GetKupceByUplataId(uplataId);

            if(kupci == null || kupci.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KupacManyToManyDto>>(kupci));
        }

        [HttpGet("ovlascenoLice/{ovlascenoLiceId}")]
        public async Task<ActionResult<List<KupacManyToManyDto>>> GetKupceByOvlascenoLiceId(Guid ovlascenoLiceId)
        {
            var kupci = await _kupacRepository.GetKupceByOvlascenoLiceId(ovlascenoLiceId);

            if(kupci == null || kupci.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KupacManyToManyDto>>(kupci));
        }

        


        

    }
}
