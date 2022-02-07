using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Helpers;
using KupacService.Model.Kupac.PravnoLice;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
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

namespace KupacService.Controllers
{
    /// <summary>
    /// Kontroler za pravna lica
    /// </summary>
    [ApiController]
    [Route("api/pravnoLice")]
    [Produces("application/json", "application/xml")]
    public class PravnoLiceController: ControllerBase
    {
        private readonly IPravnoLiceRepository _pravnoLiceRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IKupacCalls _kupacCalls;
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor za kontroler
        /// </summary>
        /// <param name="pravnoLiceRepository"></param>
        /// <param name="linkGenerator"></param>
        /// <param name="mapper"></param>
        /// <param name="kupacCalls"></param>
        /// <param name="loggerService"></param>
        public PravnoLiceController(IPravnoLiceRepository pravnoLiceRepository,LinkGenerator linkGenerator,IMapper mapper,
            IKupacCalls kupacCalls,ILoggerService loggerService) 
        {
            this._pravnoLiceRepository = pravnoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._kupacCalls = kupacCalls;
            this._loggerService = loggerService;
        }

        /// <summary>
        /// Vraća list pravnih lica na osnovu unetih filtera
        /// </summary>
        /// <param name="naziv">Naziv pravnog lica</param>
        /// <param name="maticniBroj">Matični broj pravnog lica</param>
        /// <returns>Lista pravnih lica</returns>
        /// <response code="200">Uspešno vraćena lista pravnih lica</response>
        /// <response code="204">Nije pronađena nijedno pravno lice</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<PravnoLiceDto>>> GetPravnoLica([FromQuery]string naziv,[FromQuery]string maticniBroj)
        {
            var pravnaLica = await _pravnoLiceRepository.GetPravnoLice(naziv, maticniBroj);

            if(pravnaLica == null || pravnaLica.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetPravnoLica", "Lista pravnih lica je prazna ili null.");
                return NoContent();
            }

            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            List<PravnoLiceDto> pravnaLicaDto = new List<PravnoLiceDto>();
       
            foreach (var pravnoLice in pravnaLica)
            {
                PravnoLiceDto pravnoLiceDto = _mapper.Map<PravnoLiceDto>(pravnoLice);
                var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(pravnoLice, token);
                _mapper.Map(otherServicesDto, pravnoLiceDto);
                pravnaLicaDto.Add(pravnoLiceDto);
            }
            await _loggerService.Log(LogLevel.Information, "GetPravnoLica", "Lista pravnih lica je uspešno vraćena.");
            return Ok(pravnaLicaDto);

        }

        /// <summary>
        /// Vraća pravno lice na osnovu unetog id-a
        /// </summary>
        /// <param name="kupacId">Id pravnog lica</param>
        /// <returns>Pravno lice</returns>
        /// <response code="200">Uspešno vraćeno pravno lice</response>
        /// <response code="404">Nije pronađeno pravno lice sa zadatim id-em</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet("{kupacId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PravnoLiceDto>> GetPravnoLiceById(Guid kupacId)
        {
            var pravnoLice = await _pravnoLiceRepository.GetPravnoLiceById(kupacId);

            if(pravnoLice == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetPravnoLiceById", $"Pravno lice sa id-em {kupacId} nije pronađeno.");
                return NotFound();
            }
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            PravnoLiceDto pravnoLiceDto = _mapper.Map<PravnoLiceDto>(pravnoLice);

            var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(pravnoLice, token);
            _mapper.Map(otherServicesDto, pravnoLiceDto);

            await _loggerService.Log(LogLevel.Information, "GetPravnoLiceById", $"Pravno lice  sa id-em {kupacId} je uspešno vraćeno.");
            return Ok(pravnoLiceDto);
        }

        /// <summary>
        /// Kreira novo pravno lice
        /// </summary>
        /// <param name="pravnoLice">Pravno lice</param>
        /// <returns>Potvrda o kreiranom pravnom licu</returns>
        /// <remarks>
        /// Primer kreiranja pravnog lica\
        /// Post api/pravnoLice\
        /// {\
        ///"naziv": "Firma",\
        ///"maticniBroj": "124123412",\
        ///"faks":"+232345242423",\
        ///"ostvarenaPovrsina": 0,\
        ///"imaZabranu": true,\
        ///"datumPocetkaZabrane": "2022-01-30",\
        ///"duzinaTrajanjaZabraneGod": 0,\
        ///"brojTelefona": "06624325",\
        ///"brojTelefona2": "0692354235",\
        ///"email": "firma@gmail.com",\
        ///"brojRacuna": "32523525",\
        ///"kontaktOsobaId":"244FB7C4-AAB8-4EC4-8960-E48E017BAD37",\
        ///"prioriteti":["f2b8faa4-732c-4480-8b0a-34d65e483930","2578E81B-3F01-479A-B790-F52106F639F7"]\
        /// }
        /// </remarks>
        /// <response code="201">Uspešno kreirano pravno lice</response>
        /// <response code="500">Desila se greška prilikom kreiranja novog pravnog lica</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PravnoLiceConfirmDto>> CreatePravnoLice([FromBody]PravnoLiceCreateDto pravnoLice)
        {
           
                PravnoLice newPravnoLice = _mapper.Map<PravnoLice>(pravnoLice);
                
                await _pravnoLiceRepository.CreatePravnoLice(newPravnoLice);
                await _pravnoLiceRepository.SaveChangesAsync();

                string link = _linkGenerator.GetPathByAction("GetPravnoLiceById", "PravnoLice", new { kupacId = newPravnoLice.KupacId });

                await _loggerService.Log(LogLevel.Information, "CreatePravnoLice", $"Pravno lice  sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<PravnoLiceDto>(newPravnoLice))} je uspešno kreirano.");
                return Created(link, _mapper.Map<PravnoLiceDto>(newPravnoLice));
            
        }

