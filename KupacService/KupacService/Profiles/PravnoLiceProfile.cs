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
            CreateMap<PravnoLice, PravnoLiceDto>();
                

            CreateMap<PravnoLiceCreateDto, PravnoLice>()
                .ForMember(
                dest => dest.Prioriteti,
                opt => opt.MapFrom(src => src.Prioriteti));

            CreateMap<PravnoLice, PravnoLice>();

            CreateMap<PravnoLice, PravnoLiceConfirmDto>();

            CreateMap<PravnoLiceUpdateDto, PravnoLice>()
                .ForMember(
                dest => dest.Prioriteti,
                opt => opt.MapFrom(src => src.Prioriteti)
                );

        }
    }
}
