using AutoMapper;
using LicitacijaService.Entities;
using LicitacijaService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Profiles
{
    public class LicitacijaProfile : Profile
    {
           public LicitacijaProfile()
        {
            CreateMap<Licitacija, LicitacijaDto>()
                .ForMember(
                    dest => dest.JavnaNadmetanja,
                    opt => opt.Ignore());
            
            CreateMap<LicitacijaUpdateDto, Licitacija>();
            CreateMap<LicitacijaCreationDto, Licitacija>();
            CreateMap<Licitacija, Licitacija>();

        }
    }
}
