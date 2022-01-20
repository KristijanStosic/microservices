using AutoMapper;
using ZalbaService.Entities;
using ZalbaService.Models;

namespace ZalbaService.Profiles
{
    public class RadnjaZaZalbuProfile : Profile
    {
        public RadnjaZaZalbuProfile()
        {
            CreateMap<RadnjaZaZalbu, RadnjaZaZalbuCreateDto>().ReverseMap();
            CreateMap<RadnjaZaZalbuUpdateDto, RadnjaZaZalbu>();
            CreateMap<RadnjaZaZalbu, RadnjaZaZalbu>();
            CreateMap<RadnjaZaZalbu, RadnjaZaZalbuDto>();
        }
    }
}
