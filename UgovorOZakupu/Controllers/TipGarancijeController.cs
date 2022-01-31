﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UgovorOZakupu.Data.TipGarancije;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.TipGarancije;

namespace UgovorOZakupu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipGarancijeController : ControllerBase
    {
        private readonly ITipGaranceijeRepository _tipGaranceijeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipGarancijeController(ITipGaranceijeRepository tipGaranceijeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _tipGaranceijeRepository = tipGaranceijeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TipGarancijeDto>>> GetAllTipGarancije()
        {
            var tipoviGarancije = await _tipGaranceijeRepository.GetAllTipGarancije();
            
            return Ok(_mapper.Map<List<TipGarancijeDto>>(tipoviGarancije));
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TipGarancijeDto>> GetTipGarancijeById(Guid id)
        {
            var tipGarancije = await _tipGaranceijeRepository.GetTipGarancijeById(id);

            if (tipGarancije == null) return NotFound();

            return Ok(_mapper.Map<TipGarancijeDto>(tipGarancije));
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateTipGarancije([FromBody] TipGarancijeDto tipGarancijeDto)
        {
            var tipGarancije = _mapper.Map<TipGarancije>(tipGarancijeDto);
            
            _tipGaranceijeRepository.CreateTipGarancije(tipGarancije);
            await _unitOfWork.CompleteAsync();

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
                return BadRequest();
            }
            
            var tipGarancije = await _tipGaranceijeRepository.GetTipGarancijeById(id);

            if (tipGarancije == null) return NotFound();

            _mapper.Map(tipGarancijeDto, tipGarancije, typeof(UpdateTipGarancijeDto), typeof(TipGarancije));
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTipGarancije(Guid id)
        {
            var tipGarancije = await _tipGaranceijeRepository.GetTipGarancijeById(id);

            if (tipGarancije == null) return NotFound();

            _tipGaranceijeRepository.DeleteTipGarancije(tipGarancije);
            await _unitOfWork.CompleteAsync();
            
            return NoContent();
        }
    }
}