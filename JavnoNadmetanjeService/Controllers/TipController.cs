using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.Tip;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Controllers
{
    [Route("api/tip")]
    [ApiController]
    public class TipController : ControllerBase
    {
        private readonly ITipRepository _tipRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public TipController(ITipRepository tipRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _tipRepository = tipRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipDto>>> GetAllTip(string nazivTipa)
        {
            var tipovi = await _tipRepository.GetAllTip(nazivTipa);

            if (tipovi == null || tipovi.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<TipDto>>(tipovi));
        }

        [HttpGet("{tipId}")]
        public async Task<ActionResult<TipDto>> GetTip(Guid tipId)
        {
            var tip = await _tipRepository.GetTipById(tipId);

            if (tip == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TipDto>(tip));
        }

        [HttpPost]
        public async Task<ActionResult<TipDto>> CreateTip([FromBody] TipCreationDto tip)
        {
            try
            {
                Tip noviTip = await _tipRepository.CreateTip(_mapper.Map<Tip>(tip));
                await _tipRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetTip", "Tip", new { tipId = noviTip.TipId });

                return Created(lokacija, _mapper.Map<TipDto>(noviTip));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja tipa");
            }
        }

        [HttpPut]
        public async Task<ActionResult<TipDto>> UpdateTip(TipUpdateDto tip)
        {
            try
            {
                var stariTip = await _tipRepository.GetTipById(tip.TipId);

                if (stariTip == null)
                {
                    return NotFound();
                }

                Tip noviTip = _mapper.Map<Tip>(tip);

                _mapper.Map(noviTip, stariTip);
                await _tipRepository.SaveChangesAsync();

                return Ok(_mapper.Map<TipDto>(noviTip));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene tipa");
            }
        }

        [HttpDelete("{tipId}")]
        public async Task<IActionResult> DeleteTip(Guid tipId)
        {
            try
            {
                var tip = await _tipRepository.GetTipById(tipId);

                if (tip == null)
                {
                    return NotFound();
                }

                await _tipRepository.DeleteTip(tipId);
                await _tipRepository.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja tipa");
            }
        }

        [HttpOptions]
        public IActionResult GetTipOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
