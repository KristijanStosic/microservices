using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.Etapa;

namespace JavnoNadmetanjeService.Profiles
{
    public class EtapaProfile : Profile
    {
        public EtapaProfile()
        {
            CreateMap<Etapa, EtapaDto>();
            CreateMap<EtapaUpdateDto, Etapa>();
            CreateMap<EtapaCreationDto, Etapa>();
            CreateMap<Etapa, Etapa>();
        }
    }
}
