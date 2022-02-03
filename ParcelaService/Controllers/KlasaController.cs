using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Klasa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za klasu
    /// </summary>
    [Route("api/klasa")]
    [ApiController]
    [Produces("application/json", "application/xml")]

    public class KlasaController : ControllerBase
    {
        private readonly IKlasaRepository _klasaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public KlasaController(IKlasaRepository klasaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _klasaRepository = klasaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }
        /// <summary>
        /// Vraća sve klase
        /// </summary>
        /// <returns>Lista klasa</returns>
        /// <response code="200">Vraća listu klasa</response>
        /// <response code="404">Nije pronađena ni jedna klasa</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<KlasaDto>>> GetAllKlasa(string KlasaNaziv)
        {
            var klase = await _klasaRepository.GetAllKlasa(KlasaNaziv);

            if (klase == null || klase.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KlasaDto>>(klase));
        }

        /// <summary>
        /// Vraća jednu klasu na osnovu ID-a 
        /// </summary>
        /// <param name="klasaId">Model klase</param>
        /// <returns>Klasa</returns>
        /// <response code="200">Vraća traženu klasu</response>
        /// <response code="404">Nije pronađena klasa za uneti ID</response>
        [HttpGet("{klasaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<KlasaDto>> GetKlasa(Guid klasaId)
        {
            var klasa = await _klasaRepository.GetKlasaById(klasaId);

            if (klasa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KlasaDto>(klasa));
        }

        /// <summary>
        /// Kreira novu klasu
        /// </summary>
        /// <param name="klasa">Model klase</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove klase \
        /// POST /api/klasa \
        /// {
        ///      "klasaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///      "klasaNaziv": "Naziv klase"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju klase</returns>
        /// <response code="200">Vraća kreiranu klasu</response>
        /// <response code="500">Desila se greška prilikom unosa nove klase</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KlasaDto>> CreateKlasa([FromBody] KlasaCreationDto klasa)
        {
            try
            {
                Klasa novaKlasa = await _klasaRepository.CreateKlasa(_mapper.Map<Klasa>(klasa));
                await _klasaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetKlasa", "Klasa", new { klasaId = novaKlasa.KlasaId });

                return Created(lokacija, _mapper.Map<KlasaDto>(novaKlasa));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja klase!");

            }
        }

        /// <summary>
        /// Izmena klase
        /// </summary>
        /// <param name="klasa">Model klase</param>
        /// <returns>Potvrda o izmeni klase</returns>
        /// <response code="200">Izmenjena klasa</response>
        /// <response code="404">Nije pronađena klasa za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene klase</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KlasaDto>> UpdateKlasa(KlasaUpdateDto klasa)
        {
            try
            {
                var staraKlasa = await _klasaRepository.GetKlasaById(klasa.KlasaId);

                if (staraKlasa == null)
                {
                    return NotFound();
                }

                Klasa novaKlasa = _mapper.Map<Klasa>(klasa);

                _mapper.Map(novaKlasa, staraKlasa);
                await _klasaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<KlasaDto>(staraKlasa));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene klase");
            }
        }

        /// <summary>
        /// Brisanje klase na osnovu ID-a
        /// </summary>
        /// <param name="klasaId">ID klase</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Klasa je uspešno obrisana</response>
        /// <response code="404">Nije pronađena klasa za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja klasa</response>
        [HttpDelete("{klasaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteKlasa(Guid klasaId)
        {
            try
            {
                var klasa = await _klasaRepository.GetKlasaById(klasaId);

                if (klasa == null)
                {
                    return NotFound();
                }

                await _klasaRepository.DeleteKlasa(klasaId);
                await _klasaRepository.SaveChangesAsync();

                return NoContent();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja klase");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa klasama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetKlasaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }

}
