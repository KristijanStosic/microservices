using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.Obradivost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class ObradivostProfile : Profile
    {
        public ObradivostProfile()
        {
            CreateMap<Obradivost, ObradivostDto>();
            CreateMap<ObradivostUpdateDto, Obradivost>();
            CreateMap<ObradivostCreationDto, Obradivost>();
            CreateMap<Obradivost, Obradivost>();
        }
    }
}
