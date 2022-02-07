using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PrijavaService.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrijavaService.Models.DokFizickoLice;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PrijavaService.ServiceCalls;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Kontroler za dokumenta fizickih lica
/// </summary>
namespace PrijavaService.Controllers
{
    [Route("api/dokFizickaLica")]
    [ApiController]
    public class DokFizickaLicaController : ControllerBase
    {

        private readonly IDokFizickaLicaRepository _dokFizickaLicaRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILoggerService _loggerService;

        public DokFizickaLicaController(IDokFizickaLicaRepository dokFizickaLicaRepository, IMapper mapper, LinkGenerator linkGenerator, ILoggerService loggerService)
        {
            _dokFizickaLicaRepository = dokFizickaLicaRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća listu svih dokumenata fizickih lica
        /// </summary>
        /// <returns>Lista dokumenata fizickih lica</returns>
        /// <response code="200">Vraća listu dokumenata fizickih lica</response>
        /// <response code="404">Nije pronađena ni jedn dokument fizickih lica</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<DokFizickaLicaDto>>> GetAllDokFizickaLica()
        {
            var dokFizickaLica = await _dokFizickaLicaRepository.GetAllDokFizickaLica();

            if(dokFizickaLica == null || dokFizickaLica.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllDokFizickaLica", "Lista dokumenata fizickih lica je prazna ili null.");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetAllDokFizickaLica", "Lista dokumenata fizickih lica je uspešno vraćena.");
            return Ok(_mapper.Map<List<DokFizickaLicaDto>>(dokFizickaLica));
        }

        /// <summary>
        /// Vraća jedan dokument fizickog lica na osnovu ID-a
        /// </summary>
        /// <param name="dokFizickaLicaId">ID fokuemta fizickog lica</param>
        /// <returns>Dokument fizickog lica</returns>
        /// <response code="200">Vraća traženi dokument fizickog lica</response>
        /// <response code="404">Nije pronađen dokument fizickog lica za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{dokFizickaLicaId}")]
        public async Task<ActionResult<DokFizickaLicaDto>> GetDokFizickaLica(Guid dokFizickaLicaId)
        {
            var dokFizickoLice = await _dokFizickaLicaRepository.GetDokFizickaLicaById(dokFizickaLicaId);

            if (dokFizickoLice == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetDokFizickaLica", $"Dokument fizickog lica sa id-em {dokFizickaLicaId} nije pronađen.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetDokFizickaLica", $"Dokument fizickog lica sa id-em {dokFizickaLicaId} je uspešno vraćen.");
            return Ok(_mapper.Map<DokFizickaLicaDto>(dokFizickoLice));
        }

        /// <summary>
        /// Kreira novi dokument fizickog lica
        /// </summary>
        /// <param name="dokFizickoLice">Model dokumenta fizickog lica</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove etape \
        /// POST /api/dokFizickaLica \
        /// {
        ///     "nazivDokumenta": "Dokument FL 1", \
        ///     "prijavaId": "A370BC58-2CB2-4D8D-9CFB-B444841AEB80" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju fokumenta fizickog lica</returns>
        /// <response code="201">Vraća kreiran dokument fizickog lica</response>
        /// <response code="500">Desila se greška prilikom unosa novog dokumenta fizickog lica</response>
        [Authorize(Roles = "Administrator, Superuser, Operater")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DokFizickaLicaConfirmationDto>> CreateDokFizickaLica([FromBody] DokFizickaLicaCreateDto dokFizickoLice)
        {
            try
            {
                DokFizickaLica mapiraniDok = _mapper.Map<DokFizickaLica>(dokFizickoLice);

                DokFizickaLicaConfirmation noviDokFizickoLice = await _dokFizickaLicaRepository.CreateDokFizickaLica(mapiraniDok);
                await _dokFizickaLicaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetDokFizickaLica", "DokFizickaLica", new { dokFizickaLicaId = noviDokFizickoLice.DokFizickaLicaId });


                await _loggerService.Log(LogLevel.Information, "CreateDokFizickaLica", $"Dokument fizickog lica sa vrednostima: {JsonConvert.SerializeObject(dokFizickoLice)} je uspešno kreiran.");
                return Created(lokacija, _mapper.Map<DokFizickaLicaConfirmationDto>(noviDokFizickoLice));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateDokFizickaLica", $"Greška prilikom unosa  dokument fizicko lica sa vrednostima: {JsonConvert.SerializeObject(dokFizickoLice)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa dokumanta fizickog lica");
            }
        }

        /// <summary>
        /// Izmena dokumenta fizickog lica
        /// </summary>
        /// <param name="dokFizickoLice">Model dokumenta fizickog lica</param>
        /// <returns>Potvrda o izmeni dokumenta fizickog lica</returns>
        /// <response code="200">Izmenjen dokument fizickog lica</response>
        /// <response code="404">Nije pronađen dokument fizickog lica za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene dokumenta fizickog lica</response>
        [Authorize(Roles = "Administrator, Superuser")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DokFizickaLicaDto>> UpdateDokFizickoLice(DokFizickaLicaUpdateDto dokFizickoLice)
        {
            try
            {
                var stariDokFizickaLica = await _dokFizickaLicaRepository.GetDokFizickaLicaById(dokFizickoLice.DokFizickaLicaId);
                var stareVrijednosti = JsonConvert.SerializeObject(stariDokFizickaLica);

                if (stariDokFizickaLica == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateDokFizickoLice", $"Dokument fizickog lica sa id-em {dokFizickoLice.DokFizickaLicaId} nije pronađen.");
                    return NotFound();
                }

                DokFizickaLica noviDokFizickoLice = _mapper.Map<DokFizickaLica>(dokFizickoLice);

                _mapper.Map(noviDokFizickoLice, stariDokFizickaLica);
                await _dokFizickaLicaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Warning, "UpdateDokFizickoLice", $"Dokument fizickog lica sa id-em {dokFizickoLice.DokFizickaLicaId} je uspešno izmenjen. Stare vrednosti su: {stareVrijednosti}");

                return Ok(_mapper.Map<DokFizickaLicaDto>(stariDokFizickaLica));
            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateDokFizickoLice", $"Greška prilikom izmene dokumenta fizickog lica sa id-em {dokFizickoLice.DokFizickaLicaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene dokumenta fizickog lica");
            }

        }


        /// <summary>
        /// Brisanje dokumenta fizickog lica na osnovu ID-a
        /// </summary>
        /// <param name="dokFizickaLicaId">ID dokumenta fizickog lica</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Dokument fizickog lica je uspešno obrisan</response>
        /// <response code="404">Nije pronađen dokument fizickog lica za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja dokumenta fizickog lica</response>
        [Authorize(Roles = "Administrator, Superuser")]
        [HttpDelete("{dokFizickaLicaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDokFizickaLica(Guid dokFizickaLicaId)
        {
            try
            {
                var dokFizickoLice = await _dokFizickaLicaRepository.GetDokFizickaLicaById(dokFizickaLicaId);

                if (dokFizickoLice == null)
                {
                    await _loggerService.Log(LogLevel.Information, "DeletDokFizickoLice", $"Dokument fizickog lica sa id-em {dokFizickaLicaId} nije pronadjen.");
                    return NotFound();
                }

                await _dokFizickaLicaRepository.DeleteDokFizickaLica(dokFizickaLicaId);
                await _dokFizickaLicaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeletDokFizickoLice", $"Dokument fizickog lica sa id-em {dokFizickaLicaId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(dokFizickoLice)}");
                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeletDokFizickoLice", $"Greška prilikom brisanja dokumenta fizickog lica sa id-em {dokFizickaLicaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja dokumenta fizickog lica");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa dokumentima fizickog lica
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser")]
        [HttpOptions]
        public IActionResult GetDokFizickaLicaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
