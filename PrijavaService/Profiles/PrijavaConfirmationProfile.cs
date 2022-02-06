using AutoMapper;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Models.Prijava;

namespace PrijavaService.Profiles 
{
    public class PrijavaConfirmationProfile : Profile
    {
        public PrijavaConfirmationProfile()
        {
            CreateMap<PrijavaConfirmation, PrijavaConfirmationDto>();
            CreateMap<Prijava, PrijavaConfirmation>();
        }

    }
}
