using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PrijavaService.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrijavaService.Models.DokFizickoLice;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijavaService.Controllers
{
    [Route("api/dokFizickaLica")]
    [ApiController]
    public class DokFizickaLicaController : ControllerBase
    {

        private readonly IDokFizickaLicaRepository _dokFizickaLicaRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public DokFizickaLicaController(IDokFizickaLicaRepository dokFizickaLicaRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _dokFizickaLicaRepository = dokFizickaLicaRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        // GET: api/<DokFizickaLicaController>
        [HttpGet]
        public async Task<ActionResult<List<DokFizickaLicaDto>>> GetAllDocFizickaLica()
        {
            var dokFizickaLica = await _dokFizickaLicaRepository.GetAllDokFizickaLica();

            if(dokFizickaLica == null || dokFizickaLica.Count == 0)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<List<DokFizickaLicaDto>>(dokFizickaLica));
        }

        // GET api/<DokFizickaLicaController>/5
        [HttpGet("{dokFizickaLicaId}")]
        public async Task<ActionResult<DokFizickaLicaDto>> GetDokFizickaLica(Guid dokFizickaLicaId)
        {
            var dokFizickoLice = await _dokFizickaLicaRepository.GetDokFizickaLicaById(dokFizickaLicaId);

            if (dokFizickoLice == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DokFizickaLicaDto>(dokFizickoLice));
        }

        // POST api/<DokFizickaLicaController>
        [HttpPost]
        public async Task<ActionResult<DokFizickaLicaConfirmationDto>> CreateDokFizickaLica([FromBody] DokFizickaLicaCreateDto dokFizickoLice)
        {
            try
            {
                DokFizickaLica mapiraniDok = _mapper.Map<DokFizickaLica>(dokFizickoLice);

                DokFizickaLicaConfirmation noviDokFizickoLice = await _dokFizickaLicaRepository.CreateDokFizickaLica(mapiraniDok);
                await _dokFizickaLicaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetDokFizickaLica", "DokFizickaLica", new { dokFizickaLicaId = noviDokFizickoLice.DokFizickaLicaId });

                return Created(lokacija, _mapper.Map<DokFizickaLicaConfirmationDto>(noviDokFizickoLice));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa dokumanta fizickog lica");
            }
        }

        // PUT api/<DokFizickaLicaController>/5
        [HttpPut]
        public async Task<ActionResult<DokFizickaLicaDto>> UpdateDokFizickoLice(DokFizickaLicaUpdateDto dokFizickoLice)
        {
            try
            {
                var stariDokFizickaLica = await _dokFizickaLicaRepository.GetDokFizickaLicaById(dokFizickoLice.DokFizickaLicaId);
                var stareVrijednosti = JsonConvert.SerializeObject(stariDokFizickaLica);

                if (stariDokFizickaLica == null)
                {
                    return NotFound();
                }

                DokFizickaLica noviDokFizickoLice = _mapper.Map<DokFizickaLica>(dokFizickoLice);

                _mapper.Map(noviDokFizickoLice, stariDokFizickaLica);
                await _dokFizickaLicaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<DokFizickaLicaDto>(stariDokFizickaLica));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene dokumenta fizickog lica");
            }

        }
        

        // DELETE api/<DokFizickaLicaController>/5
        [HttpDelete("{dokFizickaLicaId}")]
        public async Task<IActionResult> DeleteDokFizickaLica(Guid dokFizickaLicaId)
        {
            try
            {
                var dokFizickoLice = await _dokFizickaLicaRepository.GetDokFizickaLicaById(dokFizickaLicaId);

                if (dokFizickoLice == null)
                {
                    return NotFound();
                }

                await _dokFizickaLicaRepository.DeleteDokFizickaLica(dokFizickaLicaId);
                await _dokFizickaLicaRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja dokumenta fizickog lica");
            }
        }

        [HttpOptions]
        public IActionResult GetDokFizickaLicaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
