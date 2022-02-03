using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Obradivost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za obradivost
    /// </summary>
    [Route("api/obradivost")]
    [ApiController]
    [Produces("application/json", "application/xml")]

    public class ObradivostController : ControllerBase
    {
        private readonly IObradivostRepository _obradivostRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public ObradivostController(IObradivostRepository obradivostRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _obradivostRepository = obradivostRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraća sve obradivosti
        /// </summary>
        /// <returns>Lista obradivosti</returns>
        /// <response code="200">Vraća listu obradivosti</response>
        /// <response code="404">Nije pronađena ni jedna obradivost</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ObradivostDto>>> GetAllObradivost(string opisObradivosti)
        {
            var obradivost = await _obradivostRepository.GetAllObradivost(opisObradivosti);

            if(obradivost == null || obradivost.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<ObradivostDto>>(obradivost));
        }

        /// <summary>
        /// Vraća jednu obradivost na osnovu ID-a
        /// </summary>
        /// <param name="obradivostId">ID obradivosti</param>
        /// <returns>Obradivost</returns>
        /// <response code="200">Vraća traženu obradivost</response>
        /// <response code="404">Nije pronađena obradivost za uneti ID</response>
        [HttpGet("obradivostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ObradivostDto>> GetObradivost(Guid obradivostId)
        {
            var obradivost = await _obradivostRepository.GetObradivostById(obradivostId);

            if(obradivost == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ObradivostDto>(obradivost));
        }

        /// <summary>
        /// Kreira novu obradivost
        /// </summary>
        /// <param name="obradivost">Model obradivosti</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove obradivosti \
        /// POST /api/obradivost \
        /// {
        ///       "obradivostId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "opisObradivosti": "Opis obradivosti"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju obradivosti</returns>
        /// <response code="200">Vraća kreiranu obradivost</response>
        /// <response code="500">Desila se greška prilikom unosa nove obradivosti</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ObradivostDto>> CreateObradivost([FromBody] ObradivostCreationDto obradivost)
        {
            try
            {
                Obradivost novaObradivost = await _obradivostRepository.CreateObradivost(_mapper.Map<Obradivost>(obradivost));
                await _obradivostRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetObradivost", "Obradivost", new { obradivostId = novaObradivost.ObradivostId });

                return Created(lokacija, _mapper.Map<ObradivostDto>(novaObradivost));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja obradivosti!");
            }

        }

        /// <summary>
        /// Izmena obradivosti
        /// </summary>
        /// <param name="obradivost">Model obradivost</param>
        /// <returns>Potvrda o izmeni obradivosti</returns>
        /// <response code="200">Izmenjena obradivost</response>
        /// <response code="404">Nije pronađena obradivost za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene obradivosti</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ObradivostDto>> UpdateObradivost(ObradivostUpdateDto obradivost)
        {
            try
            {
                var staraObradivost = await _obradivostRepository.GetObradivostById(obradivost.ObradivostId);

                if(staraObradivost == null)
                {
                    return NotFound();
                }

                Obradivost novaObradivost = _mapper.Map<Obradivost>(obradivost);

                _mapper.Map(staraObradivost, novaObradivost);
                await _obradivostRepository.SaveChangesAsync();

                return Ok(_mapper.Map<ObradivostDto>(staraObradivost));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene obradivosti!");
            }
        }
        /// <summary>
        /// Brisanje obradivosti na osnovu ID-a
        /// </summary>
        /// <param name="obradivostId">ID obradivosti</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Obradivost je uspešno obrisana</response>
        /// <response code="404">Nije pronađena obradivost za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja obradivosti</response>
        [HttpDelete("{obradivostId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteObradivost (Guid obradivostId)
        {
            try
            {
                var obradivost = await _obradivostRepository.GetObradivostById(obradivostId);

                if (obradivost == null)
                {
                    return NotFound();
                }

                await _obradivostRepository.DeleteObradivost(obradivostId);
                await _obradivostRepository.SaveChangesAsync();

                return NoContent();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja obradivosti!");

            }
        }

        /// <summary>
        /// Vraća opcije za rad sa obradivostima
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetObradivostOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }

}
