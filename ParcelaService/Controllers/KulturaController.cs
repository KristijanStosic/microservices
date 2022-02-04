using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Kultura;
using ParcelaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za kulturu
    /// </summary>
    [Route("api/kultura")]
    [ApiController]
    [Produces("application/json", "application/xml")]

    public class KulturaController : ControllerBase
    {
        private readonly IKulturaRepository _kulturaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public KulturaController(IKulturaRepository kulturaRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _kulturaRepository = kulturaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }
        /// <summary>
        /// Vraća sve kulture parcele
        /// </summary>
        /// <param name="nazivKulture">Naziv kulture</param>
        /// <returns>Lista kultura</returns>
        /// <response code="200">Vraća listu kultura</response>
        /// <response code="404">Nije pronađena ni jedna kultura</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<KulturaDto>>> GetAllKlasa(string nazivKulture)
        {
            var kulture = await _kulturaRepository.GetAllKultura(nazivKulture);

            if(kulture == null || kulture.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllKultura", "Lista kultura parcele je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllKultura", "Lista kultura parcele je uspešno vraćena.");

            return Ok(_mapper.Map<List<KulturaDto>>(kulture));
        }
        /// <summary>
        /// Vraća jednu kulturu parcele na osnovu ID-a
        /// </summary>
        /// <param name="kulturaId">ID kulture</param>
        /// <returns>Kultura parcele</returns>
        /// <response code="200">Vraća traženu kulturu</response>
        /// <response code="404">Nije pronađena kultura za uneti ID</response>
        [HttpGet("kulturaId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<KulturaDto>> GetKultura(Guid kulturaId)
        {
            var kultura = await _kulturaRepository.GetKulturaById(kulturaId);

            if(kultura == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKultura", $"Kultura parcele sa id-em {kulturaId} nije pronađena.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetKultura", $"Kultura parcele sa id-em {kulturaId} je uspešno vraćena.");

            return Ok(_mapper.Map<KulturaDto>(kultura));
        }

        /// <summary>
        /// Kreira novu kulturu parcele
        /// </summary>
        /// <param name="kultura">Model kulture</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove kulture \
        /// POST /api/kultura \
        /// {
        ///       "kulturaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "nazivKulture": "Naziv kulture" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju kulture</returns>
        /// <response code="201">Vraća kreiranu kulturu</response>
        /// <response code="500">Desila se greška prilikom unosa nove kulture</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KulturaDto>> CreateKultura([FromBody] KulturaCreationDto kultura)
        {
            try
            {
                Kultura novaKultura = await _kulturaRepository.CreateKultura(_mapper.Map<Kultura>(kultura));
                await _kulturaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetKultura", "Kultura", new { kulturaId = novaKultura.KulturaId });

                await _loggerService.Log(LogLevel.Information, "CreateKultura", $"Kultura parcele sa vrednostima: {JsonConvert.SerializeObject(kultura)} je uspešno kreirana.");

                return Created(lokacija, _mapper.Map<KulturaDto>(novaKultura));
            }

            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateKultura", $"Greška prilikom unosa kulture parcele sa vrednostima: {JsonConvert.SerializeObject(kultura)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja kulture!");
            }
            
        }

        /// <summary>
        /// Izmena kulture parcele
        /// </summary>
        /// <param name="kultura">Model kulture</param>
        /// <returns>Potvrda o izmeni kulture</returns>
        /// <response code="200">Izmenjena kultura</response>
        /// <response code="404">Nije pronađena kultura za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene kulture</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<KulturaDto>> UpdateKultura(KulturaUpdateDto kultura)
        {
            try
            {
                var staraKultura = await _kulturaRepository.GetKulturaById(kultura.KulturaId);

                if(staraKultura == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateKultura", $"Kultura parcele sa id-em {kultura.KulturaId} nije pronađena.");
                    return NotFound();
                }

                var stareVrednosti = JsonConvert.SerializeObject(staraKultura);

                Kultura novaKultura = _mapper.Map<Kultura>(kultura);

                _mapper.Map(staraKultura, novaKultura);
                await _kulturaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateKultura", $"Kultura parcele sa id-em {kultura.KulturaId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<KulturaDto>(staraKultura));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateKultura", $"Greška prilikom izmene kulture parcele sa id-em {kultura.KulturaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene kulture!");
            }
        }

        /// <summary>
        /// Brisanje kulture parcele na osnovu ID-a
        /// </summary>
        /// <param name="kulturaId">ID kulture</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Kultura je uspešno obrisana</response>
        /// <response code="404">Nije pronađena kultura za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja kulture</response>
        [HttpDelete("{kulturaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteKultura (Guid kulturaId)
        {
            try
            {
                var kultura = await _kulturaRepository.GetKulturaById(kulturaId);

                if (kultura == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteKultura", $"Kultura parcele sa id-em {kulturaId} nije pronađena.");
                    return NotFound();
                }

                await _kulturaRepository.DeleteKultura(kulturaId);
                await _kulturaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteKultura", $"Kultura parcele sa id-em {kulturaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(kultura)}");

                return NoContent();
            }

            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteKultura", $"Greška prilikom brisanja kulture parcele sa id-em {kulturaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja kulture!");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa kulturama parcele
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetKulturaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
        
    }
}
