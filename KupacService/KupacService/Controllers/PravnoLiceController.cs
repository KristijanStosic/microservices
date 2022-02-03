using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.Kupac.PravnoLice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        private readonly IPrioritetRepository _prioritetRepository;

        public PravnoLiceController(IPravnoLiceRepository pravnoLiceRepository,LinkGenerator linkGenerator,IMapper mapper, IPrioritetRepository prioritetRepository) 
        {
            this._pravnoLiceRepository = pravnoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._prioritetRepository = prioritetRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PravnoLiceDto>>> GetPravnoLice([FromQuery]string naziv,[FromQuery]string maticniBroj)
        {
            var pravnaLica = await _pravnoLiceRepository.GetPravnoLice(naziv, maticniBroj);

            if(pravnaLica == null || pravnaLica.Count == 0)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<List<PravnoLiceDto>>(pravnaLica));

        }

        [HttpGet("{kupacId}")]
        public async Task<ActionResult<PravnoLiceDto>> GetPravnoLiceById(Guid kupacId)
        {
            var pravnoLice = await _pravnoLiceRepository.GetPravnoLiceById(kupacId);

            if(pravnoLice == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PravnoLiceDto>(pravnoLice));
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
