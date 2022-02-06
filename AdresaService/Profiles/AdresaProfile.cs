using AdresaService.Entities;
using AdresaService.Model.Adresa;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Profiles
{
    public class AdresaProfile : Profile
    {
        public AdresaProfile()
        {
            CreateMap<Adresa, AdresaDto>()
                .ForMember(
                dest => dest.Drzava,
                opt => opt.MapFrom(src => src.Drzava.NazivDrzave));
            CreateMap<AdresaCreationDto, Adresa>();
            CreateMap<Adresa, AdresaConformationDto>();

            CreateMap<AdresaUpdateDto, Adresa>();
            CreateMap<Adresa, Adresa>();
        }
    }
}
