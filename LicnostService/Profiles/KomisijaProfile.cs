using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LicnostService.Entities;
using LicnostService.Entities.Confirmations;
using LicnostService.Models.Komisija;

namespace LicnostService.Profiles
{
    public class KomisijaProfile : Profile
    {

        public KomisijaProfile()
        {
            CreateMap<Komisija, KomisijaDto>();
            CreateMap<KomisijaCreateDto, Komisija>()
                .ForMember(
                    dest => dest.ClanoviKomisije,
                    opt => opt.Ignore());
            CreateMap<Guid, Licnost>()
                .ForMember(
                    dest => dest.LicnostId,
                    opt => opt.MapFrom(src => src));
            CreateMap<KomisijaUpdateDto, Komisija>()
                .ForMember(
                    dest => dest.ClanoviKomisije,
                    opt => opt.Ignore());
            CreateMap<Komisija, Komisija>();
            CreateMap<KomisijaConfirmation, KomisijaConfirmationDto>();
            CreateMap<Komisija, KomisijaConfirmationDto>();
            CreateMap<Komisija, KomisijaConfirmation>();
        }


    }
}



        