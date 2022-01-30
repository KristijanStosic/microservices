using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.OblikSvojine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    [Route("api/oblikSvojine")]
    [ApiController]
    public class OblikSvojineController : ControllerBase
    {
        private readonly IOblikSvojineRepository _oblikSvojineRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public OblikSvojineController(IOblikSvojineRepository oblikSvojineRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _oblikSvojineRepository = oblikSvojineRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<OblikSvojineDto>>> GetAllOblikSvojine(string opisOblikaSvojine)
        {
            var obliciSvojine = await _oblikSvojineRepository.GetAllOblikSvojine(opisOblikaSvojine);

            if (obliciSvojine == null || obliciSvojine.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<OblikSvojineDto>>(obliciSvojine));
        }

        [HttpGet("oblikSvojineId")]
        public async Task<ActionResult<OblikSvojineDto>> GetOblikSvojine(Guid oblikSvojineId)
        {
            var oblikSvojine = await _oblikSvojineRepository.GetOblikSvojineById(oblikSvojineId);

            if (oblikSvojine == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OblikSvojineDto>(oblikSvojine));
        }

        [HttpPost]
        public async Task<ActionResult<OblikSvojineDto>> CreateOblikSvojine([FromBody] OblikSvojineCreationDto oblikSvojine)
        {
            try
            {
                OblikSvojine noviOblikSvojine = await _oblikSvojineRepository.CreateOblikSvojine(_mapper.Map<OblikSvojine>(oblikSvojine));
                await _oblikSvojineRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetOblikSvojine", "OblikSvojine", new { oblikSvojineId = noviOblikSvojine.OblikSvojineId });

                return Created(lokacija, _mapper.Map<OblikSvojineDto>(noviOblikSvojine));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja oblika svojine");
            }
        }
        [HttpPut]
        public async Task<ActionResult<OblikSvojineDto>> UpdateOblikSvojine(OblikSvojineUpdateDto oblikSvojine)
        {
            try
            {
                var stariOblikSvojine = await _oblikSvojineRepository.GetOblikSvojineById(oblikSvojine.OblikSvojineId);

                if(stariOblikSvojine == null)
                {
                    return NotFound();
                }

                OblikSvojine noviOblikSvojine = _mapper.Map<OblikSvojine>(oblikSvojine);

                _mapper.Map(stariOblikSvojine, noviOblikSvojine);
                await _oblikSvojineRepository.SaveChangesAsync();

                return Ok(_mapper.Map<OblikSvojineDto>(stariOblikSvojine));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene oblika svojine");

            }
        }

        [HttpDelete("{oblikSvojineId}")]
        public async Task<ActionResult> DeleteOblikSvojine (Guid oblikSvojineId)
        {
            try
            {
                var oblikSvojine = await _oblikSvojineRepository.GetOblikSvojineById(oblikSvojineId);

                if(oblikSvojine == null)
                {
                    return NotFound();
                }

                await _oblikSvojineRepository.DeleteOblikSvojine(oblikSvojineId);
                await _oblikSvojineRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja oblika svojine");

            }
        }

        [HttpOptions]
        public IActionResult GetOblikSvojineOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
