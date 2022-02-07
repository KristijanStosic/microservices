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
    [ApiController]
    [Route("api/pravnoLice")]
    public class PravnoLiceController: ControllerBase
    {
        private readonly IPravnoLiceRepository _pravnoLiceRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IKupacCalls _kupacCalls;
        private readonly ILoggerService _loggerService;

        public PravnoLiceController(IPravnoLiceRepository pravnoLiceRepository,LinkGenerator linkGenerator,IMapper mapper,
            IKupacCalls kupacCalls,ILoggerService loggerService) 
        {
            this._pravnoLiceRepository = pravnoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._kupacCalls = kupacCalls;
            this._loggerService = loggerService;
        }
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet]
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
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar, OperaterNadmetanja, Licitant")]
        [HttpGet("{kupacId}")]
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
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpPost]
        public async Task<ActionResult<PravnoLiceConfirmDto>> CreatePravnoLice([FromBody]PravnoLiceCreateDto pravnoLice)
        {
            try
            {
                PravnoLice newPravnoLice = _mapper.Map<PravnoLice>(pravnoLice);
                
                await _pravnoLiceRepository.CreatePravnoLice(newPravnoLice);
                await _pravnoLiceRepository.SaveChangesAsync();

                string link = _linkGenerator.GetPathByAction("GetPravnoLiceById", "PravnoLice", new { kupacId = newPravnoLice.KupacId });

                await _loggerService.Log(LogLevel.Information, "CreatePravnoLice", $"Pravno lice  sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<PravnoLiceDto>(pravnoLice))} je uspešno kreirano.");
                return Created(link, _mapper.Map<PravnoLiceDto>(newPravnoLice));
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "CreatePravnoLice", $"Greška prilikom unosa pravnog lica sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<PravnoLiceDto>(pravnoLice))}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error ");
            }
        }
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpPut]
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
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpDelete("{kupacId}")]
        public async Task<IActionResult> DeletePravnoLice(Guid kupacId)
        {
            try
            {
                var pravnoLice = _pravnoLiceRepository.GetPravnoLiceById(kupacId);

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
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, OperaterNadmetanja")]
        [HttpOptions]
        public IActionResult GetKontaktOsobaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
