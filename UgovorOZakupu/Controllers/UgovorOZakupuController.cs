using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UgovorOZakupu.Data.TipGarancije;
using UgovorOZakupu.Data.UgovorOZakupu;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Models.UgovorOZakupu;

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

        public UgovorOZakupuController(IUgovorOZakupuRepository ugovorOZakupuRepository,
            ITipGaranceijeRepository tipGaranceijeRepository, IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _ugovorOZakupuRepository = ugovorOZakupuRepository;
            _tipGaranceijeRepository = tipGaranceijeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UgovorOZakupuDto>>> GetAllUgovorOZakupu()
        {
            var ugovor = await _ugovorOZakupuRepository.GetAllUgovorOZakupu();

            return Ok(_mapper.Map<List<UgovorOZakupuDto>>(ugovor));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UgovorOZakupuDto>> GetUgovorOZakupuById(Guid id)
        {
            var ugovor = await _ugovorOZakupuRepository.GetUgovorOZakupuById(id);

            if (ugovor == null) return NotFound();

            return Ok(_mapper.Map<UgovorOZakupuDto>(ugovor));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRokDospeca([FromBody] CreateUgovorOZakupuDto ugovorOZakupuDto)
        {
            var ugovorOZakupu = _mapper.Map<Entities.UgovorOZakupu>(ugovorOZakupuDto);

            _ugovorOZakupuRepository.CreateUgovorOZakupu(ugovorOZakupu);
            await _unitOfWork.CompleteAsync();

            ugovorOZakupu.TipGarancije =
                await _tipGaranceijeRepository.GetTipGarancijeById(ugovorOZakupu.TipGarancijeId);

            return CreatedAtAction(
                "GetUgovorOZakupuById",
                new {id = ugovorOZakupu.Id},
                _mapper.Map<UgovorOZakupuDto>(ugovorOZakupu)
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUgovorOZakupu(Guid id,
            [FromBody] UpdateUgovorOZakupuDto ugovorOZakupuDto)
        {
            if (id != ugovorOZakupuDto.Id) return BadRequest();

            var ugovor = await _ugovorOZakupuRepository.GetUgovorOZakupuById(id);

            if (ugovor == null) return NotFound();

            _mapper.Map(ugovorOZakupuDto, ugovor, typeof(UpdateUgovorOZakupuDto), typeof(Entities.UgovorOZakupu));
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUgovorOZakupu(Guid id)
        {
            var ugovor = await _ugovorOZakupuRepository.GetUgovorOZakupuById(id);

            if (ugovor == null) return NotFound();

            _ugovorOZakupuRepository.DeleteUgovorOZakupu(ugovor);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}