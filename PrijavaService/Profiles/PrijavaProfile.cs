using AutoMapper;
using PrijavaService.Entities;
using PrijavaService.Models.Prijava;

namespace PrijavaService.Profiles 
{
    public class PrijavaProfile : Profile
    {
        public PrijavaProfile()
        {
              CreateMap<Prijava, PrijavaDto>()
                .ForMember(
                    dest => dest.JavnoNadmetanje,
                    opt => opt.Ignore());

               CreateMap<PrijavaUpdateDto, Prijava>();
               CreateMap<PrijavaCreationDto, Prijava>();
               CreateMap<Prijava, Prijava>();
        }
    }
}
