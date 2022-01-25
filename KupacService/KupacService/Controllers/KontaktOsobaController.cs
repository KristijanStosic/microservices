using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.KontaktOsoba;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Controllers
{
    [ApiController]
    [Route("api/kontaktOsoba")]
    public class KontaktOsobaController : ControllerBase
    {
        private readonly IKontaktOsobaRepository _kontaktOsobaRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public KontaktOsobaController(IKontaktOsobaRepository kontaktOsobaRepository,LinkGenerator linkGenerator,IMapper mapper)
        {
            this._kontaktOsobaRepository = kontaktOsobaRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<KontaktOsobaDto>>> GetKontaktOsoba(string ime, string prezime)
        {
            var kontaktOsobe = await _kontaktOsobaRepository.GetKontaktOsoba(ime, prezime);

            if(kontaktOsobe == null || kontaktOsobe.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KontaktOsobaDto>>(kontaktOsobe));
        }
        [HttpGet("{kontaktOsobaId}")]
        public async Task<ActionResult<KontaktOsobaDto>> GetKontaktOsobaById(Guid kontaktOsobaId)
        {
            var kontaktOsoba = await _kontaktOsobaRepository.GetKontaktOsobaById(kontaktOsobaId);

            if(kontaktOsoba == null)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<KontaktOsobaDto>(kontaktOsoba));
        }
        [HttpPost]
        public async Task<ActionResult<KontaktOsobaDto>> CreateKontaktOsoba(KontaktOsobaDto kontaktOsoba)
        {
            try
            {
                KontaktOsoba newKontaktOsoba = _mapper.Map<KontaktOsoba>(kontaktOsoba);

                await _kontaktOsobaRepository.CreateKontaktOsoba(newKontaktOsoba);
                await _kontaktOsobaRepository.SaveChangesAsync();

                string link = _linkGenerator.GetPathByAction("GetKontaktOsobaById", "KontaktOsoba", new { kontaktOsobaId = newKontaktOsoba.KontaktOsobaId });
                return Created(link, kontaktOsoba);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<KontaktOsobaDto>> UpdateKontaktOsoba(KontaktOsobaUpdateDto kontaktOsobaUpdate)
        {
            try
            {
                var oldKontaktOsoba = await _kontaktOsobaRepository.GetKontaktOsobaById(kontaktOsobaUpdate.KontaktOsobaId);

                if(oldKontaktOsoba == null)
                {
                    return NotFound();
                }

                KontaktOsoba kontaktOsoba = _mapper.Map<KontaktOsoba>(kontaktOsobaUpdate);

                _mapper.Map(kontaktOsoba, oldKontaktOsoba);

                await _kontaktOsobaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<KontaktOsobaDto>(kontaktOsoba));


            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }
        }
        [HttpDelete("{kontaktOsobaId}")]
        public async Task<IActionResult> DeleteKontaktOsoba(Guid kontaktOsobaId)
        {
            try
            {
                var kontaktOsoba = await _kontaktOsobaRepository.GetKontaktOsobaById(kontaktOsobaId);

                if(kontaktOsoba == null)
                {
                    return NotFound();
                }

                await _kontaktOsobaRepository.DeleteKontaktOsoba(kontaktOsobaId);
                await _kontaktOsobaRepository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }


        [HttpOptions]
        public IActionResult GetKontaktOsobaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }


    }
}
