using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Models.UgovorOZakupu;
using UgovorOZakupu.Services.Logger;
using UgovorOZakupu.Services.ServiceCalls;

namespace UgovorOZakupu.Controllers
{
    /// <summary>
    ///     Kontroler za ugovor o zakupu
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UgovorOZakupuController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        private readonly IServiceCalls _serviceCalls;

        private readonly IUnitOfWork _unitOfWork;

        public UgovorOZakupuController(IUnitOfWork unitOfWork,
            IMapper mapper, ILoggerService loggerService, IServiceCalls serviceCalls)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerService = loggerService;
            _serviceCalls = serviceCalls;
        }

        /// <summary>
        ///     Vraća sve ugovore o zakupu
        /// </summary>
        /// <returns>Lista ugovora o zakupu</returns>
        /// <response code="200">Vraća listu ugovora o zakupu</response>
        /// <response code="204">Nije pronadjen nijedan ugovor o zakupu</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<UgovorOZakupuDto>>> GetAllUgovorOZakupu()
        {
            var ugovori = await _unitOfWork.UgovoriOZakupu.GetAll();

            if (ugovori == null || ugovori.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllUgovorOZakupu",
                    "Lista ugovora o zakupu je prazna ili null.");
                return NoContent();
            }

            var ugovoriDto = Task.WhenAll(
                    ugovori.Select(u => _serviceCalls.GetUgovorOZakupuInfo(u))
                )
                .Result
                .ToList();

            await _loggerService.Log(LogLevel.Information, "GetAllUgovorOZakupu",
                "Lista ugovora o zakupu je uspešno vraćena.");

            return Ok(ugovoriDto);
        }

        /// <summary>
        ///     Vraća jedan ugovor o zakupu na osnovu ID-a
        /// </summary>
        /// <param name="id">ID ugovora o zakupu</param>
        /// <returns>Ugovor o zakupu</returns>
        /// <response code="200">Vraća traženi ugovor o zakupu</response>
        /// <response code="404">Nije pronadjen ugovor o zakupu za uneti ID</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UgovorOZakupuDto>> GetUgovorOZakupuById(Guid id)
        {
            var ugovor = await _unitOfWork.UgovoriOZakupu.GetById(id);

            if (ugovor == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetUgovorOZakupuById",
                    $"Ugovor o zakupu sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            var ugovorDto = await _serviceCalls.GetUgovorOZakupuInfo(ugovor);

            await _loggerService.Log(LogLevel.Information, "GetUgovorOZakupuById",
                $"Ugovor o zakupu sa id-jem {id} je uspešno vraćen.");

            return Ok(ugovorDto);
        }

        /// <summary>
        ///     Kreira novi ugovor o zakupu
        /// </summary>
        /// <param name="ugovorOZakupuDto">Model ugovora o zakupu za kreiranje</param>
        /// <returns>Ugovor o zakupu</returns>
        /// <response code="201">Vraća kreirani ugovor o zakupu</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUgovorOZakupu([FromBody] CreateUgovorOZakupuDto ugovorOZakupuDto)
        {
            var ugovor = _mapper.Map<Entities.UgovorOZakupu>(ugovorOZakupuDto);

            _unitOfWork.UgovoriOZakupu.Create(ugovor);
            await _unitOfWork.CompleteAsync();

            ugovor.TipGarancije =
                await _unitOfWork.TipoviGarancije.GetById(ugovor.TipGarancijeId);

            var serialized = JsonConvert.SerializeObject(ugovor, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await _loggerService.Log(LogLevel.Information, "CreateUgovorOZakupu",
                $"Ugovor o zakupu sa vrednostima: {serialized} je uspešno kreiran.");

            return CreatedAtAction(
                "GetUgovorOZakupuById",
                new {id = ugovor.Id},
                _mapper.Map<UgovorOZakupuDto>(ugovor)
            );
        }

        /// <summary>
        ///     Izmena ugovor o zakupu
        /// </summary>
        /// <param name="id">ID ugovora o zakupu</param>
        /// <param name="ugovorOZakupuDto">Model ugovora o zakupu za izmenu</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Potvrda o izmeni ugovora o zakupu</response>
        /// <response code="404">Nije pronadjen ugovor o zakupu za uneti ID</response>
        /// <response code="400">ID nije isti kao onaj proledjen u modelu ugovora o zakupu</response>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUgovorOZakupu(Guid id,
            [FromBody] UpdateUgovorOZakupuDto ugovorOZakupuDto)
        {
            if (id != ugovorOZakupuDto.Id)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateUgovorOZakupu",
                    "ID ugovora o zakupua prosledjen kroz url nije isti kao onaj u telu zahteva.");
                return BadRequest();
            }

            var ugovor = await _unitOfWork.UgovoriOZakupu.GetById(id);

            if (ugovor == null)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateUgovorOZakupu",
                    $"Ugovor o zakupu sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            var oldValue = JsonConvert.SerializeObject(ugovor, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            _mapper.Map(ugovorOZakupuDto, ugovor, typeof(UpdateUgovorOZakupuDto), typeof(Entities.UgovorOZakupu));
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "UpdateUgovorOZakupu",
                $"Ugovor o zakupu sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");

            return NoContent();
        }

        /// <summary>
        ///     Brisanje ugovora o zakupu na osnovu ID-a
        /// </summary>
        /// <param name="id">ID ugovora o zakupu</param>
        /// <response code="204">Ugovor o zakupu je uspešno obrisan</response>
        /// <response code="404">Nije pronadjen ugovor o zakupu za uneti ID</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUgovorOZakupu(Guid id)
        {
            var ugovor = await _unitOfWork.UgovoriOZakupu.GetById(id);

            if (ugovor == null)
            {
                await _loggerService.Log(LogLevel.Warning, "DeleteUgovorOZakupu",
                    $"Ugovor o zakupu sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            _unitOfWork.UgovoriOZakupu.Delete(ugovor);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "DeleteUgovorOZakupu",
                $"Ugovor o zakupu sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(ugovor)}");

            return NoContent();
        }

        /// <summary>
        ///     Vraća opcije za rad sa ugovorima o zakupu
        /// </summary>
        /// <response code="200">Vraća listu opcija u header-u</response>
        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUgovorOZakupuOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}