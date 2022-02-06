using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.Parcela;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class ParcelaProfile : Profile
    {
        public ParcelaProfile()
        {
            CreateMap<Parcela, ParcelaDto>()
                .ForMember(
                    dest => dest.KatastarskaOpstina,
                    opt => opt.MapFrom(src => $"{ src.KatastarskaOpstina.NazivKatastarskeOpstine }"))
                .ForMember(
                    dest => dest.ZasticenaZona,
                    opt => opt.MapFrom(src => $"{ src.ZasticenaZona.BrojZasticeneZone }"))
                .ForMember(
                    dest => dest.Odvodnjavanje,
                    opt => opt.MapFrom(src => $"{ src.Odvodnjavanje.OpisOdvodnjavanja }"))
                .ForMember(
                    dest => dest.OblikSvojine,
                    opt => opt.MapFrom(src => $"{ src.OblikSvojine.OpisOblikaSvojine }"));

            CreateMap<ParcelaUpdateDto, Parcela>();
            CreateMap<ParcelaCreationDto, Parcela>();
            CreateMap<Parcela, Parcela>();
        }
    }
}
