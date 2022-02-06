using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.TipGarancije;
using UgovorOZakupu.Services.Logger;

namespace UgovorOZakupu.Controllers
{
    /// <summary>
    ///     Kontroler za tip garancije
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TipGarancijeController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TipGarancijeController(IUnitOfWork unitOfWork, IMapper mapper, ILoggerService loggerService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        ///     Vraća sve tipove garancije
        /// </summary>
        /// <returns>Lista tipova garancije</returns>
        /// <response code="200">Vraća listu tipove garancije</response>
        /// <response code="204">Nije pronadjen nijedan tip garancije</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<TipGarancijeDto>>> GetAllTipGarancije()
        {
            var tipoviGarancije = await _unitOfWork.TipoviGarancije.GetAll();

            if (tipoviGarancije == null || tipoviGarancije.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllTipGarancije",
                    "Lista tipova garancije je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllTipGarancije",
                "Lista tipova garancije je uspešno vraćena.");

            return Ok(_mapper.Map<List<TipGarancijeDto>>(tipoviGarancije));
        }

        /// <summary>
        ///     Vraća jedan tip grancije na osnovu ID-a
        /// </summary>
        /// <param name="id">ID tipa grancije</param>
        /// <returns>Tip grancije</returns>
        /// <response code="200">Vraća traženi tip grancije o zakupu</response>
        /// <response code="404">Nije pronadjen tip grancije za uneti ID</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TipGarancijeDto>> GetTipGarancijeById(Guid id)
        {
            var tipGarancije = await _unitOfWork.TipoviGarancije.GetById(id);

            if (tipGarancije == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetTipGarancijeById",
                    $"Tip garancije sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetTipGarancijeById",
                $"Tip garancije sa id-jem {id} je uspešno vraćen.");

            return Ok(_mapper.Map<TipGarancijeDto>(tipGarancije));
        }

        /// <summary>
        ///     Kreira novi tip garancije
        /// </summary>
        /// <param name="tipGarancijeDto">Model tipa garancije</param>
        /// <returns>Tip garancije</returns>
        /// <response code="201">Vraća kreirani tip garancije</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTipGarancije([FromBody] TipGarancijeDto tipGarancijeDto)
        {
            var tipGarancije = _mapper.Map<TipGarancije>(tipGarancijeDto);

            _unitOfWork.TipoviGarancije.Create(tipGarancije);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "CreateTipGarancije",
                $"Tip garancije sa vrednostima: {JsonConvert.SerializeObject(tipGarancije)} je uspešno kreiran.");

            return CreatedAtAction(
                "GetTipGarancijeById",
                new {id = tipGarancije.Id},
                _mapper.Map<TipGarancijeDto>(tipGarancije)
            );
        }

        /// <summary>
        ///     Izmena tipa garancije
        /// </summary>
        /// <param name="id">ID tipa garancije</param>
        /// <param name="tipGarancijeDto">Model tipa garancije</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Potvrda o izmeni tipa garancije</response>
        /// <response code="404">Nije pronadjen tip garancije za uneti ID</response>
        /// <response code="400">ID nije isti kao onaj proledjen u modelu tipa garancije</response>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTipGarancije(Guid id, [FromBody] UpdateTipGarancijeDto tipGarancijeDto)
        {
            if (id != tipGarancijeDto.Id)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateTipGarancije",
                    "ID tipa garancije prosledjen kroz url nije isti kao onaj u telu zahteva.");
                return BadRequest();
            }

            var tipGarancije = await _unitOfWork.TipoviGarancije.GetById(id);

            if (tipGarancije == null)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateTipGarancije",
                    $"Tip garancije sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            var oldValue = JsonConvert.SerializeObject(tipGarancije);

            _mapper.Map(tipGarancijeDto, tipGarancije, typeof(UpdateTipGarancijeDto), typeof(TipGarancije));
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "UpdateTipGarancije",
                $"Tip garancije sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");

            return NoContent();
        }

        /// <summary>
        ///     Brisanje tipa garancije na osnovu ID-a
        /// </summary>
        /// <param name="id">ID tipa garancije</param>
        /// <response code="204">Tip garancije je uspešno obrisan</response>
        /// <response code="404">Nije pronadjen tip garancije za uneti ID</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTipGarancije(Guid id)
        {
            var tipGarancije = await _unitOfWork.TipoviGarancije.GetById(id);

            if (tipGarancije == null)
            {
                await _loggerService.Log(LogLevel.Warning, "DeleteTipGarancije",
                    $"Tip garancije sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            _unitOfWork.TipoviGarancije.Delete(tipGarancije);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "DeleteTipGarancije",
                $"Tip garancije sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(tipGarancije)}");

            return NoContent();
        }

        /// <summary>
        ///     Vraća opcije za rad sa tipovima garancije
        /// </summary>
        /// <response code="200">Vraća listu opcija u header-u</response>
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTipGarancijeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}