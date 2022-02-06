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
    /// Kontroler radnje za zalbu
    /// </summary>
    [ApiController]
    [Route("api/radnjaZaZalbu")]
    [Produces("application/json", "application/xml")]
    public class RadnjaZaZalbuController : ControllerBase 
    {
        private  IRadnjaZaZalbuRepository _radnjaZaZalbuRepository;
        private  LinkGenerator _linkGenerator;
        private  IMapper _mapper;
        private  ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor kontrolera radnje za zalbu - DI
        /// </summary>
        /// <param name="radnjaZaZalbuRepository">Repo radnje za albu</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="loggerService">Logger servis</param>
        public RadnjaZaZalbuController(IRadnjaZaZalbuRepository radnjaZaZalbuRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _radnjaZaZalbuRepository = radnjaZaZalbuRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraca sve radnje za zalbu
        /// </summary>
        /// <returns>Lista radnji za zalbu</returns>
        /// <response code="200">Vraća listu radnji za zalbu</response>
        /// <response code="404">Nije pronađena nijedna radnja za zalbu</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<RadnjaZaZalbuDto>>> GetAllRadnjeZaZalbu(string nazivRadnjeZaZalbu)
        {
            var radnjeZaZalbu = await _radnjaZaZalbuRepository.GetAllRadnjeZaZalbu(nazivRadnjeZaZalbu);

            if (radnjeZaZalbu == null || radnjeZaZalbu.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllRadnjaZaZalbu", "Lista radnji za žalbu je prazna ili null.");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetAllRadnjaZaZalbu", "Lista radnji za žalbu je uspešno vraćena.");

            return Ok(_mapper.Map<IEnumerable<RadnjaZaZalbuDto>>(radnjeZaZalbu));
        }

        /// <summary>
        /// Vraća jednu radnju za zalbu na osnovu ID-a
        /// </summary>
        /// <param name="radnjaZaZalbuId">ID radnje za zalbu</param>
        /// <returns>Radnja za zalbu</returns>
        /// <response code="200">Vraća traženu radnju za zalbu</response>
        /// <response code="404">Nije pronađena radnja za zalbu za uneti ID</response>
        [HttpGet("{radnjaZaZalbuId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<RadnjaZaZalbuDto>> GetRadnjaZaZalbu(Guid radnjaZaZalbuId)
        {
            var radnjaZaZalbu = await _radnjaZaZalbuRepository.GetRadnjaZaZalbuById(radnjaZaZalbuId);

            if (radnjaZaZalbu == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetRadnjaZaZalbu", $"Radnja za žalbu sa id-em {radnjaZaZalbuId} nije pronađena.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetRadnjaZaZalbu", $"Radnja za žalbu sa id-em {radnjaZaZalbuId} je uspešno vraćena.");
            return Ok(_mapper.Map<RadnjaZaZalbuDto>(radnjaZaZalbu));
        }

        /// <summary>
        /// Kreira novu radnju za zalbu
        /// </summary>
        /// <param name="radnjaZaZalbu">Model radnje za zalbu</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove radnje za zalbu \
        /// POST /api/radnjaZaZalbu \
        /// {   
        ///     "NazivRadnjeZaZalbu": "Otvorena"
        ///     
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju radnje za zalbu</returns>
        /// <response code="201">Vraća kreiranu radnju za zalbu</response>
        /// <response code="400">Vraća grešku zvog unosa istih podataka za naziv radnje</response>
        /// <response code="500">Desila se greška prilikom unosa nove radnje za zalbu</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RadnjaZaZalbuCreateDto>> CreateRadnjaZaZalbu([FromBody] RadnjaZaZalbuCreateDto radnjaZaZalbu)
        {
            try
            {
                var proveraValidnosti = await _radnjaZaZalbuRepository.IsValidRadnjaZaZalbu(radnjaZaZalbu.NazivRadnjeZaZalbu);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Warning, "CreateRadnjaZaZalbu", $"Greška prilikom unosa naziva radnje zbog istog imena: {JsonConvert.SerializeObject(radnjaZaZalbu)}.");
                    return BadRequest(response);
                }

                RadnjaZaZalbu createdRadnjaZaZalbu = await _radnjaZaZalbuRepository.CreateRadnjaZaZalbu(_mapper.Map<RadnjaZaZalbu>(radnjaZaZalbu));

                string location = _linkGenerator.GetPathByAction("GetRadnjaZaZalbu", "RadnjaZaZalbu", new { radnjaZaZalbuId = createdRadnjaZaZalbu.RadnjaZaZalbuId });
                await _loggerService.Log(LogLevel.Information, "CreateRadnjaZaZalbu", $"Radnja za žalbu sa vrednostima: {JsonConvert.SerializeObject(radnjaZaZalbu)} je uspešno kreirana.");
                return Created(location, _mapper.Map<RadnjaZaZalbuCreateDto>(createdRadnjaZaZalbu));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateRadnjaZaZalbu", $"Greška prilikom unosa radnje za žalbu sa vrednostima: {JsonConvert.SerializeObject(radnjaZaZalbu)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Radnja za zalbu error");
            }
        }

        /// <summary>
        /// Modifikacija radnje za zalbu
        /// </summary>
        /// <param name="radnjaZaZalbuId">ID radnje za zalbu</param>
        /// <param name="radnjaZaZalbu">Model radnje za zalbu</param>
        /// <returns>Potvrda o modifikaciji radnje za zalbu</returns>
        /// <response code="200">Izmenjena radnja za zalbu</response>
        /// <response code="400">Desila se greška prilikom unosa istih podataka za radnju za zalbu</response>
        /// <response code="404">Nije pronađena radnja za zalbu za uneti ID</response>
        /// <response code="500">Serverska greška tokom modifikacije radnje za zalbu</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{radnjaZaZalbuId}")]
        public async Task<ActionResult<RadnjaZaZalbuUpdateDto>> UpdateRadnjaZaZalbu(Guid radnjaZaZalbuId, [FromBody] RadnjaZaZalbuUpdateDto radnjaZaZalbu)
        {
            try
            {
                var proveraValidnosti = await _radnjaZaZalbuRepository.IsValidRadnjaZaZalbu(radnjaZaZalbu.NazivRadnjeZaZalbu);
                

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };
                    await _loggerService.Log(LogLevel.Warning, "UpdateRadnjaZaZalbu", $"Greška prilikom modifikacije naziva radnje zbog istog imena: {JsonConvert.SerializeObject(radnjaZaZalbu)}.");
                    return BadRequest(response);
                }

                var radnjaZaZalbuEntity = await _radnjaZaZalbuRepository.GetRadnjaZaZalbuById(radnjaZaZalbuId);
                var stareVrednosti = JsonConvert.SerializeObject(radnjaZaZalbuEntity);
                if (radnjaZaZalbuEntity == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateRadnjaZaZalbu", $"Radnja za žalbu sa id-em {radnjaZaZalbu} nije pronađena.");
                    return NotFound();
                }

                _mapper.Map(radnjaZaZalbu, radnjaZaZalbuEntity);

                await _radnjaZaZalbuRepository.UpdateRadnjaZaZalbu(_mapper.Map<RadnjaZaZalbu>(radnjaZaZalbu));
                await _loggerService.Log(LogLevel.Information, "UpdateRadnjaZaZalbu", $"Radnja za žalbu sa id-em {radnjaZaZalbuId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");
                return Ok(radnjaZaZalbu);
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateRadnjaZaZalbu", $"Greška prilikom izmene radnje za žalbu sa id-em {radnjaZaZalbuId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update radnja za zalbu error");
            }
        }

        /// <summary>
        /// Brisanje radnje za zalbu na osnovu ID-a
        /// </summary>
        /// <param name="radnjaZaZalbuId">ID radnje za zalbu</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Radnja za zalbu je uspešno obrisana</response>
        /// <response code="404">Nije pronađena radnja za zalbu za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja radnje za zalbu</response>
        [HttpDelete("{radnjaZaZalbuId}")]
        public async Task<ActionResult> DeleteRadnjaZaZalbu(Guid radnjaZaZalbuId)
        {
            try
            {
                var radnjaZaZalbu = await _radnjaZaZalbuRepository.GetRadnjaZaZalbuById(radnjaZaZalbuId);

                if (radnjaZaZalbu == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteRadnjaZaZalbu", $"Radnja za žalbu sa id-em {radnjaZaZalbuId} nije pronađena.");
                    return NotFound();
                }

                await _radnjaZaZalbuRepository.DeleteRadnjaZaZalbu(radnjaZaZalbuId);
                await _loggerService.Log(LogLevel.Information, "DeleteRadnjaZaZalbu", $"Radnja za žalbu sa id-em {radnjaZaZalbuId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(radnjaZaZalbu)}");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteRadnjaZaZalbu", $"Greška prilikom brisanja radnje za žalbu sa id-em {radnjaZaZalbuId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete radnja za zalbu error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa radnjama zalbi
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetRadnjaZaZalbuOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
