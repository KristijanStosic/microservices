using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Models.Etapa;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Controllers
{
    /// <summary>
    /// Kontroler za etapu
    /// </summary>
    [Route("api/etapa")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class EtapaController : ControllerBase
    {
        private readonly IEtapaRepository _etapaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public EtapaController(IEtapaRepository etapaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _etapaRepository = etapaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraća sve etape
        /// </summary>
        /// <returns>Lista etapa</returns>
        /// <response code="200">Vraća listu etapa</response>
        /// <response code="404">Nije pronađena ni jedna etapa</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<EtapaDto>>> GetAllEtapa()
        {
            var etape = await _etapaRepository.GetAllEtapa();

            if (etape == null || etape.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<EtapaDto>>(etape));
        }

        /// <summary>
        /// Vraća jednu etapu na osnovu ID-a
        /// </summary>
        /// <param name="etapaId">ID etape</param>
        /// <returns>Etapa</returns>
        /// <response code="200">Vraća traženu etapu</response>
        /// <response code="404">Nije pronađena etapa za uneti ID</response>
        [HttpGet("{etapaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EtapaDto>> GetEtapa(Guid etapaId)
        {
            var etapa = await _etapaRepository.GetEtapaById(etapaId);

            if (etapa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EtapaDto>(etapa));
        }

        /// <summary>
        /// Kreira novu etapu
        /// </summary>
        /// <param name="etapa">Model etapa</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove etape \
        /// POST /api/etapa \
        /// {  \   
        ///     "Datum": "2022-01-31T07:25:31.426Z", \
        ///     "DanPoRedu": 1, \
        ///     "VremePocetka": "12:00", \
        ///     "VremeKraja": "14:00", \
        ///     "ZavrsenaUspesno": false, \
        ///     "JavnoNadmetanjeId": "417d646c-9e90-4515-a068-21689864330a" \
        ///} \
        /// </remarks>
        /// <returns>Potvrda o kreiranju etape</returns>
        /// <response code="200">Vraća kreiranu etapu</response>
        /// <response code="500">Desila se greška prilikom unosa nove etape</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EtapaConfirmationDto>> CreateEtapa([FromBody] EtapaCreationDto etapa)
        {
            try
            {
                Etapa mapiranaEtapa = _mapper.Map<Etapa>(etapa);
                var proveraValidnosti = await _etapaRepository.IsValidEtapa(mapiranaEtapa);

                if (!proveraValidnosti)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Vec postoji etapa u okviru ovog javnog nadmetanja koja je uspesno zavrsena!");
                }

                EtapaConfirmation novaEtapa = await _etapaRepository.CreateEtapa(mapiranaEtapa);
                await _etapaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetEtapa", "Etapa", new { etapaId = novaEtapa.EtapaId });

                return Created(lokacija, _mapper.Map<EtapaConfirmationDto>(novaEtapa));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom unosa etape");
            }
        }

        /// <summary>
        /// Izmena etape
        /// </summary>
        /// <param name="etapa">Model etapa</param>
        /// <returns>Potvrda o izmeni etape</returns>
        /// <response code="200">Izmenjena etapa</response>
        /// <response code="404">Nije pronađena etapa za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene etape</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EtapaDto>> UpdateEtapa(EtapaUpdateDto etapa)
        {
            try
            {
                var staraEtapa = await _etapaRepository.GetEtapaById(etapa.EtapaId);

                if (staraEtapa == null)
                {
                    return NotFound();
                }

                Etapa novaEtapa = _mapper.Map<Etapa>(etapa);

                _mapper.Map(novaEtapa, staraEtapa);
                await _etapaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<EtapaDto>(staraEtapa));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene etape");
            }
        }

        /// <summary>
        /// Brisanje etape na osnovu ID-a
        /// </summary>
        /// <param name="etapaId">ID etape</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Etapa je uspešno obrisana</response>
        /// <response code="404">Nije pronađena etapa za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja etape</response>
        [HttpDelete("{etapaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEtapa(Guid etapaId)
        {
            try
            {
                var etapa = await _etapaRepository.GetEtapaById(etapaId);

                if (etapa == null)
                {
                    return NotFound();
                }

                await _etapaRepository.DeleteEtapa(etapaId);
                await _etapaRepository.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja etape");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa etapama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetEtapaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
