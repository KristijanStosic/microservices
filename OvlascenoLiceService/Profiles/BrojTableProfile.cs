using AutoMapper;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Models.BrojTable;

namespace OvlascenoLiceService.Profiles
{
    /// <summary>
    /// Maper za klasu broj table
    /// </summary>
    public class BrojTableProfile : Profile
    {
        /// <summary>
        /// Konstruktor za mapiranje broja table
        /// </summary>
        public BrojTableProfile()
        {
            CreateMap<BrojTable, BrojTableDto>();
            CreateMap<BrojTableUpdateDto, BrojTable>();
            CreateMap<BrojTableCreationDto, BrojTable>();
            CreateMap<BrojTable, BrojTable>();
        }
    }
}
