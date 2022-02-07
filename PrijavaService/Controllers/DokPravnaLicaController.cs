using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using PrijavaService.Data.Interfaces;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Models.DokPravnoLice;
using PrijavaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


/// <summary>
/// Kontroler za dokumenta pravnih lica
/// </summary>
namespace PrijavaService.Controllers
{
    [Route("api/dokPravnaLica")]
    [ApiController]
    public class DokPravnaLicaController : ControllerBase
    {
        private readonly IDokPravnaLicaRepository _dokPravnaLicaRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILoggerService _loggerService;

        public DokPravnaLicaController(IDokPravnaLicaRepository dokPravnoLiceRepository, IMapper mapper, LinkGenerator linkGenerator, ILoggerService loggerService)
        {
            _dokPravnaLicaRepository = dokPravnoLiceRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _loggerService = loggerService;
        }


        /// <summary>
        /// Vraća listu svih dokumenata pravnih lica
        /// </summary>
        /// <returns>Lista dokumenata pravnih lica</returns>
        /// <response code="200">Vraća listu dokumenata pravnih lica</response>
        /// <response code="404">Nije pronađena ni jedn dokument pravnih lica</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<DokPravnaLicaDto>>> GetAllDocPravnaLica()
        {
            var docPravnaLica = await _dokPravnaLicaRepository.GetAllDokPravnaLica();

            if(docPravnaLica == null || docPravnaLica.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllDokFizickaLica", "Lista dokumenata fizickih lica je prazna ili null.");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetAllDokFizickaLica", "Lista dokumenata fizickih lica je uspešno vraćena.");
            return Ok(_mapper.Map<List<DokPravnaLicaDto>>(docPravnaLica));
        }

