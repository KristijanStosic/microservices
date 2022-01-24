using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;
using JavnoNadmetanjeService.ServiceCalls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Controllers
{
    [Route("api/javnoNadmetanje")]
    [ApiController]
    public class JavnoNadmetanjeController : ControllerBase
    {
        private readonly IJavnoNadmetanjeRepository _javnoNadmetanjeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IAdresaService _adresaService;

        public JavnoNadmetanjeController(IJavnoNadmetanjeRepository javnoNadmetanjeRepository, LinkGenerator linkGenerator, IMapper mapper, IAdresaService adresaService)
        {
            _javnoNadmetanjeRepository = javnoNadmetanjeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _adresaService = adresaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<JavnoNadmetanjeDto>>> GetAllJavnoNadmetanje()
        {
            var javnaNadmetanja = await _javnoNadmetanjeRepository.GetAllJavnoNadmetanje();

            if (javnaNadmetanja == null || javnaNadmetanja.Count == 0)
            {
                return NoContent();
            }

            //Adresa mikroservis komunikacija - trenutno je preko mock-a, implementiran je i pravi nacin samo sad nema tog mikroservisa jos uvek, samo treba promeniti u startup-u
            var javnaNadmetanjaDto = new List<JavnoNadmetanjeDto>();
            foreach (var javnoNad in javnaNadmetanja)
            {
                var javnoNadDto = _mapper.Map<JavnoNadmetanjeDto>(javnoNad);
                if(javnoNad.AdresaId is not null)
                {
                    var adresaDto = _adresaService.GetAdresaDto((Guid)javnoNad.AdresaId).Result;
                    javnoNadDto.Adresa = adresaDto.Ulica + " " + adresaDto.Broj + " " + adresaDto.Mesto + ", " + adresaDto.Drzava;
                }
                javnaNadmetanjaDto.Add(javnoNadDto);
            }

            return Ok(javnaNadmetanjaDto);
        }

        [HttpGet("{javnoNadmetanjeId}")]
        public async Task<ActionResult<JavnoNadmetanjeDto>> GetJavnoNadmetanje(Guid javnoNadmetanjeId)
        {
            var javnoNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanjeId);

            if (javnoNadmetanje == null)
            {
                return NotFound();
            }

            var javnoNadmetanjeDto = _mapper.Map<JavnoNadmetanjeDto>(javnoNadmetanje);
            var adresaDto = _adresaService.GetAdresaDto((Guid)javnoNadmetanje.AdresaId).Result;
            javnoNadmetanjeDto.Adresa = adresaDto.Ulica + " " + adresaDto.Broj + " " + adresaDto.Mesto + ", " + adresaDto.Drzava;

            return Ok(javnoNadmetanjeDto);
        }

        [HttpPost]
        public async Task<ActionResult<JavnoNadmetanjeConfirmationDto>> CreateJavnoNadmetanje([FromBody] JavnoNadmetanjeCreationDto javnoNadmetanje)
        {
            try
            {
                JavnoNadmetanjeConfirmation novoNadmetanje = await _javnoNadmetanjeRepository.CreateJavnoNadmetanje(_mapper.Map<JavnoNadmetanje>(javnoNadmetanje));
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetJavnoNadmetanje", "JavnoNadmetanje", new { javnoNadmetanjeId = novoNadmetanje.JavnoNadmetanjeId });

                return Created(lokacija, _mapper.Map<JavnoNadmetanjeConfirmationDto>(novoNadmetanje));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom unosa javnog nadmetanja");
            }
        }

        [HttpPut]
        public async Task<ActionResult<JavnoNadmetanjeDto>> UpdateJavnoNadmetanje(JavnoNadmetanjeUpdateDto javnoNadmetanje)
        {
            try
            {
                var staroNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanje.JavnoNadmetanjeId);

                if (staroNadmetanje == null)
                {
                    return NotFound();
                }

                JavnoNadmetanje novoNadmetanje = _mapper.Map<JavnoNadmetanje>(javnoNadmetanje);

                _mapper.Map(novoNadmetanje, staroNadmetanje);
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                return Ok(_mapper.Map<JavnoNadmetanjeDto>(staroNadmetanje));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene javnog nadmetanja");
            }
        }

        [HttpDelete("{javnoNadmetanjeId}")]
        public async Task<IActionResult> DeleteJavnoNadmetanje(Guid javnoNadmetanjeId)
        {
            try
            {
                var javnoNadmetanje = await _javnoNadmetanjeRepository.GetJavnoNadmetanjeById(javnoNadmetanjeId);

                if (javnoNadmetanje == null)
                {
                    return NotFound();
                }

                await _javnoNadmetanjeRepository.DeleteJavnoNadmetanje(javnoNadmetanjeId);
                await _javnoNadmetanjeRepository.SaveChangesAsync();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja javnog nadmetanja");
            }
        }

        [HttpOptions]
        public IActionResult GetJavnoNadmetanjeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
