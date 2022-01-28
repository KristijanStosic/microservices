﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OvlascenoLiceService.Data.Interfaces;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using OvlascenoLiceService.Models.OtherServices;
using OvlascenoLiceService.Models.OvlascenoLice;
using OvlascenoLiceService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OvlascenoLiceService.Controllers
{
    /// <summary>
    /// Kontroler za ovlašćeno lice
    /// </summary>
    [Route("api/ovlascenoLice")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class OvlascenoLiceController : ControllerBase
    {
        private readonly IOvlascenoLiceRepository _ovlascenoLiceRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IServiceCall<AdresaDto> _adresaServiceCall;
        private readonly IServiceCall<DrzavaDto> _drzavaServiceCall;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor kontrolera ovlašćenog lica - DI
        /// </summary>
        /// <param name="ovlascenoLiceRepository">Repo ovlašćeno lice</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="adresaServiceCall">Servis adresa - dobijanje adrese</param>
        /// <param name="drzavaServiceCall">Servis adresa - dobijanje drzave</param>
        /// <param name="configuration">Konfiguracija za pristup putanji ka servisu adresa</param>
        /// <param name="loggerService">Logger servis</param>
        public OvlascenoLiceController(IOvlascenoLiceRepository ovlascenoLiceRepository, LinkGenerator linkGenerator, IMapper mapper, IServiceCall<AdresaDto> adresaServiceCall, IServiceCall<DrzavaDto> drzavaServiceCall, IConfiguration configuration, ILoggerService loggerService)
        {
            _ovlascenoLiceRepository = ovlascenoLiceRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _adresaServiceCall = adresaServiceCall;
            _drzavaServiceCall = drzavaServiceCall;
            _configuration = configuration;
            _loggerService = loggerService;
        }


        /// <summary>
        /// Vraća sva ovlašćena lica
        /// </summary>
        /// <returns>Lista ovlašćenih lica</returns>
        /// <response code="200">Vraća listu ovlašćenih lica</response>
        /// <response code="404">Nije pronađeno ni jedno ovlašćeno lice</response>
        [HttpGet]
        [HttpHead] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<OvlascenoLiceDto>>> GetAllOvlascenoLice(string ime, string prezime)
        {
            var ovlascenaLica = await _ovlascenoLiceRepository.GetAllOvlascenoLice(ime, prezime);

            if (ovlascenaLica == null || ovlascenaLica.Count == 0)
            {
                await _loggerService.Log(LogLevel.Error, "GetAllOvlascenoLice", "Lista ovlašćenih lica je prazna ili null.");
                return NoContent();
            }

            //Komunikacija sa servisom adresa i preuzimanje Adrese ili Drzave
            var ovlascenaLicaDto = new List<OvlascenoLiceDto>();
            string url = _configuration["Services:AdresaService"];
            foreach(var ovlascenoLice in ovlascenaLica)
            {
                var ovlascenoLiceDto = _mapper.Map<OvlascenoLiceDto>(ovlascenoLice);
                if(ovlascenoLice.AdresaId is not null)
                {
                    var adresaDto = _adresaServiceCall.SendGetRequestAsync(url + "adresa/" + ovlascenoLice.AdresaId).Result;
                    ovlascenoLiceDto.Stanovanje = adresaDto.Ulica + " " + adresaDto.Broj + " " + adresaDto.Mesto + ", " + adresaDto.Drzava;
                }
                else if (ovlascenoLice.DrzavaId is not null)
                {
                    var drzavaDto = _drzavaServiceCall.SendGetRequestAsync(url + "drzava/" + ovlascenoLice.DrzavaId).Result;
                    ovlascenoLiceDto.Stanovanje = drzavaDto.NazivDrzave;
                }
                ovlascenaLicaDto.Add(ovlascenoLiceDto);
            }

            await _loggerService.Log(LogLevel.Information, "GetAllOvlascenoLice", "Lista ovlašćenih lica je uspešno vraćena.");

            return Ok(ovlascenaLicaDto);
        }

        /// <summary>
        /// Vraća jedno ovlašćeno lice na osnovu ID-a
        /// </summary>
        /// <param name="ovlascenoLiceId">ID ovlašćenog lica</param>
        /// <returns>Ovlašćeno lice</returns>
        /// <response code="200">Vraća traženo ovlašćeno lice</response>
        /// <response code="404">Nije pronađeno ovlašćeno lice za uneti ID</response>
        [HttpGet("{ovlascenoLiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OvlascenoLiceDto>> GetOvlascenoLice(Guid ovlascenoLiceId)
        {
            var ovlascenoLice = await _ovlascenoLiceRepository.GetOvlascenoLiceById(ovlascenoLiceId);

            if (ovlascenoLice == null)
            {
                await _loggerService.Log(LogLevel.Error, "GetOvlascenoLice", "Ovlašćeno lice je null.");
                return NotFound();
            }

            string url = _configuration["Services:AdresaService"];
            var ovlascenoLiceDto = _mapper.Map<OvlascenoLiceDto>(ovlascenoLice);
            if (ovlascenoLice.AdresaId is not null)
            {
                var adresaDto = _adresaServiceCall.SendGetRequestAsync(url + "adresa/" + ovlascenoLice.AdresaId).Result;
                ovlascenoLiceDto.Stanovanje = adresaDto.Ulica + " " + adresaDto.Broj + " " + adresaDto.Mesto + ", " + adresaDto.Drzava;
            }
            else if (ovlascenoLice.DrzavaId is not null)
            {
                var drzavaDto = _drzavaServiceCall.SendGetRequestAsync(url + "drzava/" + ovlascenoLice.DrzavaId).Result;
                ovlascenoLiceDto.Stanovanje = drzavaDto.NazivDrzave;
            }
            await _loggerService.Log(LogLevel.Information, "GetOvlascenoLice", "Ovlašćeno lice je uspešno vraćeno.");

            return Ok(ovlascenoLiceDto);
        }

        /// <summary>
        /// Kreira novo ovlašćeno lice
        /// </summary>
        /// <param name="ovlascenoLice">Model ovlašćeno lice</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog ovlašćenog lica \
        /// POST /api/ovlascenoLice \
        /// {   
        ///     "Ime": "Milan", \
        ///     "Prezime": "Milanovic", \
        ///     "JMBG": "1008987800025", \
        ///     "AdresaId": "1c989ee3-13b2-4d3b-abeb-c4e6343eace7" \
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju ovlašćenog lica</returns>
        /// <response code="200">Vraća kreirano ovlašćeno lice</response>
        /// <response code="500">Desila se greška prilikom unosa novog ovlašćenog lica</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OvlascenoLiceConfirmationDto>> CreateOvlascenoLice([FromBody] OvlascenoLiceCreationDto ovlascenoLice)
        {
            try
            {
                OvlascenoLice mapiranoLice = _mapper.Map<OvlascenoLice>(ovlascenoLice);
                OvlascenoLiceConfirmation novoLice = await _ovlascenoLiceRepository.CreateOvlascenoLice(mapiranoLice);
                await _ovlascenoLiceRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetOvlascenoLice", "OvlascenoLice", new { ovlascenoLiceId = novoLice.OvlascenoLiceId });

                await _loggerService.Log(LogLevel.Information, "CreateOvlascenoLice", "Ovlašćeno lice je uspešno kreirano.");

                return Created(lokacija, _mapper.Map<OvlascenoLiceConfirmationDto>(novoLice));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateOvlascenoLice", "Greška prilikom unosa ovlašćenog lica.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa ovlašćenog lica.");
            }
        }


        /// <summary>
        /// Izmena ovlašćenog lica
        /// </summary>
        /// <param name="ovlascenoLice">Model ovlašćenog lica</param>
        /// <returns>Potvrda o izmeni ovlašćenog lica</returns>
        /// <response code="200">Izmenjeno ovlašćeno lice</response>
        /// <response code="404">Nije pronađeno ovlašćeno lice za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene ovlašćenog lica</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<ActionResult<OvlascenoLiceDto>> UpdateOvlascenoLice(OvlascenoLiceUpdateDto ovlascenoLice)
        {
            try
            {
                var staroLice = await _ovlascenoLiceRepository.GetOvlascenoLiceById(ovlascenoLice.OvlascenoLiceId);

                if (staroLice == null)
                {
                    await _loggerService.Log(LogLevel.Error, "UpdateOvlascenoLice", "Ovlašćeno lice je null.");
                    return NotFound();
                }

                OvlascenoLice novoLice = _mapper.Map<OvlascenoLice>(ovlascenoLice);

                _mapper.Map(novoLice, staroLice);
                await _ovlascenoLiceRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "UpdateOvlascenoLice", "Ovlašćeno lice je uspešno izmenjeno.");

                return Ok(_mapper.Map<OvlascenoLiceDto>(staroLice));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateOvlascenoLice", "Greška prilikom izmene ovlašćenog lica.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene ovlašćenog lica.");
            }
        }

        /// <summary>
        /// Brisanje ovlašćenog lica na osnovu ID-a
        /// </summary>
        /// <param name="ovlascenoLiceId">ID ovlašćenog lica</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Ovlašćeno lice je uspešno obrisano</response>
        /// <response code="404">Nije pronađeno ovlašćeno lice za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja ovlašćenog lica</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{ovlascenoLiceId}")]
        public async Task<IActionResult> DeleteOvlascenoLice(Guid ovlascenoLiceId)
        {
            try
            {
                var ovlascenoLice = await _ovlascenoLiceRepository.GetOvlascenoLiceById(ovlascenoLiceId);

                if (ovlascenoLice == null)
                {
                    await _loggerService.Log(LogLevel.Error, "DeleteOvlascenoLice", "Ovlašćeno lice je null.");
                    return NotFound();
                }

                await _ovlascenoLiceRepository.DeleteOvlascenoLice(ovlascenoLiceId);
                await _ovlascenoLiceRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "DeleteOvlascenoLice", "Ovlašćeno lice je uspešno obrisano.");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteOvlascenoLice", "Greška prilikom brisanja ovlašćenog lica.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja ovlašćenog lica.");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa ovlašćenim licima
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetOvlascenoLiceOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE, HEAD");
            return Ok();
        }
    }
}
