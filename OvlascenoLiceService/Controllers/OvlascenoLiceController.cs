using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OvlascenoLiceService.Data.Interfaces;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using OvlascenoLiceService.Models.OvlascenoLice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OvlascenoLiceService.Controllers
{
    /// <summary>
    /// Kontroler za ovlasceno lice
    /// </summary>
    [Route("api/ovlascenoLice")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class OvlascenoLiceController : ControllerBase
    {
        private readonly IOvlascenoLiceRepository _ovlascenoLiceRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Konstruktor kontrolera ovlascenog lica - DI
        /// </summary>
        /// <param name="ovlascenoLiceRepository">Repo ovlasceno lice</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        public OvlascenoLiceController(IOvlascenoLiceRepository ovlascenoLiceRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _ovlascenoLiceRepository = ovlascenoLiceRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraca sva ovlascena lica
        /// </summary>
        /// <returns>Lista ovlascenih lica</returns>
        /// <response code="200">Vraća listu ovlascenih lica</response>
        /// <response code="404">Nije pronadjeno ni jedno ovlasceno lice</response>
        [HttpGet]
        [HttpHead] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<OvlascenoLiceDto>>> GetAllOvlascenoLice()
        {
            var ovlascenaLica = await _ovlascenoLiceRepository.GetAllOvlascenoLice();

            if (ovlascenaLica == null || ovlascenaLica.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<OvlascenoLiceDto>>(ovlascenaLica));
        }

        /// <summary>
        /// Vraca jedno ovlasceno lice na osnovu ID-a
        /// </summary>
        /// <param name="ovlascenoLiceId">ID ovlascenog lica</param>
        /// <returns>Ovlasceno lice</returns>
        /// <response code="200">Vraća trazeno ovlasceno lice</response>
        /// <response code="404">Nije pronadjeno ovlasceno lice za uneti ID</response>
        [HttpGet("{ovlascenoLiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OvlascenoLiceDto>> GetOvlascenoLice(Guid ovlascenoLiceId)
        {
            var ovlascenoLice = await _ovlascenoLiceRepository.GetOvlascenoLiceById(ovlascenoLiceId);

            if (ovlascenoLice == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OvlascenoLiceDto>(ovlascenoLice));
        }

        /// <summary>
        /// Vraca sva ovlascena lica za zadate kriterijume
        /// </summary>
        /// <returns>Lista ovlascenih lica</returns>
        /// <response code="200">Vraća listu ovlascenih lica</response>
        /// <response code="404">Nije pronadjeno ni jedno ovlasceno lice</response>
        [HttpGet("/imePrezime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<OvlascenoLiceDto>>> GetAllOvlascenoLiceByImePrezime(string ime, string prezime)
        {
            var ovlascenaLica = await _ovlascenoLiceRepository.GetOvlascenaLicaByImePrezime(ime, prezime);

            if (ovlascenaLica == null || ovlascenaLica.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<OvlascenoLiceDto>>(ovlascenaLica));
        }

        /// <summary>
        /// Kreira novo ovlasceno lice
        /// </summary>
        /// <param name="ovlascenoLice">Model ovlasceno lice</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog ovlascenog lica \
        /// POST /api/ovlascenoLice \
        /// {     \
        ///     "Ime": "Milan", \
        ///     "Prezime": "Milanovic", \
        ///     "JMBG": "1008987800025", \
        ///     "AdresaId": "1c989ee3-13b2-4d3b-abeb-c4e6343eace7" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju ovlascenog lica</returns>
        /// <response code="200">Vraca kreirano ovlasceno lice</response>
        /// <response code="500">Desila se greska prilikom unosa novog ovlascenog lica</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OvlascenoLiceConfirmationDto>> CreateOvlascenoLice([FromBody] OvlascenoLiceCreationDto ovlascenoLice)
        {
            try
            {
                OvlascenoLice mapiranoLice = _mapper.Map<OvlascenoLice>(ovlascenoLice);
                OvlascenoLiceConfirmation novoLice = await _ovlascenoLiceRepository.CreateOvlascenoLice(mapiranoLice);
                await _ovlascenoLiceRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetOvlascenoLice", "OvlascenoLice", new { ovlascenoLiceId = novoLice.OvlascenoLiceId });

                return Created(lokacija, _mapper.Map<OvlascenoLiceConfirmationDto>(novoLice));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom unosa ovlascenog lica");
            }
        }

        /// <summary>
        /// Izmena ovlascenog lica
        /// </summary>
        /// <param name="ovlascenoLice">Model ovlascenog lica</param>
        /// <returns>Potvrda o izmeni ovlascenog lica</returns>
        /// <response code="200">Izmenjeno ovlasceno lice</response>
        /// <response code="404">Nije pronadjeno ovlasceno lice za uneti ID</response>
        /// <response code="500">Serverska greska tokom izmene ovlascenog lica</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<OvlascenoLiceDto>> UpdateOvlascenoLice(OvlascenoLiceUpdateDto ovlascenoLice)
        {
            try
            {
                var staroLice = await _ovlascenoLiceRepository.GetOvlascenoLiceById(ovlascenoLice.OvlascenoLiceId);

                if (staroLice == null)
                {
                    return NotFound();
                }

                OvlascenoLice novoLice = _mapper.Map<OvlascenoLice>(ovlascenoLice);

                _mapper.Map(novoLice, staroLice);
                await _ovlascenoLiceRepository.SaveChangesAsync();

                return Ok(_mapper.Map<OvlascenoLiceDto>(staroLice));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene ovlascenog lica");
            }
        }

        /// <summary>
        /// Brisanje ovlascenog lica na osnovu ID-a
        /// </summary>
        /// <param name="ovlascenoLiceId">ID ovlascenog lica</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Ovlasceno lice je uspesno obrisano</response>
        /// <response code="404">Nije pronadjeno ovlasceno lice za uneti ID</response>
        /// <response code="500">Serverska greska tokom brisanja ovlascenog lica</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{ovlascenoLiceId}")]
        public async Task<IActionResult> DeleteOvlascenoLice(Guid ovlascenoLiceId)
        {
            try
            {
                var ovlascenoLice = await _ovlascenoLiceRepository.GetOvlascenoLiceById(ovlascenoLiceId);

                if (ovlascenoLice == null)
                {
                    return NotFound();
                }

                await _ovlascenoLiceRepository.DeleteOvlascenoLice(ovlascenoLiceId);
                await _ovlascenoLiceRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja ovlascenog lica");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa ovlascenim licima
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetOvlascenoLiceOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE, HEAD");
            return Ok();
        }
    }
}
