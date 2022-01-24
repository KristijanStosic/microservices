using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DocumentService.Data.TipDokumenta;
using DocumentService.Data.UnitOfWork;
using DocumentService.Entities;
using DocumentService.Models.TipDokumenta;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipDokumentaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITipDokumentaRepository _tipDokumentaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TipDokumentaController(ITipDokumentaRepository tipDokumentaRepository, IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _tipDokumentaRepository = tipDokumentaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipDokumentaDto>>> GetAllTipDokumenta()
        {
            var tipoviDokumenta = await _tipDokumentaRepository.GetAllTipDokumenta();
            return Ok(_mapper.Map<List<TipDokumentaDto>>(tipoviDokumenta));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipDokumenta>> GetTipDokumentaById(Guid id)
        {
            var tipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(id);

            if (tipDokumenta == null) return NotFound();

            return Ok(_mapper.Map<TipDokumentaDto>(tipDokumenta));
        }

        [HttpPost]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipDokumenta(Guid id)
        {
            var tipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(id);

            if (tipDokumenta == null) return NotFound();

            _tipDokumentaRepository.DeleteTipDokumenta(tipDokumenta);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}