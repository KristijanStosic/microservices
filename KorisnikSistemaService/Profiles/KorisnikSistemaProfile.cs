using AutoMapper;
using KorisnikSistemaService.Entities;
using KorisnikSistemaService.Models.KorisnikSistema;

namespace KorisnikSistemaService.Profiles
{
    public class KorisnikSistemaProfile : Profile
    {
        public KorisnikSistemaProfile()
        {
            CreateMap<KorisnikSistema, KorisnikSistemaDto>()
                .ForMember(tip => tip.TipKorisnika, 
                    x=>x.MapFrom(src => src.TipKorisnika.NazivTipaKorisnika));
            CreateMap<KorisnikSistemaUpdateDto, KorisnikSistema>();
            CreateMap<KorisnikSistemaCreationDto, KorisnikSistema>();
            CreateMap<KorisnikSistema, KorisnikSistema>();
            CreateMap<KorisnikSistema, KorisnikSistemaConformationDto>();
        }
    }
}
