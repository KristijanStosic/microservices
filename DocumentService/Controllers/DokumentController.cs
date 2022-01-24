using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DocumentService.Data.Dokument;
using DocumentService.Data.TipDokumenta;
using DocumentService.Data.UnitOfWork;
using DocumentService.Entities;
using DocumentService.Models.Dokument;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DokumentController : ControllerBase
    {
        private readonly IDokumentRepository _dokumentaRepository;
        private readonly IMapper _mapper;
        private readonly ITipDokumentaRepository _tipDokumentaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DokumentController(IMapper mapper, IDokumentRepository dokumentRepository,
            ITipDokumentaRepository tipDokumentaRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _dokumentaRepository = dokumentRepository;
            _tipDokumentaRepository = tipDokumentaRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<DokumentDto>>> GetAllDokument()
        {
            var documents = await _dokumentaRepository.GetAllDokument();
            return Ok(_mapper.Map<List<DokumentDto>>(documents));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DokumentDto>> GetDokumentById(Guid id)
        {
            var document = await _dokumentaRepository.GetDokumentById(id);

            if (document == null) return NotFound();

            return Ok(_mapper.Map<DokumentDto>(document));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDokument([FromBody] CreateDokumentDto dokumentDto)
        {
            var document = _mapper.Map<Dokument>(dokumentDto);
            _dokumentaRepository.CreateDokument(document);
            await _unitOfWork.CompleteAsync();

            document.TipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(document.TipDokumentaId);

            return CreatedAtAction(
                "GetDokumentById",
                new {id = document.Id},
                _mapper.Map<DokumentDto>(document)
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDokument(Guid id, [FromBody] UpdateDokumentDto dokumentDto)
        {
            var document = await _dokumentaRepository.GetDokumentById(id);

            if (document == null) return NotFound();

            _mapper.Map(dokumentDto, document, typeof(DokumentDto), typeof(Dokument));

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDokument(Guid id)
        {
            var document = await _dokumentaRepository.GetDokumentById(id);

            if (document == null) return NotFound();

            _dokumentaRepository.DeleteDokument(document);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}