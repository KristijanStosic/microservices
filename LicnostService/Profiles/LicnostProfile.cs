using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LicnostService.Entities;
using LicnostService.Models.Licnost;

namespace LicnostService.Profiles
{
    public class LicnostProfile : Profile
    {
        public LicnostProfile()
        {
            CreateMap<Licnost, LicnostCreateDto>().ReverseMap();
            CreateMap<LicnostUpdateDto, Licnost>();
            CreateMap<Licnost, Licnost>();
            CreateMap<Licnost, LicnostDto>().ReverseMap();
        }
    }
}