        /// <summary>
        /// Vraća jedan dokument pravnih lica na osnovu ID-a
        /// </summary>
        /// <param name="dokPravnaLicaId">ID dokumenta pravnog lica</param>
        /// <returns>Dokument pravnih lica</returns>
        /// <response code="200">Vraća traženi dokument pravnih lica</response>
        /// <response code="404">Nije pronađen dokument pravnih lica za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer")]
        [HttpGet("{dokPravnaLicaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DokPravnaLicaDto>> GetDokPravnaLica(Guid dokPravnaLicaId)
        {
            var dokPravnoLice = await _dokPravnaLicaRepository.GetDokPravnaLicaById(dokPravnaLicaId);

            if (dokPravnoLice == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetDokFizickaLica", $"Dokument pravnog lica sa id-em {dokPravnaLicaId} nije pronađen.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetDokFizickaLica", $"Dokument pravnog lica sa id-em {dokPravnaLicaId} je uspešno vraćen.");
            return Ok(_mapper.Map<DokPravnaLicaDto>(dokPravnoLice));
        }

        /// <summary>
        /// Kreira novi dokument pravnog lica
        /// </summary>
        /// <param name="dokPravnoLice">Model dokumenta pravnog lica</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove etape \
        /// POST /api/dokPravnaLica \
        /// {
        ///     "nazivDokumenta": "Dokument PL 1", \
        ///     "prijavaId": "A370BC58-2CB2-4D8D-9CFB-B444841AEB80" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju fokumenta pravnog lica</returns>
        /// <response code="200">Vraća kreiran dokument pravnog lica</response>
        /// <response code="500">Desila se greška prilikom unosa novog dokumenta pravnih lica</response>
        [Authorize(Roles = "Administrator, Superuser, Operater")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DokPravnaLicaConfirmationDto>> CreateDokPravnaLica([FromBody] DokPravnaLicaCreateDto dokPravnoLice)
        {
            try
            {
                DokPravnaLica mapiraniDok = _mapper.Map<DokPravnaLica>(dokPravnoLice);

                DokPravnaLicaConfirmation noviDokPravnoLice = await _dokPravnaLicaRepository.CreateDokPravnaLica(mapiraniDok);
                await _dokPravnaLicaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetDokPravnaLica", "DokPravnaLica", new { dokPravnaLicaId = noviDokPravnoLice.DokPravnaLicaId });



                await _loggerService.Log(LogLevel.Information, "CreateDokFizickaLica", $"Dokument pravnog lica sa vrednostima: {JsonConvert.SerializeObject(dokPravnoLice)} je uspešno kreiran.");
                return Created(lokacija, _mapper.Map<DokPravnaLicaConfirmationDto>(noviDokPravnoLice));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateDokFizickaLica", $"Greška prilikom unosa  dokument fizicko lica sa vrednostima: {JsonConvert.SerializeObject(dokPravnoLice)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa dokumanta pravnog lica");
            }
        }

        /// <summary>
        /// Izmena dokumenta pravnih lica
        /// </summary>
        /// <param name="dokPravnoLice">Model dokumenta pravnih lica</param>
        /// <returns>Potvrda o izmeni dokumenta pravnih lica</returns>
        /// <response code="200">Izmenjen dokument pravnih lica</response>
        /// <response code="404">Nije pronađen dokument pravnih lica za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene dokumenta pravnih lica</response>
        [Authorize(Roles = "Administrator, Superuser")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DokPravnaLicaDto>> UpdateDokPravnoLice(DokPravnaLicaUpdateDto dokPravnoLice)
        {
            try
            {
                var stariDokPravnoLice = await _dokPravnaLicaRepository.GetDokPravnaLicaById(dokPravnoLice.DokPravnaLicaId);
                var stareVrijednosti = JsonConvert.SerializeObject(stariDokPravnoLice);

                if(stariDokPravnoLice == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateDokFizickoLice", $"Dokument pravnog lica sa id-em {dokPravnoLice.DokPravnaLicaId} nije pronađen.");
                    return NotFound();
                }

                DokPravnaLica noviDokPravnoLice = _mapper.Map<DokPravnaLica>(dokPravnoLice);

                _mapper.Map(noviDokPravnoLice, stariDokPravnoLice);
                await _dokPravnaLicaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Warning, "UpdateDokFizickoLice", $"Dokument pravnog lica sa id-em {dokPravnoLice.DokPravnaLicaId} je uspešno izmenjen. Stare vrednosti su: {stareVrijednosti}");

                return Ok(_mapper.Map<DokPravnaLicaDto>(stariDokPravnoLice));
            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateDokFizickoLice", $"Greška prilikom izmene dokumenta pravnog lica sa id-em {dokPravnoLice.DokPravnaLicaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene dokumenta pravnog lica");
            }
        }

        /// <summary>
        /// Brisanje dokumenta pravnog lica na osnovu ID-a
        /// </summary>
        /// <param name="dokPravnaLicaId">ID dokumenta pravnog lica</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Dokument pravnog lica je uspešno obrisan</response>
        /// <response code="404">Nije pronađen dokument pravnog lica za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja dokumenta pravnog lica</response>
        [Authorize(Roles = "Administrator, Superuser")]
        [HttpDelete("{dokPravnaLicaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDokPravnaLica(Guid dokPravnaLicaId)
        {
            try
            {
                var dokPravnoLice = await _dokPravnaLicaRepository.GetDokPravnaLicaById(dokPravnaLicaId);

                if(dokPravnoLice == null)
                {
                    await _loggerService.Log(LogLevel.Information, "DeletDokFizickoLice", $"Dokument pravnog lica sa id-em {dokPravnaLicaId} nije pronadjen.");
                    return NotFound();
                }

                await _dokPravnaLicaRepository.DeleteDokPravnaLica(dokPravnaLicaId);
                await _dokPravnaLicaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeletDokFizickoLice", $"Dokument pravnog lica sa id-em {dokPravnaLicaId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(dokPravnoLice)}");
                return NoContent();
            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeletDokFizickoLice", $"Greška prilikom brisanja dokumenta pravnog lica sa id-em {dokPravnaLicaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja dokumenta pravnog lica");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa dokumentima pravnog lica
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser")]
        [HttpOptions]
        public IActionResult GetDokPravnaLicaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
