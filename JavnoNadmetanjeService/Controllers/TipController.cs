using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.Tip;
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
    /// Kontroler za tip
    /// </summary>
    [Route("api/tip")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class TipController : ControllerBase
    {
        private readonly ITipRepository _tipRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;
        
        public TipController(ITipRepository tipRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _tipRepository = tipRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve tipove javnog nadmetanja
        /// </summary>
        /// <param name="nazivTipa">Naziv tipa javnog nadmetanja</param>
        /// <returns>Lista tipova</returns>
        /// <response code="200">Vraća listu tipova</response>
        /// <response code="404">Nije pronađen ni jedan tip</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<TipDto>>> GetAllTip(string nazivTipa)
        {
            var tipovi = await _tipRepository.GetAllTip(nazivTipa);

            if (tipovi == null || tipovi.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllTip", "Lista tipova javnog nadmetanja je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllTip", "Lista tipova javnog nadmetanja je uspešno vraćena.");
            
            return Ok(_mapper.Map<List<TipDto>>(tipovi));
        }

        /// <summary>
        /// Vraća jedan tip javnog nadmetanja na osnovu ID-a
        /// </summary>
        /// <param name="tipId">ID tipa</param>
        /// <returns>Tip javnog nadmetanja</returns>
        /// <response code="200">Vraća traženi tip</response>
        /// <response code="404">Nije pronađen tip za uneti ID</response>
        [HttpGet("{tipId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipDto>> GetTip(Guid tipId)
        {
            var tip = await _tipRepository.GetTipById(tipId);

            if (tip == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetTip", $"Tip javnog nadmetanja sa id-em {tipId} nije pronađen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetTip", $"Tip javnog nadmetanja sa id-em {tipId} je uspešno vraćen.");

            return Ok(_mapper.Map<TipDto>(tip));
        }

        /// <summary>
        /// Kreira novi tip javnog nadmetanja
        /// </summary>
        /// <param name="tip">Model tip</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog tipa \
        /// POST /api/tip \
        /// {
        ///     "NazivTipa": "Tip javnog nadmetanja" \
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju tipa</returns>
        /// <response code="201">Vraća kreiran tip</response>
        /// <response code="500">Desila se greška prilikom unosa novog tipa</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TipDto>> CreateTip([FromBody] TipCreationDto tip)
        {
            try
            {
                Tip noviTip = await _tipRepository.CreateTip(_mapper.Map<Tip>(tip));
                await _tipRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetTip", "Tip", new { tipId = noviTip.TipId });
                
                await _loggerService.Log(LogLevel.Information, "CreateTip", $"Tip javnog nadmetanja sa vrednostima: {JsonConvert.SerializeObject(tip)} je uspešno kreiran.");

                return Created(lokacija, _mapper.Map<TipDto>(noviTip));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateTip", $"Greška prilikom unosa tipa javnog nadmetanja sa vrednostima: {JsonConvert.SerializeObject(tip)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom kreiranja tipa");
            }
        }

        /// <summary>
        /// Izmena tipa javnog nadmetanja
        /// </summary>
        /// <param name="tip">Model tip</param>
        /// <returns>Potvrda o izmeni tipa</returns>
        /// <response code="200">Izmenjen tip</response>
        /// <response code="404">Nije pronađen tip za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene tipa</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TipDto>> UpdateTip(TipUpdateDto tip)
        {
            try
            {
                var stariTip = await _tipRepository.GetTipById(tip.TipId);

                if (stariTip == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateTip", $"Tip javnog nadmetanja sa id-em {tip.TipId} nije pronađen.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(stariTip);

                Tip noviTip = _mapper.Map<Tip>(tip);

                _mapper.Map(noviTip, stariTip);
                await _tipRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateTip", $"Tip javnog nadmetanja sa id-em {tip.TipId} je uspešno izmenjen. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<TipDto>(noviTip));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateTip", $"Greška prilikom izmene tipa javnog nadmetanja sa id-em {tip.TipId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene tipa");
            }
        }

        /// <summary>
        /// Brisanje tipa javnog nadmetanja na osnovu ID-a
        /// </summary>
        /// <param name="tipId">ID tipa</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Tip je uspešno obrisan</response>
        /// <response code="404">Nije pronađen tip za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja tipa</response>
        [HttpDelete("{tipId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTip(Guid tipId)
        {
            try
            {
                var tip = await _tipRepository.GetTipById(tipId);

                if (tip == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteTip", $"Tip javnog nadmetanja sa id-em {tipId} nije pronađen.");
                    return NotFound();
                }

                await _tipRepository.DeleteTip(tipId);
                await _tipRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteTip", $"Tip javnog nadmetanja sa id-em {tipId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(tip)}");

                return NoContent();

            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteTip", $"Greška prilikom brisanja tipa javnog nadmetanja sa id-em {tipId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja tipa");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa tipovima javnog nadmetanja
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetTipOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
