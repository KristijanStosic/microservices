using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Kultura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    [Route("api/kultura")]
    [ApiController]
    public class KulturaController : ControllerBase
    {
        private readonly IKulturaRepository _kulturaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public KulturaController(IKulturaRepository kulturaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _kulturaRepository = kulturaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<KulturaDto>>> GetAllKlasa(string nazivKulture)
        {
            var kulture = await _kulturaRepository.GetAllKultura(nazivKulture);

            if(kulture == null || kulture.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KulturaDto>>(kulture));
        }

        [HttpGet("kulturaId")]
        public async Task<ActionResult<KulturaDto>> GetKultura(Guid kulturaId)
        {
            var kultura = await _kulturaRepository.GetKulturaById(kulturaId);

            if(kultura == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KulturaDto>(kultura));
        }

        [HttpPost]
        public async Task<ActionResult<KulturaDto>> CreateKultura([FromBody] KulturaCreationDto kultura)
        {
            try
            {
                Kultura novaKultura = await _kulturaRepository.CreateKultura(_mapper.Map<Kultura>(kultura));
                await _kulturaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetKultura", "Kultura", new { kulturaId = novaKultura.KulturaId });

                return Created(lokacija, _mapper.Map<KulturaDto>(novaKultura));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja kulture!");
            }
            
        }
        
        [HttpPut]
        public async Task<ActionResult<KulturaDto>> UpdateKultura(KulturaUpdateDto kultura)
        {
            try
            {
                var staraKultura = await _kulturaRepository.GetKulturaById(kultura.KulturaId);

                if(staraKultura == null)
                {
                    return NotFound();
                }

                Kultura novaKultura = _mapper.Map<Kultura>(kultura);

                _mapper.Map(staraKultura, novaKultura);
                await _kulturaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<KulturaDto>(staraKultura));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene kulture!");

            }
        }

        [HttpDelete("{kulturaId}")]
        public async Task<ActionResult> DeleteKultura (Guid kulturaId)
        {
            try
            {
                var kultura = await _kulturaRepository.GetKulturaById(kulturaId);

                if (kultura == null)
                {
                    return NotFound();
                }

                await _kulturaRepository.DeleteKultura(kulturaId);
                await _kulturaRepository.SaveChangesAsync();

                return NoContent();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja kulture!");

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
