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
            CreateMap<Komisija, KomisijaDTO>();
            CreateMap<KomisijaCreateDTO, Komisija>()
                .ForMember(
                    dest => dest.ClanoviKomisije,
                    opt => opt.Ignore());
            CreateMap<Guid, Licnost>()
                .ForMember(
                    dest => dest.LicnostId,
                    opt => opt.MapFrom(src => src));
            CreateMap<KomisijaUpdateDTO, Komisija>()
                .ForMember(
                    dest => dest.ClanoviKomisije,
                    opt => opt.Ignore());
            CreateMap<Komisija, Komisija>();
            CreateMap<KomisijaConfirmation, KomisijaConfirmationDTO>();
            CreateMap<Komisija, KomisijaConfirmationDTO>();
            CreateMap<Komisija, KomisijaConfirmation>();
        }


    }
}



        