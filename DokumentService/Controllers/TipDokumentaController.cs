using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DokumentService.Data.UnitOfWork;
using DokumentService.Entities;
using DokumentService.Models.TipDokumenta;
using DokumentService.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DokumentService.Controllers
{
    /// <summary>
    ///     Kontroler za tip dokumenta
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TipDokumentaController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TipDokumentaController(IUnitOfWork unitOfWork, ILoggerService loggerService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _loggerService = loggerService;
            _mapper = mapper;
        }

        /// <summary>
        ///     Vraća sve tipove dokumenta
        /// </summary>
        /// <returns>Lista tipova dokumenta</returns>
        /// <response code="200">Vraća listu tipova dokumenta</response>
        /// <response code="204">Nije pronadjen nijedan tip</response>
        /// <response code="204">Greška prilikom vraćanja liste tipova dokumenta</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, PrvaKomisija")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TipDokumentaDto>>> GetAllTipDokumenta()
        {
            try
            {
                var tipoviDokumenta = await _unitOfWork.TipDokumenta.GetAllTipDokumenta();

                if (tipoviDokumenta == null || tipoviDokumenta.Count == 0)
                {
                    await _loggerService.Log(LogLevel.Warning, "GetAllTipDokumenta",
                        "Lista tipova dokumenta je prazna ili null.");
                    return NoContent();
                }

                await _loggerService.Log(LogLevel.Information, "GetAllTipDokumenta",
                    "Lista tipova dokumenta je uspešno vraćena.");

                return Ok(_mapper.Map<List<TipDokumentaDto>>(tipoviDokumenta));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "GetAllTipDokumenta",
                    "Greška prilikom vraćanja liste tipova dokumenta.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Greška prilikom vraćanja liste tipova dokumenta.");
            }
        }

        /// <summary>
        ///     Vraća jedan tip dokumenta na osnovu ID-a
        /// </summary>
        /// <param name="id">ID dokumenta</param>
        /// <returns>Tip dokumenta</returns>
        /// <response code="200">Vraća traženi tip dokumenta</response>
        /// <response code="404">Nije pronadjen tip dokumenta za uneti ID</response>
        /// <response code="500">Greška prilikom vraćanja tipa dokumenta</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, PrvaKomisija")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TipDokumentaDto>> GetTipDokumentaById(Guid id)
        {
            try
            {
                var tipDokumenta = await _unitOfWork.TipDokumenta.GetTipDokumentaById(id);

                if (tipDokumenta == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "GetTipDokumentaById",
                        $"Tip dokumenta sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                await _loggerService.Log(LogLevel.Information, "GetTipDokumentaById",
                    $"Tip dokumenta sa id-jem {id} je uspešno vraćen.");

                return Ok(_mapper.Map<TipDokumentaDto>(tipDokumenta));
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "GetTipDokumentaById",
                    $"Greška prilikom vraćanja tipa dokumenta sa id-jem {id}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Greška prilikom vraćanja tipa dokumenta sa id-jem {id}.");
            }
        }

        /// <summary>
        ///     Kreira novi tip dokumenta
        /// </summary>
        /// <param name="tipDokumentaDto">Model tipa dokumenta za kreiranje</param>
        /// <returns>Tip dokumenta</returns>
        /// <response code="201">Vraća kreirani tip dokumenta</response>
        /// <response code="500">Greška prilikom kreiranja tipa dokumenta</response>
        [Authorize(Roles = "Administrator, Superuser, PrvaKomisija")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTipDokumenta([FromBody] CreateTipDokumentaDto tipDokumentaDto)
        {
            try
            {
                var tipDokumenta = _mapper.Map<TipDokumenta>(tipDokumentaDto);

                _unitOfWork.TipDokumenta.CreateTipDokumenta(tipDokumenta);
                await _unitOfWork.CompleteAsync();

                await _loggerService.Log(LogLevel.Information, "CreateTipDokumenta",
                    $"Tip dokumenta sa vrednostima: {JsonConvert.SerializeObject(tipDokumenta)} je uspešno kreiran.");

                return CreatedAtAction(
                    "GetTipDokumentaById",
                    new {id = tipDokumenta.Id},
                    _mapper.Map<TipDokumentaDto>(tipDokumenta)
                );
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "CreateTipDokumenta",
                    $"Greška prilikom kreiranja tipa dokumenta sa vrednostima: {JsonConvert.SerializeObject(tipDokumentaDto)}.",
                    ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Greška prilikom kreiranja tipa dokumenta.");
            }
        }

        /// <summary>
        ///     Izmena tipa dokumenta
        /// </summary>
        /// <param name="id">ID tipa dokumenta za izmenu</param>
        /// <param name="tipDokumentaDto">Model tipa dokumenta</param>
        /// <response code="204">Potvrda o izmeni tipa dokumenta</response>
        /// <response code="404">Nije pronadjen tip dokumenta za uneti ID</response>
        /// <response code="400">ID nije isti kao onaj proledjen u modelu tipa dokumenta</response>
        /// <response code="500">Greška prilikom izmene tipa dokumenta</response>
        [Authorize(Roles = "Administrator, Superuser, PrvaKomisija")]
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTipDokumenta(Guid id, [FromBody] UpdateTipDokumentaDto tipDokumentaDto)
        {
            try
            {
                if (id != tipDokumentaDto.Id)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateTipDokumenta",
                        "ID tipa dokumenta prosledjen kroz url nije isti kao onaj u telu zahteva.");
                    return BadRequest();
                }

                var tipDokumenta = await _unitOfWork.TipDokumenta.GetTipDokumentaById(id);
                var oldValue = JsonConvert.SerializeObject(tipDokumenta);

                if (tipDokumenta == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "UpdateTipDokumenta",
                        $"Tip dokumenta sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                _mapper.Map(tipDokumentaDto, tipDokumenta, typeof(UpdateTipDokumentaDto), typeof(TipDokumenta));
                await _unitOfWork.CompleteAsync();

                await _loggerService.Log(LogLevel.Information, "UpdateTipDokumenta",
                    $"Tip dokumenta sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "UpdateTipDokumenta",
                    $"Greška prilikom izmene tipa dokumenta sa vrednostima: {JsonConvert.SerializeObject(tipDokumentaDto)}.",
                    ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene tipa dokumenta.");
            }
        }

        /// <summary>
        ///     Brisanje tipa dokumenta na osnovu ID-a
        /// </summary>
        /// <param name="id">ID tipa dokumenta</param>
        /// <response code="204">Tip dokumenta je uspešno obrisan</response>
        /// <response code="404">Nije pronadjen tip dokumenta za uneti ID</response>
        /// <response code="500">Greška prilikom brisanja tipa dokumenta</response>
        [Authorize(Roles = "Administrator, Superuser, PrvaKomisija")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTipDokumenta(Guid id)
        {
            try
            {
                var tipDokumenta = await _unitOfWork.TipDokumenta.GetTipDokumentaById(id);

                if (tipDokumenta == null)
                {
                    await _loggerService.Log(LogLevel.Warning, "DeleteTipDokumenta",
                        $"Tip dokumenta sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                _unitOfWork.TipDokumenta.DeleteTipDokumenta(tipDokumenta);
                await _unitOfWork.CompleteAsync();

                await _loggerService.Log(LogLevel.Information, "DeleteTipDokumenta",
                    $"Tip dokumenta sa id-em {id} je uspešno obrisana. Obrisane vrednosti: {JsonConvert.SerializeObject(tipDokumenta)}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _loggerService.Log(LogLevel.Error, "DeleteTipDokumenta",
                    $"Greška prilikom brisanja tipa dokumenta sa id-jem {id}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Greška prilikom brisanja tipa dokumenta sa id-jem {id}.");
            }
        }

        /// <summary>
        ///     Vraća opcije za rad sa tipovima dokumenta
        /// </summary>
        /// <response code="200">Vraća listu opcija u header-u</response>
        [Authorize(Roles = "Administrator, Superuser, Menadzer, PrvaKomisija")]
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTipDokumentaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}