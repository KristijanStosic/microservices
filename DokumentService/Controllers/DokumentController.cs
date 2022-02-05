using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DokumentService.Data.UnitOfWork;
using DokumentService.Entities;
using DokumentService.Models.Dokument;
using DokumentService.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DokumentService.Controllers
{
    /// <summary>
    ///     Kontroler za dokument
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DokumentController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DokumentController(IUnitOfWork unitOfWork, IMapper mapper,
            ILoggerService loggerService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        ///     Vraća sve dokumente
        /// </summary>
        /// <returns>Lista dokumenata</returns>
        /// <response code="200">Vraća listu dokumenata</response>
        /// <response code="204">Nije pronadjen nijedan dokument</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<DokumentDto>>> GetAllDokument()
        {
            var documents = await _unitOfWork.Dokument.GetAllDokument();

            if (documents == null || documents.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllDokument", "Lista dokumenata je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllDokument", "Lista dokumenata je uspešno vraćena.");

            return Ok(_mapper.Map<List<DokumentDto>>(documents));
        }

        /// <summary>
        ///     Vraća jedan dokument na osnovu ID-a
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
            var document = await _unitOfWork.Dokument.GetDokumentById(id);

            if (document == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetDokumentById",
                    $"Dokument sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetDokumentById",
                $"Dokument sa id-jem {id} je uspešno vraćen.");

            return Ok(_mapper.Map<DokumentDto>(document));
        }

        /// <summary>
        ///     Kreira novi dokument
        /// </summary>
        /// <param name="dokumentDto">Model dokumenta</param>
        /// <returns>Dokument</returns>
        /// <response code="201">Vraća kreirani dokument</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDokument([FromBody] CreateDokumentDto dokumentDto)
        {
            try
            {
                var document = _mapper.Map<Dokument>(dokumentDto);

                _unitOfWork.Dokument.CreateDokument(document);
                await _unitOfWork.CompleteAsync();

                document.TipDokumenta = await _unitOfWork.TipDokumenta.GetTipDokumentaById(document.TipDokumentaId);

                await _loggerService.Log(LogLevel.Information, "CreateDokument",
                    $"Dokument sa vrednostima: {JsonConvert.SerializeObject(document)} je uspešno kreiran.");

                return CreatedAtAction(
                    "GetDokumentById",
                    new {id = document.Id},
                    _mapper.Map<DokumentDto>(document)
                );
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateDokument",
                    $"Greška prilikom kreiranja dokumenta sa vrednostima: {JsonConvert.SerializeObject(dokumentDto)}.",
                    ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom kreiranja dokumenta.");
            }
        }

        /// <summary>
        ///     Izmena dokumenta
        /// </summary>
        /// <param name="id">ID dokumenta</param>
        /// <param name="dokumentDto">Model dokumenta</param>
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
            try
            {
                if (id != dokumentDto.Id)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateDokument",
                        "ID dokumenta prosledjen kroz url nije isti kao onaj u telu zahteva.");
                    return BadRequest();
                }

                var document = await _unitOfWork.Dokument.GetDokumentById(id);
                var oldValue = JsonConvert.SerializeObject(document);

                if (document == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateDokument",
                        $"Dokument sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                _mapper.Map(dokumentDto, document, typeof(DokumentDto), typeof(Dokument));
                await _unitOfWork.CompleteAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateDokument",
                    $"Dokument sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateDokument",
                    $"Greška prilikom izmene dokumenta sa vrednostima: {JsonConvert.SerializeObject(dokumentDto)}.",
                    ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene dokumenta.");
            }
        }

        /// <summary>
        ///     Brisanje dokumenta na osnovu ID-a
        /// </summary>
        /// <param name="id">ID dokumenta</param>
        /// <response code="204">Dokument je uspešno obrisan</response>
        /// <response code="404">Nije pronadjen dokument za uneti ID</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDokument(Guid id)
        {
            var document = await _unitOfWork.Dokument.GetDokumentById(id);

            if (document == null)
            {
                await _loggerService.Log(LogLevel.Warning, "DeleteDokument",
                    $"Dokument sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            _unitOfWork.Dokument.DeleteDokument(document);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "DeleteDokument",
                $"Dokument sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(document)}");

            return NoContent();
        }

        /// <summary>
        ///     Vraća opcije za rad sa dokumentima
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