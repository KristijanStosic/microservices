using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.Prioritet;
using KupacService.ServiceCalls;
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

namespace KupacService.Controllers
{
    /// <summary>
    /// Kontroler za priorite
    /// </summary>
    [ApiController]
    [Route("api/prioritet")]
    [Produces("application/json", "application/xml")]
    public class PrioritetController : ControllerBase
    {
        private readonly IPrioritetRepository _prioritetRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;
        /// <summary>
        /// Konstruktor za kontroler
        /// </summary>
        /// <param name="prioritetRepository"></param>
        /// <param name="linkGenerator"></param>
        /// <param name="mapper"></param>
        /// <param name="loggerService"></param>
        public PrioritetController(IPrioritetRepository prioritetRepository,LinkGenerator linkGenerator,IMapper mapper,ILoggerService loggerService)
        {
            this._prioritetRepository = prioritetRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._loggerService = loggerService;
        }

        /// <summary>
        /// Vraća listu priorteta na osnovu unetih filtera
        /// </summary>
        /// <param name="opis">Opis prioriteta</param>
        /// <returns>Lista prioriteta koja odgovara zadatom filteru</returns>
        /// <response code="200">Uspešno vraćena lista prioriteta</response>
        /// <response code="204">Nije pronađeno nijedan prioritet</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<PrioritetDto>>> GetPrioritete(string opis)
        {
            var prioriteti = await _prioritetRepository.GetPrioritet(opis);

            if(prioriteti == null || prioriteti.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetPrioritete", "Lista prioriteta je prazna ili null.");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetPrioritete", "Lista prioriteta je uspešno vraćena.");
            return Ok(_mapper.Map<List<PrioritetDto>>(prioriteti));
        }

        /// <summary>
        /// Vraća prioritet na osnovu unetog id-a
        /// </summary>
        /// <param name="prioritetId">Id prioriteta</param>
        /// <returns>Prioritet</returns>
        /// <response code="200">Uspešno vraćen prioritet</response>
        /// <response code="404">Nije pronađen prioritet sa zadatim id-em</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet("{prioritetId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PrioritetDto>> GetPrioritetById(Guid prioritetId)
        {
            var prioritet = await _prioritetRepository.GetPrioritetById(prioritetId);

            if(prioritet == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetPrioritetById", $"Prioritet sa id-em {prioritetId} nije pronađen.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetPrioritetById", $"Prioritet  sa id-em {prioritetId} je uspešno vraćen.");
            return Ok(_mapper.Map<PrioritetDto>(prioritet));
        }

        /// <summary>
        /// Kreira novi prioritet
        /// </summary>
        /// <param name="prioritet">Prioritet</param>
        /// <returns>Potvrdu o kreiranom prioritetu</returns>
        /// <remarks>
        /// Primer kreiranja prioritet\
        /// POST api/prioritet\
        /// {\
        /// "opis": "opis prioriteta"\
        ///}\
        /// </remarks>
        /// <response code="201">Uspešno kreiran prioritet</response>
        /// <response code="500">Desila se greška prilikom kreiranja novog prioriteta</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrioritetDto>> CreatePrioritet([FromBody] PrioritetCreateDto prioritet)
        {
            try
            {

                var newPrioritet = _mapper.Map<Prioritet>(prioritet);

               await _prioritetRepository.CreatePrioritet(newPrioritet);

                await _prioritetRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetPrioritetById", "Prioritet", new { prioritetId = newPrioritet.PrioritetId });

                await _loggerService.Log(LogLevel.Information, "CreatePrioritet", $"Prioritet sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<PrioritetDto>(newPrioritet))} je uspešno kreiran.");
                return Created(lokacija, prioritet);
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "CreatePrioritet", $"Greška prilikom unosa prioriteta  sa vrednostima: {JsonConvert.SerializeObject(prioritet)}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa prioriteta");
            }
        }

        /// <summary>
        /// Vrši ažuriranje prioriteta
        /// </summary>
        /// <param name="prioritetUpdate">Prioritet</param>
        /// <returns>Prioritet</returns>
        /// <remarks>
        /// Primer kreiranja prioritet\
        /// POST api/prioritet\
        /// {\
        /// "prioritetId":"2578e81b-3f01-479a-b790-f52106f639f7",
        /// "opis": "opis prioriteta"\
        ///}\
        /// </remarks>
        /// <response code="200">Uspešno ažuriran prioritet</response>
        /// <response code="404">Nije pronađen prioritet na osnovu prosleđenog id-a</response>
        /// <response code="500">Desila se greška prilikom ažuriranja prioritet</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrioritetDto>> UpdatePrioritet(PrioritetUpdateDto prioritetUpdate) 
        {
            try
            {
                var oldPrioritet = await _prioritetRepository.GetPrioritetById(prioritetUpdate.PrioritetId);

                if(oldPrioritet == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdatePrioritet", $"Prioritet sa id-em {prioritetUpdate.PrioritetId} nije pronađen.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(_mapper.Map<PrioritetDto>(oldPrioritet));
                Prioritet newPrioritet = _mapper.Map<Prioritet>(prioritetUpdate);

                _mapper.Map(newPrioritet, oldPrioritet);
                await _prioritetRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "UpdatePrioritet", $"Prioritet sa id-em {prioritetUpdate.PrioritetId} je uspešno izmenjen. Stare vrednosti su: {stareVrednosti}");
                return Ok(_mapper.Map<PrioritetDto>(newPrioritet));


            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "UpdatePrioritet", $"Greška prilikom izmene prioriteta sa id-em {prioritetUpdate.PrioritetId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom ažuriranja prioriteta");
            }
        }

        /// <summary>
        /// Vrši brisanje prioriteta 
        /// </summary>
        /// <param name="prioritetId"></param>
        /// <returns></returns>
        /// <response code="200">Uspešno obrisano fizičko lice</response>
        /// <response code="404">Nije pronađeno fizičko lice na osnovu unetog id-a</response>
        /// <response code="500">Desila se greška prilikom brisanja fizičkog lica</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpDelete("{prioritetId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePrioritet(Guid prioritetId) 
        {
            try
            {
                var prioritet = await _prioritetRepository.GetPrioritetById(prioritetId);

                if(prioritet == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeletePrioritet", $"Prioritet sa id-em {prioritetId} nije pronađen.");
                    return NotFound();
                }

               await _prioritetRepository.DeletePrioritet(prioritetId);
               await _prioritetRepository.SaveChangesAsync();
               await _loggerService.Log(LogLevel.Information, "DeletePrioritet", $"Prioritet sa id-em {prioritetId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(_mapper.Map<PrioritetDto>(prioritet))}");

                return Ok();
               

            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "DeletePrioritet", $"Greška prilikom brisanja prioriteta sa id-em {prioritetId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja prioriteta");
            }


        }

        /// <summary>
        /// Vraća opcije za rad sa prioritetima
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetPrioritetOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }


    }
}
