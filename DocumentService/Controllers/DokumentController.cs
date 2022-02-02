using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DocumentService.Data.Dokument;
using DocumentService.Data.TipDokumenta;
using DocumentService.Data.UnitOfWork;
using DocumentService.Entities;
using DocumentService.Models.Dokument;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DokumentController : ControllerBase
    {
        private readonly IDokumentRepository _dokumentaRepository;
        private readonly ITipDokumentaRepository _tipDokumentaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DokumentController(IDokumentRepository dokumentaRepository,
            ITipDokumentaRepository tipDokumentaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _dokumentaRepository = dokumentaRepository;
            _tipDokumentaRepository = tipDokumentaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<DokumentDto>>> GetAllDokument()
        {
            var documents = await _dokumentaRepository.GetAllDokument();

            if (documents == null || documents.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<DokumentDto>>(documents));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DokumentDto>> GetDokumentById(Guid id)
        {
            var document = await _dokumentaRepository.GetDokumentById(id);

            if (document == null) return NotFound();

            return Ok(_mapper.Map<DokumentDto>(document));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDokument(Guid id, [FromBody] UpdateDokumentDto dokumentDto)
        {
            if (id != dokumentDto.Id) return BadRequest();

            var document = await _dokumentaRepository.GetDokumentById(id);

            if (document == null) return NotFound();

            _mapper.Map(dokumentDto, document, typeof(DokumentDto), typeof(Dokument));
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDokument(Guid id)
        {
            var document = await _dokumentaRepository.GetDokumentById(id);

            if (document == null) return NotFound();

            _dokumentaRepository.DeleteDokument(document);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
        
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetDokumentOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}