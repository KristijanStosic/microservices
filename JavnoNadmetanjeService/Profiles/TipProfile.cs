using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models;

namespace JavnoNadmetanjeService.Profiles
{
    public class TipProfile : Profile
    {
        public TipProfile()
        {
            CreateMap<Tip, TipDto>().ReverseMap();
            CreateMap<TipUpdateDto, Tip>();
            CreateMap<Tip, Tip>();
        }
    }
}
