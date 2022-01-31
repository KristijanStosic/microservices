using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UgovorOZakupu.Data.RokDospeca;
using UgovorOZakupu.Data.UnitOfWork;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.RokDospeca;

namespace UgovorOZakupu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RokDospecaController : ControllerBase
    {
        private readonly IRokDospecaRepository _rokDospecaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RokDospecaController(IRokDospecaRepository rokDospecaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _rokDospecaRepository = rokDospecaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<RokDospecaDto>>> GetAllRokDospeca()
        {
            var rokoviDospeca = await _rokDospecaRepository.GetAllRokDospeca();
            
            return _mapper.Map<List<RokDospecaDto>>(rokoviDospeca);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RokDospecaDto>> GetRokDospecaById(Guid id)
        {
            var rokDospeca = await _rokDospecaRepository.GetRokDospecaById(id);
            
            return _mapper.Map<RokDospecaDto>(rokDospeca);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateRokDospeca([FromBody] CreateRokDospecaDto rokDospecaDto)
        {
            var rokDospeca = _mapper.Map<RokDospeca>(rokDospecaDto);
            
            _rokDospecaRepository.CreateRokDospeca(rokDospeca);
            await _unitOfWork.CompleteAsync();

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
                return BadRequest();
            }
            
            var rokDospeca = await _rokDospecaRepository.GetRokDospecaById(id);

            if (rokDospeca == null) return NotFound();

            _mapper.Map(rokDospecaDto, rokDospeca, typeof(UpdateRokDospecaDto), typeof(RokDospeca));
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRokDospeca(Guid id)
        {
            var rokDospeca = await _rokDospecaRepository.GetRokDospecaById(id);

            if (rokDospeca == null) return NotFound();

            _rokDospecaRepository.DeleteRokDospeca(rokDospeca);
            await _unitOfWork.CompleteAsync();
            
            return NoContent();
        }
    }
}