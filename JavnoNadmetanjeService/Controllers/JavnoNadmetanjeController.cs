﻿using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;
using JavnoNadmetanjeService.Models.Other;
using JavnoNadmetanjeService.ServiceCalls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Controllers
{
    /// <summary>
    /// Kontroler za javno nadmetanje
    /// </summary>
    [Route("api/javnoNadmetanje")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class JavnoNadmetanjeController : ControllerBase
    {
        private readonly IJavnoNadmetanjeRepository _javnoNadmetanjeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IServiceCall<AdresaDto> _adresaService;
        private readonly IConfiguration _configuration;

        public JavnoNadmetanjeController(IJavnoNadmetanjeRepository javnoNadmetanjeRepository, LinkGenerator linkGenerator, IMapper mapper, IServiceCall<AdresaDto> adresaService, IConfiguration configuration)
        {
            _javnoNadmetanjeRepository = javnoNadmetanjeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _adresaService = adresaService;
            _configuration = configuration;
        }

        /// <summary>
        /// Vraća sva javna nadmetanja
        /// </summary>
        /// <returns>Lista javnih nadmetanja</returns>
        /// <response code="200">Vraća listu javnih nadmetanja</response>
        /// <response code="404">Nije pronađeno ni jedno javno nadmetanje</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<JavnoNadmetanjeDto>>> GetAllJavnoNadmetanje()
        {
            var javnaNadmetanja = await _javnoNadmetanjeRepository.GetAllJavnoNadmetanje();

            if (javnaNadmetanja == null || javnaNadmetanja.Count == 0)
            {
                return NoContent();
            }

            //Adresa mikroservis komunikacija - trenutno je preko mock-a, implementiran je i pravi nacin samo sad nema tog mikroservisa jos uvek, samo treba promeniti u startup-u
            var javnaNadmetanjaDto = new List<JavnoNadmetanjeDto>();
            string url = _configuration["Services:AdresaService"];
            foreach (var javnoNad in javnaNadmetanja)
            {
                var javnoNadDto = _mapper.Map<JavnoNadmetanjeDto>(javnoNad);
                if (javnoNad.AdresaId is not null)
                {
                    var adresaDto = _adresaService.SendGetRequestAsync(url + javnoNad.AdresaId).Result;
                    javnoNadDto.Adresa = adresaDto.Ulica + " " + adresaDto.Broj + " " + adresaDto.Mesto + ", " + adresaDto.Drzava;
                }
                javnaNadmetanjaDto.Add(javnoNadDto);
            }

            return Ok(javnaNadmetanjaDto);
        }

        /// <summary>
        /// Vraća jedno javno nadmetanje na osnovu ID-a
        /// </summary>
        /// <param name="javnoNadmetanjeId">ID javnog nadmetanja</param>
        /// <returns>Javno nadmetanje</returns>
        /// <response code="200">Vraća traženo javno nadmetanje</response>
        /// <response code="404">Nije pronađeno javno nadmetanje za uneti ID</response>
        [HttpGet("{javnoNadmetanjeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JavnoNadmetanjeDto>> GetJavnoNadmetanje(Guid javnoNadmetanjeId)
        {
            var javnoNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanjeId);

            if (javnoNadmetanje == null)
            {
                return NotFound();
            }

            string url = _configuration["Services:AdresaService"];
            var javnoNadmetanjeDto = _mapper.Map<JavnoNadmetanjeDto>(javnoNadmetanje);
            if (javnoNadmetanje.AdresaId is not null)
            {
                var adresaDto = _adresaService.SendGetRequestAsync(url + javnoNadmetanje.AdresaId).Result;
                javnoNadmetanjeDto.Adresa = adresaDto.Ulica + " " + adresaDto.Broj + " " + adresaDto.Mesto + ", " + adresaDto.Drzava;
            }

            return Ok(javnoNadmetanjeDto);
        }

        /// <summary>
        /// Kreira novo javno nadmetanje
        /// </summary>
        /// <param name="javnoNadmetanje">Model javnog nadmetanja</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog javnog nadmetanja \
        /// POST /api/javnoNadmetanje \
        /// {  \ 
        ///     "PocetnaCenaHektar": 550.00000000, \
        ///     "PeriodZakupa": 5, \ 
        ///     "IzlicitiranaCena": 750, \
        ///     "Krug": 1, \
        ///     "Izuzeto": false, \
        ///     "StatusId": "3B7EE65F-EB68-4A32-AE69-DF7FDF463188", \
        ///     "TipId": "D6D56B98-3672-4BDB-A0CB-E916FFE053C8", \
        ///     "KupacId": "febd1c29-90e7-40c2-97f3-1e88495fe98d", \
        ///     "AdresaId": "37371ef6-4f25-48b3-9bf2-fe72a81f88d2" \
        ///} \
        /// </remarks>
        /// <returns>Potvrda o kreiranju javnog nadmetanja</returns>
        /// <response code="200">Vraća kreirano javno nadmetanje</response>
        /// <response code="500">Desila se greška prilikom unosa novog javnog nadmetanja</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JavnoNadmetanjeConfirmationDto>> CreateJavnoNadmetanje([FromBody] JavnoNadmetanjeCreationDto javnoNadmetanje)
        {
            try
            {
                JavnoNadmetanjeConfirmation novoNadmetanje = await _javnoNadmetanjeRepository.CreateJavnoNadmetanje(_mapper.Map<JavnoNadmetanje>(javnoNadmetanje));
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetJavnoNadmetanje", "JavnoNadmetanje", new { javnoNadmetanjeId = novoNadmetanje.JavnoNadmetanjeId });

                return Created(lokacija, _mapper.Map<JavnoNadmetanjeConfirmationDto>(novoNadmetanje));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom unosa javnog nadmetanja");
            }
        }

        /// <summary>
        /// Izmena javnog nadmetanja
        /// </summary>
        /// <param name="javnoNadmetanje">Model javno nadmetanje</param>
        /// <returns>Potvrda o izmeni javnog nadmetanja</returns>
        /// <response code="200">Izmenjeno javno nadmetanje</response>
        /// <response code="404">Nije pronađeno javno nadmetanje za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene javnog nadmetanja</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JavnoNadmetanjeDto>> UpdateJavnoNadmetanje(JavnoNadmetanjeUpdateDto javnoNadmetanje)
        {
            try
            {
                var staroNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanje.JavnoNadmetanjeId);

                if (staroNadmetanje == null)
                {
                    return NotFound();
                }

                JavnoNadmetanje novoNadmetanje = _mapper.Map<JavnoNadmetanje>(javnoNadmetanje);

                _mapper.Map(novoNadmetanje, staroNadmetanje);
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                return Ok(_mapper.Map<JavnoNadmetanjeDto>(staroNadmetanje));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene javnog nadmetanja");
            }
        }

        /// <summary>
        /// Brisanje javnog nadmetanja na osnovu ID-a
        /// </summary>
        /// <param name="javnoNadmetanjeId">ID javnog nadmetanja</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Javno nadmetanje je uspešno obrisano</response>
        /// <response code="404">Nije pronađeno javno nadmetanje za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja javnog nadmetanja</response>
        [HttpDelete("{javnoNadmetanjeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteJavnoNadmetanje(Guid javnoNadmetanjeId)
        {
            try
            {
                var javnoNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanjeId);

                if (javnoNadmetanje == null)
                {
                    return NotFound();
                }

                await _javnoNadmetanjeRepository.DeleteJavnoNadmetanje(javnoNadmetanjeId);
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja javnog nadmetanja");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa javnim nadmetanjima
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetJavnoNadmetanjeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
