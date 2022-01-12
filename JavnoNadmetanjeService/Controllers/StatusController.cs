using AutoMapper;
using JavnoNadmetanjeService.Data;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<List<StatusDto>>> GetAllStasuses(string nazivStatusa)
        {
            var statuses = await _statusRepository.GetAllStatuses(nazivStatusa);

            if(statuses == null || statuses.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<StatusDto>>(statuses));
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
                Status createdStatus = await _statusRepository.CreateStatus(_mapper.Map<Status>(status));

                string location = _linkGenerator.GetPathByAction("GetStatus", "Status", new { statusId = createdStatus.StatusId });

                return Created(location, _mapper.Map<StatusDto>(createdStatus));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create status error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<StatusDto>> UpdateStatus(StatusUpdateDto status)
        {
            try
            {
                var oldStatus = await _statusRepository.GetStatusById(status.StatusId);

                if (oldStatus == null)
                {
                    return NotFound();
                }

                Status editedStatus = _mapper.Map<Status>(status);

                _mapper.Map(editedStatus, oldStatus);         
                await _statusRepository.UpdateStatus();

                return Ok(_mapper.Map<StatusDto>(editedStatus));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update status error");
            }
        }

        [HttpDelete("{statusId}")]
        public async Task<ActionResult> DeleteStatus(Guid statusId)
        {
            try
            {
                var status = await _statusRepository.GetStatusById(statusId);

                if (status == null)
                {
                    return NotFound();
                }

                await _statusRepository.DeleteStatus(statusId);

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete status error");
            }
        }


    }
}
