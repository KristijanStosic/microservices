using AutoMapper;
using KorisnikSistemaService.Data.Interfaces;
using KorisnikSistemaService.Entities;
using KorisnikSistemaService.Models.TipKorisnika;
using KorisnikSistemaService.ServiceCalls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KorisnikSistemaService.Controllers
{
    /// <summary>
    /// Kontroler za tip korisnika
    /// </summary>
    [Route("api/TipKorisnika")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class TipKorisnikaController : ControllerBase
    {
        private readonly ITipKorisnikaRepository _tipKorisnikaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public TipKorisnikaController(ITipKorisnikaRepository tipKorisnikaRepository, LinkGenerator linkGenerator,IMapper mapper,ILoggerService loggerService)
        {
            this._tipKorisnikaRepository = tipKorisnikaRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve tipove korisnika sistema
        /// </summary>
        /// <param name="nazivTipaKorisnika">Naziv tipa korisnika sistema</param>
        /// <returns>Lista tipova korisnika</returns>
        /// <response code="200">Vraća listu tipova korisnika sistema</response>
        /// <response code="404">Nije pronađen ni jedan tip korisnika sistema</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<TipKorisnikaDto>>> GetAllTipKorisnika(string nazivTipaKorisnika)
        {
            var tipoviKorisnika = await _tipKorisnikaRepository.GetAllTipKorisnika(nazivTipaKorisnika);

            if(tipoviKorisnika == null || tipoviKorisnika.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllTipKorisnika", "Lista tipova korisnika sistema je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllTipKorisnika", "Lista tipova korinika sistema je uspešno vraćena.");

            return Ok(_mapper.Map<List<TipKorisnikaDto>>(tipoviKorisnika));
        }

        /// <summary>
        /// Vraća jedan tip korisnika sistema na osnovu ID-a
        /// </summary>
        /// <param name="tipKorisnikaId">ID tipa korisnika sistema</param>
        /// <returns>Tip korisnika sistema</returns>
        /// <response code="200">Vraća traženi tip korisnika sistema</response>
        /// <response code="404">Nije pronađen tip korisnika sistema za uneti ID</response>
        [HttpGet("{tipKorisnikaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipKorisnikaDto>> GetTipKorisnika(Guid tipKorisnikaId)
        {
            var tipKorisnika = await _tipKorisnikaRepository.GetTipKorisnikaById(tipKorisnikaId);

            if(tipKorisnika == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetTipKorisnika", $"Tip korisnika sistema sa id-em {tipKorisnikaId} nije pronađen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetTipKorisnika", $"Tip korisnika sistema sa id-em {tipKorisnikaId} je uspešno vraćen.");

            return Ok(_mapper.Map<TipKorisnikaDto>(tipKorisnika));
        }

        /// <summary>
        /// Kreira novi tip korisnika sistema
        /// </summary>
        /// <param name="tipKorisnika">Model tipKorisnika</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog tipa korisnika sistema \
        /// POST /api/tipKorisnika \
        /// {
        ///     "NazivTipaKorisnika": "Tip Korisnika sistema" \
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju tipa korisnika sistema</returns>
        /// <response code="200">Vraća kreiran tip korisnika sistema</response>
        /// <response code="500">Desila se greška prilikom unosa novog tipa korisnika sistema</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TipKorisnikaDto>> CreateTipKorisnika([FromBody] TipKorisnikaCreationDto tipKorisnika)
        {
            try
            {
                TipKorisnika noviTipKorisnika = await _tipKorisnikaRepository.CreateTipKorisnika(_mapper.Map<TipKorisnika>(tipKorisnika));
                await _tipKorisnikaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetAllTipKorisnika", "TipKorisnika", new { tipId = noviTipKorisnika.TipKorisnikaId });

                await _loggerService.Log(LogLevel.Information, "CreateTipKorisnika", $"Tip korisnika sistema sa vrednostima: {JsonConvert.SerializeObject(tipKorisnika)} je uspešno kreiran.");

                return Created(lokacija, _mapper.Map<TipKorisnikaDto>(noviTipKorisnika));
            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateTipKorisnika", $"Greška prilikom unosa tipa korisnika sistema sa vrednostima: {JsonConvert.SerializeObject(tipKorisnika)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom kreiranja tipa korisnika sistema");
            }
        }

        /// <summary>
        /// Izmena tipa korisnika sistema
        /// </summary>
        /// <param name="tipKorisnika">Model tipKorisnika</param>
        /// <returns>Potvrda o izmeni tipa korisnika sistema</returns>
        /// <response code="200">Izmenjen tip</response>
        /// <response code="404">Nije pronađen tip za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene tipa</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TipKorisnikaDto>> UpdateTipKorisnika(TipKorisnikaUpdateDto tipKorisnika)
        {
            try
            {
                var stariTipKorisnika = await _tipKorisnikaRepository.GetTipKorisnikaById(tipKorisnika.TipKorisnikaId);

                if(stariTipKorisnika == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateTipKorisnika", $"Tip korisnika sistema sa id-em {tipKorisnika.TipKorisnikaId} nije pronađen.");
                    return NotFound();
                }
                var stareVrijednosti = JsonConvert.SerializeObject(stariTipKorisnika);

                TipKorisnika noviTipKorisnika = _mapper.Map<TipKorisnika>(tipKorisnika);

                _mapper.Map(noviTipKorisnika, stariTipKorisnika);
                await _tipKorisnikaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateTipKorisnika", $"Tip korisnika sistema sa id-em {tipKorisnika.TipKorisnikaId} je uspešno izmenjen. Stare vrednosti su: {stareVrijednosti}");

                return Ok(_mapper.Map<TipKorisnikaDto>(noviTipKorisnika));
            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateTip", $"Greška prilikom izmene tipa korisnika sistema sa id-em {tipKorisnika.TipKorisnikaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene tipa korisnika sistema");
            }
        }

        /// <summary>
        /// Brisanje tipa korisnika sistema na osnovu ID-a
        /// </summary>
        /// <param name="tipKorisnikaId">ID tipa korisnika sistema</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Tip korisnika sistema je uspešno obrisan</response>
        /// <response code="404">Nije pronađen tip korisnika za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja tipa korisnika sistema</response>
        [HttpDelete("{tipKorisnikaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTipKorisnika(Guid tipKorisnikaId)
        {
            try
            {
                var tipKorisnika = await _tipKorisnikaRepository.GetTipKorisnikaById(tipKorisnikaId);

                if(tipKorisnikaId == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteTipKorisnika", $"Tip korisnika sistema sa id-em {tipKorisnikaId} nije pronađen.");
                    return NotFound();
                }

                await _tipKorisnikaRepository.DeleteTipKorisnika(tipKorisnikaId);
                await _tipKorisnikaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteTipKorisnika", $"Tip korisnika sistema sa id-em {tipKorisnikaId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(tipKorisnika)}");

                return NoContent();
            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteTipKorisnika", $"Greška prilikom brisanja tipa korisnika sistema sa id-em {tipKorisnikaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja tipa korisnika sistema");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa tipovima korinika sistema
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetTipKorisnikaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
