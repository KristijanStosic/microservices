using AutoMapper;
using PrijavaService.Entities;
using PrijavaService.Models.Prijava;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Profiles 
{
    public class PrijavaProfile : Profile
    {
        public PrijavaProfile()
        {
              CreateMap<Prijava, PrijavaDto>()
                .ForMember(
                    dest => dest.JavnoNadmetanje,
                    opt => opt.Ignore());

               CreateMap<PrijavaUpdateDto, Prijava>();
               CreateMap<PrijavaCreationDto, Prijava>();
               CreateMap<Prijava, Prijava>();
        }
    }
}
