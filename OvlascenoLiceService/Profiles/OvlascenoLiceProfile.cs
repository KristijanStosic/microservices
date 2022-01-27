using AutoMapper;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Models.OvlascenoLice;

namespace OvlascenoLiceService.Profiles
{
    /// <summary>
    /// Maper za klasu ovlasceno lice
    /// </summary>
    public class OvlascenoLiceProfile : Profile
    {
        /// <summary>
        /// Konstruktor za mapiranje ovlascenog lica
        /// </summary>
        public OvlascenoLiceProfile()
        {
            //CreateMap<OvlascenoLice, OvlascenoLiceDto>();
            CreateMap<OvlascenoLice, OvlascenoLiceDto>()
                .ForMember(
                    dest => dest.OvlascenoLice,
                    opt => opt.MapFrom(src => $"{ src.Ime } { src.Prezime }"))
                .ForMember(
                    dest => dest.BrojDokumenta,
                    opt => opt.MapFrom(src => src.BrojPasosa == null ? src.JMBG : src.BrojPasosa));
            CreateMap<OvlascenoLiceUpdateDto, OvlascenoLice>();
            CreateMap<OvlascenoLiceCreationDto, OvlascenoLice>();
            CreateMap<OvlascenoLice, OvlascenoLice>();
        }
    }
}
