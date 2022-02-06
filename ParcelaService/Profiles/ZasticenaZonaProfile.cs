using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.ZasticenaZona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class ZasticenaZonaProfile : Profile
    {
        public ZasticenaZonaProfile()
        {
            CreateMap<ZasticenaZona, ZasticenaZonaDto>();
            CreateMap<ZasticenaZonaUpdateDto, ZasticenaZona>();
            CreateMap<ZasticenaZonaCreationDto, ZasticenaZona>();
            CreateMap<ZasticenaZona, ZasticenaZona>();
        }
    }
}
