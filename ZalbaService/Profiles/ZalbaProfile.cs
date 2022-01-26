using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Entities.Confirmations;
using ZalbaService.Models.Zalba;

namespace ZalbaService.Profiles
{
    public class ZalbaProfile : Profile
    {
        public ZalbaProfile()
        {
            CreateMap<Zalba, ZalbaDto>()
                .ForMember(
                    dest => dest.StatusZalbe,
                    opt => opt.MapFrom(src => $"{ src.StatusZalbe.NazivStatusaZalbe }" ))
                .ForMember(
                    dest => dest.TipZalbe,
                    opt => opt.MapFrom(src => $"{ src.TipZalbe.NazivTipaZalbe }"))
                .ForMember(
                    dest => dest.RadnjaZaZalbu,
                    opt => opt.MapFrom(src => $"{ src.RadnjaZaZalbu.NazivRadnjeZaZalbu }"));
            CreateMap<Zalba, ZalbaCreateDto>().ReverseMap();
            CreateMap<ZalbaUpdateDto, Zalba>().ReverseMap();
            CreateMap<Zalba, Zalba>();
            CreateMap<ZalbaConfirmation, ZalbaConfirmationDto>();
            CreateMap<Zalba, ZalbaConfirmation>();
        }
    }
}
