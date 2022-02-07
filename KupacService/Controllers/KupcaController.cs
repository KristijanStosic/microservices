using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Helpers;
using KupacService.Model.Kupac;
using KupacService.Model.ManyToMany;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    /// <summary>
    /// Kontroler za kupce
    /// </summary>
    [ApiController]
    [Produces("application/json", "application/xml")]
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

        /// <summary>
        /// Vraća listu kupaca
        /// </summary>
        /// <returns>Lista kupaca</returns>
        /// <response code="200">Uspešno vraćena lista kupaca</response>
        /// <response code="204">Nije pronađen nijedan kupac</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

        /// <summary>
        /// Vraća kupca na osnovu unetog id-a
        /// </summary>
        /// <param name="kupacId">Id kupca</param>
        /// <returns>Kupca</returns>
        ///  <response code="200">Uspešno vraćen kupac</response>
        /// <response code="404">Nije pronađen kupac sa zadatim id-em</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet("{kupacId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Vraća uplatu
        /// </summary>
        /// <param name="uplataId">Id uplate</param>
        /// <returns>Lista kupaca</returns>
        /// <response code="200">Uspešno vraćena lista kupaca za uplatu</response>
        /// <response code="204">Nije pronađen nijedan kupac za datu uplatu</response>
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

        /// <summary>
        /// Vraća listu kupaca za koje ovlašćeno lice vrši licitacije
        /// </summary>
        /// <param name="ovlascenoLiceId">Id ovlašćenog lica</param>
        /// <returns>Lista kupaca</returns>
        /// <response code="200">Uspešno vraćena lista kupaca za koje prosleđeno ovlašćeno lice vrši licitacije</response>
        /// <response code="204">Nije pronađen nijedan kupac za dato ovlašćeno lice</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet("ovlascenoLice/{ovlascenoLiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<KupacDto>>> GetKupceByOvlascenoLiceId(Guid ovlascenoLiceId)
        {
            var kupci = await _kupacRepository.GetKupceByOvlascenoLiceId(ovlascenoLiceId);

            if(kupci == null || kupci.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKupceByOvlascenoLiceId", $"Nisu pronađeni kupci koji pripadaju ovlasšćenom licu {ovlascenoLiceId}.");
                return NoContent();
            }
            List<KupacDto> kupacDtos = new List<KupacDto>();
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            foreach (var kupac in kupci)
            {
                Kupac tempKupac = await _kupacRepository.GetKupacById(kupac.KupacId);
                var tempKupacDto = _mapper.Map<KupacDto>(tempKupac);
                var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(tempKupac, token);
                _mapper.Map(otherServicesDto, tempKupacDto);
                kupacDtos.Add(tempKupacDto);

            }

            await _loggerService.Log(LogLevel.Warning, "GetKupceByOvlascenoLiceId", $"Uspešno pronađeni kupci koji pripadaju ovlasšćenom licu {ovlascenoLiceId}.");
           
            return Ok(kupacDtos);
        }

        /// <summary>
        /// Vraća opcije za rad sa kupcima
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpOptions]
        public IActionResult GetKupcaOptions()
        {
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }

    }
}
