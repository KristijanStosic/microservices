using AutoMapper;
using LicnostService.Data;
using LicnostService.Entities;
using LicnostService.Entities.Confirmations;
using LicnostService.Models.Komisija;
using LicnostService.Models.OtherModels;
using LicnostService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Controllers
{

    /// <summary>
    /// Kontroler za komisiju
    /// </summary>
    [ApiController]
    [Route("api/komisija")]
    [Produces("application/json", "application/xml")]
    public class KomisijaController : ControllerBase
    {
        private readonly IKomisijaRepository _komisijaRepository;
        private readonly ILicnostRepository _licnostRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;
        private readonly IServiceCall<DokumentDto> _dokumentService;
        private readonly IConfiguration _configuration;

        public KomisijaController(IKomisijaRepository komisijaRepository, ILicnostRepository licnostRepository, LinkGenerator linkGenerator, IMapper mapper, ILoggerService loggerService, IServiceCall<DokumentDto> dokumentService, IConfiguration configuration)
        {
            _komisijaRepository = komisijaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _loggerService = loggerService;
            _dokumentService = dokumentService;
            _configuration = configuration;
            _licnostRepository = licnostRepository;
        }

        /// <summary>
        /// Vraca sve komisije
        /// </summary>
        /// <param name="nazivKomisije"></param>
        /// <returns>Lista komisija</returns>
        /// <response code="200">Vraća listu komisija</response>
        /// <response code="404">Nije pronađeno ni jedna komisija</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<KomisijaDto>>> GetAllKomisije(string nazivKomisije)
        {
            var komisije = await _komisijaRepository.GetAllKomisije(nazivKomisije);

            if (komisije == null || komisije.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllKomisije", "Lista komisija je prazna ili null.");
                return NoContent();

            }
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var komisijeDto = new List<KomisijaDto>();
            string url = _configuration["Services:DokumentService"];
            foreach (var komisija in komisije)
            {
                var komisijaDto = _mapper.Map<KomisijaDto>(komisija);
                if (komisija.DokumentId is not null)
                {
                    var dokumentDto = await _dokumentService.SendGetRequestAsync(url + komisija.DokumentId, token);
                    if (dokumentDto is not null)
                        komisijaDto.Dokument = dokumentDto;
                }
                komisijeDto.Add(komisijaDto);
            }

            await _loggerService.Log(LogLevel.Information, "GetAllKomisije", "Lista komisija je uspešno vraćena.");

            return Ok(komisijeDto);
        }


        /// <summary>
        /// Vraća jednu komisiju na osnovu ID-a
        /// </summary>
        /// <param name="komisijaId">ID komisije</param>
        /// <returns>Komisija</returns>
        /// <response code="200">Vraća traženu komisiju</response>
        /// <response code="404">Nije pronađena komisija za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpGet("{komisijaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<KomisijaDto>> GetKomisija(Guid komisijaId)
        {
            var komisija = await _komisijaRepository.GetKomisijeById(komisijaId);

            if (komisija == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetKomisija", $"Komisija sa id-em {komisijaId} nije pronađena.");
                return NotFound();
            }
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            string url = _configuration["Services:DokumentService"];
            var komisijaDto = _mapper.Map<KomisijaDto>(komisija);
            if (komisija.DokumentId is not null)
            {
                var dokumentDto = await _dokumentService.SendGetRequestAsync(url + komisija.DokumentId, token);
                if (dokumentDto is not null)
                    komisijaDto.Dokument = dokumentDto;
            }

            await _loggerService.Log(LogLevel.Information, "GetKomisija", $"Komisija sa id-em {komisijaId} je uspešno vraćena.");

            return Ok(komisijaDto);
        }
        /// <summary>
        /// Kreira novu komisiju
        /// </summary>
        /// <param name="komisija">Model komisije</param>
        /// <returns>Potvrda o kreiranju komisije</returns>
        /// <response code="201">Vraća kreiranu komisiju</response>
        /// <response code="500">Desila se greška prilikom unosa nove komisije</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KomisijaConfirmationDto>> CreateKomisija([FromBody] KomisijaCreateDto komisija)
        {
            try
            {
                var clanovi = new List<Licnost>();
                if (komisija.ClanoviKomisije is not null)
                {
                    foreach (var clan in komisija.ClanoviKomisije)
                    {
                        Licnost tempClan = await _licnostRepository.GetLicnostById(clan);
                        if (tempClan != null)
                            clanovi.Add(tempClan);
                    }
                }

                Komisija mapiranaKomisija = _mapper.Map<Komisija>(komisija);
                mapiranaKomisija.ClanoviKomisije = clanovi;

                KomisijaConfirmation novaKomisija = await _komisijaRepository.CreateKomisija(mapiranaKomisija);

                string lokacija = _linkGenerator.GetPathByAction("GetKomisija", "Komisija", new { komisijaId = novaKomisija.KomisijaId });

                await _loggerService.Log(LogLevel.Information, "CreateKomisija", $"Komisija sa vrednostima: {JsonConvert.SerializeObject(komisija)} je uspešno kreirana.");

                return Created(lokacija, _mapper.Map<KomisijaConfirmationDto>(novaKomisija));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateKomisija", $"Greška prilikom unosa komisije sa vrednostima: {JsonConvert.SerializeObject(komisija)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa komisije.");
            }
        }
        /// <summary>
        /// Izmena komisije
        /// </summary>
        /// <param name="komisija">Model komisije</param>
        /// <returns>Potvrda o izmeni komisije</returns>
        /// <response code="200">Izmenjena komisija</response>
        /// <response code="404">Nije pronađena komisija za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene komisije</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<KomisijaConfirmationDto>> UpdateKomisija(KomisijaUpdateDto komisija)
        {
            try
            {
                var staraKomisija = await _komisijaRepository.GetKomisijeById(komisija.KomisijaId);

                if (staraKomisija == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateKomisija", $"Komisija sa id-em {komisija.KomisijaId} nije pronađena.");
                    return NotFound();
                }

                var stariNaziv = staraKomisija.NazivKomisije;

                Komisija novaKomisija = _mapper.Map<Komisija>(komisija);

                _mapper.Map(novaKomisija, staraKomisija);

                var clanovi = new List<Licnost>();
                if (komisija.ClanoviKomisije is not null)
                {
                    foreach (var clan in komisija.ClanoviKomisije)
                    {
                        Licnost tempClan = await _licnostRepository.GetLicnostById(clan);
                        if (tempClan != null)
                            clanovi.Add(tempClan);
                    }
                }
                staraKomisija.ClanoviKomisije = clanovi;
                staraKomisija.PredsednikKomisije = await _licnostRepository.GetLicnostById(staraKomisija.PredsednikKomisijeId);

                await _komisijaRepository.UpdateKomisija(novaKomisija);
                await _loggerService.Log(LogLevel.Information, "UpdateKomisija", $"Komisija sa id-em {komisija.KomisijaId} je uspešno izmenjena. Stare vrednosti su: {stariNaziv}");

                return Ok(_mapper.Map<KomisijaConfirmationDto>(staraKomisija));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateKomisija", $"Greška prilikom izmene komisije sa id-em {komisija.KomisijaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene komisije.");
            }
        }

        /// <summary>
        /// Brisanje komisije na osnovu ID-a
        /// </summary>
        /// <param name="komisijaId">ID komisije</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Komisija je uspešno obrisano</response>
        /// <response code="404">Nije pronađena komisija za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja komisije</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpDelete("{komisijaId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteKomisija(Guid komisijaId)
        {
            try
            {
                var komisija = await _komisijaRepository.GetKomisijeById(komisijaId);

                if (komisija == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteKomisija", $"Komisija sa id-em {komisijaId} nije pronađena.");
                    return NotFound();
                }

                await _komisijaRepository.DeleteKomisija(komisijaId);

                return Ok();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteKomisija", $"Greška prilikom brisanja komisije sa id-em {komisijaId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja komisije");
            }
        }


        /// <summary>
        /// Vraća opcije za rad sa komisijama
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpOptions]
        public IActionResult GetKomisijaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
