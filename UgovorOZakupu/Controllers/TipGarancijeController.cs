using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UgovorOZakupu.Data.TipGarancije;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.TipGarancije;
using UgovorOZakupu.Services.Logger;

namespace UgovorOZakupu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipGarancijeController : ControllerBase
    {
        private readonly ITipGaranceijeRepository _tipGaranceijeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly ILoggerService _loggerService;
        
        public TipGarancijeController(ITipGaranceijeRepository tipGaranceijeRepository, IUnitOfWork unitOfWork, IMapper mapper, ILoggerService loggerService)
        {
            _tipGaranceijeRepository = tipGaranceijeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipGarancijeDto>>> GetAllTipGarancije()
        {
            var tipoviGarancije = await _tipGaranceijeRepository.GetAllTipGarancije();
            
            if (tipoviGarancije == null || tipoviGarancije.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllTipGarancije", "Lista tipova garancije je prazna ili null.");
                return NoContent();
            }
            
            await _loggerService.Log(LogLevel.Information, "GetAllTipGarancije", "Lista tipova garancije je uspešno vraćena.");
            
            return Ok(_mapper.Map<List<TipGarancijeDto>>(tipoviGarancije));
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TipGarancijeDto>> GetTipGarancijeById(Guid id)
        {
            var tipGarancije = await _tipGaranceijeRepository.GetTipGarancijeById(id);

            if (tipGarancije == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetTipGarancijeById", $"Tip garancije sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            await _loggerService.Log(LogLevel.Information, "GetTipGarancijeById", $"Tip garancije sa id-jem {id} je uspešno vraćen.");
            
            return Ok(_mapper.Map<TipGarancijeDto>(tipGarancije));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateTipGarancije([FromBody] TipGarancijeDto tipGarancijeDto)
        {
            var tipGarancije = _mapper.Map<TipGarancije>(tipGarancijeDto);
            
            _tipGaranceijeRepository.CreateTipGarancije(tipGarancije);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "CreateTipGarancije", $"Tip garancije sa vrednostima: {JsonConvert.SerializeObject(tipGarancije)} je uspešno kreiran.");
            
            return CreatedAtAction(
                "GetTipGarancijeById",
                new {id = tipGarancije.Id},
                _mapper.Map<TipGarancijeDto>(tipGarancije)
            );
        }
        
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTipGarancije(Guid id, [FromBody] UpdateTipGarancijeDto tipGarancijeDto)
        {
            if (id != tipGarancijeDto.Id)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateTipGarancije", $"ID tipa garancije prosledjen kroz url nije isti kao onaj u telu zahteva.");
                return BadRequest();
            }
            
            var tipGarancije = await _tipGaranceijeRepository.GetTipGarancijeById(id);

            if (tipGarancije == null)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateTipGarancije", $"Tip garancije sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            var oldValue = JsonConvert.SerializeObject(tipGarancije);
            
            _mapper.Map(tipGarancijeDto, tipGarancije, typeof(UpdateTipGarancijeDto), typeof(TipGarancije));
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "UpdateTipGarancije", $"Tip garancije sa id-em {id} je uspešno izmenjen. Stare vrednosti su: {oldValue}");
            
            return NoContent();
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTipGarancije(Guid id)
        {
            var tipGarancije = await _tipGaranceijeRepository.GetTipGarancijeById(id);

            if (tipGarancije == null)
            {
                await _loggerService.Log(LogLevel.Warning, "DeleteTipGarancije", $"Tip garancije sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            _tipGaranceijeRepository.DeleteTipGarancije(tipGarancije);
            await _unitOfWork.CompleteAsync();
            
            await _loggerService.Log(LogLevel.Information, "DeleteTipGarancije", $"Tip garancije sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(tipGarancije)}");
            
            return NoContent();
        }
    }
}