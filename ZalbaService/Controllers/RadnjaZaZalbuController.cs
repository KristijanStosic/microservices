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

namespace ZalbaService.Controllers
{
    [ApiController]
    [Route("api/radnjaZaZalbu")]
    [Produces("application/json", "application/xml")]
    public class RadnjaZaZalbuController : ControllerBase 
    {
        private readonly IRadnjaZaZalbuRepository _radnjaZaZalbuRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public RadnjaZaZalbuController(IRadnjaZaZalbuRepository radnjaZaZalbuRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _radnjaZaZalbuRepository = radnjaZaZalbuRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<RadnjaZaZalbuDto>>> GetAllRadnjeZaZalbu(string nazivRadnjeZaZalbu)
        {
            var radnjeZaZalbu = await _radnjaZaZalbuRepository.GetAllRadnjeZaZalbu(nazivRadnjeZaZalbu);

            if (radnjeZaZalbu == null || radnjeZaZalbu.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<IEnumerable<RadnjaZaZalbuDto>>(radnjeZaZalbu));
        }

        [HttpGet("{radnjaZaZalbuId}")]
        public async Task<ActionResult<RadnjaZaZalbuDto>> GetRadnjaZaZalbu(Guid radnjaZaZalbuId)
        {
            var radnjaZaZalbu = await _radnjaZaZalbuRepository.GetRadnjaZaZalbuById(radnjaZaZalbuId);

            if (radnjaZaZalbu == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RadnjaZaZalbuDto>(radnjaZaZalbu));
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<RadnjaZaZalbuCreateDto>> CreateRadnjaZaZalbu([FromBody] RadnjaZaZalbuCreateDto radnjaZaZalbu)
        {
            try
            {
                var proveraValidnosti = await _radnjaZaZalbuRepository.IsValidRadnjaZaZalbu(radnjaZaZalbu.NazivRadnjeZaZalbu);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };

                    return BadRequest(response);
                }

                RadnjaZaZalbu createdRadnjaZaZalbu = await _radnjaZaZalbuRepository.CreateRadnjaZaZalbu(_mapper.Map<RadnjaZaZalbu>(radnjaZaZalbu));

                string location = _linkGenerator.GetPathByAction("GetRadnjaZaZalbu", "RadnjaZaZalbu", new { radnjaZaZalbuId = createdRadnjaZaZalbu.RadnjaZaZalbuId });

                return Created(location, _mapper.Map<RadnjaZaZalbuCreateDto>(createdRadnjaZaZalbu));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Radnja za zalbu error");
            }
        }

        [HttpPut("{radnjaZaZalbuId}")]
        public async Task<ActionResult<RadnjaZaZalbuUpdateDto>> UpdateRadnjaZaZalbu(Guid radnjaZaZalbuId, [FromBody] RadnjaZaZalbuUpdateDto radnjaZaZalbu)
        {
            try
            {
                var proveraValidnosti = await _radnjaZaZalbuRepository.IsValidRadnjaZaZalbu(radnjaZaZalbu.NazivRadnjeZaZalbu);

                if (!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Unos istih podataka. Pokusajte ponovo!"
                    };

                    return BadRequest(response);
                }

                var radnjaZaZalbuEntity = await _radnjaZaZalbuRepository.GetRadnjaZaZalbuById(radnjaZaZalbuId);

                if (radnjaZaZalbuEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(radnjaZaZalbu, radnjaZaZalbuEntity);

                await _radnjaZaZalbuRepository.UpdateRadnjaZaZalbu(_mapper.Map<RadnjaZaZalbu>(radnjaZaZalbu));

                return Ok(radnjaZaZalbu);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update radnja za zalbu error");
            }
        }

        [HttpDelete("{radnjaZaZalbuId}")]
        public async Task<ActionResult> DeleteRadnjaZaZalbu(Guid radnjaZaZalbuId)
        {
            try
            {
                var radnjaZaZalbu = await _radnjaZaZalbuRepository.GetRadnjaZaZalbuById(radnjaZaZalbuId);

                if (radnjaZaZalbu == null)
                {
                    return NotFound();
                }

                await _radnjaZaZalbuRepository.DeleteRadnjaZaZalbu(radnjaZaZalbuId);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete radnja za zalbu error");
            }
        }

        /// <summary>
        /// Vraća opcije za rad sa radnjama zalbi
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        public IActionResult GetRadnjaZaZalbuOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
