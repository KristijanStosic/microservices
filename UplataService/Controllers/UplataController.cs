using AutoMapper;
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
using UplataService.Entities;
using UplataService.Model;
using UplataService.Model.Services;
using UplataService.Repository;
using UplataService.ServiceCalls;

namespace UplataService.Controllers
{
    /// <summary>
    /// Kontroler za uplatu
    /// </summary>
    [ApiController]
    [Route("api/uplata")]
    [Produces("application/json", "application/xml")]
    public class UplataController : ControllerBase
    {
        private readonly IUplataRepository _uplataRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;
        private readonly IConfiguration _configuration;
        private readonly IServiceCall<JavnoNadmetanjeDto> _javnoNadmetanjeService;

        /// <summary>
        /// Konstruktor kontrolera uplate - DI
        /// </summary>
        /// <param name="uplataRepository">Repo uplate</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="loggerService">Logger servis</param>
        /// <param name="javnoNadmetanjeService">Servis javno nadmetanje - dobijanje javnog nadmetanja</param>
        /// <param name="configuration">Konfiguracija za pristup putanji ka servisu javno nadmetanje</param>
        public UplataController(IUplataRepository uplataRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService, IConfiguration configuration, IServiceCall<JavnoNadmetanjeDto> javnoNadmetanjeService)
        {
            _uplataRepository = uplataRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
            _configuration = configuration;
            _javnoNadmetanjeService = javnoNadmetanjeService;
        }

