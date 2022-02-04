using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Obradivost;
using ParcelaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za obradivost
    /// </summary>
    [Route("api/obradivost")]
    [ApiController]
    [Produces("application/json", "application/xml")]

    public class ObradivostController : ControllerBase
    {
        private readonly IObradivostRepository _obradivostRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public ObradivostController(IObradivostRepository obradivostRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _obradivostRepository = obradivostRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve obradivosti
        /// </summary>
        /// <returns>Lista obradivosti</returns>
        /// <response code="200">Vraća listu obradivosti</response>
        /// <response code="404">Nije pronađena ni jedna obradivost</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ObradivostDto>>> GetAllObradivost(string opisObradivosti)
        {
            var obradivost = await _obradivostRepository.GetAllObradivost(opisObradivosti);

            if(obradivost == null || obradivost.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllObradivost", "Lista obradivosti parcele je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllObradivost", "Lista obradivosti parcele je uspešno vraćena.");

            return Ok(_mapper.Map<List<ObradivostDto>>(obradivost));
        }

        /// <summary>
        /// Vraća jednu obradivost na osnovu ID-a
        /// </summary>
        /// <param name="obradivostId">ID obradivosti</param>
        /// <returns>Obradivost</returns>
        /// <response code="200">Vraća traženu obradivost</response>
        /// <response code="404">Nije pronađena obradivost za uneti ID</response>
        [HttpGet("obradivostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ObradivostDto>> GetObradivost(Guid obradivostId)
        {
            var obradivost = await _obradivostRepository.GetObradivostById(obradivostId);

            if(obradivost == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetObradivost", $"Obradivost parcele sa id-em {obradivostId} nije pronađena.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetObradivost", $"Obradivost parcele sa id-em {obradivost} je uspešno vraćena.");

            return Ok(_mapper.Map<ObradivostDto>(obradivost));
        }

        /// <summary>
        /// Kreira novu obradivost
        /// </summary>
        /// <param name="obradivost">Model obradivosti</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove obradivosti \
        /// POST /api/obradivost \
        /// {
        ///       "opisObradivosti": "Opis obradivosti"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju obradivosti</returns>
        /// <response code="201">Vraća kreiranu obradivost</response>
        /// <response code="500">Desila se greška prilikom unosa nove obradivosti</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ObradivostDto>> CreateObradivost([FromBody] ObradivostCreationDto obradivost)
        {
            try
            {
                Obradivost novaObradivost = await _obradivostRepository.CreateObradivost(_mapper.Map<Obradivost>(obradivost));
                await _obradivostRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetObradivost", "Obradivost", new { obradivostId = novaObradivost.ObradivostId });

                await _loggerService.Log(LogLevel.Information, "CreateObradivost", $"Obradivost parcele sa vrednostima: {JsonConvert.SerializeObject(obradivost)} je uspešno kreiran.");

                return Created(lokacija, _mapper.Map<ObradivostDto>(novaObradivost));
            }

            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateObradivost", $"Greška prilikom unosa obradivosti parcele sa vrednostima: {JsonConvert.SerializeObject(obradivost)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja obradivosti!");
            }

        }

        /// <summary>
        /// Izmena obradivosti
        /// </summary>
        /// <param name="obradivost">Model obradivost</param>
        /// <returns>Potvrda o izmeni obradivosti</returns>
        /// <response code="200">Izmenjena obradivost</response>
        /// <response code="404">Nije pronađena obradivost za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene obradivosti</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ObradivostDto>> UpdateObradivost(ObradivostUpdateDto obradivost)
        {
            try
            {
                var staraObradivost = await _obradivostRepository.GetObradivostById(obradivost.ObradivostId);

                if(staraObradivost == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateObradivost", $"Obradivost parcele sa id-em {obradivost.ObradivostId} nije pronađena.");
                    return NotFound();
                }

                var stareVrednosti = JsonConvert.SerializeObject(staraObradivost);

                Obradivost novaObradivost = _mapper.Map<Obradivost>(obradivost);

                _mapper.Map(staraObradivost, novaObradivost);
                await _obradivostRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateObradivost", $"Obradivost parcele sa id-em {obradivost.ObradivostId} je uspešno izmenjena. Stare vrednosti su: {staraObradivost}");

                return Ok(_mapper.Map<ObradivostDto>(staraObradivost));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateObradivost", $"Greška prilikom izmene obradivosti parcele sa id-em {obradivost.ObradivostId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene obradivosti!");
            }
        }
        /// <summary>
        /// Brisanje obradivosti na osnovu ID-a
        /// </summary>
        /// <param name="obradivostId">ID obradivosti</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Obradivost je uspešno obrisana</response>
        /// <response code="404">Nije pronađena obradivost za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja obradivosti</response>
        [HttpDelete("{obradivostId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteObradivost (Guid obradivostId)
        {
            try
            {
                var obradivost = await _obradivostRepository.GetObradivostById(obradivostId);

                if (obradivost == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteObradivost", $"Obradivost parcele sa id-em {obradivostId} nije pronađena.");
                    return NotFound();
                }

                await _obradivostRepository.DeleteObradivost(obradivostId);
                await _obradivostRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteObradivost", $"Obradivost parcele sa id-em {obradivostId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(obradivost)}");

                return NoContent();
            }

            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteObradivost", $"Greška prilikom brisanja obradivosti parcele sa id-em {obradivostId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja obradivosti!");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa obradivostima
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetObradivostOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }

}
