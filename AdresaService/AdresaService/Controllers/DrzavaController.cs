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
    /// Kontroler za drzavu
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

        [HttpGet]
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

        [HttpGet("{drzavaId}")]
        public async Task<ActionResult<DrzavaDto>> GetDrzavaById(Guid drzavaId)
        {

            var drzava = await _drzavaRepository.GetDrzavaById(drzavaId);
            if(drzava == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetDrzavaById", $"Država sa id-em {drzavaId} nije pronađena.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetDrzavaById", $"Država sa id-em {drzavaId} je uspešno pronađena.");
            return Ok(_mapper.Map<DrzavaDto>(drzava));
        }


        [HttpPost]
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

        [HttpDelete("{drzavaId}")]
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

                await _loggerService.Log(LogLevel.Information, "DeleteDrzava", $"Država sa id-em {drzavaId} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(drzava)}");

                await _drzavaRepository.DeleteDrzava(drzavaId);
                await _drzavaRepository.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteDrzava", $"Greška prilikom brisanja države sa id-em {drzavaId}.", e);

                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
         }


        [HttpPut]
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


        [HttpOptions]
        public IActionResult GetDrzavaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
   


}
