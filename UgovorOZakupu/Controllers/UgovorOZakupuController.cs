﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UgovorOZakupu.Data.TipGarancije;
using UgovorOZakupu.Data.UgovorOZakupu;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Models.UgovorOZakupu;
using UgovorOZakupu.Services.Logger;
using UgovorOZakupu.Services.ServiceCalls;

namespace UgovorOZakupu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UgovorOZakupuController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITipGaranceijeRepository _tipGaranceijeRepository;
        private readonly IUgovorOZakupuRepository _ugovorOZakupuRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly ILoggerService _loggerService;
        private readonly IServiceCalls _serviceCalls;

        public UgovorOZakupuController(IUgovorOZakupuRepository ugovorOZakupuRepository,
            ITipGaranceijeRepository tipGaranceijeRepository, IUnitOfWork unitOfWork,
            IMapper mapper, ILoggerService loggerService, IServiceCalls serviceCalls)
        {
            _ugovorOZakupuRepository = ugovorOZakupuRepository;
            _tipGaranceijeRepository = tipGaranceijeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerService = loggerService;
            _serviceCalls = serviceCalls;
        }

        [HttpGet]
        public async Task<ActionResult<List<UgovorOZakupuDto>>> GetAllUgovorOZakupu()
        {
            var ugovori = await _ugovorOZakupuRepository.GetAllUgovorOZakupu();

            if (ugovori == null || ugovori.Count == 0)
            {
                await _loggerService.Log(LogLevel.Warning, "GetAllUgovorOZakupu",
                    "Lista ugovora o zakupu je prazna ili null.");
                return NoContent();
            }

            var ugovoriDto = _mapper.Map<List<UgovorOZakupuDto>>(ugovori);

            await _loggerService.Log(LogLevel.Information, "GetAllUgovorOZakupu",
                "Lista ugovora o zakupu je uspešno vraćena.");

            return Ok(ugovoriDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UgovorOZakupuDto>> GetUgovorOZakupuById(Guid id)
        {
            var ugovor = await _ugovorOZakupuRepository.GetUgovorOZakupuById(id);

            if (ugovor == null)
            {
                await _loggerService.Log(LogLevel.Warning, "GetUgovorOZakupuById",
                    $"Ugovor o zakupu sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            var ugovorDto = _mapper.Map<UgovorOZakupuDto>(ugovor);
            ugovorDto.Dokument = await _serviceCalls.GetDokumentById(ugovor.DokumentId);
            
            await _loggerService.Log(LogLevel.Information, "GetUgovorOZakupuById",
                $"Ugovor o zakupu sa id-jem {id} je uspešno vraćen.");

            return Ok(ugovorDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUgovorOZakupu([FromBody] CreateUgovorOZakupuDto ugovorOZakupuDto)
        {
            var ugovor = _mapper.Map<Entities.UgovorOZakupu>(ugovorOZakupuDto);

            _ugovorOZakupuRepository.CreateUgovorOZakupu(ugovor);
            await _unitOfWork.CompleteAsync();

            ugovor.TipGarancije =
                await _tipGaranceijeRepository.GetTipGarancijeById(ugovor.TipGarancijeId);

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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUgovorOZakupu(Guid id,
            [FromBody] UpdateUgovorOZakupuDto ugovorOZakupuDto)
        {
            if (id != ugovorOZakupuDto.Id)
            {
                await _loggerService.Log(LogLevel.Warning, "UpdateUgovorOZakupu",
                    "ID ugovora o zakupua prosledjen kroz url nije isti kao onaj u telu zahteva.");
                return BadRequest();
            }

            var ugovor = await _ugovorOZakupuRepository.GetUgovorOZakupuById(id);

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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUgovorOZakupu(Guid id)
        {
            var ugovor = await _ugovorOZakupuRepository.GetUgovorOZakupuById(id);

            if (ugovor == null)
            {
                await _loggerService.Log(LogLevel.Warning, "DeleteUgovorOZakupu",
                    $"Ugovor o zakupu sa id-jem {id} nije pronadjen.");
                return NotFound();
            }

            _ugovorOZakupuRepository.DeleteUgovorOZakupu(ugovor);
            await _unitOfWork.CompleteAsync();

            await _loggerService.Log(LogLevel.Information, "DeleteUgovorOZakupu",
                $"Ugovor o zakupu sa id-em {id} je uspešno obrisan. Obrisane vrednosti: {JsonConvert.SerializeObject(ugovor)}");

            return NoContent();
        }
    }
}