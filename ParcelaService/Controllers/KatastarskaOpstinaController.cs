using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.KatastarskaOpstina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    [Route("api/katastarskaOpstina")]
    public class KatastarskaOpstinaController : ControllerBase
    {
        private readonly IKatastarskaOpstinaRepository _katastarskaOpstinaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public KatastarskaOpstinaController(IKatastarskaOpstinaRepository katastarskaOpstinaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _katastarskaOpstinaRepository = katastarskaOpstinaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<KatastarskaOpstinaDto>>> GetAllKatastarskaOpstina(string nazivKatastarskeOpstine)
        {
            var katastarskeOpstine = await _katastarskaOpstinaRepository.GetAllKatastarskaOpstina(nazivKatastarskeOpstine);

            if(katastarskeOpstine == null || katastarskeOpstine.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KatastarskaOpstinaDto>>(katastarskeOpstine));
        }

        [HttpGet("katastarskaOpstinaId")]
        public async Task<ActionResult<KatastarskaOpstinaDto>> GetKatastarskaOpstina(Guid katastarskaOpstinaId)
        {
            var katastarskaOpstina = await _katastarskaOpstinaRepository.GetKatastarskaOpstinaById(katastarskaOpstinaId);

            if(katastarskaOpstina == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KatastarskaOpstinaDto>(katastarskaOpstina));
        }

        [HttpPost]
        public async Task<ActionResult<KatastarskaOpstinaDto>> CreateKatastarskaOpstina([FromBody] KatastarskaOpstinaCreationDto katastarskaOpstina)
        {
            try
            {
                KatastarskaOpstina novaKatastarskaOpstina = await _katastarskaOpstinaRepository.CreateKatastarskaOpstina(_mapper.Map<KatastarskaOpstina>(katastarskaOpstina));
                await _katastarskaOpstinaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetKatastarskaOpstina", "KatastarskaOpstina", new { katastarskaOpstinaId = novaKatastarskaOpstina.KatastarskaOpstinaId });
                return Created(lokacija, _mapper.Map<KatastarskaOpstinaDto>(novaKatastarskaOpstina));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja katastarske opstine");
            }
        }

        [HttpPut]
        public async Task<ActionResult<KatastarskaOpstinaDto>> UpdateKatastarskaOpstina(KatastarskaOpstinaUpdateDto katastarskaOpstina)
        {
            try
            {
                var staraKatastarskaOpstina = await _katastarskaOpstinaRepository.GetKatastarskaOpstinaById(katastarskaOpstina.KatastarskaOpstinaId);

                if(staraKatastarskaOpstina == null)
                {
                    return NotFound();
                }

                KatastarskaOpstina novaKatastarskaOpstina = _mapper.Map<KatastarskaOpstina>(katastarskaOpstina);

                _mapper.Map(staraKatastarskaOpstina, novaKatastarskaOpstina);
                await _katastarskaOpstinaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<KatastarskaOpstinaDto>(staraKatastarskaOpstina));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene katastarske opstine");
            }
        }

        [HttpDelete("{katastarskaOpstinaId}")]
        public async Task<ActionResult> DeleteKatastarskaOpstina(Guid katastarskaOpstinaId)
        {
            try
            {
                var katastarskaOpstina = await _katastarskaOpstinaRepository.GetKatastarskaOpstinaById(katastarskaOpstinaId);

                if(katastarskaOpstina == null)
                {
                    return NotFound();
                }

                await _katastarskaOpstinaRepository.DeleteKatastarskaOpstina(katastarskaOpstinaId);
                await _katastarskaOpstinaRepository.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja katastarske opstine");
            }
        }
        [HttpOptions]
        public IActionResult GetKatastarskaOpstinaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
