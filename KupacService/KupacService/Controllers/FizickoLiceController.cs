using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.Kupac.FizickoLice;
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
    [Route("api/fizickoLice")]
    public class FizickoLiceController : ControllerBase
    {
        private readonly IFizickoLiceRepository _fizickoLiceRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;
        private readonly IPrioritetRepository _prioritetRepository;

        public FizickoLiceController(IFizickoLiceRepository fizickoLiceRepository,LinkGenerator linkGenerator,IMapper mapper,IPrioritetRepository prioritetRepository)
        {
            this._fizickoLiceRepository = fizickoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
            this._prioritetRepository = prioritetRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<FizickoLiceDto>>> GetFizickoLice(string ime, string prezime, string brojRacuna )
        {
            var fizickaLica = await _fizickoLiceRepository.GetFizickoLice(ime, prezime, brojRacuna);

            if(fizickaLica == null || fizickaLica.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<FizickoLiceDto>>(fizickaLica));

        }
        [HttpGet("{kupacId}")]
        public async Task<ActionResult<FizickoLiceDto>> GetFizickoLiceById(Guid kupacId)
        {
            var fizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(kupacId);

            if(fizickoLice == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FizickoLiceDto>(fizickoLice));

        }
        [HttpPost]
        public async Task<ActionResult<FizickoLiceConfirmDto>> CreateFizickoLice([FromBody]FizickoLiceCreationDto fizickoLice)
        {
            try
            {
                FizickoLice newFizickoLice = _mapper.Map<FizickoLice>(fizickoLice);

                if (fizickoLice.Prioriteti != null && fizickoLice.Prioriteti.Count > 0)
                {
                    List<Prioritet> prioriteti = new List<Prioritet>();

                    foreach (Guid prioritet in fizickoLice.Prioriteti)
                    {
                        Prioritet tempPrioritet = await _prioritetRepository.GetPrioritetById(prioritet);
                        if (tempPrioritet == null)
                        {
                            continue;
                        }
                        prioriteti.Add(tempPrioritet);
                    }

                    newFizickoLice.Prioriteti = prioriteti;
                }
                await _fizickoLiceRepository.CreateFizickoLice(newFizickoLice);
                await _fizickoLiceRepository.SaveChangesAsync();

                string link = _linkGenerator.GetPathByAction("GetFizickoLiceById", "FizickoLice", new { kupacId = newFizickoLice.KupacId });


                return Created(link,_mapper.Map<FizickoLiceConfirmDto>(newFizickoLice));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }
        
        [HttpPut]
        public async Task<ActionResult<FizickoLiceDto>> UpdateFizickoLice([FromBody]FizickoLiceUpdateDto fizickoLiceUpdate)
        {
           
                FizickoLice oldFizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(fizickoLiceUpdate.KupacId);

                if(oldFizickoLice == null)
                {
                    return NoContent();
                }
                FizickoLice newFizickoLice = _mapper.Map<FizickoLice>(fizickoLiceUpdate);
       
                _mapper.Map(newFizickoLice, oldFizickoLice);
                
                 if (fizickoLiceUpdate.Prioriteti != null && fizickoLiceUpdate.Prioriteti.Count > 0)
                 {
                    List<Prioritet> prioriteti = new List<Prioritet>();
                    foreach (string prioritetId in fizickoLiceUpdate.Prioriteti)
                    {
                        Prioritet prioritet = await _prioritetRepository.GetPrioritetById(Guid.Parse(prioritetId));
                        if (prioritet == null)
                        {
                            continue;
                        }
                        prioriteti.Add(prioritet);
                    }
                    oldFizickoLice.Prioriteti = prioriteti;
                  }

                await _fizickoLiceRepository.SaveChangesAsync();

                return Ok(_mapper.Map<FizickoLiceDto>(oldFizickoLice));


        }

        [HttpDelete("{kupacId}")]
        public async Task<IActionResult> DeleteFizickoLice(Guid kupacId)
        {
            try
            {
                FizickoLice fizickoLice = await _fizickoLiceRepository.GetFizickoLiceById(kupacId);

                if(fizickoLice == null)
                {
                    return NotFound();
                }

                await _fizickoLiceRepository.DeleteFizickoLice(kupacId);
                await _fizickoLiceRepository.SaveChangesAsync();

                return Ok();

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

    }
}
