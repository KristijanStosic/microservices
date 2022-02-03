using AutoMapper;
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
using ZalbaService.ServicesCalls;

namespace ZalbaService.Controllers
{
    /// <summary>
    /// Kontroler za tip zalbe
    /// </summary>
    [ApiController]
    [Route("api/tipZalbe")]
    [Produces("application/json", "application/xml")]
    public class TipZalbeController : ControllerBase
    {
        private readonly ITipZalbeRepository _tipZalbeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor kontrolera tipa zalbe - DI
        /// </summary>
        /// <param name="tipZalbeRepository">Repo tip zalbe</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="loggerService">Logger servis</param>
        public TipZalbeController(ITipZalbeRepository tipZalbeRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _tipZalbeRepository = tipZalbeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve tipove zalbi
        /// </summary>
        /// <returns>Lista tipova zalbi</returns>
        /// <response code="200">Vraća listu tipova zalbi</response>
        /// <response code="204">Nije pronađen ni jedan tip zalbe</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<TipZalbeDto>>> GetAllTipoviZalbe(string nazivTipaZalbe)
        {
            var tipoviZalbe = await _tipZalbeRepository.GetAllTipoviZalbe(nazivTipaZalbe);

            if (tipoviZalbe == null || tipoviZalbe.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllTipZalbe", "Lista tipova zalbi je prazna ili null");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetAllTipZalbe", "Lista tipova zalbi je uspešno vraćena.");
            return Ok(_mapper.Map<IEnumerable<TipZalbeDto>>(tipoviZalbe));
        }

        /// <summary>
        /// Vraća jedan tip zalbe na osnovu ID-a
        /// </summary>
        /// <param name="tipZalbeId">ID tipa zalbe</param>
        /// <returns>Tip zalbe</returns>
        /// <response code="200">Vraća traženi tip zalbe</response>
        /// <response code="404">Nije pronađen tip zalbe za uneti ID</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{tipZalbeId}")]
        public async Task<ActionResult<TipZalbeDto>> GetTipZalbe(Guid tipZalbeId)
        {
            var tipZalbe = await _tipZalbeRepository.GetTipZalbeById(tipZalbeId);

            if (tipZalbe == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetTipZalbe", $"Tip zalbe sa id-em {tipZalbeId} nije pronađen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetTipZalbe", $"Tip zalbe sa id-em {tipZalbeId} je uspešno vraćen.");

            return Ok(_mapper.Map<TipZalbeDto>(tipZalbe));
        }

        /// <summary>
        /// Kreira novi tip zalbe
        /// </summary>
        /// <param name="tipZalbe">Model tip zalbe</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog tipa zalbe \
        /// POST /api/tipZalbe \
        /// {   
        ///     "NazivTipaZalbe": "Zalba na Odluku o davanju u zakup"
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju tipa zalbe</returns>
        /// <response code="201">Vraća kreiran tip zalbe</response>
        /// <response code="400">Desila se greška prilikom unosa istih podataka za tip žalbe</response>
        /// <response code="500">Desila se greška prilikom unosa novog tipa zalbe</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<TipZalbeCreateDto>> CreateTipZalbe([FromBody] TipZalbeCreateDto tipZalbe)
        {
            try
            {
                var proveraValidnosti = await _tipZalbeRepository.IsValidTipZalbe(tipZalbe.NazivTipaZalbe);

                if(!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Information, "CreateTipZalbe", $"Greška prilikom unosa tipa žalbe sa vrednostima: {JsonConvert.SerializeObject(tipZalbe)}.");

                    return BadRequest(response);
                }

                TipZalbe createdTipZalbe = await _tipZalbeRepository.CreateTipZalbe(_mapper.Map<TipZalbe>(tipZalbe));

                string location = _linkGenerator.GetPathByAction("GetTipZalbe", "TipZalbe", new { tipZalbeId = createdTipZalbe.TipZalbeId });

                await _loggerService.Log(LogLevel.Information, "CreateTipZalbe", $"Tip žalbe sa vrednostima: {JsonConvert.SerializeObject(tipZalbe)} je uspešno kreiran.");

                return Created(location, _mapper.Map<TipZalbeCreateDto>(createdTipZalbe));
                 
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateTipZalbe", $"Greška prilikom unosa tipa žalbe sa vrednostima: {JsonConvert.SerializeObject(tipZalbe)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Tip Zalbe error");
            }
        }

        /// <summary>
        /// Modifikacija tipa zalbe
        /// </summary>
        /// <param name="tipZalbeId">ID tipa žalbe</param>
        /// <param name="tipZalbe">Model tip zalbe</param>
        /// <returns>Potvrda o modifikaciji tipa zalbe</returns>
        /// <response code="200">Izmenjen tip zalbe</response>
        /// <response code="400">Desila se greška prilikom unosa istih podataka za tip žalbe</response>
        /// <response code="404">Nije pronađen tip zalbe za uneti ID</response>
        /// <response code="500">Serverska greška tokom modifikacije tipa zalbe</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{tipZalbeId}")]
        public async Task<ActionResult<TipZalbeUpdateDto>> UpdateTipZalbe(Guid tipZalbeId, [FromBody] TipZalbeUpdateDto tipZalbe)
        {
            try
            {
                var proveraValidnosti = await _tipZalbeRepository.IsValidTipZalbe(tipZalbe.NazivTipaZalbe);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Warning, "UpdateTipZalbe", $"Greška prilikom unosa tipa žalbe sa vrednostima: {JsonConvert.SerializeObject(tipZalbe)}.");
                    return BadRequest(response);
                }

                var tipZalbeEntity = await _tipZalbeRepository.GetTipZalbeById(tipZalbeId);

                if (tipZalbeEntity == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateTipZalbe", $"Tip zalbe sa id-em {tipZalbeId} nije pronađen.");
                    return NotFound();
                }

                _mapper.Map(tipZalbe, tipZalbeEntity);

                await _tipZalbeRepository.UpdateTipZalbe(_mapper.Map<TipZalbe>(tipZalbe));

                await _loggerService.Log(LogLevel.Information, "UpdateTipZalbe", $"Tip zalbe sa id-em {tipZalbeId} je uspešno modifikovan.");

                return Ok(tipZalbe);
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateTipZalbe", $"Greška prilikom unosa tipa žalbe sa vrednostima: {JsonConvert.SerializeObject(tipZalbe)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update tip zalbe error");
            }
        }

        /// <summary>
        /// Brisanje tipa žalbe na osnovu ID-a
        /// </summary>
        /// <param name="tipZalbeId">ID tipa žalbe</param>
        /// <returns>Status 204 (No Content)</returns>
        /// <response code="204">Tip žalbe je uspešno obrisan</response>
        /// <response code="404">Nije pronađen tip žalbe za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja tipa žalbe</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{tipZalbeId}")]
        public async Task<ActionResult> DeleteTipZalbe(Guid tipZalbeId)
        {
            try
            {
                var tipZalbe = await _tipZalbeRepository.GetTipZalbeById(tipZalbeId);

                if (tipZalbe == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteTipZalbe", $"Tip žalbe sa id-em {tipZalbeId} nije pronađen.");
                    return NotFound();
                }

                await _tipZalbeRepository.DeleteTipZalbe(tipZalbeId);

                await _loggerService.Log(LogLevel.Information, "DeleteTipZalbe", $"Tip žalbe sa id-em {tipZalbeId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(tipZalbe)}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteTipZalbe", $"Greška prilikom brisanja tipa žalbe sa id-em {tipZalbeId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete tip zalbe error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa tipovima zalbi
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetTipZalbeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
