using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.KontaktOsoba;
using KupacService.ServiceCalls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Controllers
{
    /// <summary>
    /// Kontroler za kontakt osobe
    /// </summary>
    [ApiController]
    [Route("api/kontaktOsoba")]
    [Produces("application/json", "application/xml")]
    public class KontaktOsobaController : ControllerBase
    {
        private readonly IKontaktOsobaRepository _kontaktOsobaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public KontaktOsobaController(IKontaktOsobaRepository kontaktOsobaRepository,LinkGenerator linkGenerator,IMapper mapper,ILoggerService loggerService)
        {
            this._kontaktOsobaRepository = kontaktOsobaRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._loggerService = loggerService;
        }
        /// <summary>
        /// Vraća listu kontakt osoba na osnovu zadatih filtera
        /// </summary>
        /// <param name="ime">Ime kontakt osobe</param>
        /// <param name="prezime">Prezime kontakt osobe</param>
        /// <returns>Lista kontakt osoba</returns>
        /// <response code="200">Uspešno vraćena lista kontakt osoba</response>
        /// <response code="204">Nije pronađena nijedna kontakt osoba</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<KontaktOsobaDto>>> GetKontaktOsobe(string ime, string prezime)
        {
            var kontaktOsobe = await _kontaktOsobaRepository.GetKontaktOsoba(ime, prezime);

            if(kontaktOsobe == null || kontaktOsobe.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKontaktOsobe", "Lista kontak osobi je prazna ili null.");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetKontaktOsobe", "Lista kontakt osobi je uspešno vraćena.");
            return Ok(_mapper.Map<List<KontaktOsobaDto>>(kontaktOsobe));
        }
        /// <summary>
        /// Vraća kontakt osobu na osnovu unetog id-a
        /// </summary>
        /// <param name="kontaktOsobaId">Id kontakt osobe</param>
        /// <returns>Konatk osoba</returns>
        /// <response code="200">Uspešno vraćena kontakt osoba</response>
        /// <response code="404">Nije pronađena kontakt osoba sa zadatim id-em</response>
        [HttpGet("{kontaktOsobaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<KontaktOsobaDto>> GetKontaktOsobaById(Guid kontaktOsobaId)
        {
            var kontaktOsoba = await _kontaktOsobaRepository.GetKontaktOsobaById(kontaktOsobaId);

            if(kontaktOsoba == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKontaktOsobaById", $"Kontakt osoba sa id-em {kontaktOsobaId} nije pronađena.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetKontaktOsobaById", $"Kontakt osoba sa id-em {kontaktOsobaId} je uspešno vraćena.");
            return Ok(_mapper.Map<KontaktOsobaDto>(kontaktOsoba));
        }
        /// <summary>
        /// Kreira novu kontakt osobu
        /// </summary>
        /// <param name="kontaktOsoba">Kontakt osoba</param>
        /// <returns>Potvrdu o kreiranoj kontakt osobi</returns>
        /// <remarks>
        /// Primer kreiranja kontakt osobe \
        /// POST api/kontakt osoba \
        /// { \
        ///  "ime": "Marko", \
        ///  "prezime": "Marković",\
        ///  "telefon": "06632452345",\
        ///  "funkcija": "Funkcija"\
        ///}
        /// </remarks>
        /// <response code="201">Uspešno kreirana kontakt osoba</response>
        /// <response code="500">Desila se greška prilikom kreiranja nove kontakt osobe</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KontaktOsobaDto>> CreateKontaktOsoba(KontaktOsobaCreateDto kontaktOsoba)
        {
            try
            {
                KontaktOsoba newKontaktOsoba = _mapper.Map<KontaktOsoba>(kontaktOsoba);

                await _kontaktOsobaRepository.CreateKontaktOsoba(newKontaktOsoba);
                await _kontaktOsobaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "CreateJavnoNadmetanje", $"Kontakt osoba sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<KontaktOsobaDto>(kontaktOsoba))} je uspešno kreirana.");
                string link = _linkGenerator.GetPathByAction("GetKontaktOsobaById", "KontaktOsoba", new { kontaktOsobaId = newKontaktOsoba.KontaktOsobaId });
                return Created(link, kontaktOsoba);
            }
            catch(Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "CreateKontaktOsoba", $"Greška prilikom unosa kontakt osobe sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<KontaktOsobaDto>(kontaktOsoba))}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }
        /// <summary>
        /// Vrši ažuriranje kontakt osobe
        /// </summary>
        /// <param name="kontaktOsobaUpdate">Kontak osoba</param>
        /// <returns>Kontakt osobu</returns>
        /// <remarks>
        /// Primer ažuriranja kontakt osobe \
        /// PUT api/kontakt osoba \
        /// { \
        ///  "kontaktOsobaId":"da2197a4-891f-4a40-a1f2-313962701627", \
        ///  "ime": "Marko", \
        ///  "prezime": "Marković",\
        ///  "telefon": "06632452345",\
        ///  "funkcija": "Funkcija"\
        ///}
        /// </remarks>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KontaktOsobaDto>> UpdateKontaktOsoba(KontaktOsobaUpdateDto kontaktOsobaUpdate)
        {
            try
            {
                var oldKontaktOsoba = await _kontaktOsobaRepository.GetKontaktOsobaById(kontaktOsobaUpdate.KontaktOsobaId);

                if(oldKontaktOsoba == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateKontaktOsoba", $"Kontakt osoba  sa id-em {kontaktOsobaUpdate.KontaktOsobaId} nije pronađena.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(_mapper.Map<KontaktOsobaDto>(oldKontaktOsoba));
                KontaktOsoba kontaktOsoba = _mapper.Map<KontaktOsoba>(kontaktOsobaUpdate);

                _mapper.Map(kontaktOsoba, oldKontaktOsoba);

                await _kontaktOsobaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateKontaktOsoba", $"Kontakt osoba sa id-em {kontaktOsoba.KontaktOsobaId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");
                return Ok(_mapper.Map<KontaktOsobaDto>(kontaktOsoba));


            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateKontaktOsoba", $"Greška prilikom izmene kontakt osobe sa id-em {kontaktOsobaUpdate.KontaktOsobaId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }
        }
        /// <summary>
        /// Vrši brisanje kontakt osobe na osnovu unetog id-a
        /// </summary>
        /// <param name="kontaktOsobaId">Id kontakt osobe</param>
        /// <returns></returns>
        /// <response code="200">Uspešno obrisana kontakt osoba</response>
        /// <response code="404">Nije pronađena kontakt osoba na osnovu unetog id-a</response>
        /// <response code="500" >Desila se greška prilikom brisanja kontakt osobe</response>
        [HttpDelete("{kontaktOsobaId}")]
        public async Task<IActionResult> DeleteKontaktOsoba(Guid kontaktOsobaId)
        {
            try
            {
                var kontaktOsoba = await _kontaktOsobaRepository.GetKontaktOsobaById(kontaktOsobaId);

                if(kontaktOsoba == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteKontaktOsoba", $"Kontakt osoba sa id-em {kontaktOsobaId} nije pronađena.");
                    return NotFound();
                }

                await _kontaktOsobaRepository.DeleteKontaktOsoba(kontaktOsobaId);
                await _kontaktOsobaRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "DeleteJavnoNadmetanje", $"Kontakt osoba sa id-em {kontaktOsobaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(_mapper.Map<KontaktOsobaDto>(kontaktOsoba))}");
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        /// <summary>
        /// Opcije za rad sa kontakt osobama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetKontaktOsobaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }


    }
}
