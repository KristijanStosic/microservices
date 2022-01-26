using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Data.Interfaces;
using ZalbaService.Entities;
using ZalbaService.Entities.Confirmations;
using ZalbaService.Models.Services;
using ZalbaService.Models.Zalba;
using ZalbaService.ServicesCalls;

namespace ZalbaService.Controllers
{
    [ApiController]
    [Route("api/zalba")]
    [Produces("application/json", "application/xml")]
    public class ZalbaController : ControllerBase
    {
        private readonly IZalbaRepository _zalbaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IServiceCall<KupacDto> _kupacService;
        
        public ZalbaController(IZalbaRepository zalbaRepository, LinkGenerator linkGenerator, IMapper mapper, IServiceCall<KupacDto> kupacService, IConfiguration configuration)
        {
            _zalbaRepository = zalbaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _kupacService = kupacService;
            _configuration = configuration; 
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<ZalbaDto>>> GetAllZalbe()
        {
            var zalbe = await _zalbaRepository.GetAllZalbe();

            if (zalbe == null || zalbe.Count == 0)
            {
                return NoContent();
            }

            var zalbeDto = new List<ZalbaDto>();
            string url = _configuration["Services:KupacService"];
            foreach (var zalba in zalbe)
            {
                var zalbaDto = _mapper.Map<ZalbaDto>(zalba);
                if (zalba.KupacId is not null)
                {
                    var kupacDto = _kupacService.SendGetRequestAsync(url + zalba.KupacId).Result;
                    zalbaDto.Kupac = kupacDto.Kupac + ", " + kupacDto.Email + ", " + kupacDto.BrojRacuna + ", " + kupacDto.BrojTelefona1;
                }
                zalbeDto.Add(zalbaDto);
            }
            return Ok(zalbeDto);
        }

        [HttpGet("{zalbaId}")]
        public async Task<ActionResult<List<ZalbaDto>>> GetZalba(Guid zalbaId)
        {
            var zalba = await _zalbaRepository.GetZalbaById(zalbaId);

            if (zalba == null)
            {
                return NoContent();
            }

            string url = _configuration["Services:KupacService"];

            var zalbaDto = _mapper.Map<ZalbaDto>(zalba);
            if(zalba.KupacId is not null)
            {
                var kupacDto = _kupacService.SendGetRequestAsync(url + zalba.KupacId).Result;
                zalbaDto.Kupac = kupacDto.Kupac + ", " + kupacDto.Email + ", " + kupacDto.BrojRacuna + ", " + kupacDto.BrojTelefona1;
            }

            return Ok(zalbaDto);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<ZalbaConfirmationDto>> CreateZalba([FromBody] ZalbaCreateDto zalba)
        {
            try
            {
                Zalba mapiranaZalba = _mapper.Map<Zalba>(zalba);
                var proveraValidnosti = await _zalbaRepository.IsValidZalba(mapiranaZalba);

                if(!proveraValidnosti)
                {
                    var response = new
                    {
                        Message = "Uneli ste iste podatke za broj resenja ili broj nadmetanja. Pokusajte ponovo!"
                    };

                    return BadRequest(response);
                }

                ZalbaConfirmation createdZalba = await _zalbaRepository.CreateZalba(_mapper.Map<Zalba>(mapiranaZalba));

                string location = _linkGenerator.GetPathByAction("GetZalba", "Zalba", new { zalbaId = createdZalba.ZalbaId });

                return Created(location, _mapper.Map<ZalbaConfirmationDto>(createdZalba));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Zalba error");
            }
        }

        [HttpPut("{zalbaId}")]
        [Consumes("application/json")]
        public async Task<ActionResult<ZalbaUpdateDto>> UpdateZalba(Guid zalbaId, [FromBody] ZalbaUpdateDto zalba)
        {
            try
            {
                var zalbaEntity = await _zalbaRepository.GetZalbaById(zalbaId);

                if (zalbaEntity == null)
                {
                    return NotFound();
                }

                Zalba mapiranaZalba = _mapper.Map<Zalba>(zalba);
                var proveraValidnosti = await _zalbaRepository.IsValidZalba(mapiranaZalba);

                if (!proveraValidnosti)
                {
                    var errorResponse = new
                    {
                        Message = "Unos istih podataka za broj nadmetanja i broj resenja. Pokusajte ponovo!"
                    };

                    return BadRequest(errorResponse);
                }

                _mapper.Map(zalba, zalbaEntity);

                await _zalbaRepository.UpdateZalba(_mapper.Map<Zalba>(zalba));

                var response = new
                {
                    Message = "Uspesno modifikovana zalba sa ID: " + zalbaId,
                    ZalbaEntity = _mapper.Map<ZalbaUpdateDto>(zalbaEntity)
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update zalba error");
            }
        }

        [HttpDelete("{zalbaId}")]
        public async Task<ActionResult> DeleteZalba(Guid zalbaId)
        {
            try
            {
                var zalba = await _zalbaRepository.GetZalbaById(zalbaId);

                if (zalba == null)
                {
                    return NotFound();
                }

                await _zalbaRepository.DeleteZalba(zalbaId);

                var response = new
                {
                    Message = "Uspesno obrisana zalba sa ID-jem: " + zalbaId
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete zalba error");
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
