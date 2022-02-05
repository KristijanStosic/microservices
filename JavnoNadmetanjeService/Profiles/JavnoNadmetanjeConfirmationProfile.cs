using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using JavnoNadmetanjeService.Models.JavnoNadmetanje;

namespace JavnoNadmetanjeService.Profiles
{
    public class JavnoNadmetanjeConfirmationProfile : Profile
    {
        public JavnoNadmetanjeConfirmationProfile()
        {
            CreateMap<JavnoNadmetanjeConfirmation, JavnoNadmetanjeConfirmationDto>();
            CreateMap<JavnoNadmetanje, JavnoNadmetanjeConfirmation>();
        }
    }
}
