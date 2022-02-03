using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.ZasticenaZona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za zaštićenu zonu
    /// </summary>
    [Route("api/zasticenaZona")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class ZasticenaZonaController : ControllerBase
    {
        private readonly IZasticenaZonaRepository _zasticenaZonaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public ZasticenaZonaController(IZasticenaZonaRepository zasticenaZonaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _zasticenaZonaRepository = zasticenaZonaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraća sve zaštićene zone
        /// </summary>
        /// <returns>Lista zaštićenih zona</returns>
        /// <response code="200">Vraća listu zaštićenih zona</response>
        /// <response code="404">Nije pronađena ni jedna zaštićena zona</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ZasticenaZonaDto>>> GetAllZasticenaZona(string brojZasticeneZone)
        {
            var zasticeneZone = await _zasticenaZonaRepository.GetAllZasticenaZona(brojZasticeneZone);

            if(zasticeneZone == null || zasticeneZone.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<ZasticenaZonaDto>>(zasticeneZone));
        }

        /// <summary>
        /// Vraća jednu zaštićenu zonu na osnovu ID-a
        /// </summary>
        /// <param name="zasticenaZonaId">ID zaštićene zone</param>
        /// <returns>Zaštićena zona</returns>
        /// <response code="200">Vraća traženu zaštićenu zonu</response>
        /// <response code="404">Nije pronađena zaštićena zona za uneti ID</response>
        [HttpGet("zasticenaZonaId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ZasticenaZonaDto>> GetZasticenaZona(Guid zasticenaZonaId)
        {
            var zasticenaZona = await _zasticenaZonaRepository.GetZasticenaZonaById(zasticenaZonaId);

            if(zasticenaZona == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ZasticenaZonaDto>(zasticenaZona));
        }

        /// <summary>
        /// Kreira novu zaštićenu zonu
        /// </summary>
        /// <param name="zasticenaZona">Model zaštićena zona</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove zaštićene zone \
        /// POST /api/zasticenaZona \
        /// {
        ///   "ZasticenaZonaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "brojZasticeneZone": "Broj zaštićene zone"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju zaštićene zone</returns>
        /// <response code="200">Vraća kreiranu zaštićenu zonu</response>
        /// <response code="500">Desila se greška prilikom unosa nove zaštićene zone</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ZasticenaZonaDto>> CreateZasticenaZona([FromBody] ZasticenaZonaCreationDto zasticenaZona)
        {
            try
            {
                ZasticenaZona novaZasticenaZona = await _zasticenaZonaRepository.CreateZasticenaZona(_mapper.Map<ZasticenaZona>(zasticenaZona));
                await _zasticenaZonaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetZasticenaZona", "ZasticenaZona", new { zasticenaZonaId = novaZasticenaZona.ZasticenaZonaId });

                return Created(lokacija, _mapper.Map<ZasticenaZonaDto>(novaZasticenaZona));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja zasticene zone!");
            }
        }

        /// <summary>
        /// Izmena zaštićene zone
        /// </summary>
        /// <param name="zasticenaZona">Model zaštićena zona</param>
        /// <returns>Potvrda o izmeni zaštićene zone</returns>
        /// <response code="200">Izmenjena zaštićena zona</response>
        /// <response code="404">Nije pronađena zaštićena zona za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene zaštićene zone</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ZasticenaZonaDto>> UpdateZasticenaZona(ZasticenaZonaUpdateDto zasticenaZona)
        {
            try
            {
                var staraZasticenaZona = await _zasticenaZonaRepository.GetZasticenaZonaById(zasticenaZona.ZasticenaZonaId);

                if(staraZasticenaZona == null)
                {
                    return NotFound();
                }

                ZasticenaZona novaZasticenaZona = _mapper.Map<ZasticenaZona>(zasticenaZona);

                _mapper.Map(staraZasticenaZona, novaZasticenaZona);
                await _zasticenaZonaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<ZasticenaZonaDto>(staraZasticenaZona));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene zasticene zone!");
            }
        }

        /// <summary>
        /// Brisanje zaštićene zone na osnovu ID-a
        /// </summary>
        /// <param name="zasticenaZonaId">ID zaštićene zone</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Zaštićena zona je uspešno obrisana</response>
        /// <response code="404">Nije pronađena zaštićena zona za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja zaštićene zone</response>
        [HttpDelete("{zasticenaZonaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteZasticenaZona (Guid zasticenaZonaId)
        {
            try
            {
                var zasticenaZona = await _zasticenaZonaRepository.GetZasticenaZonaById(zasticenaZonaId);

                if(zasticenaZona == null)
                {
                    return NotFound();
                }

                await _zasticenaZonaRepository.DeleteZasticenaZona(zasticenaZonaId);
                await _zasticenaZonaRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja zasticene zone!");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa zaštićenim zonama
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
