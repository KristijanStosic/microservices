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
    [ApiController]
    [Route("api/kontaktOsoba")]
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

        [HttpGet]
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
        [HttpGet("{kontaktOsobaId}")]
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
        [HttpPost]
        public async Task<ActionResult<KontaktOsobaDto>> CreateKontaktOsoba(KontaktOsobaDto kontaktOsoba)
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

        [HttpPut]
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


        [HttpOptions]
        public IActionResult GetKontaktOsobaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }


    }
}
