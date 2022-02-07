using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Entities.Confirmations;
using ParcelaService.Models.DeoParcele;
using ParcelaService.Models.OtherServices;
using ParcelaService.ServiceCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    /// <summary>
    /// Kontroler za deo parcele
    /// </summary>
    [Route("api/deoParcele")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class DeoParceleController : ControllerBase
    {
        private readonly IDeoParceleRepository _deoParceleRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IServiceCall<KupacDto> _kupacServiceCall;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor kontrolera dela parcele
        /// </summary>
        /// <param name="deoParceleRepository">Repo deo parcele</param>
        /// <param name="linkGenerator">Link generator za create zahtev</param>
        /// <param name="mapper">AutoMapper</param>
        /// <param name="kupacServiceCall">Servis kupac - dobijanje kupca</param>
        /// <param name="configuration">Konfiguracija za pristup putanji ka servisu kupac</param>
        /// <param name="loggerService">Logger servis</param>
        public DeoParceleController(IDeoParceleRepository deoParceleRepository, LinkGenerator linkGenerator, IMapper mapper, IServiceCall<KupacDto> kupacServiceCall, IConfiguration configuration, ILoggerService loggerService)
        {
            _deoParceleRepository = deoParceleRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _kupacServiceCall = kupacServiceCall;
            _configuration = configuration;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve delove parcela
        /// </summary>
        /// <returns>Lista delova parcela</returns>
        /// <response code="200">Vraća listu delova parcela</response>
        /// <response code="404">Nije pronađeno ni jedan deo parcele</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, OperaterNadmetanja")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<DeoParceleDto>>> GetAllDeoParcele()
        {
            var deloviParcela = await _deoParceleRepository.GetAllDeoParcele();

            if (deloviParcela == null || deloviParcela.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllDeoParcele", "Lista delova parcela je prazna ili null.");
                return NoContent();
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            //Komunikacija sa servisom kupac
            var deloviParcelaDto = new List<DeoParceleDto>();
            string url = _configuration["Services:KupacServices"];
            foreach (var deoParcele in deloviParcela)
            {
                var deoParceleDto = _mapper.Map<DeoParceleDto>(deoParcele);
                if (deoParcele.KupacId is not null)
                {
                    var kupacDto = await _kupacServiceCall.SendGetRequestAsync(url + "kupac/" + deoParcele.KupacId, token);
                    if (kupacDto is not null)
                        deoParceleDto.Kupac = kupacDto;
                }
                deloviParcelaDto.Add(deoParceleDto);
            }

            await _loggerService.Log(LogLevel.Information, "GetAllDeoParcele", "Lista delova parcela je uspešno vraćena.");

            return Ok(deloviParcelaDto);

        }

        /// <summary>
        /// Vraća jedan deo parcele na osnovu ID-a
        /// </summary>
        /// <param name="deoParceleId">ID dela parcele</param>
        /// <returns>Deo parcele</returns>
        /// <response code="200">Vraća traženi deo parcele</response>
        /// <response code="404">Nije pronađen deo parcele za uneti ID</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, OperaterNadmetanja")]
        [HttpGet("{deoParceleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeoParceleDto>> GetDeoParcele(Guid deoParceleId)
        {
            var deoParcele = await _deoParceleRepository.GetDeoParceleById(deoParceleId);

            if (deoParcele == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetDeoParcele", $"Deo parcele sa id-em {deoParceleId} je null.");
                return NotFound();
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string url = _configuration["Services:KupacService"];
            var deoParceleDto = _mapper.Map<DeoParceleDto>(deoParcele);
            if (deoParcele.KupacId is not null)
            {
                var kupacDto = _kupacServiceCall.SendGetRequestAsync(url + "kupac/" + deoParcele.KupacId, token).Result;
                if (kupacDto is not null)
                    deoParceleDto.Kupac = kupacDto;
            }
            await _loggerService.Log(LogLevel.Information, "GetDeoParcele", $"Deo parcele sa id-em {deoParceleId} je uspešno vraćen.");

            return Ok(deoParceleDto);
        }

        /// <summary>
        /// Kreira novi deo parcele
        /// </summary>
        /// <param name="deoParcele">Model deo parcele</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog dela parcele \
        /// POST /api/deoParcele \
        /// {   
        ///}
        /// </remarks>
        /// <returns>Potvrda o kreiranju dela parcele</returns>
        /// <response code="200">Vraća kreirani deo parcele</response>
        /// <response code="500">Desila se greška prilikom unosa novog dela parcele</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeoParceleConfirmationDto>> CreateDeoParcele([FromBody] DeoParceleCreationDto deoParcele)
        {
            try
            {
                DeoParcele mapiraniDeoParcele = _mapper.Map<DeoParcele>(deoParcele);
                DeoParceleConfirmation noviDeoParcele = await _deoParceleRepository.CreateDeoParcele(mapiraniDeoParcele);
                await _deoParceleRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetDeoParcele", "DeoParcele", new { deoParceleId = noviDeoParcele.DeoParceleId });

                await _loggerService.Log(LogLevel.Information, "CreateDeoParcele", $"Deo parcele sa vrednostima: {JsonConvert.SerializeObject(deoParcele)} je uspešno kreiran.");

                return Created(lokacija, _mapper.Map<DeoParceleConfirmationDto>(noviDeoParcele));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateDeoParcele", $"Greška prilikom unosa dela parcele sa vrednostima: {JsonConvert.SerializeObject(deoParcele)}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa dela parcele.");
            }
        }

        /// <summary>
        /// Izmena dela parcele
        /// </summary>
        /// <param name="deoParcele">Model deo parcele</param>
        /// <returns>Potvrda o izmeni dela parcele</returns>
        /// <response code="200">Izmenjeni deo parcele</response>
        /// <response code="404">Nije pronađen deo parcele za uneti ID</response>
        /// <response code="500">Serverska greška tokom izmene dela parcele</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeoParceleDto>> UpdateDeoParcele(DeoParceleUpdateDto deoParcele)
        {
            try
            {
                var stariDeoParcele = await _deoParceleRepository.GetDeoParceleById(deoParcele.DeoParceleId);

                if(stariDeoParcele == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateDeoParcele", $"Deo parcele sa id-em {deoParcele.DeoParceleId} nije pronađen.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(stariDeoParcele);

                DeoParcele noviDeoParcele = _mapper.Map<DeoParcele>(deoParcele);

                _mapper.Map(noviDeoParcele, stariDeoParcele);
                await _deoParceleRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "UpdateDeoParcele", $"Deo parcele sa id-em {deoParcele.DeoParceleId} je uspešno izmenjen. Stare vrednosti su: {stareVrednosti}");

                return Ok(_mapper.Map<DeoParceleDto>(stariDeoParcele));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateDeoParcele", $"Greška prilikom izmene dela parcele sa id-em {deoParcele.DeoParceleId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene dela parcele.");
            }
        }

        /// <summary>
        /// Brisanje dela parcele na osnovu ID-a
        /// </summary>
        /// <param name="deoParceleId">ID dela parcele</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Deo parcele je uspešno obrisan</response>
        /// <response code="404">Nije pronađeno deo parcele za uneti ID</response>
        /// <response code="500">Serverska greška tokom brisanja dela parcele</response>
        [Authorize(Roles = "Administrator, Superuser, OperaterNadmetanja")]
        [HttpDelete("{deoParceleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDeoParcele(Guid deoParceleId)
        {
            try
            {
                var deoParcele = await _deoParceleRepository.GetDeoParceleById(deoParceleId);

                if(deoParcele == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteDeoParcele", $"Deo parcele sa id-em {deoParceleId} nije pronađen.");
                    return NotFound();
                }

                await _deoParceleRepository.DeleteDeoParcele(deoParceleId);
                await _deoParceleRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "DeleteDeoParcele", $"Deo parcele sa id-em {deoParceleId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(deoParcele)}");

                return NoContent();
            }
            catch(Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteDeoParcele", $"Greška prilikom brisanja dela parcele sa id-em {deoParceleId}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja dela parcele.");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa delom parcele
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetDeoParceleOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE, HEAD");
            return Ok();
        }
    }
}

