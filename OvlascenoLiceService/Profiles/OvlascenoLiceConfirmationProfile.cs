using AutoMapper;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using OvlascenoLiceService.Models.OvlascenoLice;

namespace OvlascenoLiceService.Profiles
{
    /// <summary>
    /// Maper za klasu potvrde kreiranja ovlascenog lica
    /// </summary>
    public class OvlascenoLiceConfirmationProfile : Profile
    {
        /// <summary>
        /// Konstruktor za mapiranje potvrde ovlascenog lica
        /// </summary>
        public OvlascenoLiceConfirmationProfile()
        {
            CreateMap<OvlascenoLiceConfirmation, OvlascenoLiceConfirmationDto>();
            CreateMap<OvlascenoLice, OvlascenoLiceConfirmation>();
        }
    }
}
