using AutoMapper;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Models.DokPravnoLice;

namespace PrijavaService.Profiles
{
    public class DokPravnaLicaConfirmationProfile : Profile
    {
        public DokPravnaLicaConfirmationProfile()
        {
            CreateMap<DokPravnaLicaConfirmation, DokPravnaLicaConfirmationDto>();
            CreateMap<DokPravnaLica, DokPravnaLicaConfirmation>();
        }
    }
}