        /// <summary>
        /// Vrši ažuriranje pravnog lica
        /// </summary>
        /// <param name="pravnoLiceUpdate">Pravno lice</param>
        /// <returns>Pravno lice</returns>
        /// <remarks>
        /// Primer ažuriranja pravnog lica\
        /// PUT api/pravnoLice\
        /// {\
        /// "kupacId":"4ba95c01-aa89-4d36-a467-c72b0fcc5b80",\
        ///"naziv": "Firma",\
        ///"maticniBroj": "124123412",\
        ///"faks":"+232345242423",\
        ///"ostvarenaPovrsina": 0,\
        ///"imaZabranu": true,\
        ///"datumPocetkaZabrane": "2022-01-30",\
        ///"duzinaTrajanjaZabraneGod": 0,\
        ///"brojTelefona": "06624325",\
        ///"brojTelefona2": "0692354235",\
        ///"email": "firma@gmail.com",\
        ///"brojRacuna": "32523525",\
        ///"kontaktOsobaId":"244FB7C4-AAB8-4EC4-8960-E48E017BAD37",\
        ///"prioriteti":["f2b8faa4-732c-4480-8b0a-34d65e483930","2578E81B-3F01-479A-B790-F52106F639F7"]\
        /// }
        /// </remarks>
        /// <response code="200">Uspešno ažurirano pravno lice</response>
        /// <response code="404">Nije pronađeno pravno lice na osnovu prosleđenog id-a</response>
        /// <response code="500">Desila se greška prilikom pravnog fizičkog lica</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PravnoLiceDto>> UpdatePravnoLice([FromBody]PravnoLiceUpdateDto pravnoLiceUpdate)
        {
            try
            {
                var oldPravnoLice = await _pravnoLiceRepository.GetPravnoLiceById(pravnoLiceUpdate.KupacId);
                
                if (oldPravnoLice == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdatePravnoLice", $"Pravno lice  sa id-em {pravnoLiceUpdate.KupacId} nije pronađeno.");
                    return NotFound();
                }
                KontaktOsoba kontaktOsoba = oldPravnoLice.KontaktOsoba;
                var stareVrednosti = JsonConvert.SerializeObject(_mapper.Map<PravnoLiceDto>(oldPravnoLice));

                _mapper.Map(pravnoLiceUpdate, oldPravnoLice);
                await _pravnoLiceRepository.UpdateManyToManyTables(oldPravnoLice);

               
                oldPravnoLice.KontaktOsoba = kontaktOsoba;
                await _pravnoLiceRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateJavnoNadmetanje", $"Pravno lice sa id-em {pravnoLiceUpdate.KupacId} je uspešno izmenjeno. Stare vrednosti su: {stareVrednosti}");
                return Ok(_mapper.Map<PravnoLiceDto>(oldPravnoLice));

            }catch(Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "UpdatePravnoLice", $"Greška prilikom izmene pravnog lica sa id-em {pravnoLiceUpdate.KupacId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }
        }

        /// <summary>
        /// Vrši brisanje pravnog lica na osnovu unetog id-a
        /// </summary>
        /// <param name="kupacId">Id pravnog lica</param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpDelete("{kupacId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePravnoLice(Guid kupacId)
        {
            try
            {
                var pravnoLice = await _pravnoLiceRepository.GetPravnoLiceById(kupacId);

                if (pravnoLice == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeletePravnoLice", $"Pravno lice  sa id-em {kupacId} nije pronađeno.");
                    return NotFound();
                }

                await _pravnoLiceRepository.DeletePravnoLice(kupacId);
                await _pravnoLiceRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeletePravnoLice", $"Pravno lice  sa id-em {kupacId} je uspešno obrisano. Obrisane vrednosti: {JsonConvert.SerializeObject(_mapper.Map<PravnoLiceDto>(pravnoLice))}");
                return Ok();



            }catch(Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "DeletePravnoLice", $"Greška prilikom brisanja pravnog lica sa id-em {kupacId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error "+e.Message+"\n iinner "+e.InnerException );
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa pravnim licima
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetPravnoLiceOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
