using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Models.Odvodnjavanje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Controllers
{
    [Route("api/odvodnjavanje")]
    public class OdvodnjavanjeController : ControllerBase
    {
        private readonly IOdvodnjavanjeRepository _odvodnjavanjeRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public OdvodnjavanjeController(IOdvodnjavanjeRepository odvodnjavanjeRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            _odvodnjavanjeRepository = odvodnjavanjeRepository;
            _linkGenerator = linkGenerator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<OdvodnjavanjeDto>>> GetAllOdvodnjavanje(string opisOdvodnjavanja)
        {
            var odvodnjavanje = await _odvodnjavanjeRepository.GetAllOdvodnjavanje(opisOdvodnjavanja);

            if(odvodnjavanje == null || odvodnjavanje.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<OdvodnjavanjeDto>>(odvodnjavanje));
        }

        [HttpGet("odvodnjavanjeId")]
        public async Task<ActionResult<OdvodnjavanjeDto>> GetOdvodnjavanje(Guid odvodnjavanjeId)
        {
            var odvodnjavanje = await _odvodnjavanjeRepository.GetOdvodnjavanjeById(odvodnjavanjeId);

            if(odvodnjavanje == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OdvodnjavanjeDto>(odvodnjavanje));
        }

        [HttpPost]
        public async Task<ActionResult<OdvodnjavanjeDto>> CreateOdvodnjavanje([FromBody] OdvodnjavanjeCreationDto odvodnjavanje)
        {
            try
            {
                Odvodnjavanje novoOdvodnjavanje = await _odvodnjavanjeRepository.CreateOdvodnjavanje(_mapper.Map<Odvodnjavanje>(odvodnjavanje));
                await _odvodnjavanjeRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetOdvodnjavanje", "Odvodnjavanje", new { odvodnjavanjeId = novoOdvodnjavanje.OdvodnjavanjeId });

                return Created(lokacija, _mapper.Map<OdvodnjavanjeDto>(novoOdvodnjavanje));
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja odvodnjavanja!");
            }
        }

        [HttpPut]
        public async Task<ActionResult<OdvodnjavanjeDto>> UpdateOdvodnjavanje(OdvodnjavanjeUpdateDto odvodnjavanje)
        {
            try
            {
                var staroOdvodnjavanje = await _odvodnjavanjeRepository.GetOdvodnjavanjeById(odvodnjavanje.OdvodnjavanjeId);

                if(staroOdvodnjavanje == null)
                {
                    return NotFound();
                }

                Odvodnjavanje novoOdvodnjavanje = _mapper.Map<Odvodnjavanje>(odvodnjavanje);

                _mapper.Map(staroOdvodnjavanje, novoOdvodnjavanje);
                await _odvodnjavanjeRepository.SaveChangesAsync();

                return Ok(_mapper.Map<OdvodnjavanjeDto>(staroOdvodnjavanje));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom izmene odvodnjavanja!");
            }
        }

        [HttpDelete("{odvodnjavanjeId}")]
        public async Task<ActionResult> DeleteOdvodnjavanje(Guid odvodnjavanjeId)
        {
            try
            {
                var odvodnjavanje = await _odvodnjavanjeRepository.GetOdvodnjavanjeById(odvodnjavanjeId);

                if(odvodnjavanje == null)
                {
                    return NotFound();
                }

                await _odvodnjavanjeRepository.DeleteOdvodnjavanje(odvodnjavanjeId);
                await _odvodnjavanjeRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja odvodnjavanja!");

            }
        }
        [HttpOptions]
        public IActionResult GetOdvodnjavanjeOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
