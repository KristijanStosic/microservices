using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LicnostService.Data;
using LicnostService.Entities;
using LicnostService.Models.Licnost;
using LicnostService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LicnostService.Controllers
{
    /// <summary>
    /// Kontroler za ličnost
    /// </summary>
    [ApiController]
    [Route("api/licnost")]
    [Produces("application/json", "application/xml")]
    public class LicnostController : ControllerBase
    {

        private readonly ILicnostRepository _licnostRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public LicnostController(ILicnostRepository licnostRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _licnostRepository = licnostRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }
        /// <summary>
        /// Vraća sve ličnosti
        /// </summary>
        /// <param name="imeLicnosti"></param>
        /// <returns>Lista ličnosti</returns>
        /// <response code="200">Vraća listu ličnosti</response>
        /// <response code="404">Nije pronađena ni jedna ličnosti</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<LicnostDto>>> GetAllLicnosti(string imeLicnosti)
        {
            var licnosti = await _licnostRepository.GetAllLicnosti(imeLicnosti);

            if (licnosti == null || licnosti.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllLicnosti", "Lista licnosti je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllLicnosti", "Lista licnosti je uspešno vraćena.");

            return Ok(_mapper.Map<IEnumerable<LicnostDto>>(licnosti));
        }
        /// <summary>
        /// Vraća jednu ličnost na osnovu ID-a
        /// </summary>
        /// <param name="licnostId">ID ličnosti</param>
        /// <returns>Licnost</returns>
        /// <response code="200">Vraća traženu ličnost</response>
        /// <response code="404">Nije pronađena ličnost za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpGet("{licnostId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LicnostDto>> GetLicnost(Guid licnostId)
        {
            var licnost = await _licnostRepository.GetLicnostById(licnostId);

            if (licnost == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetLicnost", $"Licnost sa id-em {licnostId} nije pronađena.");
                return NotFound();
            }


            await _loggerService.Log(LogLevel.Information, "GetLicnost", $"Licnost sa id-em {licnostId} je uspešno vraćena.");

            return Ok(_mapper.Map<LicnostDto>(licnost));
        }

        /// <summary>
        /// Kreira novu ličnost
        /// </summary>
        /// <param name="licnost">Model licnost</param>
        /// /// <returns>Potvrda o kreiranju ličnosti</returns>
        /// <response code="201">Vraća kreiranu ličnost</response>
        /// <response code="500">Desila se greška prilikom unosa nove ličnosti</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LicnostCreateDto>> CreateLicnost([FromBody] LicnostCreateDto licnost)
        {
            try
            {
                Licnost createdLicnost = await _licnostRepository.CreateLicnost(_mapper.Map<Licnost>(licnost));

                string location = _linkGenerator.GetPathByAction("GetLicnost", "Licnost", new { licnostId = createdLicnost.LicnostId });

                await _loggerService.Log(LogLevel.Information, "CreateLicnost", $"Licnost sa vrednostima: {JsonConvert.SerializeObject(licnost)} je uspešno kreirana.");

                return Created(location, _mapper.Map<LicnostCreateDto>(createdLicnost));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateLicnost", $"Greška prilikom unosa licnosti sa vrednostima: {JsonConvert.SerializeObject(licnost)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa komisije");
            }
        }


        ///<summary>
        /// Izmena ličnosti
        /// </summary>
        /// <param name="licnost">Model ličnost</param>
        /// <param name="licnostId"></param>
        /// <returns>Potvrda o izmeni ličnosti</returns>
        /// <response code="200">Izmenjena ličnost</response>
        /// <response code="404">Nije pronađena ličnost za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene ličnosti</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpPut("{licnostId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LicnostUpdateDto>> UpdateLicnost(Guid licnostId, [FromBody] LicnostUpdateDto licnost)
        {
            try
            {
                var licnostEntity = await _licnostRepository.GetLicnostById(licnostId);

                if (licnostEntity == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateLicnost", $"Licnost sa id-em {licnost.LicnostId} nije pronađena.");
                    return NotFound();
                }

                _mapper.Map(licnost, licnostEntity);

                await _licnostRepository.UpdateLicnost(_mapper.Map<Licnost>(licnost));
                //await _loggerService.Log(LogLevel.Information, "UpdateLicnost", $"Licnost sa id-em {licnost.LicnostId} je uspešno izmenjena."); //log
                return Ok(licnost);
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateLicnost", $"Greška prilikom izmene licnosti sa id-em {licnost.LicnostId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene licnosti.");
            }
        }
        /// <summary>
        /// Brisanje ličnosti na osnovu ID-a
        /// </summary>
        /// <param name="licnostId">ID ličnosti</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Ličnost je uspešno obrisana</response>
        /// <response code="404">Nije pronađena ličnost za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja ličnosti</response>
        [Authorize(Roles = "Administrator, Superuser,TehnickiSekretar")]
        [HttpDelete("{licnostId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteLicnost(Guid licnostId)
        {
            try
            {
                var licnost = await _licnostRepository.GetLicnostById(licnostId);

                if (licnost == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteLicnost", $"Licnost sa id-em {licnostId} nije pronađena.");
                    return NotFound();
                }

                await _licnostRepository.DeleteLicnost(licnostId);

                return Ok();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteLicnost", $"Greška prilikom brisanja licnosti sa id-em {licnostId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja licnosti");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa licnostima
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpOptions]
        public IActionResult GetLicnostOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
