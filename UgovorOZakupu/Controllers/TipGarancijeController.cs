using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.TipGarancije;
using UgovorOZakupu.Services.ServiceCalls;

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
        private readonly IMapper _mapper;
        private readonly IServiceCalls _serviceCalls;
        private readonly IUnitOfWork _unitOfWork;

        public TipGarancijeController(IUnitOfWork unitOfWork, IServiceCalls serviceCalls, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _serviceCalls = serviceCalls;
            _mapper = mapper;
        }

        /// <summary>
        ///     Vraća sve tipove garancije
        /// </summary>
        /// <returns>Lista tipova garancije</returns>
        /// <response code="200">Vraća listu tipove garancije</response>
        /// <response code="204">Nije pronadjen nijedan tip garancije</response>
        /// <response code="500">Greška prilikom vraćanja liste tipova garancije</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, Menadzer")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TipGarancijeDto>>> GetAllTipGarancije()
        {
            try
            {
                var tipoviGarancije = await _unitOfWork.TipoviGarancije.GetAll();

                if (tipoviGarancije == null || tipoviGarancije.Count == 0)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "GetAllTipGarancije",
                        "Lista tipova garancije je prazna ili null.");
                    return NoContent();
                }

                await _serviceCalls.Log(LogLevel.Information, "GetAllTipGarancije",
                    "Lista tipova garancije je uspešno vraćena.");

                return Ok(_mapper.Map<List<TipGarancijeDto>>(tipoviGarancije));
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "GetAllTipGarancije",
                    "Greška prilikom vraćanja liste tipova garancije.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Greška prilikom vraćanja liste tipova garancije.");
            }
        }

        /// <summary>
        ///     Vraća jedan tip grancije na osnovu ID-a
        /// </summary>
        /// <param name="id">ID tipa grancije</param>
        /// <returns>Tip grancije</returns>
        /// <response code="200">Vraća traženi tip grancije o zakupu</response>
        /// <response code="404">Nije pronadjen tip grancije za uneti ID</response>
        /// <response code="500">Greška prilikom vraćanja tipa garancije</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, Menadzer")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TipGarancijeDto>> GetTipGarancijeById(Guid id)
        {
            try
            {
                var tipGarancije = await _unitOfWork.TipoviGarancije.GetById(id);

                if (tipGarancije == null)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "GetTipGarancijeById",
                        $"Tip garancije sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                await _serviceCalls.Log(LogLevel.Information, "GetTipGarancijeById",
                    $"Tip garancije sa id-jem {id} je uspešno vraćen.");

                return Ok(_mapper.Map<TipGarancijeDto>(tipGarancije));
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "GetTipGarancijeById",
                    $"Greška prilikom vraćanja tipa garancije sa id-jem {id}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Greška prilikom vraćanja tipa garancije sa id-jem {id}.");
            }
        }

        /// <summary>
        ///     Kreira novi tip garancije
        /// </summary>
        /// <param name="tipGarancijeDto">Model tipa garancije</param>
        /// <returns>Tip garancije</returns>
        /// <response code="201">Vraća kreirani tip garancije</response>
        /// <response code="500">Greška prilikom kreiranja tipa garancije.</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTipGarancije([FromBody] TipGarancijeDto tipGarancijeDto)
        {
            try
            {
                var tipGarancije = _mapper.Map<TipGarancije>(tipGarancijeDto);

                _unitOfWork.TipoviGarancije.Create(tipGarancije);
                await _unitOfWork.CompleteAsync();

                await _serviceCalls.Log(LogLevel.Information, "CreateTipGarancije",
                    $"Tip garancije sa vrednostima: {JsonConvert.SerializeObject(tipGarancije)} je uspešno kreiran.");

                return CreatedAtAction(
                    "GetTipGarancijeById",
                    new {id = tipGarancije.Id},
                    _mapper.Map<TipGarancijeDto>(tipGarancije)
                );
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "CreateTipGarancije",
                    $"Greška prilikom kreiranja tipa garancije sa vrednostima: {JsonConvert.SerializeObject(tipGarancijeDto)}.",
                    ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Greška prilikom kreiranja tipa garancije.");
            }
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
        /// <response code="500">Greška prilikom izmene tipa garancije.</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTipGarancije(Guid id, [FromBody] UpdateTipGarancijeDto tipGarancijeDto)
        {
            try
            {
                if (id != tipGarancijeDto.Id)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "UpdateTipGarancije",
                        "ID tipa garancije prosledjen kroz url nije isti kao onaj u telu zahteva.");
                    return BadRequest();
                }

                var tipGarancije = await _unitOfWork.TipoviGarancije.GetById(id);

                if (tipGarancije == null)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "UpdateTipGarancije",
                        $"Tip garancije sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                var oldValue = JsonConvert.SerializeObject(tipGarancije);

                _mapper.Map(tipGarancijeDto, tipGarancije, typeof(UpdateTipGarancijeDto), typeof(TipGarancije));
                await _unitOfWork.CompleteAsync();

                await _serviceCalls.Log(LogLevel.Information, "UpdateTipGarancije",
                    $"Tip garancije sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "UpdateTipGarancije",
                    $"Greška prilikom izmene tipa garancije sa vrednostima: {JsonConvert.SerializeObject(tipGarancijeDto)}.",
                    ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene tipa garancije.");
            }
        }

        /// <summary>
        ///     Brisanje tipa garancije na osnovu ID-a
        /// </summary>
        /// <param name="id">ID tipa garancije</param>
        /// <response code="204">Tip garancije je uspešno obrisan</response>
        /// <response code="404">Nije pronadjen tip garancije za uneti ID</response>
        /// <response code="500">Greška prilikom brisanja tipa garancije</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTipGarancije(Guid id)
        {
            try
            {
                var tipGarancije = await _unitOfWork.TipoviGarancije.GetById(id);

                if (tipGarancije == null)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "DeleteTipGarancije",
                        $"Tip garancije sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                _unitOfWork.TipoviGarancije.Delete(tipGarancije);
                await _unitOfWork.CompleteAsync();

                await _serviceCalls.Log(LogLevel.Information, "DeleteTipGarancije",
                    $"Tip garancije sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(tipGarancije)}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "DeleteTipGarancije",
                    $"Greška prilikom brisanja tipa garancije sa id-jem {id}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Greška prilikom brisanja tipa garancije sa id-jem {id}.");
            }
        }

        /// <summary>
        ///     Vraća opcije za rad sa tipovima garancije
        /// </summary>
        /// <response code="200">Vraća listu opcija u header-u</response>
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar, Menadzer")]
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTipGarancijeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}