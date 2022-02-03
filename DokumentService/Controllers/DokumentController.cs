using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DokumentService.Data.Dokument;
using DokumentService.Data.TipDokumenta;
using DokumentService.Data.UnitOfWork;
using DokumentService.Entities;
using DokumentService.Models.Dokument;
using DokumentService.Models.Log;
using DokumentService.Services;
using DokumentService.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DokumentService.Controllers
{
    /// <summary>
    /// Kontroler za dokument
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DokumentController : ControllerBase
    {
        private readonly IDokumentRepository _dokumentaRepository;
        private readonly ITipDokumentaRepository _tipDokumentaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        private readonly ILoggerService _loggerService;

        public DokumentController(IDokumentRepository dokumentaRepository,
            ITipDokumentaRepository tipDokumentaRepository, IUnitOfWork unitOfWork, IMapper mapper, ILoggerService loggerService)
        {
            _dokumentaRepository = dokumentaRepository;
            _tipDokumentaRepository = tipDokumentaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Vraća sve dokumente
        /// </summary>
        /// <returns>Lista dokumenata</returns>
        /// <response code="200">Vraća listu dokumenata</response>
        /// <response code="204">Nije pronadjen nijedan dokument</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<DokumentDto>>> GetAllDokument()
        {
            var documents = await _dokumentaRepository.GetAllDokument();

            if (documents == null || documents.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllDokument", "Lista dokumenata je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllDokument", "Lista dokumenata je uspešno vraćena.");
            
            return Ok(_mapper.Map<List<DokumentDto>>(documents));
        }

        /// <summary>
        /// Vraća jedan dokument na osnovu ID-a
        /// </summary>
        /// <param name="id">ID dokumenta</param>
        /// <returns>Dokument</returns>
        /// <response code="200">Vraća traženi dokument</response>
        /// <response code="404">Nije pronadjen dokument za uneti ID</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DokumentDto>> GetDokumentById(Guid id)
        {
            var document = await _dokumentaRepository.GetDokumentById(id);

            if (document == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetDokumentById", $"Dokument sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetDokumentById", $"Dokument sa id-jem {id} je uspešno vraćen.");
            
            return Ok(_mapper.Map<DokumentDto>(document));
        }

        /// <summary>
        /// Kreira novi dokument
        /// </summary>
        /// <param name="dokumentDto">Model dokumenta</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog dokumenta \
        /// POST /api/Dokument \
        /// {
        ///     "zavodniBroj": "PSPG-1/2022", \
        ///     "datum": "2022-02-02T17:16:53.577Z", \
        ///     "datumDonosenjaDokumenta": "2022-02-02T17:16:53.577Z", \
        ///     "tipDokumentaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6" \
        /// }
        /// </remarks>
        /// <returns>Dokument</returns>
        /// <response code="201">Vraća kreirani dokument</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDokument([FromBody] CreateDokumentDto dokumentDto)
        {
            var document = _mapper.Map<Dokument>(dokumentDto);

            _dokumentaRepository.CreateDokument(document);
            await _unitOfWork.CompleteAsync();
            
            document.TipDokumenta = await _tipDokumentaRepository.GetTipDokumentaById(document.TipDokumentaId);
            
            await _loggerService.Log(LogLevel.Information, "CreateDokument", $"Dokument sa vrednostima: {JsonConvert.SerializeObject(document)} je uspešno kreiran.");

            return CreatedAtAction(
                "GetDokumentById",
                new {id = document.Id},
                _mapper.Map<DokumentDto>(document)
            );
        }

        /// <summary>
        /// Izmena dokumenta
        /// </summary>
        /// <param name="id">ID dokumenta</param>
        /// <param name="dokumentDto">Model dokumenta</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Potvrda o izmeni dokumenta</response>
        /// <response code="404">Nije pronadjen dokument za uneti ID</response>
        /// <response code="400">ID nije isti kao onaj proledjen u modelu dokumenta</response>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDokument(Guid id, [FromBody] UpdateDokumentDto dokumentDto)
        {
            if (id != dokumentDto.Id)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateDokument", $"ID dokumenta prosledjen kroz url nije isti kao onaj u telu zahteva.");
                return BadRequest();
            }

            var document = await _dokumentaRepository.GetDokumentById(id);
            var oldValue = JsonConvert.SerializeObject(document);

            if (document == null)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateDokument", $"Dokument sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            _mapper.Map(dokumentDto, document, typeof(DokumentDto), typeof(Dokument));
            await _unitOfWork.CompleteAsync();
            
            await _loggerService.Log(LogLevel.Information, "UpdateDokument", $"Dokument sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");
            
            return NoContent();
        }

        /// <summary>
        /// Brisanje dokumenta na osnovu ID-a
        /// </summary>
        /// <param name="id">ID dokumenta</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Dokument je uspešno obrisan</response>
        /// <response code="404">Nije pronadjen dokument za uneti ID</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDokument(Guid id)
        {
            var document = await _dokumentaRepository.GetDokumentById(id);

            if (document == null)
            {
                await _loggerService.Log(LogLevel.Warning, "DeleteDokument", $"Dokument sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            _dokumentaRepository.DeleteDokument(document);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "DeleteDokument", $"Dokument sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(document)}");
            
            return NoContent();
        }

        /// <summary>
        /// Vraća opcije za rad sa dokumentima
        /// </summary>
        /// <response code="200">Vraća listu opcija u header-u</response>
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetDokumentOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}