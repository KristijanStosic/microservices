using AdresaService.Data.Interfaces;
using AdresaService.Entities;
using AdresaService.Model.Adresa;
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
    [Route("api/adresa")]
    public class AdresaController : ControllerBase
    {
        private readonly IAdresaRepository _adresaRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public AdresaController(IAdresaRepository adresaRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this._adresaRepository = adresaRepository;
            this._mapper = mapper;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdresaDto>>> GetAdrese(string ulica, string mesto, string postanskiBroj)
        {
            var adrese = await _adresaRepository.GetAdrese(ulica, mesto, postanskiBroj);

            if (adrese == null || adrese.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<AdresaDto>>(adrese));
        }

        [HttpGet("{adresaId}")]
        public async Task<ActionResult<AdresaDto>> GetAdresaById(Guid adresaId)
        {
            var adresa = await _adresaRepository.GetAdresaById(adresaId);

            if (adresa == null)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<AdresaDto>(adresa));
        }
        [HttpPost]
        public async Task<ActionResult<AdresaConformationDto>> CreateAdresa(AdresaCreationDto adresa)
        {
            try
            {
                Adresa novaAdresa = await _adresaRepository.CreateAdresa(_mapper.Map<Adresa>(adresa));
                await _adresaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetAdresaById", "Adresa", new { adresaId = novaAdresa.AdresaId });

                return Created(lokacija, _mapper.Map<AdresaConformationDto>(novaAdresa));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }
        [HttpDelete("{adresaId}")]
        public async Task<ActionResult> DeleteAdresa(Guid adresaId)
        {
            try
            {
                var adresa = await _adresaRepository.GetAdresaById(adresaId);

                if (adresa == null)
                {
                    return NotFound();
                }

                await _adresaRepository.DeleteAdresa(adresaId);
                await _adresaRepository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<AdresaDto>> UpdateAdresa(AdresaUpdateDto adresaUpdate)
        {
            try
            {
                var oldAdresa = await _adresaRepository.GetAdresaById(adresaUpdate.AdresaId);

                if(oldAdresa == null)
                {
                    return NotFound();
                }

                Adresa adresa = _mapper.Map<Adresa>(adresaUpdate);
                
                _mapper.Map(adresa, oldAdresa);

                await _adresaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<AdresaDto>(adresa));
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
