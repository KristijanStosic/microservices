using AutoMapper;
using ZalbaService.Entities;
using ZalbaService.Models;

namespace ZalbaService.Profiles
{
    /// <summary>
    /// Maper za klasu radnja za zalbu
    /// </summary>
    public class RadnjaZaZalbuProfile : Profile
    {
        /// <summary>
        /// Konstruktor za mapiranje radnje za zalbu
        /// </summary>
        public RadnjaZaZalbuProfile()
        {
            CreateMap<RadnjaZaZalbu, RadnjaZaZalbuCreateDto>().ReverseMap();
            CreateMap<RadnjaZaZalbuUpdateDto, RadnjaZaZalbu>();
            CreateMap<RadnjaZaZalbu, RadnjaZaZalbu>();
            CreateMap<RadnjaZaZalbu, RadnjaZaZalbuDto>();
        }
    }
}
