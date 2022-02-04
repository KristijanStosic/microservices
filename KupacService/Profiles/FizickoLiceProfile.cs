using AutoMapper;
using KupacService.Entities;
using KupacService.Model.Kupac;
using KupacService.Model.Kupac.FizickoLice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Profiles
{
    public class FizickoLiceProfile : Profile
    {

        public FizickoLiceProfile()
        {

            CreateMap<FizickoLice, FizickoLiceDto>()
                .ForMember(
                dest => dest.OvlascenaLica,
                opt => opt.Ignore());
            /*
            CreateMap<FizickoLiceDto, FizickoLice>()
                .ForMember(
                dest => dest.Prioriteti,
                opt => opt.Ignore());
            */


            CreateMap<KupacOtherServicesDto, FizickoLiceDto>();

            CreateMap<FizickoLice, FizickoLiceConfirmDto>();

            CreateMap<FizickoLice, FizickoLice>();

            CreateMap<FizickoLiceUpdateDto, FizickoLice>()
                .ForMember(
                dest => dest.Prioriteti,
                opt => opt.MapFrom(src => src.Prioriteti));


            
            CreateMap<FizickoLiceCreationDto, FizickoLice>()
                .ForMember(
                dest => dest.Prioriteti,
                opt => opt.MapFrom(src => src.Prioriteti));

        }
    }
}
