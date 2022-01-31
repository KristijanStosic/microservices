using AutoMapper;
using UgovorOZakupu.Models.UgovorOZakupu;

namespace UgovorOZakupu.Profiles
{
    public class UgovorOZakupuProfile : Profile
    {
        public UgovorOZakupuProfile()
        {
            CreateMap<Entities.UgovorOZakupu, UgovorOZakupuDto>();

            CreateMap<CreateUgovorOZakupuDto, Entities.UgovorOZakupu>();
            
            CreateMap<UpdateUgovorOZakupuDto, Entities.UgovorOZakupu>();
        }
    }
}