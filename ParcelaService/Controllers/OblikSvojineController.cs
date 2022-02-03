using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.OblikSvojine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za oblik svojine
    /// </summary>
    [Route("api/oblikSvojine")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class OblikSvojineController : ControllerBase
    {
        private readonly IOblikSvojineRepository _oblikSvojineRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public OblikSvojineController(IOblikSvojineRepository oblikSvojineRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _oblikSvojineRepository = oblikSvojineRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraća sve oblike svojine parcele
        /// </summary>
        /// <param name="opisOblikaSvojine">Oblik svojine parcele</param>
        /// <returns>Lista oblika svojina</returns>
        /// <response code="200">Vraća listu oblika svojina</response>
        /// <response code="404">Nije pronađen ni jedan oblik svojine</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<OblikSvojineDto>>> GetAllOblikSvojine(string opisOblikaSvojine)
        {
            var obliciSvojine = await _oblikSvojineRepository.GetAllOblikSvojine(opisOblikaSvojine);

            if (obliciSvojine == null || obliciSvojine.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<OblikSvojineDto>>(obliciSvojine));
        }

        /// <summary>
        /// Vraća jedan oblik svojine parcele na osnovu ID-a
        /// </summary>
        /// <param name="oblikSvojineId">ID oblika svojine</param>
        /// <returns>Oblik svojine parcele</returns>
        /// <response code="200">Vraća traženi oblik svojine</response>
        /// <response code="404">Nije pronađen oblik svojine za uneti ID</response>
        [HttpGet("oblikSvojineId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OblikSvojineDto>> GetOblikSvojine(Guid oblikSvojineId)
        {
            var oblikSvojine = await _oblikSvojineRepository.GetOblikSvojineById(oblikSvojineId);

            if (oblikSvojine == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OblikSvojineDto>(oblikSvojine));
        }

        /// <summary>
        /// Kreira novi oblik svojine parcele
        /// </summary>
        /// <param name="oblikSvojine">Model oblik svojine</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog oblika svojine \
        /// POST /api/oblikSvojine \
        /// {
        ///   "oblikSvojineId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///    "opisOblikaSvojine": "Opis oblika svojine"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju oblika svojine</returns>
        /// <response code="200">Vraća kreiran oblik svojine</response>
        /// <response code="500">Desila se greška prilikom unosa novog oblika svojine</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OblikSvojineDto>> CreateOblikSvojine([FromBody] OblikSvojineCreationDto oblikSvojine)
        {
            try
            {
                OblikSvojine noviOblikSvojine = await _oblikSvojineRepository.CreateOblikSvojine(_mapper.Map<OblikSvojine>(oblikSvojine));
                await _oblikSvojineRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetOblikSvojine", "OblikSvojine", new { oblikSvojineId = noviOblikSvojine.OblikSvojineId });

                return Created(lokacija, _mapper.Map<OblikSvojineDto>(noviOblikSvojine));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja oblika svojine");
            }
        }
        /// <summary>
        /// Izmena oblika svojine parcele
        /// </summary>
        /// <param name="oblikSvojine">Model oblik svojine</param>
        /// <returns>Potvrda o izmeni oblika svojine</returns>
        /// <response code="200">Izmenjen oblika svojine</response>
        /// <response code="404">Nije pronađen oblik svojine za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene oblika svojine</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OblikSvojineDto>> UpdateOblikSvojine(OblikSvojineUpdateDto oblikSvojine)
        {
            try
            {
                var stariOblikSvojine = await _oblikSvojineRepository.GetOblikSvojineById(oblikSvojine.OblikSvojineId);

                if(stariOblikSvojine == null)
                {
                    return NotFound();
                }

                OblikSvojine noviOblikSvojine = _mapper.Map<OblikSvojine>(oblikSvojine);

                _mapper.Map(stariOblikSvojine, noviOblikSvojine);
                await _oblikSvojineRepository.SaveChangesAsync();

                return Ok(_mapper.Map<OblikSvojineDto>(stariOblikSvojine));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene oblika svojine");

            }
        }

        /// <summary>
        /// Brisanje oblika svojine javnog nadmetanja na osnovu ID-a
        /// </summary>
        /// <param name="oblikSvojineId">ID oblika svojine</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Oblik svojine je uspešno obrisan</response>
        /// <response code="404">Nije pronađen oblik svojine za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja oblika svojine</response>
        [HttpDelete("{oblikSvojineId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteOblikSvojine (Guid oblikSvojineId)
        {
            try
            {
                var oblikSvojine = await _oblikSvojineRepository.GetOblikSvojineById(oblikSvojineId);

                if(oblikSvojine == null)
                {
                    return NotFound();
                }

                await _oblikSvojineRepository.DeleteOblikSvojine(oblikSvojineId);
                await _oblikSvojineRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja oblika svojine");

            }
        }

        /// <summary>
        /// Vraća opcije za rad sa oblicima svojine parcele
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetOblikSvojineOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
