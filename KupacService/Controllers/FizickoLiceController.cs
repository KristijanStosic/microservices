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
    [ApiController]
    [Route("api/fizickoLice")]
    public class FizickoLiceController : ControllerBase
    {
        private readonly IFizickoLiceRepository _fizickoLiceRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IKupacCalls _kupacCalls;
        private readonly ILoggerService _loggerService;

        public FizickoLiceController(IFizickoLiceRepository fizickoLiceRepository,LinkGenerator linkGenerator,IMapper mapper
            ,IKupacCalls kupacCalls,ILoggerService loggerService)
        {
            this._fizickoLiceRepository = fizickoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._kupacCalls = kupacCalls;
            this._loggerService = loggerService;
        }

        [HttpGet]
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
        [HttpGet("{kupacId}")]
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


        [HttpPost]
        public async Task<ActionResult<FizickoLiceConfirmDto>> CreateFizickoLice([FromBody]FizickoLiceCreationDto fizickoLice)
        {
            try
            {
                FizickoLice newFizickoLice = _mapper.Map<FizickoLice>(fizickoLice);
               
                await _fizickoLiceRepository.CreateFizickoLice(newFizickoLice);
                await _fizickoLiceRepository.SaveChangesAsync();

                string link = _linkGenerator.GetPathByAction("GetFizickoLiceById", "FizickoLice", new { kupacId = newFizickoLice.KupacId });


                await _loggerService.Log(LogLevel.Information, "CreateFizickoLice", $"Fizičko lice sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<FizickoLiceDto>(fizickoLice))} je uspešno kreirano.");
                return Created(link,_mapper.Map<FizickoLiceConfirmDto>(newFizickoLice));
            }
            catch (Exception e)
            {
                await _loggerService.Log(LogLevel.Error, "GetFizickoLiceById", $"Greška prilikom unosa fizičkog lica sa vrednostima: {JsonConvert.SerializeObject(_mapper.Map<FizickoLiceDto>(fizickoLice))}.", e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }
        
        [HttpPut]
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

        [HttpDelete("{kupacId}")]
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

        [HttpOptions]
        public IActionResult GetKontaktOsobaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
