using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DocumentService.Data.TipDokumenta;
using DocumentService.Data.UnitOfWork;
using DocumentService.Entities;
using DocumentService.Models.TipDokumenta;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]

    public class TipDokumentaController : ControllerBase
    {
        private readonly ITipDokumentaRepository _tipDokumentaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipDokumentaController(ITipDokumentaRepository tipDokumentaRepository, IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _tipDokumentaRepository = tipDokumentaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<TipDokumentaDto>>> GetAllTipDokumenta()
        {
            var tipoviDokumenta = await _tipDokumentaRepository.GetAllTipDokumenta();

            if (tipoviDokumenta == null || tipoviDokumenta.Count == 0)
            {
                return NoContent();
            }
            
            return Ok(_mapper.Map<List<TipDokumentaDto>>(tipoviDokumenta));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipDokumentaDto>> GetTipDokumentaById(Guid id)
        {
            var tipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(id);

            if (tipDokumenta == null) return NotFound();

            return Ok(_mapper.Map<TipDokumentaDto>(tipDokumenta));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTipDokumenta([FromBody] TipDokumentaDto tipDokumentaDto)
        {
            var tipDokumenta = _mapper.Map<TipDokumenta>(tipDokumentaDto);

            _tipDokumentaRepository.CreateTipDokumenta(tipDokumenta);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(
                "GetTipDokumentaById",
                new {id = tipDokumenta.Id},
                _mapper.Map<TipDokumentaDto>(tipDokumenta)
            );
        }

        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTipDokumenta(Guid id, [FromBody] UpdateTipDokumentaDto tipDokumentaDto)
        {
            if (id != tipDokumentaDto.Id) return BadRequest();

            var tipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(id);

            if (tipDokumenta == null) return NotFound();

            _mapper.Map(tipDokumentaDto, tipDokumenta, typeof(UpdateTipDokumentaDto), typeof(TipDokumenta));
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTipDokumenta(Guid id)
        {
            var tipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(id);

            if (tipDokumenta == null) return NotFound();

            _tipDokumentaRepository.DeleteTipDokumenta(tipDokumenta);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
        
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTipDokumentaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}