using AutoMapper;
using UplataService.Entities;
using UplataService.Model;

namespace UplataService.Profiles
{
    public class UplataProfile : Profile
    {
        public UplataProfile()
        {
            CreateMap<Uplata, UplataCreateDto>().ReverseMap();
            CreateMap<UplataUpdateDto, Uplata>().ReverseMap();
            CreateMap<Uplata, Uplata>();
            CreateMap<Uplata, UplataDto>();
            CreateMap<UplataConfirmation, UplataConfirmationDto>();
            CreateMap<Uplata, UplataConfirmation>();
        }
    }
}
