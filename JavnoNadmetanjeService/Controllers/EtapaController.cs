using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Models.Etapa;
using JavnoNadmetanjeService.ServiceCalls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Controllers
{
    /// <summary>
    /// Kontroler za etapu
    /// </summary>
    [Route("api/etapa")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class EtapaController : ControllerBase
    {
        private readonly IEtapaRepository _etapaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public EtapaController(IEtapaRepository etapaRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _etapaRepository = etapaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve etape
        /// </summary>
        /// <returns>Lista etapa</returns>
        /// <response code="200">Vraća listu etapa</response>
        /// <response code="404">Nije pronađena ni jedna etapa</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<EtapaDto>>> GetAllEtapa()
        {
            var etape = await _etapaRepository.GetAllEtapa();

            if (etape == null || etape.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllEtapa", "Lista etapa je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllEtapa", "Lista etapa je uspešno vraćena.");

            return Ok(_mapper.Map<List<EtapaDto>>(etape));
        }

        /// <summary>
        /// Vraća jednu etapu na osnovu ID-a
        /// </summary>
        /// <param name="etapaId">ID etape</param>
        /// <returns>Etapa</returns>
        /// <response code="200">Vraća traženu etapu</response>
        /// <response code="404">Nije pronađena etapa za uneti ID</response>
        [HttpGet("{etapaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EtapaDto>> GetEtapa(Guid etapaId)
        {
            var etapa = await _etapaRepository.GetEtapaById(etapaId);

            if (etapa == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetEtapa", $"Etapa sa id-em {etapaId} nije pronađena.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetEtapa", $"Etapa sa id-em {etapaId} je uspešno vraćena.");

            return Ok(_mapper.Map<EtapaDto>(etapa));
        }

        /// <summary>
        /// Kreira novu etapu
        /// </summary>
        /// <param name="etapa">Model etapa</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove etape \
        /// POST /api/etapa \
        /// {
        ///     "Datum": "2022-01-31T07:25:31.426Z", \
        ///     "DanPoRedu": 1, \
        ///     "VremePocetka": "12:00", \
        ///     "VremeKraja": "14:00", \
        ///     "ZavrsenaUspesno": false, \
        ///     "JavnoNadmetanjeId": "417d646c-9e90-4515-a068-21689864330a" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju etape</returns>
        /// <response code="200">Vraća kreiranu etapu</response>
        /// <response code="500">Desila se greška prilikom unosa nove etape</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EtapaConfirmationDto>> CreateEtapa([FromBody] EtapaCreationDto etapa)
        {
            try
            {
                Etapa mapiranaEtapa = _mapper.Map<Etapa>(etapa);
                var proveraValidnosti = await _etapaRepository.IsValidEtapa(mapiranaEtapa);

                if (!proveraValidnosti)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Već postoji etapa u okviru ovog javnog nadmetanja koja je uspešno završena!");
                }

                EtapaConfirmation novaEtapa = await _etapaRepository.CreateEtapa(mapiranaEtapa);
                await _etapaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetEtapa", "Etapa", new { etapaId = novaEtapa.EtapaId });

                await _loggerService.Log(LogLevel.Information, "CreateEtapa", $"Etapa sa vrednostima: {JsonConvert.SerializeObject(etapa)} je uspešno kreirana.");

                return Created(lokacija, _mapper.Map<EtapaConfirmationDto>(novaEtapa));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateEtapa", $"Greška prilikom unosa etape sa vrednostima: {JsonConvert.SerializeObject(etapa)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa etape");
            }
        }

        /// <summary>
        /// Izmena etape
        /// </summary>
        /// <param name="etapa">Model etapa</param>
        /// <returns>Potvrda o izmeni etape</returns>
        /// <response code="200">Izmenjena etapa</response>
        /// <response code="404">Nije pronađena etapa za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene etape</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EtapaDto>> UpdateEtapa(EtapaUpdateDto etapa)
        {
            try
            {
                var staraEtapa = await _etapaRepository.GetEtapaById(etapa.EtapaId);

                if (staraEtapa == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateEtapa", $"Etapa sa id-em {etapa.EtapaId} nije pronađena.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(staraEtapa);

                Etapa novaEtapa = _mapper.Map<Etapa>(etapa);

                _mapper.Map(novaEtapa, staraEtapa);
                await _etapaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateEtapa", $"Etapa sa id-em {etapa.EtapaId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<EtapaDto>(staraEtapa));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateEtapa", $"Greška prilikom izmene etape sa id-em {etapa.EtapaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene etape");
            }
        }

        /// <summary>
        /// Brisanje etape na osnovu ID-a
        /// </summary>
        /// <param name="etapaId">ID etape</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Etapa je uspešno obrisana</response>
        /// <response code="404">Nije pronađena etapa za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja etape</response>
        [HttpDelete("{etapaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEtapa(Guid etapaId)
        {
            try
            {
                var etapa = await _etapaRepository.GetEtapaById(etapaId);

                if (etapa == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteEtapa", $"Etapa sa id-em {etapaId} nije pronađena.");
                    return NotFound();
                }

                await _etapaRepository.DeleteEtapa(etapaId);
                await _etapaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteEtapa", $"Etapa sa id-em {etapaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(etapa)}");

                return NoContent();

            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteEtapa", $"Greška prilikom brisanja etape sa id-em {etapaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja etape");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa etapama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetEtapaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
