using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Models.Etapa;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Controllers
{
    [Route("api/etapa")]
    [ApiController]
    public class EtapaController : ControllerBase
    {
        private readonly IEtapaRepository _etapaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public EtapaController(IEtapaRepository etapaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _etapaRepository = etapaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EtapaDto>>> GetAllEtapa()
        {
            var etape = await _etapaRepository.GetAllEtapa();

            if (etape == null || etape.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<EtapaDto>>(etape));
        }

        [HttpGet("{etapaId}")]
        public async Task<ActionResult<EtapaDto>> GetEtapa(Guid etapaId)
        {
            var etapa = await _etapaRepository.GetEtapaById(etapaId);

            if (etapa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EtapaDto>(etapa));
        }

        [HttpPost]
        public async Task<ActionResult<EtapaConfirmationDto>> CreateEtapa([FromBody] EtapaCreationDto etapa)
        {
            try
            {
                EtapaConfirmation novaEtapa = await _etapaRepository.CreateEtapa(_mapper.Map<Etapa>(etapa));
                await _etapaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetEtapa", "Etapa", new { etapaId = novaEtapa.EtapaId });

                return Created(lokacija, _mapper.Map<EtapaConfirmationDto>(novaEtapa));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom unosa etape");
            }
        }

        [HttpPut]
        public async Task<ActionResult<EtapaDto>> UpdateEtapa(EtapaUpdateDto etapa)
        {
            try
            {
                var staraEtapa = await _etapaRepository.GetEtapaById(etapa.EtapaId);

                if (staraEtapa == null)
                {
                    return NotFound();
                }

                Etapa novaEtapa = _mapper.Map<Etapa>(etapa);

                _mapper.Map(novaEtapa, staraEtapa);
                await _etapaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<EtapaDto>(staraEtapa));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene etape");
            }
        }

        [HttpDelete("{etapaId}")]
        public async Task<IActionResult> DeleteEtapa(Guid etapaId)
        {
            try
            {
                var etapa = await _etapaRepository.GetEtapaById(etapaId);

                if (etapa == null)
                {
                    return NotFound();
                }

                await _etapaRepository.DeleteEtapa(etapaId);
                await _etapaRepository.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja etape");
            }
        }

        [HttpOptions]
        public IActionResult GetEtapaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
