using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Helpers;
using KupacService.Model.Kupac.PravnoLice;
using KupacService.Model.OtherServices;
using KupacService.ServiceCalls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly IServiceCall<AdresaDto> _adresaServiceCall;
        private readonly IKupacCalls _kupacCalls;

        public PravnoLiceController(IPravnoLiceRepository pravnoLiceRepository,LinkGenerator linkGenerator,IMapper mapper,
            IConfiguration configuration, IServiceCall<AdresaDto> adresaServiceCall,IKupacCalls kupacCalls) 
        {
            this._pravnoLiceRepository = pravnoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._configuration = configuration;
            this._adresaServiceCall = adresaServiceCall;
            this._kupacCalls = kupacCalls;
        }

        [HttpGet]
        public async Task<ActionResult<List<PravnoLiceDto>>> GetPravnoLice([FromQuery]string naziv,[FromQuery]string maticniBroj)
        {
            var pravnaLica = await _pravnoLiceRepository.GetPravnoLice(naziv, maticniBroj);

            if(pravnaLica == null || pravnaLica.Count == 0)
            {
                return NoContent();
            }

            List<PravnoLiceDto> pravnaLicaDto = new List<PravnoLiceDto>();
       
            foreach (var pravnoLice in pravnaLica)
            {
                PravnoLiceDto pravnoLiceDto = _mapper.Map<PravnoLiceDto>(pravnoLice);
                var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(pravnoLice);
                _mapper.Map(otherServicesDto, pravnoLiceDto);
                pravnaLicaDto.Add(pravnoLiceDto);
            }

            return Ok(pravnaLicaDto);

        }

        [HttpGet("{kupacId}")]
        public async Task<ActionResult<PravnoLiceDto>> GetPravnoLiceById(Guid kupacId)
        {
            var pravnoLice = await _pravnoLiceRepository.GetPravnoLiceById(kupacId);

            if(pravnoLice == null)
            {
                return NotFound();
            }

            PravnoLiceDto pravnoLiceDto = _mapper.Map<PravnoLiceDto>(pravnoLice);

            var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(pravnoLice);
            _mapper.Map(otherServicesDto, pravnoLiceDto);
         

            return Ok(pravnoLiceDto);
        }

        [HttpPost]
        public async Task<ActionResult<PravnoLiceConfirmDto>> CreatePravnoLice([FromBody]PravnoLiceCreateDto pravnoLice)
        {
            try
            {
                PravnoLice newPravnoLice = _mapper.Map<PravnoLice>(pravnoLice);
                
                await _pravnoLiceRepository.CreatePravnoLice(newPravnoLice);
                await _pravnoLiceRepository.SaveChangesAsync();

                string link = _linkGenerator.GetPathByAction("GetPravnoLiceById", "PravnoLice", new { kupacId = newPravnoLice.KupacId });

                return Created(link, _mapper.Map<PravnoLiceDto>(newPravnoLice));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error ");
            }
        }
        [HttpPut]
        public async Task<ActionResult<PravnoLiceDto>> UpdatePravnoLice([FromBody]PravnoLiceUpdateDto pravnoLiceUpdate)
        {
            try
            {
                var oldPravnoLice = await _pravnoLiceRepository.GetPravnoLiceById(pravnoLiceUpdate.KupacId);
                KontaktOsoba kontaktOsoba = oldPravnoLice.KontaktOsoba;
                if (oldPravnoLice == null)
                {
                    return NotFound();
                }
              

                _mapper.Map(pravnoLiceUpdate, oldPravnoLice);
                await _pravnoLiceRepository.UpdateManyToManyTables(oldPravnoLice);

               
                oldPravnoLice.KontaktOsoba = kontaktOsoba;
                await _pravnoLiceRepository.SaveChangesAsync();

                return Ok(_mapper.Map<PravnoLiceDto>(oldPravnoLice));

            }catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }

        }
        [HttpDelete("{kupacId}")]
        public async Task<IActionResult> DeletePravnoLice(Guid kupacId)
        {
            try
            {
                var pravnoLice = _pravnoLiceRepository.GetPravnoLiceById(kupacId);

                if (pravnoLice == null)
                {
                    return NotFound();
                }

                await _pravnoLiceRepository.DeletePravnoLice(kupacId);
                await _pravnoLiceRepository.SaveChangesAsync();
                return Ok();



            }catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }



    }
}
