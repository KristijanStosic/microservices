using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.Prioritet;
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
    [Route("api/prioritet")]
    public class PrioritetController : ControllerBase
    {
        private readonly IPrioritetRepository _prioritetRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public PrioritetController(IPrioritetRepository prioritetRepository,LinkGenerator linkGenerator,IMapper mapper)
        {
            this._prioritetRepository = prioritetRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<PrioritetDto>>> GetPrioritet(string opis)
        {
            var prioriteti = await _prioritetRepository.GetPrioritet(opis);

            if(prioriteti == null || prioriteti.Count == 0)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<List<PrioritetDto>>(prioriteti));

        }
        [HttpGet("{prioritetId}")]
        public async Task<ActionResult<PrioritetDto>> GetPrioritetById(Guid prioritetId)
        {
            var prioritet = await _prioritetRepository.GetPrioritetById(prioritetId);

            if(prioritet == null)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<PrioritetDto>(prioritet));
        }
        [HttpPost]
        public async Task<ActionResult<PrioritetDto>> CreatePrioritet([FromBody] PrioritetDto prioritet)
        {
            try
            {

                var newPrioritet = _mapper.Map<Prioritet>(prioritet);

               await _prioritetRepository.CreatePrioritet(newPrioritet);

                await _prioritetRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetPrioritetById", "Prioritet", new { prioritetId = newPrioritet.PrioritetId });

                return Created(lokacija, prioritet);
            }
            catch (Exception)
            {
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
                    return NoContent();
                }

                Prioritet newPrioritet = _mapper.Map<Prioritet>(prioritetUpdate);

                _mapper.Map(newPrioritet, oldPrioritet);
                await _prioritetRepository.SaveChangesAsync();

                return Ok(_mapper.Map<PrioritetDto>(newPrioritet));


            }
            catch (Exception)
            {
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
                    return NoContent();
                }

               await _prioritetRepository.DeletePrioritet(prioritetId);
               await _prioritetRepository.SaveChangesAsync();

                return Ok();
               

            }
            catch (Exception)
            {
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
