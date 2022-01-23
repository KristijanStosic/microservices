using AdresaService.Data.Interfaces;
using AdresaService.Entities;
using AdresaService.Model.Drzava;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Controllers
{
    [ApiController]
    [Route("api/drzava")]
    [Produces("application/json", "application/xml")]
    public class DrzavaController : ControllerBase
    {
        private readonly IDrzavaRepository _drzavaRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public DrzavaController(IDrzavaRepository drzavaRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this._drzavaRepository = drzavaRepository;
            this._mapper = mapper;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<List<DrzavaDto>>> GetAllDrzava()
        {
            var drzave = await _drzavaRepository.GetAllDrzava();

            if (drzave == null || drzave.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<DrzavaDto>>(drzave));
        }

        [HttpGet("{drzavaId}")]
        public async Task<ActionResult<DrzavaDto>> GetDrzavaById(Guid drzavaId)
        {

            var drzava = await _drzavaRepository.GetDrzavaById(drzavaId);
            if(drzava == null)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<DrzavaDto>(drzava));
        }


        [HttpPost]
        public async Task<ActionResult<DrzavaDto>> CreateDrzava([FromBody] DrzavaDto drzava)
        {
            try
            {
                //add test to see if country already exist

                var novaDrzava = await _drzavaRepository.CreateDrzava(_mapper.Map<Drzava>(drzava));
                await _drzavaRepository.SaveChangesAsync();
                Console.WriteLine("ID: "+novaDrzava.DrzavaId);
                string lokacija = _linkGenerator.GetPathByAction("GetDrzavaById", "Drzava", new { drzavaid = novaDrzava.DrzavaId });
                Console.WriteLine("Lokacija: "+lokacija);
                return Created(lokacija,_mapper.Map<DrzavaDto>(novaDrzava));
      

            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [HttpDelete("{drzavaId}")]
        public async Task<IActionResult> DeleteDrzava(Guid drzavaId)
        {
            try
            {
                var drzava = await _drzavaRepository.GetDrzavaById(drzavaId);

                if (drzava == null)
                {
                    return NotFound();
                }

                await _drzavaRepository.DeleteDrzava(drzavaId);
                await _drzavaRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
         }


        [HttpPut]
        public async Task<ActionResult<DrzavaDto>> UpdateDrzava(DrzavaUpdateDto updateDrzava)
        {
            try
            {
                var oldDrzava = await _drzavaRepository.GetDrzavaById(updateDrzava.DrzavaId);
                if(oldDrzava == null)
                {
                    return NotFound();
                }

                Drzava drzava = _mapper.Map<Drzava>(updateDrzava);

               _mapper.Map(drzava, oldDrzava);
                await _drzavaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<DrzavaDto>(drzava));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }


        [HttpOptions]
        public IActionResult GetExamRegistrationOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
   


}
