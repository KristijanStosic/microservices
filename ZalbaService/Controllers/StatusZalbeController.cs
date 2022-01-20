using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Data;
using ZalbaService.Entities;
using ZalbaService.Models;
using ZalbaService.Models.StatusZalbe;

namespace ZalbaService.Controllers
{
    [ApiController]
    [Route("api/statusZalbe")]
    [Produces("application/json", "application/xml")]
    public class StatusZalbeController : ControllerBase
    {
        private readonly IStatusZalbeRepository _statusZalbeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        public StatusZalbeController(IStatusZalbeRepository statusZalbeRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _statusZalbeRepository = statusZalbeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }
        
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<StatusZalbeDto>>> GetAllStatusesZalbe(string nazivStatusaZalbe)
        {
            var statusesZalbe = await _statusZalbeRepository.GetAllStatusesZalbe(nazivStatusaZalbe);

            if(statusesZalbe == null || statusesZalbe.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<StatusZalbeDto>>(statusesZalbe));
        }

        [HttpGet("{statusZalbeId}")]
        public async Task<ActionResult<StatusZalbeDto>> GetStatusZalbe(Guid statusZalbeId)
        {
            var statusZalbe = await _statusZalbeRepository.GetStatusZalbeById(statusZalbeId);

            if(statusZalbe == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<StatusZalbeDto>(statusZalbe));
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<StatusZalbeCreateDto>> CreateStatusZalbe([FromBody] StatusZalbeCreateDto statusZalbe)
        {
            try
            {
                var proveraValidnosti = await _statusZalbeRepository.IsValidStatusZalbe(statusZalbe.NazivStatusaZalbe);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };

                    return BadRequest(response);
                }

                StatusZalbe createdStatusZalbe = await _statusZalbeRepository.CreateStatusZalbe(_mapper.Map<StatusZalbe>(statusZalbe));

                string location = _linkGenerator.GetPathByAction("GetStatusZalbe", "StatusZalbe", new { statusZalbeId = createdStatusZalbe.StatusZalbeId });

                return Created(location, _mapper.Map<StatusZalbeCreateDto>(createdStatusZalbe));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Status Zalbe error");
            }
        }

        [HttpPut("{statusZalbeId}")]
        public async Task<ActionResult<StatusZalbeUpdateDto>> UpdateStatusZalbe(Guid statusZalbeId, [FromBody]StatusZalbeUpdateDto statusZalbe)
        {
            try
            {
                var proveraValidnosti = await _statusZalbeRepository.IsValidStatusZalbe(statusZalbe.NazivStatusaZalbe);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };

                    return BadRequest(response);
                }

                var statusZalbeEntity = await _statusZalbeRepository.GetStatusZalbeById(statusZalbeId);

                if(statusZalbeEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(statusZalbe, statusZalbeEntity);

                await _statusZalbeRepository.UpdateStatusZalbe(_mapper.Map<StatusZalbe>(statusZalbe));

                return Ok(statusZalbe);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update status zalbe error");
            }
        }

        [HttpDelete("{statusZalbeId}")]
        public async Task<ActionResult> DeleteStatusZalbe(Guid statusZalbeId)
        {
            try
            {
                var statusZalbe = await _statusZalbeRepository.GetStatusZalbeById(statusZalbeId);

                if (statusZalbe == null)
                {
                    return NotFound();
                }

                await _statusZalbeRepository.DeleteStatusZalbe(statusZalbeId);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete status zalbe error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa zalbama
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetZalbaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
