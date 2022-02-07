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
using UgovorOZakupu.Models.RokDospeca;
using UgovorOZakupu.Services.ServiceCalls;

namespace UgovorOZakupu.Controllers
{
    /// <summary>
    ///     Kontroler za rok dospeca
    /// </summary>
    /// 
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RokDospecaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceCalls _serviceCalls;
        private readonly IUnitOfWork _unitOfWork;

        public RokDospecaController(IUnitOfWork unitOfWork, IServiceCalls serviceCalls, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _serviceCalls = serviceCalls;
            _mapper = mapper;
        }

        /// <summary>
        ///     Vraća sve rokove dospeca
        /// </summary>
        /// <returns>Lista rokova dospeca</returns>
        /// <response code="200">Vraća listu rokova dospeca</response>
        /// <response code="204">Nije pronadjen nijedan rok dospeca</response>
        /// <response code="500">Greška prilikom vraćanja liste rokova dospeća</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RokDospecaDto>>> GetAllRokDospeca()
        {
            try
            {
                var rokoviDospeca = await _unitOfWork.RokoviDospeca.GetAll();

                if (rokoviDospeca == null || rokoviDospeca.Count == 0)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "GetAllRokDospeca",
                        "Lista rokova dospeca je prazna ili null.");
                    return NoContent();
                }

                await _serviceCalls.Log(LogLevel.Information, "GetAllRokDospeca",
                    "Lista rokova dospeca je uspešno vraćena.");

                return _mapper.Map<List<RokDospecaDto>>(rokoviDospeca);
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "GetAllRokDospeca",
                    "Greška prilikom vraćanja liste rokova dospeća.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Greška prilikom vraćanja liste rokova dospeća.");
            }
        }

        /// <summary>
        ///     Vraća jedan rok garancije na osnovu ID-a
        /// </summary>
        /// <param name="id">ID roka garancije</param>
        /// <returns>Rok garancije</returns>
        /// <response code="200">Vraća traženi rok garancije</response>
        /// <response code="404">Nije pronadjen rok garancije za uneti ID</response>
        /// <response code="500">Greška prilikom vraćanja roka dospeća</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RokDospecaDto>> GetRokDospecaById(Guid id)
        {
            try
            {
                var rokDospeca = await _unitOfWork.RokoviDospeca.GetById(id);

                if (rokDospeca == null)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "GetRokDospecaById",
                        $"Rok dospeca sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                await _serviceCalls.Log(LogLevel.Information, "GetRokDospecaById",
                    $"Rok dospeca sa id-jem {id} je uspešno vraćen.");

                return _mapper.Map<RokDospecaDto>(rokDospeca);
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "GetRokDospecaById",
                    $"Greška prilikom vraćanja roka dospeća sa id-jem {id}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Greška prilikom vraćanja roka dospeća sa id-jem {id}.");
            }
        }

        /// <summary>
        ///     Kreira novi rok dospeca
        /// </summary>
        /// <param name="rokDospecaDto">Model roka dospeca za kreiranje</param>
        /// <returns>Rok dospeca</returns>
        /// <response code="201">Vraća kreirani rok dospeca</response>
        /// <response code="500">Greška prilikom kreiranja roka dospeća</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRokDospeca([FromBody] CreateRokDospecaDto rokDospecaDto)
        {
            try
            {
                var rokDospeca = _mapper.Map<RokDospeca>(rokDospecaDto);

                _unitOfWork.RokoviDospeca.Create(rokDospeca);
                await _unitOfWork.CompleteAsync();

                await _serviceCalls.Log(LogLevel.Information, "CreateRokDospeca",
                    $"Rok dospeca sa vrednostima: {JsonConvert.SerializeObject(rokDospeca)} je uspešno kreiran.");

                return CreatedAtAction(
                    "GetRokDospecaById",
                    new {id = rokDospeca.Id},
                    _mapper.Map<RokDospecaDto>(rokDospeca)
                );
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "CreateRokDospeca",
                    $"Greška prilikom kreiranja roka dospeća sa vrednostima: {JsonConvert.SerializeObject(rokDospecaDto)}.",
                    ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom kreiranja roka dospeća.");
            }
        }

        /// <summary>
        ///     Izmena roka dospeca
        /// </summary>
        /// <param name="id">ID roka dospeca</param>
        /// <param name="rokDospecaDto">Model roka dospeca za izmenu</param>
        /// <response code="204">Potvrda o izmeni roka dospeca</response>
        /// <response code="404">Nije pronadjen rok dospeca za uneti ID</response>
        /// <response code="400">ID nije isti kao onaj proledjen u modelu roka dospeca</response>
        /// <response code="500">Greška prilikom izmene roka dospeća</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRokDospeca(Guid id, [FromBody] UpdateRokDospecaDto rokDospecaDto)
        {
            try
            {
                if (id != rokDospecaDto.Id)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "UpdateRokDospeca",
                        "ID roka dospeca prosledjen kroz url nije isti kao onaj u telu zahteva.");
                    return BadRequest();
                }

                var rokDospeca = await _unitOfWork.RokoviDospeca.GetById(id);

                if (rokDospeca == null)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "UpdateRokDospeca",
                        $"Rok dospeca sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                var oldValue = JsonConvert.SerializeObject(rokDospeca);

                _mapper.Map(rokDospecaDto, rokDospeca, typeof(UpdateRokDospecaDto), typeof(RokDospeca));
                await _unitOfWork.CompleteAsync();

                await _serviceCalls.Log(LogLevel.Information, "UpdateRokDospeca",
                    $"Rok dospeca sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "UpdateRokDospeca",
                    $"Greška prilikom izmene roka dospeća sa vrednostima: {JsonConvert.SerializeObject(rokDospecaDto)}.",
                    ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene roka dospeća.");
            }
        }

        /// <summary>
        ///     Brisanje roka dospeca na osnovu ID-a
        /// </summary>
        /// <param name="id">ID roka dospeca</param>
        /// <response code="204">Rok dospeca je uspešno obrisan</response>
        /// <response code="404">Nije pronadjen rok dospeca za uneti ID</response>
        /// <response code="500">Greška prilikom brisanja roka dospeća</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, TehnickiSekretar")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRokDospeca(Guid id)
        {
            try
            {
                var rokDospeca = await _unitOfWork.RokoviDospeca.GetById(id);

                if (rokDospeca == null)
                {
                    await _serviceCalls.Log(LogLevel.Warning, "DeleteRokDospeca",
                        $"Rok dospeca sa id-jem {id} nije pronadjen.");
                    return NotFound();
                }

                _unitOfWork.RokoviDospeca.Delete(rokDospeca);
                await _unitOfWork.CompleteAsync();

                await _serviceCalls.Log(LogLevel.Information, "DeleteRokDospeca",
                    $"Rok dospeca sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(rokDospeca)}");

                return NoContent();
            }
            catch (Exception ex)
            {
                await _serviceCalls.Log(LogLevel.Error, "DeleteRokDospeca",
                    $"Greška prilikom brisanja roka dospeća sa id-jem {id}.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Greška prilikom brisanja roka dospeća sa id-jem {id}.");
            }
        }

        /// <summary>
        ///     Vraća opcije za rad sa rokovima dospeca
        /// </summary>
        /// <response code="200">Vraća listu opcija u header-u</response>
        /// 
        [Authorize(Roles = "Administrator, Superuser, Menadzer, TehnickiSekretar")]
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetRokDospecaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}