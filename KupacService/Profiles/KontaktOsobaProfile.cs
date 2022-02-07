using AutoMapper;
using KupacService.Entities;
using KupacService.Model.KontaktOsoba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Profiles
{
    public class KontaktOsobaProfile : Profile
    {
        public KontaktOsobaProfile()
        {
            CreateMap<KontaktOsoba, KontaktOsobaDto>();
            CreateMap<KontaktOsobaDto, KontaktOsoba>();
            CreateMap<KontaktOsobaUpdateDto, KontaktOsoba>();
            CreateMap<KontaktOsoba, KontaktOsoba>();
            CreateMap<KontaktOsobaCreateDto, KontaktOsoba>();
        }
    }
}
