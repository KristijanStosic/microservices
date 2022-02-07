using AdresaService.Data.Interfaces;
using AdresaService.Entities;
using AdresaService.Model.Adresa;
using AdresaService.ServiceCalls;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Controllers
{
    /// <summary>
    /// Kontroler za adrese
    /// </summary>
    [ApiController]
    [Route("api/adresa")]
    [Produces("application/json", "application/xml")]
    public class AdresaController : ControllerBase
    {
        private readonly IAdresaRepository _adresaRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILoggerService _loggerService;

        public AdresaController(IAdresaRepository adresaRepository, IMapper mapper, LinkGenerator linkGenerator, ILoggerService loggerService)
        {
            this._adresaRepository = adresaRepository;
            this._mapper = mapper;
            this._linkGenerator = linkGenerator;
            this._loggerService = loggerService;
        }
        /// <summary>
        /// Vraća listu adresa na osnovu zadatih filtera
        /// </summary>
        /// <param name="ulica">Naziv ulice</param>
        /// <param name="mesto">Naziv mesta u kom se nalazi adresa</param>
        /// <param name="postanskiBroj">Poštanski broj</param>
        /// <returns>Listu adresa koje zadovoljavaju zadate filtere</returns>
        /// <response code="200">Uspešno vraćena lista državi</response>
        /// <response code="204">Nije pronađena nijedna država</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<AdresaDto>>> GetAdrese(string ulica, string mesto, string postanskiBroj)
        {
            var adrese = await _adresaRepository.GetAdrese(ulica, mesto, postanskiBroj);

            if (adrese == null || adrese.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAdrese", "Lista adresa je prazna ili null");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetAdrese", "Lista adresa je uspešno pronađena");
            return Ok(_mapper.Map<List<AdresaDto>>(adrese));
        }
        /// <summary>
        /// Vraća adresu na osnovu unetog id-a
        /// </summary>
        /// <param name="adresaId">Id adrese</param>
        /// <returns>Adresu</returns>
        /// <response code="200">Uspešno vraćena adresa</response>
        /// <response code="404">Nije pronađena adresa sa zadatim id-em</response>
        [HttpGet("{adresaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AdresaDto>> GetAdresaById(Guid adresaId)
        {
            var adresa = await _adresaRepository.GetAdresaById(adresaId);

            if (adresa == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAdresaById", $"Adresa sa id-em {adresaId} nije pronađena.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetAdresaById", $"Adresa sa id-em {adresaId} je uspešno pronađena.");
            return Ok(_mapper.Map<AdresaDto>(adresa));
        }
        /// <summary>
        /// Kreira novu adresu
        /// </summary>
        /// <param name="adresa">Adresa</param>
        /// <returns>Potvrdu o kreiranoj adresi</returns>
        /// <remarks>
        /// Primer kreiranja nove adrese \
        /// POST /api/adresa \ 
        /// { \
        ///	"Ulica": "Stepan", \
        ///	"Broj": "3a", \
        ///	"Mesto": "Novi Sad",\
        ///	"PostanskiBroj": "21000", \
        ///	"DrzavaId": "F320743F-6C87-47CA-9F82-50191C1D31AC" \
        ///}
        /// </remarks>
        /// <response code="201">Uspešno kreirana adresa</response>
        /// <response code="500">Desila se greška prilikom kreiranja nove adrese</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AdresaConformationDto>> CreateAdresa(AdresaCreationDto adresa)
        {
            try
            {
                Adresa newAdresa = await _adresaRepository.CreateAdresa(_mapper.Map<Adresa>(adresa));
                await _adresaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetAdresaById", "Adresa", new { adresaId = newAdresa.AdresaId });
                await _loggerService.Log(LogLevel.Information, "CreateAdresa", $"Adresa sa vrednostima: {JsonConvert.SerializeObject(adresa)} je uspešno kreirana.");

                return Created(lokacija, _mapper.Map<AdresaConformationDto>(newAdresa));
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "CreateAdresa", $"Greška prilikom unosa adrese sa vrednostima: {JsonConvert.SerializeObject(adresa)}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }
        /// <summary>
        /// Vrši brisanje adrese na osnovu unetog id-a
        /// </summary>
        /// <param name="adresaId">Id adrese</param>
        /// <returns></returns>
        /// <response code="200">Uspešno obrisana adresa</response>
        /// <response code="404">Nije pronađena adresa na osnovu unetog id-a</response>
        /// <response code="" 500>Desila se greška prilikom brisanja adrese</response>
        [HttpDelete("{adresaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAdresa(Guid adresaId)
        {
            try
            {
                var adresa = await _adresaRepository.GetAdresaById(adresaId);

                if (adresa == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteAdresa", $"Adresa sa id-em {adresaId} nije pronađena.");
                    return NotFound();
                }

                await _adresaRepository.DeleteAdresa(adresaId);
                await _adresaRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "DeleteAdresa", $"Adresa sa id-em {adresaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(adresa)}");

                return Ok();
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteAdresa", $"Greška prilikom brisanja adrese sa id-em {adresaId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }
        /// <summary>
        /// Vrši ažuriranje adrese
        /// </summary>
        /// <param name="adresaUpdate">Adresa</param>
        /// <returns>Adresu</returns>
        /// <remarks>
        /// Primer kreiranja nove adrese \
        /// POST /api/adresa \ 
        /// { \
        /// "adresaId":"1c989ee3-13b2-4d3b-abeb-c4e6343eace7", \
        ///	"Ulica": "Stepan", \
        ///	"Broj": "3a", \
        ///	"Mesto": "Novi Sad",\
        ///	"PostanskiBroj": "21000", \
        ///	"DrzavaId": "F320743F-6C87-47CA-9F82-50191C1D31AC" \
        ///}
        /// </remarks>
        /// <response code="200">Uspešno ažurirana adresa</response>
        /// <response code="404">Nije pronađena adresa na osnovu prosleđenog id-a</response>
        /// <response code="500">Desila se greška prilikom ažuriranja adrese</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AdresaDto>> UpdateAdresa(AdresaUpdateDto adresaUpdate)
        {
            try
            {
                var oldAdresa = await _adresaRepository.GetAdresaById(adresaUpdate.AdresaId);
             
                if (oldAdresa == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateAdresa", $"Adresa sa id-em {adresaUpdate.AdresaId} nije pronađena.");
                    return NotFound();
                }
                //Ovo radim kako bi izbegao bug gde se azurirana drzava obrise
                Drzava drzava = oldAdresa.Drzava;

                var stareVrednosti = JsonConvert.SerializeObject(oldAdresa);

                Adresa adresa = _mapper.Map<Adresa>(adresaUpdate);
                
                _mapper.Map(adresa, oldAdresa);

                oldAdresa.Drzava = drzava;

                await _adresaRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "UpdateAdresa", $"Adresa sa id-em {adresaUpdate} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<AdresaDto>(adresa));
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateAdresa", $"Greška prilikom izmene adrese sa id-em {adresaUpdate}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa adresama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetAdresaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
