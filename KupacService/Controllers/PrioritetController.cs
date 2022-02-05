using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.Prioritet;
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
    [Route("api/prioritet")]
    public class PrioritetController : ControllerBase
    {
        private readonly IPrioritetRepository _prioritetRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public PrioritetController(IPrioritetRepository prioritetRepository,LinkGenerator linkGenerator,IMapper mapper,ILoggerService loggerService)
        {
            this._prioritetRepository = prioritetRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._loggerService = loggerService;
        }


        [HttpGet]
        public async Task<ActionResult<List<PrioritetDto>>> GetPrioritete(string opis)
        {
            var prioriteti = await _prioritetRepository.GetPrioritet(opis);

            if(prioriteti == null || prioriteti.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetPrioritete", "Lista prioriteta je prazna ili null.");
                return NoContent();
            }
            await _loggerService.Log(LogLevel.Information, "GetPrioritete", "Lista prioriteta je uspešno vraćena.");
            return Ok(_mapper.Map<List<PrioritetDto>>(prioriteti));

        }
        [HttpGet("{prioritetId}")]
        public async Task<ActionResult<PrioritetDto>> GetPrioritetById(Guid prioritetId)
        {
            var prioritet = await _prioritetRepository.GetPrioritetById(prioritetId);

            if(prioritet == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetPrioritetById", $"Prioritet sa id-em {prioritetId} nije pronađen.");
                return NotFound();
            }
            await _loggerService.Log(LogLevel.Information, "GetPrioritetById", $"Prioritet  sa id-em {prioritetId} je uspešno vraćen.");
            return Ok(_mapper.Map<PrioritetDto>(prioritet));
        }
        [HttpPost]
        public async Task<ActionResult<PrioritetDto>> CreatePrioritet([FromBody] PrioritetCreateDto prioritet)
        {
            try
            {

                var newPrioritet = _mapper.Map<Prioritet>(prioritet);

               await _prioritetRepository.CreatePrioritet(newPrioritet);

                await _prioritetRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetPrioritetById", "Prioritet", new { prioritetId = newPrioritet.PrioritetId });

                await _loggerService.Log(LogLevel.Information, "CreatePrioritet", $"Prioritet sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<PrioritetDto>(prioritet))} je uspešno kreiran.");
                return Created(lokacija, prioritet);
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "CreatePrioritet", $"Greška prilikom unosa prioriteta  sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<PrioritetDto>(prioritet))}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<PrioritetDto>> UpdatePrioritet(PrioritetUpdateDto prioritetUpdate) 
        {
            try
            {
                var oldPrioritet = await _prioritetRepository.GetPrioritetById(prioritetUpdate.PrioritetId);

                if(oldPrioritet == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdatePrioritet", $"Prioritet sa id-em {prioritetUpdate.PrioritetId} nije pronađen.");
                    return NotFound();
                }
                var stareVrednosti = JsonConvert.SerializeObject(_mapper.Map<PrioritetDto>(oldPrioritet));
                Prioritet newPrioritet = _mapper.Map<Prioritet>(prioritetUpdate);

                _mapper.Map(newPrioritet, oldPrioritet);
                await _prioritetRepository.SaveChangesAsync();
                await _loggerService.Log(LogLevel.Information, "UpdatePrioritet", $"Prioritet sa id-em {prioritetUpdate.PrioritetId} je uspešno izmenjen. Stare vrednosti su: {stareVrednosti}");
                return Ok(_mapper.Map<PrioritetDto>(newPrioritet));


            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "UpdatePrioritet", $"Greška prilikom izmene prioriteta sa id-em {prioritetUpdate.PrioritetId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }


        }
        [HttpDelete("{prioritetId}")]
        public async Task<IActionResult> DeletePrioritet(Guid prioritetId) 
        {
            try
            {
                var prioritet = await _prioritetRepository.GetPrioritetById(prioritetId);

                if(prioritet == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeletePrioritet", $"Prioritet sa id-em {prioritetId} nije pronađen.");
                    return NotFound();
                }

               await _prioritetRepository.DeletePrioritet(prioritetId);
               await _prioritetRepository.SaveChangesAsync();
               await _loggerService.Log(LogLevel.Information, "DeletePrioritet", $"Prioritet sa id-em {prioritetId} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(_mapper.Map<PrioritetDto>(prioritet))}");

                return Ok();
               

            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "DeletePrioritet", $"Greška prilikom brisanja prioriteta sa id-em {prioritetId}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }


        }


        [HttpOptions]
        public IActionResult GetPrioritetOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }


    }
}
