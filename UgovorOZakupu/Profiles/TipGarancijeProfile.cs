using AutoMapper;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.TipGarancije;

namespace UgovorOZakupu.Profiles
{
    public class TipGarancijeProfile : Profile
    {
        public TipGarancijeProfile()
        {
            CreateMap<TipGarancije, TipGarancijeDto>()
                .ReverseMap();
        }
    }
}