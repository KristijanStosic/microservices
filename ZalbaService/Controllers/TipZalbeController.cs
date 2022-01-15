using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Data;
using ZalbaService.Entities;
using ZalbaService.Models;

namespace ZalbaService.Controllers
{
    [ApiController]
    [Route("api/tipZalbe")]
    [Produces("application/json", "application/xml")]
    public class TipZalbeController : ControllerBase
    {
        private readonly ITipZalbeRepository _tipZalbeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public TipZalbeController(ITipZalbeRepository tipZalbeRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _tipZalbeRepository = tipZalbeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<TipZalbeCreateDto>>> GetAllTipoviZalbe(string nazivTipaZalbe)
        {
            var tipoviZalbe = await _tipZalbeRepository.GetAllTipoviZalbe(nazivTipaZalbe);

            if (tipoviZalbe == null || tipoviZalbe.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<TipZalbeCreateDto>>(tipoviZalbe));
        }

        [HttpGet("{tipZalbeId}")]
        public async Task<ActionResult<TipZalbeCreateDto>> GetTipZalbe(Guid tipZalbeId)
        {
            var tipZalbe = await _tipZalbeRepository.GetTipZalbeById(tipZalbeId);

            if (tipZalbe == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TipZalbeCreateDto>(tipZalbe));
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<TipZalbeCreateDto>> CreateTipZalbe([FromBody] TipZalbeCreateDto tipZalbe)
        {
            try
            {
                TipZalbe createdTipZalbe = await _tipZalbeRepository.CreateTipZalbe(_mapper.Map<TipZalbe>(tipZalbe));

                string location = _linkGenerator.GetPathByAction("GetTipZalbe", "TipZalbe", new { tipZalbeId = createdTipZalbe.TipZalbeId });

                return Created(location, _mapper.Map<TipZalbeCreateDto>(createdTipZalbe));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Tip Zalbe error");
            }
        }

        [HttpPut("{tipZalbeId}")]
        public async Task<ActionResult<TipZalbeUpdateDto>> UpdateTipZalbe(Guid tipZalbeId, [FromBody] TipZalbeUpdateDto tipZalbe)
        {
            try
            {
                var tipZalbeEntity = await _tipZalbeRepository.GetTipZalbeById(tipZalbeId);

                if (tipZalbeEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(tipZalbe, tipZalbeEntity);

                await _tipZalbeRepository.UpdateTipZalbe(_mapper.Map<TipZalbe>(tipZalbe));

                return Ok(tipZalbe);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update tip zalbe error");
            }
        }

        [HttpDelete("{tipZalbeId}")]
        public async Task<ActionResult> DeleteTipZalbe(Guid tipZalbeId)
        {
            try
            {
                var tipZalbe = await _tipZalbeRepository.GetTipZalbeById(tipZalbeId);

                if (tipZalbe == null)
                {
                    return NotFound();
                }

                await _tipZalbeRepository.DeleteTipZalbe(tipZalbeId);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete tip zalbe error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa tipovima zalbi
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetTipZalbeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
