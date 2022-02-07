using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Helpers;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;
using JavnoNadmetanjeService.ServiceCalls;
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
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Controllers
{
    /// <summary>
    /// Kontroler za javno nadmetanje
    /// </summary>
    [Route("api/javnoNadmetanje")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class JavnoNadmetanjeController : ControllerBase
    {
        private readonly IJavnoNadmetanjeRepository _javnoNadmetanjeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;
        private readonly IJavnoNadmetanjeCalls _javnoNadmetanjeCalls;

        public JavnoNadmetanjeController(IJavnoNadmetanjeRepository javnoNadmetanjeRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService, IJavnoNadmetanjeCalls javnoNadmetanjeCalls)
        {
            _javnoNadmetanjeRepository = javnoNadmetanjeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
            _javnoNadmetanjeCalls = javnoNadmetanjeCalls;
        }

        /// <summary>
        /// Vraća sva javna nadmetanja
        /// </summary>
        /// <returns>Lista javnih nadmetanja</returns>
        /// <response code="200">Vraća listu javnih nadmetanja</response>
        /// <response code="404">Nije pronađeno ni jedno javno nadmetanje</response>
        [Authorize(Roles = "Administrator, Superuser, Manager, OperaterNadmetanje")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JavnoNadmetanjeDto>>> GetAllJavnoNadmetanje()
        {
            var javnaNadmetanja = await _javnoNadmetanjeRepository.GetAllJavnoNadmetanje();

            if (javnaNadmetanja == null || javnaNadmetanja.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllJavnoNadmetanje", "Lista javnih nadmetanja je prazna ili null.");
                return NoContent();
            }

            var javnaNadmetanjaDto = new List<JavnoNadmetanjeDto>();
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            foreach (var javnoNad in javnaNadmetanja)
            {
                javnaNadmetanjaDto.Add(await _javnoNadmetanjeCalls.GetJavnoNadmetanjeDtoWithOtherServicesInfo(javnoNad, token));
            }

            await _loggerService.Log(LogLevel.Information, "GetAllJavnoNadmetanje", "Lista javnih nadmetanja je uspešno vraćena.");

            return Ok(javnaNadmetanjaDto);
        }

        /// <summary>
        /// Vraća jedno javno nadmetanje na osnovu ID-a
        /// </summary>
        /// <param name="javnoNadmetanjeId">ID javnog nadmetanja</param>
        /// <returns>Javno nadmetanje</returns>
        /// <response code="200">Vraća traženo javno nadmetanje</response>
        /// <response code="404">Nije pronađeno javno nadmetanje za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Manager, OperaterNadmetanje")]
        [HttpGet("{javnoNadmetanjeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JavnoNadmetanjeDto>> GetJavnoNadmetanje(Guid javnoNadmetanjeId)
        {
            var javnoNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanjeId);

            if (javnoNadmetanje == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetJavnoNadmetanje", $"Javno nadmetanje sa id-em {javnoNadmetanjeId} nije pronađeno.");
                return NotFound();
            }
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            await _loggerService.Log(LogLevel.Information, "GetJavnoNadmetanje", $"Javno nadmetanje sa id-em {javnoNadmetanjeId} je uspešno vraćeno.");

            return Ok(await _javnoNadmetanjeCalls.GetJavnoNadmetanjeDtoWithOtherServicesInfo(javnoNadmetanje,token));
        }

        /// <summary>
        /// Kreira novo javno nadmetanje
        /// </summary>
        /// <param name="javnoNadmetanje">Model javnog nadmetanja</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog javnog nadmetanja \
        /// POST /api/javnoNadmetanje \
        /// {
        ///     "PocetnaCenaHektar": 550.00000000, \
        ///     "PeriodZakupa": 5, \
        ///     "IzlicitiranaCena": 750, \
        ///     "Krug": 1, \
        ///     "Izuzeto": false, \
        ///     "StatusId": "3B7EE65F-EB68-4A32-AE69-DF7FDF463188", \
        ///     "TipId": "D6D56B98-3672-4BDB-A0CB-E916FFE053C8", \
        ///     "KupacId": "FEBD1C29-90E7-40C2-97F3-1E88495FE98D", \
        ///     "AdresaId": "37371ef6-4f25-48b3-9bf2-fe72a81f88d2", \
        ///     "OvlascenaLica": ["5E1BFFFC-1AEE-4662-BC04-341C35B9EBDC", "1B070B3A-BBA6-470D-AAD7-40986EFB00EF"], \
        ///     "Kupci": ["FEBD1C29-90E7-40C2-97F3-1E88495FE98D", "4BA95C01-AA89-4D36-A467-C72B0FCC5B80"] \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju javnog nadmetanja</returns>
        /// <response code="200">Vraća kreirano javno nadmetanje</response>
        /// <response code="500">Desila se greška prilikom unosa novog javnog nadmetanja</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanje")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JavnoNadmetanjeConfirmationDto>> CreateJavnoNadmetanje([FromBody] JavnoNadmetanjeCreationDto javnoNadmetanje)
        {
            try
            {
                JavnoNadmetanjeConfirmation novoNadmetanje = await _javnoNadmetanjeRepository.CreateJavnoNadmetanje(_mapper.Map<JavnoNadmetanje>(javnoNadmetanje));
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetJavnoNadmetanje", "JavnoNadmetanje", new { javnoNadmetanjeId = novoNadmetanje.JavnoNadmetanjeId });

                await _loggerService.Log(LogLevel.Information, "CreateJavnoNadmetanje", $"Javno nadmetanje sa vrednostima: {JsonConvert.SerializeObject(javnoNadmetanje)} je uspešno kreirano.");

                return Created(lokacija, _mapper.Map<JavnoNadmetanjeConfirmationDto>(novoNadmetanje));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateJavnoNadmetanje", $"Greška prilikom unosa javnog nadmetanja sa vrednostima: {JsonConvert.SerializeObject(javnoNadmetanje)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa javnog nadmetanja");
            }
        }

        /// <summary>
        /// Izmena javnog nadmetanja
        /// </summary>
        /// <param name="javnoNadmetanje">Model javno nadmetanje</param>
        /// <returns>Potvrda o izmeni javnog nadmetanja</returns>
        /// <response code="200">Izmenjeno javno nadmetanje</response>
        /// <response code="404">Nije pronađeno javno nadmetanje za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene javnog nadmetanja</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanje")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JavnoNadmetanjeDto>> UpdateJavnoNadmetanje(JavnoNadmetanjeUpdateDto javnoNadmetanje)
        {
            try
            {
                var staroNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanje.JavnoNadmetanjeId);

                if (staroNadmetanje == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateJavnoNadmetanje", $"Javno nadmetanje sa id-em {javnoNadmetanje.JavnoNadmetanjeId} nije pronađeno.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(javnoNadmetanje);

                JavnoNadmetanje novoNadmetanje = _mapper.Map<JavnoNadmetanje>(javnoNadmetanje);

                _mapper.Map(novoNadmetanje, staroNadmetanje);
                await _javnoNadmetanjeRepository.UpdateJavnoNadmetanje(novoNadmetanje);
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateJavnoNadmetanje", $"Javno nadmetanje sa id-em {javnoNadmetanje.JavnoNadmetanjeId} je uspešno izmenjeno. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<JavnoNadmetanjeDto>(staroNadmetanje));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateJavnoNadmetanje", $"Greška prilikom izmene javnog nadmetanja sa id-em {javnoNadmetanje.JavnoNadmetanjeId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene javnog nadmetanja");
            }
        }

        /// <summary>
        /// Brisanje javnog nadmetanja na osnovu ID-a
        /// </summary>
        /// <param name="javnoNadmetanjeId">ID javnog nadmetanja</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Javno nadmetanje je uspešno obrisano</response>
        /// <response code="404">Nije pronađeno javno nadmetanje za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja javnog nadmetanja</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanje")]
        [HttpDelete("{javnoNadmetanjeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteJavnoNadmetanje(Guid javnoNadmetanjeId)
        {
            try
            {
                var javnoNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanjeId);

                if (javnoNadmetanje == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteJavnoNadmetanje", $"Javno nadmetanje sa id-em {javnoNadmetanjeId} nije pronađeno.");
                    return NotFound();
                }

                await _javnoNadmetanjeRepository.DeleteJavnoNadmetanje(javnoNadmetanjeId);
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteJavnoNadmetanje", $"Javno nadmetanje sa id-em {javnoNadmetanjeId} je uspešno obrisano. Obrisane vrednosti: {JsonConvert.SerializeObject(javnoNadmetanje)}");

                return NoContent();

            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteJavnoNadmetanje", $"Greška prilikom brisanja javnog nadmetanja sa id-em {javnoNadmetanjeId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja javnog nadmetanja");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa javnim nadmetanjima
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, Manager, OperaterNadmetanje")]
        [HttpOptions]
        public IActionResult GetJavnoNadmetanjeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
