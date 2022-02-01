using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Klasa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{

    [Route("api/klasa")]
    [ApiController]
    public class KlasaController : ControllerBase
    {
        private readonly IKlasaRepository _klasaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public KlasaController(IKlasaRepository klasaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _klasaRepository = klasaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<KlasaDto>>> GetAllKlasa(string KlasaNaziv)
        {
            var klase = await _klasaRepository.GetAllKlasa(KlasaNaziv);

            if (klase == null || klase.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KlasaDto>>(klase));
        }

        [HttpGet("{klasaId}")]
        public async Task<ActionResult<KlasaDto>> GetKlasa(Guid klasaId)
        {
            var klasa = await _klasaRepository.GetKlasaById(klasaId);

            if (klasa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KlasaDto>(klasa));
        }

        [HttpPost]
        public async Task<ActionResult<KlasaDto>> CreateKlasa([FromBody] KlasaCreationDto klasa)
        {
            try
            {
                Klasa novaKlasa = await _klasaRepository.CreateKlasa(_mapper.Map<Klasa>(klasa));
                await _klasaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetKlasa", "Klasa", new { klasaId = novaKlasa.KlasaId });

                return Created(lokacija, _mapper.Map<KlasaDto>(novaKlasa));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja klase!");

            }
        }

        [HttpPut]
        public async Task<ActionResult<KlasaDto>> UpdateKlasa(KlasaUpdateDto klasa)
        {
            try
            {
                var staraKlasa = await _klasaRepository.GetKlasaById(klasa.KlasaId);

                if (staraKlasa == null)
                {
                    return NotFound();
                }

                Klasa novaKlasa = _mapper.Map<Klasa>(klasa);

                _mapper.Map(novaKlasa, staraKlasa);
                await _klasaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<KlasaDto>(staraKlasa));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene klase");
            }
        }

        [HttpDelete("{klasaId}")]
        public async Task<IActionResult> DeleteKlasa(Guid klasaId)
        {
            try
            {
                var klasa = await _klasaRepository.GetKlasaById(klasaId);

                if (klasa == null)
                {
                    return NotFound();
                }

                await _klasaRepository.DeleteKlasa(klasaId);
                await _klasaRepository.SaveChangesAsync();

                return NoContent();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja klase");
            }
        }

        [HttpOptions]
        public IActionResult GetKlasaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }

}
