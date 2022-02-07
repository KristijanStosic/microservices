using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Data;
using ZalbaService.Entities;
using ZalbaService.Models;
using ZalbaService.Models.StatusZalbe;
using ZalbaService.ServicesCalls;

namespace ZalbaService.Controllers
{
    /// <summary>
    /// Kontroler statusa zalbe
    /// </summary>
    [ApiController]
    [Route("api/statusZalbe")]
    [Produces("application/json", "application/xml")]
    public class StatusZalbeController : ControllerBase
    {
        private readonly IStatusZalbeRepository _statusZalbeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor kontrolera broj table - DI
        /// </summary>
        /// <param name="statusZalbeRepository">Repo statusa zalbe</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="loggerService">Logger servis</param>
        public StatusZalbeController(IStatusZalbeRepository statusZalbeRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _statusZalbeRepository = statusZalbeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve statuse zalbi
        /// </summary>
        /// <returns>Lista statusa zalbi</returns>
        /// <response code="200">Vraća listu statusa zalbi</response>
        /// <response code="204">Nije pronađen ni jedan status zalbe</response>
        [Authorize(Roles = "Administrator, Superuser,Menadzer, TehnickiSekretar, OperaterNadmetanja")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<StatusZalbeDto>>> GetAllStatusesZalbe(string nazivStatusaZalbe)
        {
            var statusesZalbe = await _statusZalbeRepository.GetAllStatusesZalbe(nazivStatusaZalbe);

            if(statusesZalbe == null || statusesZalbe.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllStatusZalbe", "Lista statusa zalbi je prazna ili null");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetAllStatusZalbe", "Lista statusa zalbi je uspešno vraćena.");
            return Ok(_mapper.Map<IEnumerable<StatusZalbeDto>>(statusesZalbe));
        }

        /// <summary>
        /// Vraća jedan status zalbe na osnovu ID-a
        /// </summary>
        /// <param name="statusZalbeId">ID statusa zalbe</param>
        /// <returns>Status zalbe</returns>
        /// <response code="200">Vraća traženi status zalbe</response>
        /// <response code="404">Nije pronađen status zalbe za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser,Menadzer, TehnickiSekretar, OperaterNadmetanja")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{statusZalbeId}")]
        public async Task<ActionResult<StatusZalbeDto>> GetStatusZalbe(Guid statusZalbeId)
        {
            var statusZalbe = await _statusZalbeRepository.GetStatusZalbeById(statusZalbeId);

            if(statusZalbe == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetStatusZalbe", $"Status zalbe sa id-em {statusZalbeId} nije pronađen.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetStatusZalbe", $"Status zalbe sa id-em {statusZalbeId} je uspešno vraćen.");
            return Ok(_mapper.Map<StatusZalbeDto>(statusZalbe));
        }

        /// <summary>
        /// Kreira novi status zalbe
        /// </summary>
        /// <param name="statusZalbe">Model status zalbe</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog statusa zalbe \
        /// POST /api/statusZalbe \
        /// {   
        ///     "NazivStatusaZalbe": "Odbijena"
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju statusa zalbe</returns>
        /// <response code="201">Vraća kreiran status zalbe</response>
        /// <response code="400">Desila se greška prilikom unosa istih podataka za status žalbe</response>
        /// <response code="500">Desila se greška prilikom unosa novog statusa zalbe</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<StatusZalbeCreateDto>> CreateStatusZalbe([FromBody] StatusZalbeCreateDto statusZalbe)
        {
            try
            {
                var proveraValidnosti = await _statusZalbeRepository.IsValidStatusZalbe(statusZalbe.NazivStatusaZalbe);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Information, "CreateStatusZalbe", $"Greška prilikom unosa statusa žalbe sa vrednostima: {JsonConvert.SerializeObject(statusZalbe)}.");
                    return BadRequest(response);
                }

                StatusZalbe createdStatusZalbe = await _statusZalbeRepository.CreateStatusZalbe(_mapper.Map<StatusZalbe>(statusZalbe));

                string location = _linkGenerator.GetPathByAction("GetStatusZalbe", "StatusZalbe", new { statusZalbeId = createdStatusZalbe.StatusZalbeId });
                await _loggerService.Log(LogLevel.Information, "CreateStatusZalbe", $"Status žalbe sa vrednostima: {JsonConvert.SerializeObject(statusZalbe)} je uspešno kreiran.");
                return Created(location, _mapper.Map<StatusZalbeCreateDto>(createdStatusZalbe));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateStatusZalbe", $"Greška prilikom unosa statusa žalbe sa vrednostima: {JsonConvert.SerializeObject(statusZalbe)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Status Zalbe error");
            }
        }

        /// <summary>
        /// Modifikacija statusa zalbe
        /// </summary>
        /// <param name="statusZalbeId">ID statusa žalbe</param>
        /// <param name="statusZalbe">Model statusa zalbe</param>
        /// <returns>Potvrda o modifikaciji statusa zalbe</returns>
        /// <response code="200">Izmenjen status zalbe</response>
        /// <response code="400">Desila se greška prilikom unosa istih podataka za status žalbe</response>
        /// <response code="404">Nije pronađen status zalbe za uneti ID</response>
        /// <response code="500">Serverska greška tokom modifikacije statusa zalbe</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{statusZalbeId}")]
        public async Task<ActionResult<StatusZalbeUpdateDto>> UpdateStatusZalbe(Guid statusZalbeId, [FromBody]StatusZalbeUpdateDto statusZalbe)
        {
            try
            {
                var proveraValidnosti = await _statusZalbeRepository.IsValidStatusZalbe(statusZalbe.NazivStatusaZalbe);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Warning, "UpdateStatusZalbe", $"Greška prilikom unosa statusa žalbe sa vrednostima: {JsonConvert.SerializeObject(statusZalbe)}.");
                    return BadRequest(response);
                }

                var statusZalbeEntity = await _statusZalbeRepository.GetStatusZalbeById(statusZalbeId);

                if(statusZalbeEntity == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateStatusZalbe", $"Status zalbe sa id-em {statusZalbeId} nije pronađen.");
                    return NotFound();
                }

                _mapper.Map(statusZalbe, statusZalbeEntity);

                await _statusZalbeRepository.UpdateStatusZalbe(_mapper.Map<StatusZalbe>(statusZalbe));
                await _loggerService.Log(LogLevel.Information, "UpdateStatusZalbe", $"Status zalbe sa id-em {statusZalbeId} je uspešno modifikovan.");
                return Ok(statusZalbe);
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateStatusZalbe", $"Greška prilikom unosa statusa žalbe sa vrednostima: {JsonConvert.SerializeObject(statusZalbe)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update status zalbe error");
            }
        }

        /// <summary>
        /// Brisanje statusa žalbe na osnovu ID-a
        /// </summary>
        /// <param name="statusZalbeId">ID statusa žalbe</param>
        /// <returns>Status 204 (No Content)</returns>
        /// <response code="204">Status žalbe je uspešno obrisan</response>
        /// <response code="404">Nije pronađen status žalbe za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja statusa žalbe</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{statusZalbeId}")]
        public async Task<ActionResult> DeleteStatusZalbe(Guid statusZalbeId)
        {
            try
            {
                var statusZalbe = await _statusZalbeRepository.GetStatusZalbeById(statusZalbeId);

                if (statusZalbe == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteStatusZalbe", $"Status žalbe sa id-em {statusZalbeId} nije pronađen.");
                    return NotFound();
                }

                await _statusZalbeRepository.DeleteStatusZalbe(statusZalbeId);
                await _loggerService.Log(LogLevel.Information, "DeleteStatusZalbe", $"Status žalbe sa id-em {statusZalbeId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(statusZalbe)}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteStatusZalbe", $"Greška prilikom brisanja statusa žalbe sa id-em {statusZalbeId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete status zalbe error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa zalbama
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Administrator, Superuser,Menadzer, TehnickiSekretar, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetZalbaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
