using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentService.Data.TipDokumenta;
using DocumentService.Data.UnitOfWork;
using DocumentService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipDokumentaController : ControllerBase
    {
        private readonly ITipDokumentaRepository _tipDokumentaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TipDokumentaController(ITipDokumentaRepository tipDokumentaRepository, IUnitOfWork unitOfWork)
        {
            _tipDokumentaRepository = tipDokumentaRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipDokumenta>>> GetAllTipDokumenta()
        {
            var tipoviDokumenta = await _tipDokumentaRepository.GetAllTipDokumenta();
            return Ok(tipoviDokumenta);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipDokumenta>> GetTipDokumentaById(Guid id)
        {
            var tipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(id);

            if (tipDokumenta == null) return NotFound();

            return Ok(tipDokumenta);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTipDokumenta([FromBody] TipDokumenta tipDokumenta)
        {
            _tipDokumentaRepository.CreateTipDokumenta(tipDokumenta);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction("GetTipDokumentaById",new {id = tipDokumenta.Id}, tipDokumenta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> UpdateTipDokumenta(Guid id)
        {
            var tipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(id);
            
            if (tipDokumenta == null) return NotFound();
            
            _tipDokumentaRepository.DeleteTipDokumenta(tipDokumenta);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}