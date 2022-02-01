using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.ZasticenaZona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    [Route("api/zasticenaZona")]
    [ApiController]
    public class ZasticenaZonaController : ControllerBase
    {
        private readonly IZasticenaZonaRepository _zasticenaZonaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public ZasticenaZonaController(IZasticenaZonaRepository zasticenaZonaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _zasticenaZonaRepository = zasticenaZonaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ZasticenaZonaDto>>> GetAllZasticenaZona(string brojZasticeneZone)
        {
            var zasticeneZone = await _zasticenaZonaRepository.GetAllZasticenaZona(brojZasticeneZone);

            if(zasticeneZone == null || zasticeneZone.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<ZasticenaZonaDto>>(zasticeneZone));
        }

        [HttpGet("zasticenaZonaId")]
        public async Task<ActionResult<ZasticenaZonaDto>> GetZasticenaZona(Guid zasticenaZonaId)
        {
            var zasticenaZona = await _zasticenaZonaRepository.GetZasticenaZonaById(zasticenaZonaId);

            if(zasticenaZona == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ZasticenaZonaDto>(zasticenaZona));
        }

        [HttpPost]
        public async Task<ActionResult<ZasticenaZonaDto>> CreateZasticenaZona([FromBody] ZasticenaZonaCreationDto zasticenaZona)
        {
            try
            {
                ZasticenaZona novaZasticenaZona = await _zasticenaZonaRepository.CreateZasticenaZona(_mapper.Map<ZasticenaZona>(zasticenaZona));
                await _zasticenaZonaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetZasticenaZona", "ZasticenaZona", new { zasticenaZonaId = novaZasticenaZona.ZasticenaZonaId });

                return Created(lokacija, _mapper.Map<ZasticenaZonaDto>(novaZasticenaZona));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja zasticene zone!");
            }
        }

        [HttpPut]
        public async Task<ActionResult<ZasticenaZonaDto>> UpdateZasticenaZona(ZasticenaZonaUpdateDto zasticenaZona)
        {
            try
            {
                var staraZasticenaZona = await _zasticenaZonaRepository.GetZasticenaZonaById(zasticenaZona.ZasticenaZonaId);

                if(staraZasticenaZona == null)
                {
                    return NotFound();
                }

                ZasticenaZona novaZasticenaZona = _mapper.Map<ZasticenaZona>(zasticenaZona);

                _mapper.Map(staraZasticenaZona, novaZasticenaZona);
                await _zasticenaZonaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<ZasticenaZonaDto>(staraZasticenaZona));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene zasticene zone!");
            }
        }

        [HttpDelete("{zasticenaZonaId}")]
        public async Task<ActionResult> DeleteZasticenaZona (Guid zasticenaZonaId)
        {
            try
            {
                var zasticenaZona = await _zasticenaZonaRepository.GetZasticenaZonaById(zasticenaZonaId);

                if(zasticenaZona == null)
                {
                    return NotFound();
                }

                await _zasticenaZonaRepository.DeleteZasticenaZona(zasticenaZonaId);
                await _zasticenaZonaRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja zasticene zone!");
            }
        }

        [HttpOptions]
        public IActionResult GetKulturaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
