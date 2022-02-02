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
    [ApiController]
    [Route("api/adresa")]
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

        [HttpGet]
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

        [HttpGet("{adresaId}")]
        public async Task<ActionResult<AdresaDto>> GetAdresaById(Guid adresaId)
        {
            var adresa = await _adresaRepository.GetAdresaById(adresaId);

            if (adresa == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAdresaById", $"Adresa sa id-em {adresaId} nije pronađena.");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetAdresaById", $"Adresa sa id-em {adresaId} je uspešno pronađena.");
            return Ok(_mapper.Map<AdresaDto>(adresa));
        }
        [HttpPost]
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
        [HttpDelete("{adresaId}")]
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

        [HttpPut]
        public async Task<ActionResult<AdresaDto>> UpdateAdresa(AdresaUpdateDto adresaUpdate)
        {
            try
            {
                var oldAdresa = await _adresaRepository.GetAdresaById(adresaUpdate.AdresaId);
                var stareVrednosti = JsonConvert.SerializeObject(oldAdresa);
                if (oldAdresa == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateAdresa", $"Adresa sa id-em {adresaUpdate.AdresaId} nije pronađena.");
                    return NotFound();
                }

                Adresa adresa = _mapper.Map<Adresa>(adresaUpdate);
                
                _mapper.Map(adresa, oldAdresa);

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


        [HttpOptions]
        public IActionResult GetAdresaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
