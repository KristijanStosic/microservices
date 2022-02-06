using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.Odvodnjavanje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class OdvodnjavanjeProfile : Profile
    {
        public OdvodnjavanjeProfile()
        {
            CreateMap<Odvodnjavanje, OdvodnjavanjeDto>();
            CreateMap<OdvodnjavanjeUpdateDto, Odvodnjavanje>();
            CreateMap<OdvodnjavanjeCreationDto, Odvodnjavanje>();
            CreateMap<Odvodnjavanje, Odvodnjavanje>();

        }
    }
}
