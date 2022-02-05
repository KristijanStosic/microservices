using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.Kultura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class KulturaProfile : Profile
    {
        public KulturaProfile()
        {
            CreateMap<Kultura, KulturaDto>();
            CreateMap<KulturaUpdateDto, Kultura>();
            CreateMap<KulturaCreationDto, Kultura>();
            CreateMap<Kultura, Kultura>();
        }
    }
}
