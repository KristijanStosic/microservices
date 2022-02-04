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
        private readonly IConfiguration _configuration;
        private readonly IServiceCall<AdresaDto> _adresaServiceCall;
        private readonly IKupacCalls _kupacCalls;

        public FizickoLiceController(IFizickoLiceRepository fizickoLiceRepository,LinkGenerator linkGenerator,IMapper mapper,
            IConfiguration configuration,IServiceCall<AdresaDto> adresaServiceCall,IKupacCalls kupacCalls)
        {
            this._fizickoLiceRepository = fizickoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._configuration = configuration;
            this._adresaServiceCall = adresaServiceCall;
            this._kupacCalls = kupacCalls;
        }

        [HttpGet]
        public async Task<ActionResult<List<FizickoLiceDto>>> GetFizickoLice(string ime, string prezime, string brojRacuna )
        {
            var fizickaLica = await _fizickoLiceRepository.GetFizickoLice(ime, prezime, brojRacuna);

            if(fizickaLica == null || fizickaLica.Count == 0)
            {
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

            return Ok(fizickaLicaDto);

        }
        [HttpGet("{kupacId}")]
        public async Task<ActionResult<FizickoLiceDto>> GetFizickoLiceById(Guid kupacId)
        {
            var fizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(kupacId);

            if(fizickoLice == null)
            {
                return NotFound();
            }

            FizickoLiceDto fizickoLiceDto = _mapper.Map<FizickoLiceDto>(fizickoLice);

            var otherServicesDto = await _kupacCalls.GetKupacDtoWithOtherServicesInfo(fizickoLice);
            _mapper.Map(otherServicesDto, fizickoLiceDto);

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


                return Created(link,_mapper.Map<FizickoLiceConfirmDto>(newFizickoLice));
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Create Errors " +e.Message);
            }
        }
        
        [HttpPut]
        public async Task<ActionResult<FizickoLiceDto>> UpdateFizickoLice([FromBody]FizickoLiceUpdateDto fizickoLiceUpdate)
        {
           
                FizickoLice oldFizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(fizickoLiceUpdate.KupacId);
                
                if(oldFizickoLice == null)
                {
                    return NoContent();
                }
               
                _mapper.Map(fizickoLiceUpdate, oldFizickoLice);
                await _fizickoLiceRepository.UpdateKupacOvlascenoLice(oldFizickoLice);
                
                await _fizickoLiceRepository.SaveChangesAsync();

                return Ok(_mapper.Map<FizickoLiceDto>(oldFizickoLice));


        }

        [HttpDelete("{kupacId}")]
        public async Task<IActionResult> DeleteFizickoLice(Guid kupacId)
        {
            try
            {
                FizickoLice fizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(kupacId);

                if(fizickoLice == null)
                {
                    return NotFound();
                }

                await _fizickoLiceRepository.DeleteFizickoLice(kupacId);
                await _fizickoLiceRepository.SaveChangesAsync();

                return Ok();

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

    }
}
