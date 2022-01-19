using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;

namespace JavnoNadmetanjeService.Profiles
{
    public class JavnoNadmetanjeProfile : Profile
    {
        public JavnoNadmetanjeProfile()
        {
            CreateMap<JavnoNadmetanje, JavnoNadmetanjeDto>();
            CreateMap<JavnoNadmetanjeUpdateDto, JavnoNadmetanje>();
            CreateMap<JavnoNadmetanjeCreationDto, JavnoNadmetanje>();
            CreateMap<JavnoNadmetanje, JavnoNadmetanje>();
        }
    }
}
