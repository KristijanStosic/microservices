using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Models.Etapa;

namespace JavnoNadmetanjeService.Profiles
{
    public class EtapaConfirmationProfile : Profile
    {
        public EtapaConfirmationProfile()
        {
            CreateMap<EtapaConfirmation, EtapaConfirmationDto>();
            CreateMap<Etapa, EtapaConfirmation>();
        }
    }
}
