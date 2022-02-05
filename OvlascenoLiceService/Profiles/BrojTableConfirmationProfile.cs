using AutoMapper;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using OvlascenoLiceService.Models.BrojTable;

namespace OvlascenoLiceService.Profiles
{
    /// <summary>
    /// Maper za klasu potvrde kreiranja broja table
    /// </summary>
    public class BrojTableConfirmationProfile : Profile
    {
        /// <summary>
        /// Konstruktor za mapiranje potvrde kreiranja broja table
        /// </summary>
        public BrojTableConfirmationProfile()
        {
            CreateMap<BrojTableConfirmation, BrojTableConfirmationDto>();
            CreateMap<BrojTable, BrojTableConfirmation>();
        }
    }
}