        /// <summary>
        /// Vraća sve uplate
        /// </summary>
        /// <returns>Lista uplata</returns>
        /// <response code="200">Vraća listu uplata</response>
        /// <response code="204">Nije pronađen ni jedan tip zalbe</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<UplataDto>>> GetAllUplate()
        {
            var uplate = await _uplataRepository.GetAllUplate();

            if (uplate == null || uplate.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllUplata", "Lista uplata je prazna ili null.");
                return NoContent();
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var uplateDto = new List<UplataDto>();
            string url = _configuration["Services:JavnoNadmetanjeService"];
            foreach (var uplata in uplate)
            {
                var uplataDto = _mapper.Map<UplataDto>(uplata);
                if (uplata.JavnoNadmetanjeId is not null)
                {
                    var javnoNadDto = await _javnoNadmetanjeService.SendGetRequestAsync(url + uplata.JavnoNadmetanjeId, token);
                    if (javnoNadDto is not null)
                    {
                        uplataDto.JavnoNadmetanje = javnoNadDto.IzlicitiranaCena + ", " 
                                                  + javnoNadDto.Izuzeto + ", " 
                                                  + javnoNadDto.Krug + ", "
                                                  + javnoNadDto.PeriodZakupa + ", "
                                                  + javnoNadDto.PocetnaCenaHektar + ", "
                                                  + javnoNadDto.Status + ", "
                                                  + javnoNadDto.Tip + ", "
                                                  + javnoNadDto.VisinaDopuneDepozita + ", "
                                                  + javnoNadDto.BrojUcesnika;
                    }
                }
                uplateDto.Add(uplataDto);
            }

            await _loggerService.Log(LogLevel.Information, "GetAllUplata", "Lista uplata je uspešno vraćena.");
            return Ok(uplateDto);
        }

        /// <summary>
        /// Vraća jednu uplatu na osnovu ID-a
        /// </summary>
        /// <param name="uplataId">ID uplate</param>
        /// <returns>Žalba</returns>
        /// <response code="200">Vraća traženu uplatu</response>
        /// <response code="404">Nije pronađena uplata za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja")]
        [HttpGet("{uplataId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UplataDto>>> GetUplata(Guid uplataId)
        {
            var uplata = await _uplataRepository.GetUplataById(uplataId);

            if (uplata == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetUplata", $"Uplata sa id-em {uplataId} nije pronađena.");
                return NotFound();
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string url = _configuration["Services:JavnoNadmetanjeService"];

            var uplataDto = _mapper.Map<UplataDto>(uplata);
            if (uplata.JavnoNadmetanjeId is not null)
            {
                var javnoNadDto = await _javnoNadmetanjeService.SendGetRequestAsync(url + uplata.JavnoNadmetanjeId, token);
                if (javnoNadDto is not null)
                {
                    uplataDto.JavnoNadmetanje = javnoNadDto.IzlicitiranaCena + ", "
                                                  + javnoNadDto.Izuzeto + ", "
                                                  + javnoNadDto.Krug + ", "
                                                  + javnoNadDto.PeriodZakupa + ", "
                                                  + javnoNadDto.PocetnaCenaHektar + ", "
                                                  + javnoNadDto.Status + ", "
                                                  + javnoNadDto.Tip + ", "
                                                  + javnoNadDto.VisinaDopuneDepozita + ", "
                                                  + javnoNadDto.BrojUcesnika;
                }
            }
            //await _loggerService.Log(LogLevel.Information, "GetUplata", $"Uplata sa id-em {uplataId} je uspešno vraćena."); //log
            return Ok(uplataDto);
        }

        /// <summary>
        /// Kreira novu uplatu
        /// </summary>
        /// <param name="uplata">Model uplata</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove uplate \
        /// POST /api/uplata \
        /// {   
        ///     "brojRacuna": "115-5658588888800-55", \
        ///     "pozivNaBroj": "90-7485-5858-9558", \
        ///     "iznos": "2000", \
        ///     "svrhaUplate": "Uplata na racun", \
        ///     "datumUplate": "2017-05-03", \
        ///     "kurs": {
        ///         "vrednostKursa": 120
        ///     }
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju uplate</returns>
        /// <response code="201">Vraća kreiranu uplatu</response>
        /// <response code="500">Desila se greška prilikom unosa nove uplate</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UplataConfirmation>> CreateUplata([FromBody] UplataCreateDto uplata)
        {
            try
            {
                var proveraValidnosti = await _uplataRepository.IsValidUplata(uplata.BrojRacuna);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Uneli ste iste podatke za broj racuna. Racun je jedinstven. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Information, "CreateUplata", $"Greška prilikom unosa uplate sa vrednostima: {JsonConvert.SerializeObject(uplata)}.");
                    return BadRequest(response);
                }

                UplataConfirmation createdUplata = await _uplataRepository.CreateUplata(_mapper.Map<Uplata>(uplata));

                string location = _linkGenerator.GetPathByAction("GetUplata", "Uplata", new { uplataId = createdUplata.UplataId });
                await _loggerService.Log(LogLevel.Information, "CreateUplata", $"Uplata sa vrednostima: {JsonConvert.SerializeObject(uplata)} je uspešno kreirana.");
                return Created(location, _mapper.Map<UplataConfirmationDto>(createdUplata));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateUplata", $"Greška prilikom unosa uplate sa vrednostima: {JsonConvert.SerializeObject(uplata)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Uplata error");
            }
        }

        /// <summary>
        /// Modifikacija uplate
        /// </summary>
        /// <param name="uplataId">ID uplate</param>
        /// <param name="uplata">Model uplate</param>
        /// <returns>Potvrda o modifikaciji uplate</returns>
        /// <response code="200">Izmenjena uplate</response>
        /// <response code="400">Desila se greška prilikom unosa istih podataka za uplatu</response>
        /// <response code="404">Nije pronađena uplata za uneti ID</response>
        /// <response code="500">Serverska greška tokom modifikacije uplate</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{uplataId}")]
        public async Task<ActionResult<UplataUpdateDto>> UpdateUplata(Guid uplataId, [FromBody] UplataUpdateDto uplata)
        {
            try
            {
                var uplataEntity = await _uplataRepository.GetUplataById(uplataId);

                if (uplataEntity == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateUplata", $"Uplata sa id-em {uplataId} nije pronađena.");
                    return NotFound();
                }

                _mapper.Map(uplata, uplataEntity);

                await _uplataRepository.UpdateUplata(_mapper.Map<Uplata>(uplata));
                await _loggerService.Log(LogLevel.Information, "UpdateUplata", $"Uplata sa id-em {uplataId} je uspešno izmenjena. Stare vrednosti su: {uplataEntity}");
                return Ok(uplata);
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateUplata", $"Greška prilikom izmene uplate sa id-em {uplataId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Uplata error");
            }
        }

        /// <summary>
        /// Brisanje uplate na osnovu ID-a
        /// </summary>
        /// <param name="uplataId">ID uplate</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Uplata je uspešno obrisana</response>
        /// <response code="404">Nije pronađena uplata za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja uplate</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{uplataId}")]
        public async Task<ActionResult> DeleteUplata(Guid uplataId)
        {
            try
            {
                var uplata = await _uplataRepository.GetUplataById(uplataId);

                if (uplata == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteUplata", $"Uplata sa id-em {uplataId} nije pronađena.");
                    return NotFound();
                }

                await _uplataRepository.DeleteUplata(uplataId);
                await _loggerService.Log(LogLevel.Information, "DeleteUplata", $"Uplata sa id-em {uplataId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(uplata)}");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteUplata", $"Greška prilikom brisanja uplate sa id-em {uplataId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Uplata error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa uplata
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, Menadzer, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetUplataOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
