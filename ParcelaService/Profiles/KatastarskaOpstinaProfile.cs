using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.KatastarskaOpstina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class KatastarskaOpstinaProfile : Profile
    {
        public KatastarskaOpstinaProfile()
        {
            CreateMap<KatastarskaOpstina, KatastarskaOpstinaDto>();
            CreateMap<KatastarskaOpstinaUpdateDto, KatastarskaOpstina>();
            CreateMap<KatastarskaOpstinaCreationDto, KatastarskaOpstina>();
            CreateMap<KatastarskaOpstina, KatastarskaOpstina>();
        }
    }
}
