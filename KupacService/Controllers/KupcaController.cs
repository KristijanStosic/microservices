using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Helpers;
using KupacService.Model.Kupac;
using KupacService.Model.ManyToMany;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
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
        private readonly IKupacCalls _kupacCalls;
        private readonly ILoggerService _loggerService;

        public KupcaController(IKupacRepository kupacRepository,IMapper mapper,
            IKupacCalls kupacCalls,ILoggerService loggerService)
        {
            this._kupacRepository = kupacRepository;
            this._mapper = mapper;
            this._kupacCalls = kupacCalls;
            this._loggerService = loggerService;
        }
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet]
        public async Task<ActionResult<List<KupacDto>>> GetKupci()
        {
            var kupci = await _kupacRepository.GetKupci();
            if(kupci == null || kupci.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKupci", "Lista kupaca je prazna ili null.");
                return NoContent();
            }


            List<KupacDto> kupciDto = new List<KupacDto>();

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            foreach (var kupac in kupci)
            {
                KupacDto kupacDto = _mapper.Map<KupacDto>(kupac);
                var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(kupac, token);
                _mapper.Map(otherServicesDto, kupacDto);
                kupciDto.Add(kupacDto);
            }
            await _loggerService.Log(LogLevel.Information, "GetKupci", "Lista kupaca je uspešno vraćena.");
            return Ok(kupciDto);
        }
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet("{kupacId}")]
        public async Task<ActionResult<KupacDto>> GetKupacById(Guid kupacId)
        {
            var kupac = await _kupacRepository.GetKupacById(kupacId);

            if(kupac == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKupacById", $"Kupac sa id-em {kupacId} nije pronađen.");
                return NotFound();
            }

            KupacDto kupacDto = _mapper.Map<KupacDto>(kupac);

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(kupac, token);
            _mapper.Map(otherServicesDto, kupacDto);

            await _loggerService.Log(LogLevel.Information, "GetKupacById", $"Kupac sa id-em {kupacId} je uspešno vraćen.");
            return Ok(kupacDto);
        }
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet("uplata/{uplataId}")]
        public async Task<ActionResult<List<KupacManyToManyDto>>> GetKupceByUplataId(Guid uplataId)
        {
            var kupci = await _kupacRepository.GetKupceByUplataId(uplataId);

            if(kupci == null || kupci.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKupceByUplataId", $"Nisu pronađeni kupci koji pripadaju uplati {uplataId}.");
                return NoContent();
            }

            return Ok(_mapper.Map<List<KupacManyToManyDto>>(kupci));
        }
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet("ovlascenoLice/{ovlascenoLiceId}")]
        public async Task<ActionResult<List<KupacManyToManyDto>>> GetKupceByOvlascenoLiceId(Guid ovlascenoLiceId)
        {
            var kupci = await _kupacRepository.GetKupceByOvlascenoLiceId(ovlascenoLiceId);

            if(kupci == null || kupci.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKupceByOvlascenoLiceId", $"Nisu pronađeni kupci koji pripadaju ovlasšćenom licu {ovlascenoLiceId}.");
                return NoContent();
            }

            return Ok(_mapper.Map<List<KupacManyToManyDto>>(kupci));
        }


        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpOptions]
        public IActionResult GetKontaktOsobaOptions()
        {
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }


    }
}
