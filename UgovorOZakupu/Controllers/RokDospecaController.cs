using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UgovorOZakupu.Data.RokDospeca;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.RokDospeca;
using UgovorOZakupu.Services.Logger;

namespace UgovorOZakupu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RokDospecaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRokDospecaRepository _rokDospecaRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILoggerService _loggerService;

        public RokDospecaController(IRokDospecaRepository rokDospecaRepository, IUnitOfWork unitOfWork, IMapper mapper,
            ILoggerService loggerService)
        {
            _rokDospecaRepository = rokDospecaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RokDospecaDto>>> GetAllRokDospeca()
        {
            var rokoviDospeca = await _rokDospecaRepository.GetAllRokDospeca();

            if (rokoviDospeca == null || rokoviDospeca.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllRokDospeca",
                    "Lista rokova dospeca je prazna ili null.");
                return NoContent();
            }

            await _loggerService.Log(LogLevel.Information, "GetAllRokDospeca",
                "Lista rokova dospeca je uspešno vraćena.");

            return _mapper.Map<List<RokDospecaDto>>(rokoviDospeca);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RokDospecaDto>> GetRokDospecaById(Guid id)
        {
            var rokDospeca = await _rokDospecaRepository.GetRokDospecaById(id);

            if (rokDospeca == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetRokDospecaById",
                    $"Rok dospeca sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetRokDospecaById",
                $"Rok dospeca sa id-jem {id} je uspešno vraćen.");

            return _mapper.Map<RokDospecaDto>(rokDospeca);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRokDospeca([FromBody] CreateRokDospecaDto rokDospecaDto)
        {
            var rokDospeca = _mapper.Map<RokDospeca>(rokDospecaDto);

            _rokDospecaRepository.CreateRokDospeca(rokDospeca);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "CreateRokDospeca",
                $"Rok dospeca sa vrednostima: {JsonConvert.SerializeObject(rokDospeca)} je uspešno kreiran.");

            return CreatedAtAction(
                "GetRokDospecaById",
                new {id = rokDospeca.Id},
                _mapper.Map<RokDospecaDto>(rokDospeca)
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRokDospeca(Guid id, [FromBody] UpdateRokDospecaDto rokDospecaDto)
        {
            if (id != rokDospecaDto.Id)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateRokDospeca",
                    "ID roka dospeca prosledjen kroz url nije isti kao onaj u telu zahteva.");
                return BadRequest();
            }

            var rokDospeca = await _rokDospecaRepository.GetRokDospecaById(id);

            if (rokDospeca == null)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateRokDospeca",
                    $"Rok dospeca sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            var oldValue = JsonConvert.SerializeObject(rokDospeca);

            _mapper.Map(rokDospecaDto, rokDospeca, typeof(UpdateRokDospecaDto), typeof(RokDospeca));
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "UpdateRokDospeca",
                $"Rok dospeca sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRokDospeca(Guid id)
        {
            var rokDospeca = await _rokDospecaRepository.GetRokDospecaById(id);

            if (rokDospeca == null)
            {
                await _loggerService.Log(LogLevel.Warning, "DeleteRokDospeca",
                    $"Rok dospeca sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            _rokDospecaRepository.DeleteRokDospeca(rokDospeca);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "DeleteRokDospeca",
                $"Rok dospeca sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(rokDospeca)}");

            return NoContent();
        }
    }
}