using AutoMapper;
using KupacService.Entities;
using KupacService.Model.Kupac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Profiles
{
    public class KupacProfile : Profile
    {
        public KupacProfile()
        {
            CreateMap<FizickoLice, KupacDto>()
                .ForMember(
                dest => dest.Naziv,
                opt => opt.MapFrom(src => src.Ime+" "+src.Prezime))
                .ForMember(
                dest => dest.KontaktOsoba,
                opt => opt.Ignore())
                .ForMember(
                dest => dest.Faks,
                opt => opt.Ignore())
                  .ForMember(
                dest => dest.OvlascenaLica,
                opt => opt.Ignore());

            CreateMap<PravnoLice, KupacDto>()
                  .ForMember(
                dest => dest.OvlascenaLica,
                opt => opt.Ignore());

            CreateMap<KupacOtherServicesDto, KupacDto>();
        }
    }
}
