using AutoMapper;
using KupacService.Entities;
using KupacService.Model.Kupac.PravnoLice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Profiles
{
    public class PravnoLiceProfile : Profile
    {
        public PravnoLiceProfile()
        {
            CreateMap<PravnoLice, PravnoLiceDto>()
                .ForMember(
                dest => dest.Prioriteti,
                opt => opt.MapFrom(src => src.Prioriteti.Select(p => p.Opis).ToList())
                )
                .ForMember(
                dest => dest.KontaktOsoba,
                opt => opt.MapFrom(src => src.KontaktOsoba.Ime+" "+src.KontaktOsoba.Prezime+","+src.KontaktOsoba.Telefon)
                );

            CreateMap<PravnoLiceCreateDto, PravnoLice>()
                .ForMember(
                dest => dest.Prioriteti,
                opt => opt.Ignore());

            CreateMap<PravnoLice, PravnoLice>();

            CreateMap<PravnoLice, PravnoLiceConfirmDto>();

            CreateMap<PravnoLiceUpdateDto, PravnoLice>()
                .ForMember(
                dest => dest.Prioriteti,
                opt => opt.Ignore()
                );

        }
    }
}
