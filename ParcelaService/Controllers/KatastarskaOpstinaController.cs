using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.KatastarskaOpstina;
using ParcelaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za katastarsku opstinu
    /// </summary>
    [Route("api/katastarskaOpstina")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class KatastarskaOpstinaController : ControllerBase
    {
        private readonly IKatastarskaOpstinaRepository _katastarskaOpstinaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;


        public KatastarskaOpstinaController(IKatastarskaOpstinaRepository katastarskaOpstinaRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _katastarskaOpstinaRepository = katastarskaOpstinaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve katastarske opštine
        /// </summary>
        /// <returns>Lista katastarskih opština</returns>
        /// <response code="200">Vraća listu katastarskih opština</response>
        /// <response code="404">Nije pronađena ni jedna katastarska opština</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<KatastarskaOpstinaDto>>> GetAllKatastarskaOpstina(string nazivKatastarskeOpstine)
        {
            var katastarskeOpstine = await _katastarskaOpstinaRepository.GetAllKatastarskaOpstina(nazivKatastarskeOpstine);

            if(katastarskeOpstine == null || katastarskeOpstine.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllKatastarskaOpstina", "Lista katastarskih opština je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllKatastarskaOpstina", "Lista katastarskih opština je uspešno vraćena.");


            return Ok(_mapper.Map<List<KatastarskaOpstinaDto>>(katastarskeOpstine));
        }


        /// <summary>
        /// Vraća jednu katastarsku opštinu na osnovu ID-a
        /// </summary>
        /// <param name="katastarskaOpstinaId">ID katastarske opštine</param>
        /// <returns>Katastarska opština</returns>
        /// <response code="200">Vraća traženu katastarsku opštinu</response>
        /// <response code="404">Nije pronađena katastarska opština za uneti ID</response>
        [HttpGet("katastarskaOpstinaId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<KatastarskaOpstinaDto>> GetKatastarskaOpstina(Guid katastarskaOpstinaId)
        {
            var katastarskaOpstina = await _katastarskaOpstinaRepository.GetKatastarskaOpstinaById(katastarskaOpstinaId);

            if(katastarskaOpstina == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKatastarskaOpstina", $"Katastarska opština sa id-em {katastarskaOpstinaId} nije pronađena.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetKatastarskaOpstina", $"Etapa sa id-em {katastarskaOpstinaId} je uspešno vraćena.");


            return Ok(_mapper.Map<KatastarskaOpstinaDto>(katastarskaOpstina));
        }

        /// <summary>
        /// Kreira novu katastarsku opštinu
        /// </summary>
        /// <param name="katastarskaOpstina">Model katastarske opštine</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove katastarske opštine \
        /// POST /api/katastarskaOpstina \
        /// {
        ///       "katastarskaOpstinaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "nazivKatastarskeOpstine": "Naziv katastarske opštine"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju katastarske opštine</returns>
        /// <response code="201">Vraća kreiranu katastarsku opštinu</response>
        /// <responese code="500">Desila se greška prilikom unosa nove katastarske opštine</responese>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KatastarskaOpstinaDto>> CreateKatastarskaOpstina([FromBody] KatastarskaOpstinaCreationDto katastarskaOpstina)
        {
            try
            {
                KatastarskaOpstina novaKatastarskaOpstina = await _katastarskaOpstinaRepository.CreateKatastarskaOpstina(_mapper.Map<KatastarskaOpstina>(katastarskaOpstina));
                await _katastarskaOpstinaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetKatastarskaOpstina", "KatastarskaOpstina", new { katastarskaOpstinaId = novaKatastarskaOpstina.KatastarskaOpstinaId });

                await _loggerService.Log(LogLevel.Information, "CreateKatastarskaOpstina", $"Katastarska opština sa vrednostima: {JsonConvert.SerializeObject(katastarskaOpstina)} je uspešno kreirana.");


                return Created(lokacija, _mapper.Map<KatastarskaOpstinaDto>(novaKatastarskaOpstina));
            }

            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateKatastarskaOpstina", $"Greška prilikom unosa katastarske opštine sa vrednostima: {JsonConvert.SerializeObject(katastarskaOpstina)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja katastarske opstine");
            }
        }
        /// <summary>
        /// Izmena katastarske oštine
        /// </summary>
        /// <param name="katastarskaOpstina">Model katastarske opštine</param>
        /// <returns>Potvrda o izmeni katastarske opštine</returns>
        /// <response code="200">Izmena katastarske opštine</response>
        /// <response code="404">Nije pronađena katastarska opština za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene katastarske opštine</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KatastarskaOpstinaDto>> UpdateKatastarskaOpstina(KatastarskaOpstinaUpdateDto katastarskaOpstina)
        {
            try
            {
                var staraKatastarskaOpstina = await _katastarskaOpstinaRepository.GetKatastarskaOpstinaById(katastarskaOpstina.KatastarskaOpstinaId);

                if(staraKatastarskaOpstina == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateKatastarskaOpstina", $"Katastarska opština sa id-em {katastarskaOpstina.KatastarskaOpstinaId} nije pronađena.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(staraKatastarskaOpstina);

                KatastarskaOpstina novaKatastarskaOpstina = _mapper.Map<KatastarskaOpstina>(katastarskaOpstina);

                _mapper.Map(staraKatastarskaOpstina, novaKatastarskaOpstina);
                await _katastarskaOpstinaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateKatastarskaOpstina", $"Katastarska opština sa id-em {katastarskaOpstina.KatastarskaOpstinaId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");


                return Ok(_mapper.Map<KatastarskaOpstinaDto>(staraKatastarskaOpstina));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateKatastarskaOpstina", $"Greška prilikom izmene katastarske opštine sa id-em {katastarskaOpstina.KatastarskaOpstinaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene katastarske opstine");
            }
        }
        /// <summary>
        /// Brisanje katastarske opštine
        /// </summary>
        /// <param name="katastarskaOpstinaId"></param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Katastarska opština je uspešno obrisana</response>
        /// <response code="404">Nije pronađena katastarska opština za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja katastarske opštine</response>
        [HttpDelete("{katastarskaOpstinaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteKatastarskaOpstina(Guid katastarskaOpstinaId)
        {
            try
            {
                var katastarskaOpstina = await _katastarskaOpstinaRepository.GetKatastarskaOpstinaById(katastarskaOpstinaId);

                if(katastarskaOpstina == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteKatastarskaOpstina", $"Katastarska opština sa id-em {katastarskaOpstinaId} nije pronađen.");
                    return NotFound();
                }

                await _katastarskaOpstinaRepository.DeleteKatastarskaOpstina(katastarskaOpstinaId);
                await _katastarskaOpstinaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteKatastarskaOpstina", $"Katastarska opštinasa id-em {katastarskaOpstinaId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(katastarskaOpstina)}");


                return NoContent();
            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteKatastarskaOpstina", $"Greška prilikom brisanja katastarske opštine sa id-em {katastarskaOpstinaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja katastarske opstine");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa katastarskim opštinama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetKatastarskaOpstinaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
