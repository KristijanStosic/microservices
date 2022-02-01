using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;

namespace JavnoNadmetanjeService.Profiles
{
    public class JavnoNadmetanjeProfile : Profile
    {
        public JavnoNadmetanjeProfile()
        {
            CreateMap<JavnoNadmetanje, JavnoNadmetanjeDto>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => $"{ src.Status.NazivStatusa }"))
                .ForMember(
                    dest => dest.Tip,
                    opt => opt.MapFrom(src => $"{ src.Tip.NazivTipa }"))
                .ForMember(
                    dest => dest.OvlascenaLica,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Kupac,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Kupci,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.DeloviParcele,
                    opt => opt.Ignore());
            CreateMap<JavnoNadmetanjeUpdateDto, JavnoNadmetanje>();
            CreateMap<JavnoNadmetanjeCreationDto, JavnoNadmetanje>();
            CreateMap<JavnoNadmetanje, JavnoNadmetanje>();
        }
    }
}
