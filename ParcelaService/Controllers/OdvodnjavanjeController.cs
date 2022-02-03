using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Odvodnjavanje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za odvodnjavanje
    /// </summary>
    [Route("api/odvodnjavanje")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class OdvodnjavanjeController : ControllerBase
    {
        private readonly IOdvodnjavanjeRepository _odvodnjavanjeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public OdvodnjavanjeController(IOdvodnjavanjeRepository odvodnjavanjeRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _odvodnjavanjeRepository = odvodnjavanjeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraća sva odvodnjavanja
        /// </summary>
        /// <returns>Lista odvodnjavanja</returns>
        /// <response code="200">Vraća listu odvodnjavanja</response>
        /// <response code="404">Nije pronađeno ni jedno odvodnjavanje</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<OdvodnjavanjeDto>>> GetAllOdvodnjavanje(string opisOdvodnjavanja)
        {
            var odvodnjavanje = await _odvodnjavanjeRepository.GetAllOdvodnjavanje(opisOdvodnjavanja);

            if(odvodnjavanje == null || odvodnjavanje.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<OdvodnjavanjeDto>>(odvodnjavanje));
        }

        /// <summary>
        /// Vraća jedno odvodnjavanje na osnovu ID-a
        /// </summary>
        /// <param name="odvodnjavanjeId">ID odvodnjavanja</param>
        /// <returns>Odvodnjavanje</returns>
        /// <response code="200">Vraća traženo odvodnjavanje</response>
        /// <response code="404">Nije pronađeno odvodnjavanje za uneti ID</response>
        [HttpGet("odvodnjavanjeId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OdvodnjavanjeDto>> GetOdvodnjavanje(Guid odvodnjavanjeId)
        {
            var odvodnjavanje = await _odvodnjavanjeRepository.GetOdvodnjavanjeById(odvodnjavanjeId);

            if(odvodnjavanje == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OdvodnjavanjeDto>(odvodnjavanje));
        }

        /// <summary>
        /// Kreira novo odvodnjavanje
        /// </summary>
        /// <param name="odvodnjavanje">Model odvodnjavanje</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog odvodnjavanja \
        /// POST /api/odvodnjavanje \
        /// {
        ///       "odvodnjavanjeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "opisOdvodnjavanja": "Opis odvodnjavanja"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju odvodnjavanja</returns>
        /// <response code="200">Vraća kreirano odvodnjavanje</response>
        /// <response code="500">Desila se greška prilikom unosa novog odvodnjavanja</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OdvodnjavanjeDto>> CreateOdvodnjavanje([FromBody] OdvodnjavanjeCreationDto odvodnjavanje)
        {
            try
            {
                Odvodnjavanje novoOdvodnjavanje = await _odvodnjavanjeRepository.CreateOdvodnjavanje(_mapper.Map<Odvodnjavanje>(odvodnjavanje));
                await _odvodnjavanjeRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetOdvodnjavanje", "Odvodnjavanje", new { odvodnjavanjeId = novoOdvodnjavanje.OdvodnjavanjeId });

                return Created(lokacija, _mapper.Map<OdvodnjavanjeDto>(novoOdvodnjavanje));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja odvodnjavanja!");
            }
        }

        /// <summary>
        /// Izmena odvodnjavanja
        /// </summary>
        /// <param name="odvodnjavanje">Model odvodnjavanje</param>
        /// <returns>Potvrda o izmeni odvodnjavanja</returns>
        /// <response code="200">Izmenjena odvodnjavanja</response>
        /// <response code="404">Nije pronađeno odvodnjavanje za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene odvodnjavanja</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OdvodnjavanjeDto>> UpdateOdvodnjavanje(OdvodnjavanjeUpdateDto odvodnjavanje)
        {
            try
            {
                var staroOdvodnjavanje = await _odvodnjavanjeRepository.GetOdvodnjavanjeById(odvodnjavanje.OdvodnjavanjeId);

                if(staroOdvodnjavanje == null)
                {
                    return NotFound();
                }

                Odvodnjavanje novoOdvodnjavanje = _mapper.Map<Odvodnjavanje>(odvodnjavanje);

                _mapper.Map(staroOdvodnjavanje, novoOdvodnjavanje);
                await _odvodnjavanjeRepository.SaveChangesAsync();

                return Ok(_mapper.Map<OdvodnjavanjeDto>(staroOdvodnjavanje));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene odvodnjavanja!");
            }
        }

        /// <summary>
        /// Brisanje odvodnjavanja na osnovu ID-a
        /// </summary>
        /// <param name="odvodnjavanjeId">ID odvodnjavanja</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Odvodnjavanje je uspešno obrisano</response>
        /// <response code="404">Nije pronađeno odvodnjavanje za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja odvodnjavanja</response>
        [HttpDelete("{odvodnjavanjeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteOdvodnjavanje(Guid odvodnjavanjeId)
        {
            try
            {
                var odvodnjavanje = await _odvodnjavanjeRepository.GetOdvodnjavanjeById(odvodnjavanjeId);

                if(odvodnjavanje == null)
                {
                    return NotFound();
                }

                await _odvodnjavanjeRepository.DeleteOdvodnjavanje(odvodnjavanjeId);
                await _odvodnjavanjeRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja odvodnjavanja!");

            }
        }

        /// <summary>
        /// Vraća opcije za rad sa odvodnjavanjem
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetOdvodnjavanjeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
