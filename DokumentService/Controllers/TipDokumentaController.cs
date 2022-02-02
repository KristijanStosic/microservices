using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DokumentService.Data.TipDokumenta;
using DokumentService.Data.UnitOfWork;
using DokumentService.Entities;
using DokumentService.Models.TipDokumenta;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DokumentService.Controllers
{
    /// <summary>
    /// Kontroler za tip dokumenta
    /// </summary>
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

        /// <summary>
        /// Vraća sve tipove dokumenta
        /// </summary>
        /// <returns>Lista tipova dokumenta</returns>
        /// <response code="200">Vraća listu tipova dokumenta</response>
        /// <response code="204">Nije pronadjen nijedan tip</response>
        [HttpGet]
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

        /// <summary>
        /// Vraća jedan tip dokumenta na osnovu ID-a
        /// </summary>
        /// <param name="id">ID dokumenta</param>
        /// <returns>Tip dokumenta</returns>
        /// <response code="200">Vraća traženi tip dokumenta</response>
        /// <response code="404">Nije pronadjen tip dokumenta za uneti ID</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipDokumentaDto>> GetTipDokumentaById(Guid id)
        {
            var tipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(id);

            if (tipDokumenta == null) return NotFound();

            return Ok(_mapper.Map<TipDokumentaDto>(tipDokumenta));
        }
        
        /// <summary>
        /// Kreira novi tip dokumenta
        /// </summary>
        /// <param name="tipDokumentaDto">Model tipa dokumenta</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog tipa dokumenta \
        /// POST /api/TipDokumenta \
        /// {
        ///     "nazivTipa": "Izvod iz lista nepokretnosti"
        /// }
        /// </remarks>
        /// <returns>Tip dokumenta</returns>
        /// <response code="201">Vraća kreirani tip dokumenta</response>
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

        /// <summary>
        /// Izmena tipa dokumenta
        /// </summary>
        /// <param name="id">ID tipa dokumenta za izmenu</param>
        /// <param name="tipDokumentaDto">Model tipa dokumenta</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Potvrda o izmeni tipa dokumenta</response>
        /// <response code="404">Nije pronadjen tip dokumenta za uneti ID</response>
        /// <response code="400">ID nije isti kao onaj proledjen u modelu tipa dokumenta</response>
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

        /// <summary>
        /// Brisanje tipa dokumenta na osnovu ID-a
        /// </summary>
        /// <param name="id">ID tipa dokumenta</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Tip dokumenta je uspešno obrisan</response>
        /// <response code="404">Nije pronadjen tip dokumenta za uneti ID</response>
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
        
        /// <summary>
        /// Vraća opcije za rad sa tipovima dokumenta
        /// </summary>
        /// <response code="200">Vraća listu opcija u header-u</response>
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTipDokumentaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}