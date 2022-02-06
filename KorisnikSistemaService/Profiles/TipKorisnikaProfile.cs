using KorisnikSistemaService.Entities;
using KorisnikSistemaService.Models.TipKorisnika;
using AutoMapper;
namespace KorisnikSistemaService.Profiles
{
    public class TipKorisnikaProfile : Profile
    {
        public TipKorisnikaProfile()
        {
            CreateMap<TipKorisnika, TipKorisnikaDto>();
            CreateMap<TipKorisnikaUpdateDto, TipKorisnika>();
            CreateMap<TipKorisnikaCreationDto, TipKorisnika>();
            CreateMap<TipKorisnika, TipKorisnika>();
        }
    }
}
