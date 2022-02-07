using AutoMapper;
using LicitacijaService.Data.Interfaces;
using LicitacijaService.Entities;
using LicitacijaService.Entities.Confirmations;
using LicitacijaService.Models;
using LicitacijaService.Models.OtherServices;
using LicitacijaService.ServiceCalls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Controllers
{
    /// <summary>
    /// Kontroler za licitaciju
    /// </summary>
    [Route("api/licitacija")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class LicitacijaController : ControllerBase
    {

        private readonly ILicitacijaRepository _licitacijaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;
        private readonly IServiceCall<JavnoNadmetanjeDto> _javnoNadmetanjeServiceCall;

        public LicitacijaController(ILicitacijaRepository licitacijaRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService, IServiceCall<JavnoNadmetanjeDto> javnoNadmetanjeServiceCall, IConfiguration configuration)
        {
            _licitacijaRepository = licitacijaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
            _javnoNadmetanjeServiceCall = javnoNadmetanjeServiceCall;
            _configuration = configuration;
        }

        /// <summary>
        /// Vraća sve licitacije
        /// </summary>
        /// <returns>Lista licitacija</returns>
        /// <response code="200">Vraća listu licitacija</response>
        /// <response code="404">Nije pronađena ni jedna licitacija</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, PrvaKomisija, OperaterNadmetanja")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<LicitacijaDto>>> GetAllLicitacija()
        {
            var licitacije = await _licitacijaRepository.GetAllLicitacija();

            
            if(licitacije == null || licitacije.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllLicitacija", "Lista licitacija je prazna ili null.");
                return NoContent();
            }
            string urlJavnoNadmetanje = _configuration["Services:JavnoNadmetanje"]; 
            var licitacijeDto = new List<LicitacijaDto>();
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            foreach (var lic in licitacije)
            {
                var licitacijaDto = _mapper.Map<LicitacijaDto>(lic);
                licitacijaDto.JavnaNadmetanja = new List<JavnoNadmetanjeDto>();

                foreach(var jn in lic.JavnaNadmetanja)
                {
                    var javnoNadmetanje = await _javnoNadmetanjeServiceCall.SendGetRequestAsync(urlJavnoNadmetanje + jn, token);
                    licitacijaDto.JavnaNadmetanja.Add(javnoNadmetanje);

                }
                licitacijeDto.Add(licitacijaDto);
                
            }
            await _loggerService.Log(LogLevel.Information, "GetAllLicitacija", "Lista licitacija je uspešno vraćena."); 

            return Ok(licitacijeDto);
        }
        /// <summary>
        /// Vraća jednu licitacije na osnovu ID-a
        /// </summary>
        /// <param name="licitacijaId">ID licitacije</param>
        /// <returns>Licitacija</returns>
        /// <response code="200">Vraća traženu licitaciju</response>
        /// <response code="404">Nije pronađena licitacija za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, PrvaKomisija, OperaterNadmetanja")]
        [HttpGet("{licitacijaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LicitacijaDto>> GetLicitacija(Guid licitacijaId)
        {
            var licitacija = await _licitacijaRepository.GetLicitacijaById(licitacijaId);

            if(licitacija == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetLicitacija", $"Licitacija sa id-em {licitacijaId} nije pronađena.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetLicitacija", $"Licitacija sa id-em {licitacijaId} je uspešno vraćena.");
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var licitacijaDto = _mapper.Map<LicitacijaDto>(licitacija);
            licitacijaDto.JavnaNadmetanja = new List<JavnoNadmetanjeDto>();
            string urlJavnoNadmetanje = _configuration["Services:JavnoNadmetanje"];

            foreach (var jn in licitacija.JavnaNadmetanja)
            {
                var javnoNadmetanje = await _javnoNadmetanjeServiceCall.SendGetRequestAsync(urlJavnoNadmetanje + jn, token);
                if (javnoNadmetanje == null)
                {
                    return NotFound(); 
                }
                licitacijaDto.JavnaNadmetanja.Add(javnoNadmetanje);

            }

            return Ok(licitacijaDto);

        }


        /// <summary>
        /// Kreira novu licitaciju
        /// </summary>
        /// <param name="licitacija">Model licitacije</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove licitacije \
        /// POST /api/licitacija \
        ///   "brojLicitacije": 1,
        ///   "godinaLicitacije": 8,
        ///   "ogranicenjeLicitacije": 8,
        ///   "rokLicitacije": "2022-02-06T20:29:26.426Z",
        ///   "korakCeneLicitacije": 2
        /// </remarks>
        /// <returns>Potvrda o kreiranju licitacije</returns>
        /// <response code="201">Vraća kreiranu licitaciju</response>
        /// <response code="500">Desila se greška prilikom unosa nove licitacije</response>
        [Authorize(Roles = "Administrator, Superuser, PrvaKomisija, OperaterNadmetanja")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LicitacijaConfirmationDto>> CreateLicitacija([FromBody] LicitacijaCreationDto licitacija)
        {
            try
            {
                LicitacijaConfirmation novaLicitacija = await _licitacijaRepository.CreateLicitacija(_mapper.Map<Licitacija>(licitacija));
                await _licitacijaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetLicitacija", "Licitacija", new { licitacijaId = novaLicitacija.LicitacijaId });

                await _loggerService.Log(LogLevel.Information, "CreateLicitacija", $"Licitacija sa vrednostima: {JsonConvert.SerializeObject(licitacija)} je uspešno kreirana.");

                return Created(lokacija, _mapper.Map<LicitacijaConfirmationDto>(novaLicitacija));
            }

            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateLicitacija", $"Greška prilikom unosa licitacije sa vrednostima: {JsonConvert.SerializeObject(licitacija)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa licitacije");
            }
        }

        /// <summary>
        /// Izmena licitacije
        /// </summary>
        /// <param name="licitacija">Model licitacija</param>
        /// <returns>Potvrda o izmeni licitacije</returns>
        /// <response code="200">Izmenjena licitacija</response>
        /// <response code="404">Nije pronađena licitacija za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene licitacije</response>
        [Authorize(Roles = "Administrator, Superuser, PrvaKomisija, OperaterNadmetanja")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LicitacijaDto>> UpdateLicitacija(LicitacijaUpdateDto licitacija)
        {
            try
            {
                var staraLicitacija = await _licitacijaRepository.GetLicitacijaById(licitacija.LicitacijaId);

                if(staraLicitacija == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateLicitacija", $"Licitacija sa id-em {licitacija.LicitacijaId} nije pronađena.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(licitacija);

                Licitacija novaLicitacija = _mapper.Map<Licitacija>(licitacija);

                _mapper.Map(novaLicitacija, staraLicitacija);
                await _licitacijaRepository.UpdateLicitacija(novaLicitacija);
                await _licitacijaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateLicitacija", $"Licitacija sa id-em {licitacija.LicitacijaId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<LicitacijaDto>(staraLicitacija));

            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateLicitacija", $"Greška prilikom izmene licitacije sa id-em {licitacija.LicitacijaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene licitacije");

            }
        }
        /// <summary>
        /// Brisanje licitacije na osnovu ID-a
        /// </summary>
        /// <param name="licitacijaId">ID licitacije</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Licitacija je uspešno obrisana</response>
        /// <response code="404">Nije pronađena licitacija za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja licitacije</response>
        [Authorize(Roles = "Administrator, Superuser, PrvaKomisija, OperaterNadmetanja")]
        [HttpDelete("{licitacijaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLicitacija(Guid licitacijaId)
        {
            try
            {
                var licitacija = await _licitacijaRepository.GetLicitacijaById(licitacijaId);

                if(licitacija == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteLicitacija", $"Licitacija sa id-em {licitacijaId} nije pronađena.");
                    return NotFound();
                }

                await _licitacijaRepository.DeleteLicitacija(licitacijaId);
                await _licitacijaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteLicitacija", $"Licitacija sa id-em {licitacijaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(licitacija)}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteLicitacija", $"Greška prilikom brisanja licitacije sa id-em {licitacijaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja licitacije");
            }
        }
        /// <summary>
        /// Vraća opcije za rad sa licitacijama
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, PrvaKomisija, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetLicitacijaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }

}
