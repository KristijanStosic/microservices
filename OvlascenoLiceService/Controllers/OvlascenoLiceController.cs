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
    /// Kontroler za ovlašćeno lice
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
        /// Konstruktor kontrolera ovlašćenog lica - DI
        /// </summary>
        /// <param name="ovlascenoLiceRepository">Repo ovlašćeno lice</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        public OvlascenoLiceController(IOvlascenoLiceRepository ovlascenoLiceRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _ovlascenoLiceRepository = ovlascenoLiceRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraća sva ovlašćena lica
        /// </summary>
        /// <returns>Lista ovlašćenih lica</returns>
        /// <response code="200">Vraća listu ovlašćenih lica</response>
        /// <response code="404">Nije pronađeno ni jedno ovlašćeno lice</response>
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
        /// Vraća jedno ovlašćeno lice na osnovu ID-a
        /// </summary>
        /// <param name="ovlascenoLiceId">ID ovlašćenog lica</param>
        /// <returns>Ovlašćeno lice</returns>
        /// <response code="200">Vraća traženo ovlašćeno lice</response>
        /// <response code="404">Nije pronađeno ovlašćeno lice za uneti ID</response>
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
        /// Vraća sva ovlašćena lica za zadate kriterijume
        /// </summary>
        /// <returns>Lista ovlašćenih lica</returns>
        /// <response code="200">Vraća listu ovlašćenih lica</response>
        /// <response code="404">Nije pronađeno ni jedno ovlašćeno lice</response>
        [HttpGet("imePrezime")]
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
        /// Kreira novo ovlašćeno lice
        /// </summary>
        /// <param name="ovlascenoLice">Model ovlašćeno lice</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog ovlašćenog lica \
        /// POST /api/ovlascenoLice \
        /// {   
        ///     "Ime": "Milan", \
        ///     "Prezime": "Milanovic", \
        ///     "JMBG": "1008987800025", \
        ///     "AdresaId": "1c989ee3-13b2-4d3b-abeb-c4e6343eace7" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju ovlašćenog lica</returns>
        /// <response code="200">Vraća kreirano ovlašćeno lice</response>
        /// <response code="500">Desila se greška prilikom unosa novog ovlašćenog lica</response>
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa ovlašćenog lica");
            }
        }

        /// <summary>
        /// Izmena ovlašćenog lica
        /// </summary>
        /// <param name="ovlascenoLice">Model ovlašćenog lica</param>
        /// <returns>Potvrda o izmeni ovlašćenog lica</returns>
        /// <response code="200">Izmenjeno ovlašćeno lice</response>
        /// <response code="404">Nije pronađeno ovlašćeno lice za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene ovlašćenog lica</response>
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene ovlašćenog lica");
            }
        }

        /// <summary>
        /// Brisanje ovlašćenog lica na osnovu ID-a
        /// </summary>
        /// <param name="ovlascenoLiceId">ID ovlašćenog lica</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Ovlašćeno lice je uspešno obrisano</response>
        /// <response code="404">Nije pronađeno ovlašćeno lice za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja ovlašćenog lica</response>
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja ovlašćenog lica");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa ovlašćenim licima
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
