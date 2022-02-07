using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Entities.Confirmations;
using ParcelaService.Models.Parcela;
using ParcelaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za parcelu
    /// </summary>
    [Route("api/parcela")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class ParcelaController : ControllerBase
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public ParcelaController(IParcelaRepository parcelaRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            _parcelaRepository = parcelaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
        }
        /// <summary>
        /// Vraća sve parcele
        /// </summary>
        /// <returns>Lista parcela</returns>
        /// <response code="200">Vraća listu parcela</response>
        /// <response code="404">Nije pronađena ni jedna parcela </response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, Menadzer, OperaterNadmetanja")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<ParcelaDto>>> GetAllParcela()
        {
            var parcele = await _parcelaRepository.GetAllParcela();

            if(parcele == null || parcele.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllParcela", "Lista oblika parcela je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllParcela", "Lista parcela je uspešno vraćena.");

            return Ok(_mapper.Map<List<ParcelaDto>>(parcele));

        }

        /// <summary>
        /// Vraća jednu parcelu na osnovu ID-a
        /// </summary>
        /// <param name="parcelaId">ID parcele</param>
        /// <returns>Parcela</returns>
        /// <response code="200">Vraća traženu parcelu</response>
        /// <response code="404">Nije pronađena parcela za uneti ID</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, Menadzer, OperaterNadmetanja")]
        [HttpGet("parcelaId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParcelaDto>> GetParcela(Guid parcelaId)
        {
            var parcela = await _parcelaRepository.GetParcelaById(parcelaId);

            if(parcela == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetParcela", $"Parcela sa id-em {parcelaId} nije pronađena.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetParcela", $"Parcela sa id-em {parcelaId} je uspešno vraćena.");

            return Ok(_mapper.Map<ParcelaDto>(parcela));
        }

        /// <summary>
        /// Kreira novu parcelu
        /// </summary>
        /// <param name="parcela">Model parcele</param>
        /// <remarks>
        /// Primer zahteva za kreiranje nove parcele \
        /// POST /api/parcela \
        /// {
        ///   "brojParcele": "broj parcele",
        ///   "povrsinaParcele": "Površina parcele",
        ///   "brojListeNepokretnosti": "Broj liste nepokretnosti",
        ///   "zasticenaZonaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "oblikSvojineId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "odvodnjavanjeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///   "katastarskaOpstinaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju parcele</returns>
        /// <response code="201">Vraća kreiranu parcelu </response>
        /// <response code="500">Desila se greška prilikom unosa nove parcele</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ParcelaConfirmationDto>> CreateParcela([FromBody] ParcelaCreationDto parcela)
        {
            try
            {
                ParcelaConfirmation novaParcela = await _parcelaRepository.CreateParcela(_mapper.Map<Parcela>(parcela));
                await _parcelaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetParcela", "Parcela", new { parcelaId = novaParcela.ParcelaId });

                await _loggerService.Log(LogLevel.Information, "CreateParcela", $"Parcela sa vrednostima: {JsonConvert.SerializeObject(parcela)} je uspešno kreirana.");

                return Created(lokacija, _mapper.Map<ParcelaConfirmationDto>(novaParcela));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateOblikSvojine", $"Greška prilikom unosa parcele sa vrednostima: {JsonConvert.SerializeObject(parcela)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja parcele");
            }
        }

        /// <summary>
        /// Izmena parcele
        /// </summary>
        /// <param name="parcela">Model parcele</param>
        /// <returns>Potvrda o izmeni parcele</returns>
        /// <response code="200">Izmenjena parcela</response>
        /// <response code="404">Nije pronađena parcela za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene parcele</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ParcelaDto>> UpdateParcela(ParcelaUpdateDto parcela)
        {
            try
            {
                var staraParcela = await _parcelaRepository.GetParcelaById(parcela.ParcelaId);

                if(staraParcela == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateParcela", $"Parcela sa id-em {parcela.ParcelaId} nije pronađena.");
                    return NotFound();
                }

                var stareVrednosti = JsonConvert.SerializeObject(staraParcela);

                Parcela novaParcela = _mapper.Map<Parcela>(parcela);

                _mapper.Map(staraParcela, novaParcela);
                await _parcelaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateParcela", $"Parcela sa id-em {parcela.ParcelaId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<ParcelaDto>(staraParcela));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateParcela", $"Greška prilikom izmene parcele sa id-em {parcela.ParcelaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene parcele");
            }
        }

        /// <summary>
        /// Brisanje parcele na osnovu ID-a
        /// </summary>
        /// <param name="parcelaId">ID parcele</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Parcela je uspešno obrisan</response>
        /// <response code="404">Nije pronađena parcela za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja parcele</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpDelete("{parcelaId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteParcela(Guid parcelaId)
        {
            try
            {
                var parcela = await _parcelaRepository.GetParcelaById(parcelaId);

                if(parcela == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteParcela", $"Parcela sa id-em {parcelaId} nije pronađena.");
                    return NotFound();
                }

                await _parcelaRepository.DeleteParcela(parcelaId);
                await _parcelaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteParcela", $"Parcela sa id-em {parcelaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(parcela)}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteParcela", $"Greška prilikom brisanja parcele sa id-em {parcelaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja parcele");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa parcelama
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Administrator, Superuser,Menadzer, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetParcelaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
