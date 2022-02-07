using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.Klasa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class KlasaProfile : Profile
    {
        public KlasaProfile()
        {
            CreateMap<Klasa, KlasaDto>();
            CreateMap<KlasaUpdateDto, Klasa>();
            CreateMap<KlasaCreationDto, Klasa>();
            CreateMap<Klasa, Klasa>(); //update        
        }
    }
}
