using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OvlascenoLiceService.Data.Interfaces;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using OvlascenoLiceService.Models.BrojTable;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace OvlascenoLiceService.Controllers
{
    /// <summary>
    /// Kontroler za broj table
    /// </summary>
    [Route("api/brojTable")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class BrojTableController : ControllerBase
    {
        private IBrojTableRepository _brojTableRepository;
        private LinkGenerator _linkGenerator;
        private IMapper _mapper;

        /// <summary>
        /// Konstruktor kontrolera broj table - DI
        /// </summary>
        /// <param name="brojTableRepository">Repo broj table</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        public BrojTableController(IBrojTableRepository brojTableRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _brojTableRepository = brojTableRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraća sve brojeve tabli
        /// </summary>
        /// <returns>Lista brojeva tabli</returns>
        /// <response code="200">Vraća listu brojeva tabli</response>
        /// <response code="404">Nije pronađen ni jedan broj table</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<BrojTableDto>>> GetAllBrojTable()
        {
            var brojeviTabli = await _brojTableRepository.GetAllBrojTable();

            if (brojeviTabli == null || brojeviTabli.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<BrojTableDto>>(brojeviTabli));
        }

        /// <summary>
        /// Vraća jedan broj table na osnovu ID-a
        /// </summary>
        /// <param name="brojTableId">ID broja table</param>
        /// <returns>Broj table</returns>
        /// <response code="200">Vraća tražen broj table</response>
        /// <response code="404">Nije pronađen broj table za uneti ID</response>
        [HttpGet("{brojTableId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrojTableDto>> GetBrojTable(Guid brojTableId)
        {
            var brojTable = await _brojTableRepository.GetBrojTableById(brojTableId);

            if (brojTable == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BrojTableDto>(brojTable));
        }

        /// <summary>
        /// Vraća sve brojeve table za unetu oznaku table
        /// </summary>
        /// <returns>Lista brojeva tabli</returns>
        /// <response code="200">Vraća listu brojeva tabli</response>
        /// <response code="404">Nije pronađen ni jedan broj table</response>
        [HttpGet("oznaka")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<BrojTableDto>>> GetAllBrojTableByBrojTable(string oznakaTable)
        {
            var brojeviTabli = await _brojTableRepository.GetBrojTableByOznakaTable(oznakaTable);

            if (brojeviTabli == null || brojeviTabli.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<BrojTableDto>>(brojeviTabli));
        }

        /// <summary>
        /// Kreira novi broj table
        /// </summary>
        /// <param name="brojTable">Model broj table</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog broja table \
        /// POST /api/brojTable \
        /// {   
        ///     "OznakaTable": "OT23", \
        ///     "OvlascenoLiceId": "5E1BFFFC-1AEE-4662-BC04-341C35B9EBDC" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju broja table</returns>
        /// <response code="200">Vraća kreiran broj table</response>
        /// <response code="500">Desila se greška prilikom unosa novog broja table</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BrojTableConfirmationDto>> CreateBrojTable([FromBody] BrojTableCreationDto brojTable)
        {
            try
            {
                BrojTable mapiranBroj = _mapper.Map<BrojTable>(brojTable);
                BrojTableConfirmation noviBroj = await _brojTableRepository.CreateBrojTable(mapiranBroj);
                await _brojTableRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetBrojTable", "BrojTable", new { brojTableId = noviBroj.BrojTableId });

                return Created(lokacija, _mapper.Map<BrojTableConfirmationDto>(noviBroj));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa broja table.");
            }
        }

        /// <summary>
        /// Izmena broja table
        /// </summary>
        /// <param name="brojTable">Model broj table</param>
        /// <returns>Potvrda o izmeni broja table</returns>
        /// <response code="200">Izmenjen broj table</response>
        /// <response code="404">Nije pronađen broj table za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene broja table</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<BrojTableDto>> UpdateBrojTable(BrojTableUpdateDto brojTable)
        {
            try
            {
                var stariBroj = await _brojTableRepository.GetBrojTableById(brojTable.BrojTableId);

                if (stariBroj == null)
                {
                    return NotFound();
                }

                BrojTable noviBroj = _mapper.Map<BrojTable>(brojTable);
                noviBroj.RbTable = stariBroj.RbTable;

                _mapper.Map(noviBroj, stariBroj);
                await _brojTableRepository.SaveChangesAsync();

                return Ok(_mapper.Map<BrojTableDto>(stariBroj));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene broja table.");
            }
        }

        /// <summary>
        /// Brisanje broja table na osnovu ID-a
        /// </summary>
        /// <param name="brojTableId">ID broja table</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Broj table je uspešno obrisan</response>
        /// <response code="404">Nije pronađen broj table za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja broja table</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{brojTableId}")]
        public async Task<IActionResult> DeleteBrojTable(Guid brojTableId)
        {
            try
            {
                var brojTable = await _brojTableRepository.GetBrojTableById(brojTableId);

                if (brojTable == null)
                {
                    return NotFound();
                }

                await _brojTableRepository.DeleteBrojTable(brojTableId);
                await _brojTableRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja broja table.");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa brojevima tabli
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetBrojTableOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE, HEAD");
            return Ok();
        }

    }
}
