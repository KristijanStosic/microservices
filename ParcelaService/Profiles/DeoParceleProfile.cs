using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.DeoParcele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class DeoParceleProfile : Profile
    {
        public DeoParceleProfile()
        {
            CreateMap<DeoParcele, DeoParceleDto>()
                .ForMember(
                    dest => dest.Kultura,
                    opt => opt.MapFrom(src => $"{src.Kultura.NazivKulture}"))
                .ForMember(
                    dest => dest.Klasa,
                    opt => opt.MapFrom(src => $"{src.Klasa.KlasaNaziv}"))
                .ForMember(
                    dest => dest.Obradivost,
                    opt => opt.MapFrom(src => $"{src.Obradivost.OpisObradivosti}"))
               .ForMember(
                    dest => dest.BrojParcele,
                    opt => opt.MapFrom(src => $"{src.Parcela.BrojParcele}"))
              .ForMember(
                    dest => dest.KatastarskaOpstina,
                    opt => opt.MapFrom(src => $"{src.Parcela.KatastarskaOpstina.NazivKatastarskeOpstine}"))
               .ForMember(
                    dest => dest.ZasticenaZona,
                    opt => opt.MapFrom(src => $"{src.Parcela.ZasticenaZona.BrojZasticeneZone}"))
               .ForMember(
                    dest => dest.OblikSvojine,
                    opt => opt.MapFrom(src => $"{src.Parcela.OblikSvojine.OpisOblikaSvojine}"))
               .ForMember(
                    dest => dest.Odvodnjavanje,
                    opt => opt.MapFrom(src => $"{src.Parcela.Odvodnjavanje.OpisOdvodnjavanja}"));


            CreateMap<DeoParceleUpdateDto, DeoParcele>();
            CreateMap<DeoParceleCreationDto, DeoParcele>();
            CreateMap<DeoParcele, DeoParcele>();
        }
    }
}
