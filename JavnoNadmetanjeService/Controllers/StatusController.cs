using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.Status;
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
    /// Kontroler za status
    /// </summary>
    [Route("api/status")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public StatusController(IStatusRepository statusRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _statusRepository = statusRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve statuse javnog nadmetanja
        /// </summary>
        /// <param name="nazivStatusa">Naziv statusa javnog nadmetanja</param>
        /// <returns>Lista statusa</returns>
        /// <response code="200">Vraća listu statusa</response>
        /// <response code="404">Nije pronađen ni jedan status</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<StatusDto>>> GetAllStatus(string nazivStatusa)
        {
            var statusi = await _statusRepository.GetAllStatus(nazivStatusa);

            if (statusi == null || statusi.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllStatus", "Lista statusa javnog nadmetanja je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllStatus", "Lista statusa javnog nadmetanja je uspešno vraćena.");

            return Ok(_mapper.Map<List<StatusDto>>(statusi));
        }

        /// <summary>
        /// Vraća jedan status javnog nadmetanja na osnovu ID-a
        /// </summary>
        /// <param name="statusId">ID statusa</param>
        /// <returns>Status javnog nadmetanja</returns>
        /// <response code="200">Vraća traženi status</response>
        /// <response code="404">Nije pronađen status za uneti ID</response>
        [HttpGet("{statusId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StatusDto>> GetStatus(Guid statusId)
        {
            var status = await _statusRepository.GetStatusById(statusId);

            if (status == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetStatus", $"Status javnog nadmetanja sa id-em {statusId} nije pronađen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetStatus", $"Status javnog nadmetanja sa id-em {statusId} je uspešno vraćen.");

            return Ok(_mapper.Map<StatusDto>(status));
        }

        /// <summary>
        /// Kreira novi status javnog nadmetanja
        /// </summary>
        /// <param name="status">Model status</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog statusa \
        /// POST /api/status \
        /// {
        ///     "NazivStatusa": "Treci krug" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju statusa</returns>
        /// <response code="200">Vraća kreiran status</response>
        /// <response code="500">Desila se greška prilikom unosa novog statusa</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StatusDto>> CreateStatus([FromBody] StatusCreationDto status)
        {
            try
            {
                Status noviStatus = await _statusRepository.CreateStatus(_mapper.Map<Status>(status));
                await _statusRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetStatus", "Status", new { statusId = noviStatus.StatusId });

                await _loggerService.Log(LogLevel.Information, "CreateStatus", $"Status javnog nadmetanja sa vrednostima: {JsonConvert.SerializeObject(status)} je uspešno kreiran.");

                return Created(lokacija, _mapper.Map<StatusDto>(noviStatus));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateStatus", $"Greška prilikom unosa statusa javnog nadmetanja sa vrednostima: {JsonConvert.SerializeObject(status)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom kreiranja statusa");
            }
        }

        /// <summary>
        /// Izmena statusa javnog nadmetanja
        /// </summary>
        /// <param name="status">Model status</param>
        /// <returns>Potvrda o izmeni statusa</returns>
        /// <response code="200">Izmenjen status</response>
        /// <response code="404">Nije pronađen status za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene statusa</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StatusDto>> UpdateStatus(StatusUpdateDto status)
        {
            try
            {
                var stariStatus = await _statusRepository.GetStatusById(status.StatusId);

                if (stariStatus == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateStatus", $"Status javnog nadmetanja sa id-em {status.StatusId} nije pronađen.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(stariStatus);

                Status noviStatus = _mapper.Map<Status>(status);

                _mapper.Map(noviStatus, stariStatus);
                await _statusRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateStatus", $"Status javnog nadmetanja sa id-em {status.StatusId} je uspešno izmenjen. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<StatusDto>(stariStatus));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateStatus", $"Greška prilikom izmene statusa javnog nadmetanja sa id-em {status.StatusId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene statusa");
            }
        }

        /// <summary>
        /// Brisanje statusa javnog nadmetanja na osnovu ID-a
        /// </summary>
        /// <param name="statusId">ID statusa</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Status je uspešno obrisan</response>
        /// <response code="404">Nije pronađen status za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja statusa</response>
        [HttpDelete("{statusId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStatus(Guid statusId)
        {
            try
            {
                var status = await _statusRepository.GetStatusById(statusId);

                if (status == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteStatus", $"Status javnog nadmetanja sa id-em {statusId} nije pronađen.");
                    return NotFound();
                }

                await _statusRepository.DeleteStatus(statusId);
                await _statusRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteStatus", $"Status javnog nadmetanja sa id-em {statusId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(status)}");
                
                return NoContent();

            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteStatus", $"Greška prilikom brisanja statusa javnog nadmetanja sa id-em {statusId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja statusa");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa statusima javnog nadmetanja
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetStatusOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
