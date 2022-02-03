using AdresaService.Data.Interfaces;
using AdresaService.Entities;
using AdresaService.Model.Drzava;
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
    /// <summary>
    /// Kontroler za državu
    /// </summary>
    [ApiController]
    [Route("api/drzava")]
    [Produces("application/json", "application/xml")]
    public class DrzavaController : ControllerBase
    {
        private readonly IDrzavaRepository _drzavaRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILoggerService _loggerService;

        public DrzavaController(IDrzavaRepository drzavaRepository, IMapper mapper, LinkGenerator linkGenerator,ILoggerService loggerService)
        {
            this._drzavaRepository = drzavaRepository;
            this._mapper = mapper;
            this._linkGenerator = linkGenerator;
            this._loggerService = loggerService;
        }
        /// <summary>
        ///  Vraća sve države za zadate filtere
        /// </summary>
        /// <param name="nazivDrzave"> Naziv države</param>
        /// <returns>Lista država</returns>
        /// <response code="200">Vraća listu država</response>
        /// <response code="404">Nije pronađena ni jedna jedina država</response>
        [HttpGet]
        [HttpHead] //Podržavamo i HTTP head zahtev koji nam vraća samo zaglavlja u odgovoru    
        [ProducesResponseType(StatusCodes.Status200OK)] //Eksplicitno definišemo šta sve može ova akcija da vrati
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<DrzavaDto>>> GetAllDrzava(string nazivDrzave)
        {
            var drzave = await _drzavaRepository.GetAllDrzava(nazivDrzave);

            if (drzave == null || drzave.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllDrzava", "Lista država je prazna ili null");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetAllDrzava", "Lista država je uspešno pronađena");

            return Ok(_mapper.Map<List<DrzavaDto>>(drzave));
        }
        /// <summary>
        /// Vraća jednu državu na osnovu zadatog Id-a
        /// </summary>
        /// <param name="drzavaId">Id države</param>
        /// <returns>Državu</returns>
        /// <response code="200">Vraća traženu državu</response>
        /// <response code="404">Nije pronađena država za uneti ID</response>
        [HttpGet("{drzavaId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DrzavaDto>> GetDrzavaById(Guid drzavaId)
        {

            var drzava = await _drzavaRepository.GetDrzavaById(drzavaId);
            if(drzava == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetDrzavaById", $"Država sa id-em {drzavaId} nije pronađena.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetDrzavaById", $"Država sa id-em {drzavaId} je uspešno pronađena.");
            return Ok(_mapper.Map<DrzavaDto>(drzava));
        }

        /// <summary>
        /// Kreira novu državu
        /// </summary>
        /// <param name="drzava"> Država</param>
        /// <returns>Državu</returns>
        /// <remarks>
        /// Primer zahteva za kreiranje nove države \
        /// POST /api/drzava \
        /// {    \
        ///"nazivDrzave": "string" \
        /// }
        /// </remarks>
        /// <returns>Potvrda o kreiranju države</returns>
        /// <response code="200">Vraća kreiranu državu</response>
        /// <response code="500">Desila se greška prilikom unosa nove države</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DrzavaDto>> CreateDrzava([FromBody] DrzavaDto drzava)
        {
            try
            {
                //add test to see if country already exist

                var newDrzava = await _drzavaRepository.CreateDrzava(_mapper.Map<Drzava>(drzava));
                await _drzavaRepository.SaveChangesAsync();
        
                string lokacija = _linkGenerator.GetPathByAction("GetDrzavaById", "Drzava", new { drzavaid = newDrzava.DrzavaId });

                await _loggerService.Log(LogLevel.Information, "CreateDrzava", $"Država sa vrednostima: {JsonConvert.SerializeObject(drzava)} je uspešno kreirana.");


                return Created(lokacija,_mapper.Map<DrzavaDto>(newDrzava));
      

            }
            catch(Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "CreateDrzava", $"Greška prilikom unosa države sa vrednostima: {JsonConvert.SerializeObject(drzava)}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }
        /// <summary>
        /// Vrši brisanje jedne države na osnovu unetog id-a
        /// </summary>
        /// <param name="drzavaId">Id države</param>
        /// <returns></returns>
        /// <response code="200">Uspešno obrisana država</response>
        /// <response code="404">Nije pronađena država sa zadatim id-em</response>
        /// <response code="500">Desila se greška prilikom brisanja</response>
        [HttpDelete("{drzavaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDrzava(Guid drzavaId)
        {
            try
            {
                var drzava = await _drzavaRepository.GetDrzavaById(drzavaId);

                if (drzava == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteDrzava", $"Država sa id-em {drzavaId} nije pronađena.");
                    return NotFound();
                }

              
                await _drzavaRepository.DeleteDrzava(drzavaId);
                await _drzavaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteDrzava", $"Država sa id-em {drzavaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(drzava)}");

                return Ok();
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteDrzava", $"Greška prilikom brisanja države sa id-em {drzavaId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
         }

        /// <summary>
        /// Vrši ažuriranje države
        /// </summary>
        /// <param name="updateDrzava">Država</param>
        /// <returns>Državu</returns>
        /// <remarks>
        /// Primer zahteva za ažuriranje države \
        /// PUT /api/drzava \
        /// {
        ///     drzavaId:"f320743f-6c87-47ca-9f82-50191c1d31ac", \
        ///     nazviDrzave: "Češka" \
        /// } 
        /// </remarks>
        /// <response code="200">Uspešno ažurirana država</response>
        /// <response code="404">Nije pronađena država sa datim id-em</response>
        /// <response code="500">Desila se greška prilikom ažuriranja</response>
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DrzavaDto>> UpdateDrzava(DrzavaUpdateDto updateDrzava)
        {
            try
            {
                var oldDrzava = await _drzavaRepository.GetDrzavaById(updateDrzava.DrzavaId);
                var stareVrednosti = JsonConvert.SerializeObject(oldDrzava);
                if (oldDrzava == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateDrzava", $"Država sa id-em {updateDrzava.DrzavaId} nije pronađena.");
                    return NotFound();
                }

                Drzava drzava = _mapper.Map<Drzava>(updateDrzava);

                 _mapper.Map(drzava, oldDrzava);
                await _drzavaRepository.SaveChangesAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateDrzava", $"Država sa id-em {updateDrzava.DrzavaId} je uspešno izmenjena. Stare vrednosti su: {stareVrednosti}");
                return Ok(_mapper.Map<DrzavaDto>(drzava));

            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateDrzava", $"Greška prilikom izmene države sa id-em {updateDrzava.DrzavaId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa državama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetDrzavaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
   


}
