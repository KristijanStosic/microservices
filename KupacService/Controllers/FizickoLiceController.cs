using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Helpers;
using KupacService.Model.Kupac.FizickoLice;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Controllers
{
    /// <summary>
    /// Kontroler za fizička lica
    /// </summary>
    [ApiController]
    [Route("api/fizickoLice")]
    [Produces("application/json", "application/xml")]
    public class FizickoLiceController : ControllerBase
    {
        private readonly IFizickoLiceRepository _fizickoLiceRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IKupacCalls _kupacCalls;
        private readonly ILoggerService _loggerService;

        /// <summary>
        ///  Konstruktor za kontroler
        /// </summary>
        /// <param name="fizickoLiceRepository"></param>
        /// <param name="linkGenerator"></param>
        /// <param name="mapper"></param>
        /// <param name="kupacCalls"></param>
        /// <param name="loggerService"></param>
        public FizickoLiceController(IFizickoLiceRepository fizickoLiceRepository,LinkGenerator linkGenerator,IMapper mapper
            ,IKupacCalls kupacCalls,ILoggerService loggerService)
        {
            this._fizickoLiceRepository = fizickoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._kupacCalls = kupacCalls;
            this._loggerService = loggerService;
        }

        /// <summary>
        /// Vraća listu fizičkih lica
        /// </summary>
        /// <param name="ime">ime fizičkog lica</param>
        /// <param name="prezime">prezime fizičkog lica</param>
        /// <param name="brojRacuna">broj računa fizičkog lica</param>
        /// <returns>Listu fizičkih lica koji zadovoljavaju zadate filtere</returns>
        /// <response code="200">Uspešno vraćena lista fizičkih lica</response>
        /// <response code="204">Nije pronađeno nijedno fizičko lice</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<FizickoLiceDto>>> GetFizickaLica(string ime, string prezime, string brojRacuna )
        {
            var fizickaLica = await _fizickoLiceRepository.GetFizickoLice(ime, prezime, brojRacuna);

            if(fizickaLica == null || fizickaLica.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetFizickaLica", "Lista fizičkih lica je prazna ili null.");
                return NoContent();
            }

            List<FizickoLiceDto> fizickaLicaDto = new List<FizickoLiceDto>();
       
            foreach (var fizickoLice in fizickaLica)
            {
                FizickoLiceDto fizickoLiceDto = _mapper.Map<FizickoLiceDto>(fizickoLice);
                var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(fizickoLice);
                _mapper.Map(otherServicesDto, fizickoLiceDto);
                fizickaLicaDto.Add(fizickoLiceDto);
            }
            await _loggerService.Log(LogLevel.Information, "GetFizickaLica", "Lista fizičkih lica je uspešno vraćena.");
            return Ok(fizickaLicaDto);

        }

        /// <summary>
        /// Vraća fizičko lice na osnovu unetog id-a
        /// </summary>
        /// <param name="kupacId">Id fizičkog lica</param>
        /// <returns>Fizičko lice</returns>
        /// <response code="200">Uspešno vraćeno fizičko lice</response>
        /// <response code="404">Nije pronađeno fizičko lice sa zadatim id-em</response>
        [HttpGet("{kupacId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FizickoLiceDto>> GetFizickoLiceById(Guid kupacId)
        {
            var fizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(kupacId);

            if(fizickoLice == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetFizickoLiceById", $"Fizičko lice sa id-em {kupacId} nije pronađen.");
                return NotFound();
            }

            FizickoLiceDto fizickoLiceDto = _mapper.Map<FizickoLiceDto>(fizickoLice);

            var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(fizickoLice);
            _mapper.Map(otherServicesDto, fizickoLiceDto);
            await _loggerService.Log(LogLevel.Information, "GetFizickoLiceById", $"Fizičko lice sa id-em {kupacId} je uspešno vraćen.");
            return Ok(fizickoLiceDto);

        }

        /// <summary>
        /// Kreira novo fizičko lice
        /// </summary>
        /// <param name="fizickoLice">Fizičko lice</param>
        /// <returns>Potvrdu o kreiranom fizičkom licu</returns>
        /// <remarks>
        /// Primer kreiranja fizičkog lica \
        /// POST api/fizickolice \ 
        /// { \ 
        ///  "ime": "Petar", \ 
        ///  "prezime": "Petrović", \
        ///  "jmbg": "3485938493123", \ 
        ///  "ostvarenaPovrsina": 500, \
        ///  "imaZabranu": true, \ 
        ///  "datumPocetkaZabrane": "2022-01-30", \
        ///  "duzinaTrajanjaZabraneGod": 2, \
        ///  "brojTelefona": "0665678974", \ 
        ///  "brojTelefona2": "0665678934", \
        ///  "email": "petar@gmail.com", \
        ///  "brojRacuna": "3525235234234535", \ 
        ///	"prioriteti":["f2b8faa4-732c-4480-8b0a-34d65e483930"], \
        ///	"AdresaId":"37375EF6-4F25-48B3-9BF2-FE72A81F88D2", \ 
        ///	"OvlascenaLica":["5E1BFFFC-1AEE-4662-BC04-341C35B9EBDC",\
        ///									 "5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5"] \
        ///    }
        /// </remarks>
        /// <response code="201">Uspešno kreirano fizičko lice</response>
        /// <response code="500">Desila se greška prilikom kreiranja novog fizičkog lica</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FizickoLiceConfirmDto>> CreateFizickoLice([FromBody]FizickoLiceCreationDto fizickoLice)
        {
            try
            {
                FizickoLice newFizickoLice = _mapper.Map<FizickoLice>(fizickoLice);
               
                await _fizickoLiceRepository.CreateFizickoLice(newFizickoLice);
                await _fizickoLiceRepository.SaveChangesAsync();

                string link = _linkGenerator.GetPathByAction("GetFizickoLiceById", "FizickoLice", new { kupacId = newFizickoLice.KupacId });


                await _loggerService.Log(LogLevel.Information, "CreateFizickoLice", $"Fizičko lice sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<FizickoLiceDto>(newFizickoLice))} je uspešno kreirano.");
                return Created(link,_mapper.Map<FizickoLiceConfirmDto>(newFizickoLice));
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "GetFizickoLiceById", $"Greška prilikom unosa fizičkog lica sa vrednostima: {JsonConvert.SerializeObject(fizickoLice)}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }
        /// <summary>
        /// Vrši ažuriranje fizičkog lica 
        /// </summary>
        /// <param name="fizickoLiceUpdate">Fizičko lice</param>
        /// <returns>Fizičko lice</returns>
        /// <remarks>
        /// Primer ažuriranja kontakt osobe
        /// PUT api/kontaktOsoba
        ///  { \ 
        ///  "kupacId":"febd1c29-90e7-40c2-97f3-1e88495fe98d",
        ///  "ime": "Petar", \ 
        ///  "prezime": "Petrović", \
        ///  "jmbg": "3485938493123", \ 
        ///  "ostvarenaPovrsina": 500, \
        ///  "imaZabranu": true, \ 
        ///  "datumPocetkaZabrane": "2022-01-30", \
        ///  "duzinaTrajanjaZabraneGod": 2, \
        ///  "brojTelefona": "0665678974", \ 
        ///  "brojTelefona2": "0665678934", \
        ///  "email": "petar@gmail.com", \
        ///  "brojRacuna": "3525235234234535", \ 
        ///	"prioriteti":["f2b8faa4-732c-4480-8b0a-34d65e483930"], \
        ///	"AdresaId":"37375EF6-4F25-48B3-9BF2-FE72A81F88D2", \ 
        ///	"OvlascenaLica":["5E1BFFFC-1AEE-4662-BC04-341C35B9EBDC",\
        ///									 "5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5"] \
        ///    }
        /// 
        /// </remarks>
        /// <response code="200">Uspešno ažurirano fizičko lice</response>
        /// <response code="404">Nije pronađeno fizičko lice na osnovu prosleđenog id-a</response>
        /// <response code="500">Desila se greška prilikom ažuriranja fizičkog lica</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FizickoLiceDto>> UpdateFizickoLice([FromBody]FizickoLiceUpdateDto fizickoLiceUpdate)
        {
            try
            {
                FizickoLice oldFizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(fizickoLiceUpdate.KupacId);

                if (oldFizickoLice == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateFizickoLice", $"Fizičko lice sa id-em {fizickoLiceUpdate.KupacId} nije pronađen.");
                    return NoContent();
                }
                var stareVrednosti = JsonConvert.SerializeObject(_mapper.Map<FizickoLiceDto>(oldFizickoLice));
                _mapper.Map(fizickoLiceUpdate, oldFizickoLice);
                await _fizickoLiceRepository.UpdateManyToManyTables(oldFizickoLice);

                await _fizickoLiceRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateFizickoLice", $"Fizičko lice sa id-em {fizickoLiceUpdate.KupacId} je uspešno izmenjen. Stare vrednosti su: {stareVrednosti}");
                return Ok(_mapper.Map<FizickoLiceDto>(oldFizickoLice));
            }catch(Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateFizickoLice", $"Greška prilikom izmene fizičkog lica sa id-em {fizickoLiceUpdate.KupacId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }


        }
        /// <summary>
        /// Vrši brisanje fizičkog lica na osnovu unetog id-a
        /// </summary>
        /// <param name="kupacId">Id fizičkog lica</param>
        /// <returns></returns>
        /// <response code="200">Uspešno obrisano fizičko lice</response>
        /// <response code="404">Nije pronađeno fizičko lice na osnovu unetog id-a</response>
        /// <response code="500">Desila se greška prilikom brisanja fizičkog lica</response>
        [HttpDelete("{kupacId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFizickoLice(Guid kupacId)
        {
            try
            {
                FizickoLice fizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(kupacId);

                if(fizickoLice == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteFizickoLice", $"Fizičko lice sa id-em {kupacId} nije pronađen.");
                    return NotFound();
                }

                await _fizickoLiceRepository.DeleteFizickoLice(kupacId);
                await _fizickoLiceRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteFizickoLice", $"Fizičko lice sa id-em {kupacId} je uspešno obrisano. Obrisane vrednosti: {JsonConvert.SerializeObject(_mapper.Map<FizickoLiceDto>(fizickoLice))}");
                return Ok();

            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteFizickoLice", $"Greška prilikom brisanja fizičkog lica sa id-em {kupacId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }
        /// <summary>
        /// Vraća opcije za rad sa fizičkim licima
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
