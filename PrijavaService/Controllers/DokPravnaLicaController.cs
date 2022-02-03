using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using PrijavaService.Data.Interfaces;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Models.DokPravnoLice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrijavaService.Controllers
{
    [Route("api/dokPravnaLica")]
    [ApiController]
    public class DokPravnaLicaController : ControllerBase
    {
        private readonly IDokPravnaLicaRepository _dokPravnaLicaRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public DokPravnaLicaController(IDokPravnaLicaRepository dokPravnoLiceRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _dokPravnaLicaRepository = dokPravnoLiceRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }


        // GET: api/<DokPravnoLiceController>
        [HttpGet]
        public async Task<ActionResult<List<DokPravnaLicaDto>>> GetAllDocPravnaLica()
        {
            var docPravnaLica = await _dokPravnaLicaRepository.GetAllDokPravnaLica();

            if(docPravnaLica == null || docPravnaLica.Count == 0)
            {
                return NoContent();
            }
            return Ok(_mapper.Map<List<DokPravnaLicaDto>>(docPravnaLica));
        }

        // GET api/<DokPravnoLiceController>/5
        [HttpGet("{dokPravnaLicaId}")]
        public async Task<ActionResult<DokPravnaLicaDto>> GetDokPravnaLica(Guid dokPravnaLicaId)
        {
            var dokPravnoLice = await _dokPravnaLicaRepository.GetDokPravnaLicaById(dokPravnaLicaId);

            if (dokPravnoLice == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DokPravnaLicaDto>(dokPravnoLice));
        }

        // POST api/<DokPravnoLiceController>
        [HttpPost]
        public async Task<ActionResult<DokPravnaLicaConfirmationDto>> CreateDokPravnaLica([FromBody] DokPravnaLicaCreateDto dokPravnoLice)
        {
            try
            {
                DokPravnaLica mapiraniDok = _mapper.Map<DokPravnaLica>(dokPravnoLice);

                DokPravnaLicaConfirmation noviDokPravnoLice = await _dokPravnaLicaRepository.CreateDokPravnaLica(mapiraniDok);
                await _dokPravnaLicaRepository.SaveChangesAsync();

                string lokacija = _linkGenerator.GetPathByAction("GetDokPravnaLica", "DokPravnaLica", new { dokPravnaLicaId = noviDokPravnoLice.DokPravnaLicaId });

                return Created(lokacija, _mapper.Map<DokPravnaLicaConfirmationDto>(noviDokPravnoLice));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom unosa dokumanta pravnog lica");
            }
        }

        // PUT api/<DokPravnoLiceController>/5
        [HttpPut]
        public async Task<ActionResult<DokPravnaLicaDto>> UpdateDokPravnoLice(DokPravnaLicaUpdateDto dokPravnoLice)
        {
            try
            {
                var stariDokPravnoLice = await _dokPravnaLicaRepository.GetDokPravnaLicaById(dokPravnoLice.DokPravnaLicaId);
                var stareVrijednosti = JsonConvert.SerializeObject(stariDokPravnoLice);

                if(stariDokPravnoLice == null)
                {
                    return NotFound();
                }

                DokPravnaLica noviDokPravnoLice = _mapper.Map<DokPravnaLica>(dokPravnoLice);

                _mapper.Map(noviDokPravnoLice, stariDokPravnoLice);
                await _dokPravnaLicaRepository.SaveChangesAsync();

                return Ok(_mapper.Map<DokPravnaLicaDto>(stariDokPravnoLice));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom izmene dokumenta pravnog lica");
            }
        }

        // DELETE api/<DokPravnoLiceController>/5
        [HttpDelete("{dokPravnaLicaId}")]
        public async Task<IActionResult> DeleteDokPravnaLica(Guid dokPravnaLicaId)
        {
            try
            {
                var dokPravnoLice = await _dokPravnaLicaRepository.GetDokPravnaLicaById(dokPravnaLicaId);

                if(dokPravnoLice == null)
                {
                    return NotFound();
                }

                await _dokPravnaLicaRepository.DeleteDokPravnaLica(dokPravnaLicaId);
                await _dokPravnaLicaRepository.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greška prilikom brisanja dokumenta pravnog lica");
            }
        }

        [HttpOptions]
        public IActionResult GetDokPravnaLicaOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }
    }
}
