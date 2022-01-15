using AutoMapper;
using JavnoNadmetanjeService.Data;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public StatusController(IStatusRepository statusRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _statusRepository = statusRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<StatusDto>>> GetAllStatus(string nazivStatusa)
        {
            var statusi = await _statusRepository.GetAllStatus(nazivStatusa);

            if(statusi == null || statusi.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<StatusDto>>(statusi));
        }

        [HttpGet("{statusId}")]
        public async Task<ActionResult<StatusDto>> GetStatus(Guid statusId)
        {
            var status = await _statusRepository.GetStatusById(statusId);

            if (status == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<StatusDto>(status));
        }

        [HttpPost]
        public async Task<ActionResult<StatusDto>> CreateStatus([FromBody] StatusDto status)
        {
            try
            {
                Status noviStatus = await _statusRepository.CreateStatus(_mapper.Map<Status>(status));
                await _statusRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetStatus", "Status", new { statusId = noviStatus.StatusId });

                return Created(lokacija, _mapper.Map<StatusDto>(noviStatus));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja statusa");
            }
        }

        [HttpPut]
        public async Task<ActionResult<StatusDto>> UpdateStatus(StatusUpdateDto status)
        {
            try
            {
                var stariStatus = await _statusRepository.GetStatusById(status.StatusId);

                if (stariStatus == null)
                {
                    return NotFound();
                }

                Status noviStatus = _mapper.Map<Status>(status);

                _mapper.Map(noviStatus, stariStatus);
                await _statusRepository.SaveChangesAsync();

                return Ok(_mapper.Map<StatusDto>(stariStatus));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene statusa");
            }
        }

        [HttpDelete("{statusId}")]
        public async Task<IActionResult> DeleteStatus(Guid statusId)
        {
            try
            {
                var status = await _statusRepository.GetStatusById(statusId);

                if (status == null)
                {
                    return NotFound();
                }

                await _statusRepository.DeleteStatus(statusId);
                await _statusRepository.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja statusa");
            }
        }

        [HttpOptions]
        public IActionResult GetStatusOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

    }
}
