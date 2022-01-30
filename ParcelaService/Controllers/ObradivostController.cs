using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Obradivost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    [Route("api/obradivost")]
    [ApiController]
    public class ObradivostController : ControllerBase
    {
        private readonly IObradivostRepository _obradivostRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public ObradivostController(IObradivostRepository obradivostRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _obradivostRepository = obradivostRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ObradivostDto>>> GetAllObradivost(string opisObradivosti)
        {
            var obradivost = await _obradivostRepository.GetAllObradivost(opisObradivosti);

            if(obradivost == null || obradivost.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<ObradivostDto>>(obradivost));
        }

        [HttpGet("obradivostId")]
        public async Task<ActionResult<ObradivostDto>> GetObradivost(Guid obradivostId)
        {
            var obradivost = await _obradivostRepository.GetObradivostById(obradivostId);

            if(obradivost == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ObradivostDto>(obradivost));
        }

        [HttpPost]
        public async Task<ActionResult<ObradivostDto>> CreateObradivost([FromBody] ObradivostCreationDto obradivost)
        {
            try
            {
                Obradivost novaObradivost = await _obradivostRepository.CreateObradivost(_mapper.Map<Obradivost>(obradivost));
                await _obradivostRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetObradivost", "Obradivost", new { obradivostId = novaObradivost.ObradivostId });

                return Created(lokacija, _mapper.Map<ObradivostDto>(novaObradivost));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja obradivosti!");
            }

        }

        public async Task<ActionResult<ObradivostDto>> UpdateObradivost(ObradivostUpdateDto obradivost)
        {
            try
            {
                var staraObradivost = await _obradivostRepository.GetObradivostById(obradivost.ObradivostId);

                if(staraObradivost == null)
                {
                    return NotFound();
                }

                Obradivost novaObradivost = _mapper.Map<Obradivost>(obradivost);

                _mapper.Map(staraObradivost, novaObradivost);
                await _obradivostRepository.SaveChangesAsync();

                return Ok(_mapper.Map<ObradivostDto>(staraObradivost));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene obradivosti!");
            }
        }

        [HttpDelete("{obradivostId}")]
        public async Task<ActionResult> DeleteObradivost (Guid obradivostId)
        {
            try
            {
                var obradivost = await _obradivostRepository.GetObradivostById(obradivostId);

                if (obradivost == null)
                {
                    return NotFound();
                }

                await _obradivostRepository.DeleteObradivost(obradivostId);
                await _obradivostRepository.SaveChangesAsync();

                return NoContent();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja obradivosti!");

            }
        }

        [HttpOptions]
        public IActionResult GetObradivostOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }

}
