using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using PrijavaService.Data.Interfaces;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Helpers;
using PrijavaService.Models.Prijava;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijavaService.Controllers
{
    [Route("api/prijava")]
    [ApiController]
    public class PrijavaController : ControllerBase
    {

        private readonly IPrijavaRepository _prijavaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IPrijavaCalls _prijavaCalls;

        public PrijavaController(IPrijavaRepository prijavaRepository, LinkGenerator linkGenerator, IMapper mapper, IPrijavaCalls prijavaCalls)
        {
            _prijavaRepository = prijavaRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
            _prijavaCalls = prijavaCalls;
        }

        // GET: api/<PrijavaController>
        [HttpGet]
        public async Task<ActionResult<List<PrijavaDto>>> GetAllPrijava()
        {
            var prijave = await _prijavaRepository.GetAllPrijava();

            if(prijave == null || prijave.Count == 0)
            {
                return NoContent();
            }

            var prijaveDto = new List<PrijavaDto>();
            foreach (var prijava in prijave)
            {
                prijaveDto.Add(await _prijavaCalls.GetPrijvaDtoWithOtherServicesInfo(prijava));
            }

            return Ok(prijaveDto);

        }

        // GET api/<PrijavaController>/5
        [HttpGet("{prijavaId}")]
        public async Task<ActionResult<PrijavaDto>> GetPrijava(Guid prijavaId)
        {
            var prijava = await _prijavaRepository.GetPrijavaById(prijavaId);

            if(prijava == null)
            {
                return NotFound();
            }

            return Ok(await _prijavaCalls.GetPrijvaDtoWithOtherServicesInfo(prijava));
        }

        // POST api/<PrijavaController>
        [HttpPost]
        public async Task<ActionResult<PrijavaConfirmationDto>> CreatePrijava([FromBody] PrijavaCreationDto prijava)
        {
            try
            {
                PrijavaConfirmation novaPrijava = await _prijavaRepository.CreatePrijava(_mapper.Map<Prijava>(prijava));
                await _prijavaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetPrijava", "Prijava", new { prijavaId = novaPrijava.PrijavaId });


                return Created(lokacija, _mapper.Map<PrijavaConfirmationDto>(novaPrijava));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa prijave");
            }
        }

        // PUT api/<PrijavaController>/5
        [HttpPut]
        public async Task<ActionResult<PrijavaDto>> UpdatePrijava(PrijavaUpdateDto prijava)
        {
            try
            {
                var staraPrijava = await _prijavaRepository.GetPrijavaById(prijava.PrijavaId);

                if(staraPrijava == null)
                {
                    return NotFound();
                }

                var stareVrednosti = JsonConvert.SerializeObject(prijava);

                Prijava novaPrijava = _mapper.Map<Prijava>(prijava);

                _mapper.Map(novaPrijava, staraPrijava);
                await _prijavaRepository.UpdatePrijava(novaPrijava);
                await _prijavaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<PrijavaDto>(staraPrijava));

            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene prijave.");
            }
        }

        // DELETE api/<PrijavaController>/5
        [HttpDelete("{prijavaId}")]
        public async Task<IActionResult> DeletePrijava(Guid prijavaId)
        {
            try
            {
                var prijava = await _prijavaRepository.GetPrijavaById(prijavaId);

                if(prijava == null)
                {
                    return NotFound();
                }

                await _prijavaRepository.DeletePrijava(prijavaId);
                await _prijavaRepository.SaveChangesAsync();

                return NoContent();

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja prijave");
            }
        }

        [HttpOptions]
        public IActionResult GetPrijavaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
