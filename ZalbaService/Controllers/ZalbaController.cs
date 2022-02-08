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
using ZalbaService.Data.Interfaces;
using ZalbaService.Entities;
using ZalbaService.Entities.Confirmations;
using ZalbaService.Models.Services;
using ZalbaService.Models.Zalba;
using ZalbaService.ServicesCalls;

namespace ZalbaService.Controllers
{
    /// <summary>
    /// Kontroler za žalbu
    /// </summary>
    [ApiController]
    [Route("api/zalba")]
    [Produces("application/json", "application/xml")]
    public class ZalbaController : ControllerBase
    {
        private readonly IZalbaRepository _zalbaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IServiceCall<KupacDto> _kupacService;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor kontrolera žalbe - DI
        /// </summary>
        /// <param name="zalbaRepository">Repo žalbe</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="kupacService">Servis kupac - dobijanje kupca</param>
        /// <param name="configuration">Konfiguracija za pristup putanji ka servisu kupac</param>
        /// <param name="loggerService">Logger servis</param>
        public ZalbaController(IZalbaRepository zalbaRepository, LinkGenerator linkGenerator, IMapper mapper, IServiceCall<KupacDto> kupacService, IConfiguration configuration, ILoggerService loggerService)
        {
            _zalbaRepository = zalbaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _kupacService = kupacService;
            _configuration = configuration;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve žalbe
        /// </summary>
        /// <returns>Lista žalbi</returns>
        /// <response code="200">Vraća listu žalbi</response>
        /// <response code="404">Nije pronađena ni jedna žalba</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ZalbaDto>>> GetAllZalbe()
        {
            var zalbe = await _zalbaRepository.GetAllZalbe();

            if (zalbe == null || zalbe.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllZalba", "Lista žalbi je prazna ili null.");
                return NoContent();
            }
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var zalbeDto = new List<ZalbaDto>();
            string url = _configuration["Services:KupacService"];
            foreach (var zalba in zalbe)
            {
                var zalbaDto = _mapper.Map<ZalbaDto>(zalba);
                if (zalba.KupacId is not null)
                {
                    var kupacDto = await _kupacService.SendGetRequestAsync(url + zalba.KupacId, token);
                    if(kupacDto is not null)
                    {
                        zalbaDto.Kupac = kupacDto;
                    }
                }
                zalbeDto.Add(zalbaDto);
            }

            await _loggerService.Log(LogLevel.Information, "GetAllZalba", "Lista žalbi je uspešno vraćena.");
            return Ok(zalbeDto);
        }

        /// <summary>
        /// Vraća jednu žalbu na osnovu ID-a
        /// </summary>
        /// <param name="zalbaId">ID žalbe</param>
        /// <returns>Žalba</returns>
        /// <response code="200">Vraća traženu žalbu</response>
        /// <response code="404">Nije pronađena žalba za uneti ID</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser,Menadzer, TehnickiSekretar, OperaterNadmetanja")]
        [HttpGet("{zalbaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ZalbaDto>>> GetZalba(Guid zalbaId)
        {
            var zalba = await _zalbaRepository.GetZalbaById(zalbaId);

            if (zalba == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetZalba", $"Žalba sa id-em {zalbaId} nije pronađena.");
                return NotFound();
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string url = _configuration["Services:KupacService"];

            var zalbaDto = _mapper.Map<ZalbaDto>(zalba);
            if(zalba.KupacId is not null)
            {
                var kupacDto = await _kupacService.SendGetRequestAsync(url + zalba.KupacId, token);
                if(kupacDto is not null)
                {
                    zalbaDto.Kupac = kupacDto;
                }
            }
            await _loggerService.Log(LogLevel.Information, "GetZalba", $"Žalba sa id-em {zalbaId} je uspešno vraćena.");
            return Ok(zalbaDto);
        }

        /// <summary>
        /// Kreira novu žalbu
        /// </summary>
        /// <param name="zalba">Model zalba</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove žalbe \
        /// POST /api/zalba \
        /// {   
        ///     "datumPodnosenja": "05-05-2020", \
        ///     "datumResenja": "05-12-2021", \
        ///     "razlogZalbe": "Nepotpuna dokumentacija", \
        ///     "obrazlozenje": "Nevalidna dokumentacija", \
        ///     "brojNadmetanja": "TT2913", \
        ///     "brojResenja": "PP291", \
        ///     "statusZalbeId": "1c989ee3-13b2-4d3b-abeb-c4e6343eace7" \
        ///     "tipZalbeId": "1c989ee3-13b2-4d3b-abeb-c4e6343eace7" \
        ///     "radnjaZaZalbuId": "1c989ee3-13b2-4d3b-abeb-c4e6343eace7" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju žalbe</returns>
        /// <response code="201">Vraća kreiranu žalbu</response>
        /// <response code="500">Desila se greška prilikom unosa nove žalbe</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ZalbaConfirmationDto>> CreateZalba([FromBody] ZalbaCreateDto zalba)
        {
            try
            {
                Zalba mapiranaZalba = _mapper.Map<Zalba>(zalba);
                var proveraValidnosti = await _zalbaRepository.IsValidZalba(mapiranaZalba);

                if(!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Uneli ste iste podatke za broj resenja ili broj nadmetanja. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Information, "CreateZalba", $"Greška prilikom unosa žalbe sa vrednostima: {JsonConvert.SerializeObject(zalba)}.");
                    return BadRequest(response);
                }

                ZalbaConfirmation createdZalba = await _zalbaRepository.CreateZalba(_mapper.Map<Zalba>(mapiranaZalba));

                string location = _linkGenerator.GetPathByAction("GetZalba", "Zalba", new { zalbaId = createdZalba.ZalbaId });
                await _loggerService.Log(LogLevel.Information, "CreateZalba", $"Žalba sa vrednostima: {JsonConvert.SerializeObject(zalba)} je uspešno kreirana.");
                return Created(location, _mapper.Map<ZalbaConfirmationDto>(createdZalba));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateZalba", $"Greška prilikom unosa žalbe sa vrednostima: {JsonConvert.SerializeObject(zalba)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Zalba error");
            }
        }

        /// <summary>
        /// Modifikacija  zalbe
        /// </summary>
        /// <param name="zalbaId">ID žalbe</param>
        /// <param name="zalba">Model zalbe</param>
        /// <returns>Potvrda o modifikaciji zalbe</returns>
        /// <response code="200">Izmenjena zalba</response>
        /// <response code="400">Desila se greška prilikom unosa istih podataka za žalbu</response>
        /// <response code="404">Nije pronađena zalba za uneti ID</response>
        /// <response code="500">Serverska greška tokom modifikacije zalbe</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{zalbaId}")]
        public async Task<ActionResult<ZalbaUpdateDto>> UpdateZalba(Guid zalbaId, [FromBody] ZalbaUpdateDto zalba)
        {
            try
            {
                Zalba mapiranaZalba = _mapper.Map<Zalba>(zalba);
                var proveraValidnosti = await _zalbaRepository.IsValidZalba(mapiranaZalba);

                if (!proveraValidnosti)
                {
                    var errorResponse = new
                    {
                        Message = "Unos istih podataka za broj nadmetanja i broj resenja. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Warning, "UpdateZalba", $"Greška prilikom unosa žalbe sa vrednostima: {JsonConvert.SerializeObject(zalba)}.");
                    return BadRequest(errorResponse);
                }

                var zalbaEntity = await _zalbaRepository.GetZalbaById(zalbaId);

                if (zalbaEntity == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateZalba", $"Zalba sa id-em {zalbaId} nije pronađena.");
                    return NotFound();
                }

                _mapper.Map(zalba, zalbaEntity);

                await _zalbaRepository.UpdateZalba(_mapper.Map<Zalba>(zalba));
                await _loggerService.Log(LogLevel.Information, "UpdateZalba", $"Zalba sa id-em {zalbaId} je uspešno izmenjena. Stare vrednosti su: {zalbaEntity}");
                return Ok(zalba);
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateZalba", $"Greška prilikom izmene žalbe sa id-em {zalbaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update zalba error");
            }
        }

        /// <summary>
        /// Brisanje žalbe na osnovu ID-a
        /// </summary>
        /// <param name="zalbaId">ID žalbe</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Žalba je uspešno obrisana</response>
        /// <response code="404">Nije pronađena žalba za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja žalbe</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{zalbaId}")]
        public async Task<ActionResult> DeleteZalba(Guid zalbaId)
        {
            try
            {
                var zalba = await _zalbaRepository.GetZalbaById(zalbaId);

                if (zalba == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteZalba", $"Žalba sa id-em {zalbaId} nije pronađena.");
                    return NotFound();
                }

                await _zalbaRepository.DeleteZalba(zalbaId);
                await _loggerService.Log(LogLevel.Information, "DeleteZalba", $"Žalba sa id-em {zalbaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(zalba)}");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteZalba", $"Greška prilikom brisanja žalbe sa id-em {zalbaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete zalba error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa zalbama
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetZalbaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
