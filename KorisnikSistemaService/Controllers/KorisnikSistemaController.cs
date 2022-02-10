using AutoMapper;
using KorisnikSistemaService.Data.Interfaces;
using KorisnikSistemaService.Entities;
using KorisnikSistemaService.Models.KorisnikSistema;
using KorisnikSistemaService.ServiceCalls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KorisnikSistemaService.Controllers
{
    /// <summary>
    /// Kontroler za korisnika sistema
    /// </summary>
	[Authorize(Roles ="Administrator")]
    [Route("api/KorisnikSistema")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class KorisnikSistemaController : ControllerBase
    {

        private readonly IKorisnikSistemaRepository _korisnikSistemaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public KorisnikSistemaController(IKorisnikSistemaRepository korisnikSistemaRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService)
        {
            this._korisnikSistemaRepository = korisnikSistemaRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve Korisnike sistema
        /// </summary>
        /// <returns>Lista korisnika sistema</returns>
        /// <response code="200">Vraća listu korisnika sistema</response>
        /// <response code="404">Nije pronađen ni jedan korisnik sistema</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<KorisnikSistemaDto>>> GetAllKorisnikSistema()
        {
            var korisniciSistema = await _korisnikSistemaRepository.GetAllKorisnikSistema();

            if (korisniciSistema == null || korisniciSistema.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllKorisnikSistema", "Lista korisnika sistema je prazna.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllKorisnikSistema", "Lista korisnika sistema je uspešno vraćena.");
            return Ok(_mapper.Map<List<KorisnikSistemaDto>>(korisniciSistema));
        }

        /// <summary>
        /// Vraća jednog korisnika sistema na osnovu ID-a
        /// </summary>
        /// <param name="korisnikSistemaId">ID korisnikaSistema</param>
        /// <returns>Korisnik sistema</returns>
        /// <response code="200">Vraća traženog korisnika sistema</response>
        /// <response code="404">Nije pronađen korisnik sistema za uneti ID</response>
        [HttpGet("{korisnikSistemaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<KorisnikSistemaDto>> GetKorisnikSistema(Guid korisnikSistemaId)
        {
            var korisnikSistema = await _korisnikSistemaRepository.GetKorisnikSistemaById(korisnikSistemaId);

            if (korisnikSistema == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKorisnikSitema", $"Korisnik sistema sa id-em {korisnikSistemaId} nije pronađen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetKorisnikSitema", $"Korisnik sistema sa id-em {korisnikSistemaId} je uspešno vraćen.");

            return Ok(_mapper.Map<KorisnikSistemaDto>(korisnikSistema));
        }

        /// <summary>
        /// Kreira novog korisnika
        /// </summary>
        /// <param name="korisnikSistema">KorisnikSistema</param>
        /// <returns>Potvrdu o kreiranom korisniku sistema</returns>
        /// <remarks>
        /// Primer kreiranja novog korisnika sistema \
        /// POST /api/korisnikSistema \ 
        /// { \
        ///	"ime": "Mladen", \
        ///	"prezime": "Bajic", \
        ///	"email": "mladen_ba@gmail.com",\
        ///	"korisnickoIme": "Mladen33", \
        ///	"lozinka": "Mladen, \
        ///	"tipKorisnikaId": "F76FFDB2-32D6-4E36-84A1-431C5158C028" \
        ///}
        /// </remarks>
        /// <response code="201">Uspešno kreiran korisnik sistema</response>
        /// <response code="500">Desila se greška prilikom kreiranja korisnika sistema</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KorisnikSistemaConformationDto>> CreateKorisnikSistema(KorisnikSistemaCreationDto korisnikSistema)
        {
            try
            {
                KorisnikSistemaCreationDto korisnik = korisnikSistema;
                korisnik.Lozinka = BCrypt.Net.BCrypt.HashPassword(korisnikSistema.Lozinka);


                KorisnikSistema noviKorisnikSistma = await _korisnikSistemaRepository.CreateKorisnikSistema(_mapper.Map<KorisnikSistema>(korisnik));
                await _korisnikSistemaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetKorisnikSistema", "KorisnikSistema", new { korisnikSistemaId = noviKorisnikSistma.KorisnikSistemaId });
                await _loggerService.Log(LogLevel.Information, "CreateKorisnikSistema", $"Korisnik sistema sa vrednostima: {JsonConvert.SerializeObject(korisnikSistema)} je uspešno kreiran.");

                return Created(lokacija, _mapper.Map<KorisnikSistemaConformationDto>(noviKorisnikSistma));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateKorisnikSistema", $"Greška prilikom unosa korisnika sistema sa vrednostima: {JsonConvert.SerializeObject(korisnikSistema)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        /// <summary>
        /// Vrši izmjenu korisnika sistema
        /// </summary>
        /// <param name="korisnikSistema">Korisnik sistema</param>
        /// <returns>KorisnikSistema</returns>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Uspešna izmjena adrese</response>
        /// <response code="404">Nije pronađen korisnik sistema na osnovu prosleđenog id-a</response>
        /// <response code="500">Desila se greška prilikom brisanja korisnika sistema</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KorisnikSistemaDto>> UpdateKorisnikSistema(KorisnikSistemaUpdateDto korisnikSistema)
        {
            try
            {
                var stariKorisnikSistema = await _korisnikSistemaRepository.GetKorisnikSistemaById(korisnikSistema.KorisnikSistemaId);

                if (stariKorisnikSistema == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateKorisnikSistema", $"KorisnikSistema sa id-em {korisnikSistema.KorisnikSistemaId} nije pronađen.");
                    return NotFound();
                }
                if (!String.Equals(stariKorisnikSistema.Lozinka, korisnikSistema.Lozinka))
                {
                    korisnikSistema.Lozinka = BCrypt.Net.BCrypt.HashPassword(korisnikSistema.Lozinka);
                }

                TipKorisnika tipKorisnika = stariKorisnikSistema.TipKorisnika;
                var stareVrijednosti = JsonConvert.SerializeObject(stariKorisnikSistema);

                KorisnikSistema noviKorisnik = _mapper.Map<KorisnikSistema>(korisnikSistema);
                _mapper.Map(noviKorisnik, stariKorisnikSistema);

                stariKorisnikSistema.TipKorisnika = tipKorisnika;

                await _korisnikSistemaRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "UpdateKorisnikSistema", $"Korisnik sistema sa id-em {korisnikSistema} je uspešno izmenjen. Stare vrednosti su: {stareVrijednosti}");

                return Ok(_mapper.Map<KorisnikSistemaDto>(noviKorisnik));

            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateKorisnikSistema", $"Greška prilikom izmene korisnika sistema sa id-em {korisnikSistema}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }

        /// <summary>
        /// Vrši brisanje korisnika sistema na osnovu unetog id-a
        /// </summary>
        /// <param name="korisnikSistemaId">Id korisnika sistema</param>
        /// <returns></returns>
        /// <response code="200">Uspešno obrisan korisnik sistema</response>
        /// <response code="404">Nije pronađen korisnik sistema na osnovu unetog id-a</response>
        /// <response code="500">Desila se greška prilikom brisanja korisnika sistema</response>
        [HttpDelete("{korisnikSistemaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteKorisnikSistema(Guid korisnikSistemaId)
        {
            try
            {
                var korisnikSistema = await _korisnikSistemaRepository.GetKorisnikSistemaById(korisnikSistemaId);

                if (korisnikSistema == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteKorisnikSistema", $"KorisnikSistema sa id-em {korisnikSistemaId} nije pronađen.");
                    return NotFound();
                }

                await _korisnikSistemaRepository.DeleteKorisnikSistema(korisnikSistemaId);
                await _korisnikSistemaRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "DeleteKorisnikSistema", $"Korisnik sistema sa id-em {korisnikSistemaId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(korisnikSistema)}");

                return Ok();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteKorisnikSistema", $"Greška prilikom brisanja korisnika sistema sa id-em {korisnikSistemaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa korisnicima sistema
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetKorisnikSistemaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
