using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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

        public StatusController(IStatusRepository statusRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _statusRepository = statusRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
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
                return NoContent();
            }

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
                return NotFound();
            }

            return Ok(_mapper.Map<StatusDto>(status));
        }

        /// <summary>
        /// Kreira novi status javnog nadmetanja
        /// </summary>
        /// <param name="status">Model status</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog statusa \
        /// POST /api/status \
        /// { \   
        ///     "NazivStatusa": "Treci krug" \
        ///} \
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

                return Created(lokacija, _mapper.Map<StatusDto>(noviStatus));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja statusa");
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
                    return NotFound();
                }

                Status noviStatus = _mapper.Map<Status>(status);

                _mapper.Map(noviStatus, stariStatus);
                await _statusRepository.SaveChangesAsync();

                return Ok(_mapper.Map<StatusDto>(stariStatus));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene statusa");
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
                    return NotFound();
                }

                await _statusRepository.DeleteStatus(statusId);
                await _statusRepository.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja statusa");
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
