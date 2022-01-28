using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.Kupac.FizickoLice;
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

        public FizickoLiceController(IFizickoLiceRepository fizickoLiceRepository,LinkGenerator linkGenerator,IMapper mapper)
        {
            this._fizickoLiceRepository = fizickoLiceRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<FizickoLice>>> GetFizickoLice(string ime, string prezime, string brojRacuna )
        {
            var fizickaLica = await _fizickoLiceRepository.GetFizickoLice(ime, prezime, brojRacuna);

            if(fizickaLica == null || fizickaLica.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<FizickoLiceDto>>(fizickaLica));


        }

    }
}
