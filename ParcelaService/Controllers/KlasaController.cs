using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Klasa;
using ParcelaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za klasu
    /// </summary>
    [Route("api/klasa")]
    [ApiController]
    [Produces("application/json", "application/xml")]

    public class KlasaController : ControllerBase
    {
        private readonly IKlasaRepository _klasaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public KlasaController(IKlasaRepository klasaRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _klasaRepository = klasaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }
        /// <summary>
        /// Vraća sve klase
        /// </summary>
        /// <returns>Lista klasa</returns>
        /// <response code="200">Vraća listu klasa</response>
        /// <response code="404">Nije pronađena ni jedna klasa</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, OperaterNadmetanja")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<KlasaDto>>> GetAllKlasa(string KlasaNaziv)
        {
            var klase = await _klasaRepository.GetAllKlasa(KlasaNaziv);

            if (klase == null || klase.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllKlasa", "Lista klasa parcele je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllKlasa", "Lista klasa parcele je uspešno vraćena.");

            return Ok(_mapper.Map<List<KlasaDto>>(klase));
        }

        /// <summary>
        /// Vraća jednu klasu na osnovu ID-a 
        /// </summary>
        /// <param name="klasaId">Model klase</param>
        /// <returns>Klasa</returns>
        /// <response code="200">Vraća traženu klasu</response>
        /// <response code="404">Nije pronađena klasa za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, OperaterNadmetanja")]
        [HttpGet("{klasaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<KlasaDto>> GetKlasa(Guid klasaId)
        {
            var klasa = await _klasaRepository.GetKlasaById(klasaId);

            if (klasa == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKlasa", $"Klasa parcele sa id-em {klasaId} nije pronađena.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetKlasa", $"Klasa parcele sa id-em {klasaId} je uspešno vraćena.");

            return Ok(_mapper.Map<KlasaDto>(klasa));
        }

        /// <summary>
        /// Kreira novu klasu
        /// </summary>
        /// <param name="klasa">Model klase</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove klase \
        /// POST /api/klasa \
        /// {
        ///      "klasaNaziv": "Naziv klase"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju klase</returns>
        /// <response code="201">Vraća kreiranu klasu</response>
        /// <response code="500">Desila se greška prilikom unosa nove klase</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KlasaDto>> CreateKlasa([FromBody] KlasaCreationDto klasa)
        {
            try
            {
                Klasa novaKlasa = await _klasaRepository.CreateKlasa(_mapper.Map<Klasa>(klasa));
                await _klasaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetKlasa", "Klasa", new { klasaId = novaKlasa.KlasaId });

                await _loggerService.Log(LogLevel.Information, "CreateKlasa", $"Klasa parcele sa vrednostima: {JsonConvert.SerializeObject(klasa)} je uspešno kreirana.");


                return Created(lokacija, _mapper.Map<KlasaDto>(novaKlasa));
            }

            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateKlasa", $"Greška prilikom unosa klase parcele sa vrednostima: {JsonConvert.SerializeObject(klasa)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja klase!");
            }
        }

        /// <summary>
        /// Izmena klase
        /// </summary>
        /// <param name="klasa">Model klase</param>
        /// <returns>Potvrda o izmeni klase</returns>
        /// <response code="200">Izmenjena klasa</response>
        /// <response code="404">Nije pronađena klasa za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene klase</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KlasaDto>> UpdateKlasa(KlasaUpdateDto klasa)
        {
            try
            {
                var staraKlasa = await _klasaRepository.GetKlasaById(klasa.KlasaId);

                if (staraKlasa == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateKlasa", $"Klasa parcele sa id-em {klasa.KlasaId} nije pronađena.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(staraKlasa);

                Klasa novaKlasa = _mapper.Map<Klasa>(klasa);

                _mapper.Map(novaKlasa, staraKlasa);
                await _klasaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateKlasa", $"Klasa parcele sa id-em {klasa.KlasaId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<KlasaDto>(staraKlasa));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateKlasa", $"Greška prilikom izmene klase parcele sa id-em {klasa.KlasaId}.",ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene klase");
            }
        }

        /// <summary>
        /// Brisanje klase na osnovu ID-a
        /// </summary>
        /// <param name="klasaId">ID klase</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Klasa je uspešno obrisana</response>
        /// <response code="404">Nije pronađena klasa za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja klasa</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpDelete("{klasaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteKlasa(Guid klasaId)
        {
            try
            {
                var klasa = await _klasaRepository.GetKlasaById(klasaId);

                if (klasa == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteKlasa", $"Klasa parcele sa id-em {klasaId} nije pronađena.");
                    return NotFound();
                }

                await _klasaRepository.DeleteKlasa(klasaId);
                await _klasaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteKlasa", $"Klasa parcele sa id-em {klasaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(klasa)}");

                return NoContent();
            }

            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteKlasa", $"Greška prilikom brisanja klase parcele sa id-em {klasaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja klase");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa klasama
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetKlasaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }

}
