using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrijavaService.Data.Interfaces;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Helpers;
using PrijavaService.Models.Prijava;
using PrijavaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijavaService.Controllers
{
    /// <summary>
    /// Kontroler za prijavu
    /// </summary>
    [Route("api/prijava")]
    [ApiController]
    public class PrijavaController : ControllerBase
    {

        private readonly IPrijavaRepository _prijavaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IPrijavaCalls _prijavaCalls;
        private readonly ILoggerService _loggerService;

        public PrijavaController(IPrijavaRepository prijavaRepository, LinkGenerator linkGenerator, IMapper mapper, IPrijavaCalls prijavaCalls, ILoggerService loggerService)
        {
            _prijavaRepository = prijavaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _prijavaCalls = prijavaCalls;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve prijave
        /// </summary>
        /// <returns>Lista prijava</returns>
        /// <response code="200">Vraća listu prijava</response>
        /// <response code="404">Nije pronađena ni jedna prijava</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<PrijavaDto>>> GetAllPrijava()
        {
            var prijave = await _prijavaRepository.GetAllPrijava();

            if(prijave == null || prijave.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllPrijava", "Lista prijava je prazna ili null.");
                return NoContent();
            }

            var prijaveDto = new List<PrijavaDto>();
            foreach (var prijava in prijave)
            {
                prijaveDto.Add(await _prijavaCalls.GetPrijvaDtoWithOtherServicesInfo(prijava));
            }

            await _loggerService.Log(LogLevel.Information, "GetAllPrijava", "Lista prijava je uspešno vraćena.");
            return Ok(prijaveDto);

        }

        /// <summary>
        /// Vraća jednu prijavu na osnovu ID-a
        /// </summary>
        /// <param name="prijavaId">ID prijave</param>
        /// <returns>Prijava</returns>
        /// <response code="200">Vraća traženu prijavu</response>
        /// <response code="404">Nije pronađena prijava za uneti ID</response>
        [HttpGet("{prijavaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PrijavaDto>> GetPrijava(Guid prijavaId)
        {
            var prijava = await _prijavaRepository.GetPrijavaById(prijavaId);

            if(prijava == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetPrijava", $"prijava sa id-em {prijavaId} nije pronađena.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetPrijava", $"prijava sa id-em {prijavaId} je uspešno vraćena.");
            return Ok(await _prijavaCalls.GetPrijvaDtoWithOtherServicesInfo(prijava));
        }

        /// <summary>
        /// Kreira novu prijavu
        /// </summary>
        /// <param name="prijava">Model prijava</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove prijvau \
        /// POST /api/prijava \
        ///   {
        ///        "brojPrijave": "B255", \
        ///        "datumPrijave": "2022-02-06T19:35:46.806Z", \
        ///        "mestoPrijave": "Trebinje", \
        ///        "satPrijema": "22", \
        ///        "zatvorenaPonuda": false, \
        ///        "kupacId": "FEBD1C29-90E7-40C2-97F3-1E88495FE98D", \
        ///        "javnoNadmetanje": [ \
        ///          "5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5", \
        ///          "E22F999D-5C61-4DCE-965B-9C6667EFC74D" \
        ///         ] \
        ///    }
        /// </remarks>
        /// <returns>Potvrda o kreiranju prijave</returns>
        /// <response code="200">Vraća kreiranu prijavu</response>
        /// <response code="500">Desila se greška prilikom unosa nove prijave</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrijavaConfirmationDto>> CreatePrijava([FromBody] PrijavaCreationDto prijava)
        {
            try
            {
                PrijavaConfirmation novaPrijava = await _prijavaRepository.CreatePrijava(_mapper.Map<Prijava>(prijava));
                await _prijavaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetPrijava", "Prijava", new { prijavaId = novaPrijava.PrijavaId });

                await _loggerService.Log(LogLevel.Information, "CreatePrijava", $"Prijava sa vrednostima: {JsonConvert.SerializeObject(prijava)} je uspešno kreirana.");
                return Created(lokacija, _mapper.Map<PrijavaConfirmationDto>(novaPrijava));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreatePrijava", $"Greška prilikom unosa prijave sa vrednostima: {JsonConvert.SerializeObject(prijava)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa prijave");
            }
        }

        /// <summary>
        /// Izmena prijave
        /// </summary>
        /// <param name="prijava">Model prijava</param>
        /// <returns>Potvrda o izmeni prijave</returns>
        /// <response code="200">Izmenjena prijava</response>
        /// <response code="404">Nije pronađena prijava za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene prijave</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrijavaDto>> UpdatePrijava(PrijavaUpdateDto prijava)
        {
            try
            {
                var staraPrijava = await _prijavaRepository.GetPrijavaById(prijava.PrijavaId);

                if(staraPrijava == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdatePrijava", $"Prijava sa id-em {prijava.PrijavaId} nije pronađena.");
                    return NotFound();
                }

                var stareVrijednosti = JsonConvert.SerializeObject(prijava);

                Prijava novaPrijava = _mapper.Map<Prijava>(prijava);

                _mapper.Map(novaPrijava, staraPrijava);
                await _prijavaRepository.UpdatePrijava(novaPrijava);
                await _prijavaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdatePrijava", $"Prijava sa id-em {prijava.PrijavaId} je uspešno izmenjena. Stare vrednosti su: {stareVrijednosti}");

                return Ok(_mapper.Map<PrijavaDto>(staraPrijava));

            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdatePrijava", $"Greška prilikom izmene prijave sa id-em {prijava.PrijavaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene prijave.");
            }
        }
        /// <summary>
        /// Brisanje prijave na osnovu ID-a
        /// </summary>
        /// <param name="prijavaId">ID prijave</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Prijava je uspešno obrisana</response>
        /// <response code="404">Nije pronađena prijava za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja prijave</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{prijavaId}")]
        public async Task<IActionResult> DeletePrijava(Guid prijavaId)
        {
            try
            {
                var prijava = await _prijavaRepository.GetPrijavaById(prijavaId);

                if(prijava == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeletePrijava", $"Prijava sa id-em {prijavaId} nije pronađena.");
                    return NotFound();
                }

                await _prijavaRepository.DeletePrijava(prijavaId);
                await _prijavaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeletePrijava", $"Prijava sa id-em {prijavaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(prijava)}");
                return NoContent();

            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeletePrijava", $"Greška prilikom brisanja prijave sa id-em {prijavaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja prijave");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa prijavama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetPrijavaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
