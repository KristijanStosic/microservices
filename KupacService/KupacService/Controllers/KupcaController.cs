using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using KupacService.Model.Kupac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Controllers
{
    [ApiController]
    [Route("api/kupac")]
    public class KupcaController : ControllerBase
    {
        private readonly IKupacRepository _kupacRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IMapper _mapper;

        public KupcaController(IKupacRepository kupacRepository,LinkGenerator linkGenerator,IMapper mapper)
        {
            this._kupacRepository = kupacRepository;
            this._linkGenerator = linkGenerator;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<KupacDto>>> GetKupci()
        {
            var kupci = await _kupacRepository.GetKupci();
            if(kupci == null && kupci.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KupacDto>>(kupci));
        }

        [HttpGet("{kupacId}")]
        public async Task<ActionResult<KupacDto>> GetKupacById(Guid kupacId)
        {
            var kupac = await _kupacRepository.GetKupacById(kupacId);

            if(kupac == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KupacDto>(kupac));
        }


        

    }
}
