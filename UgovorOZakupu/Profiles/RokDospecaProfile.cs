using AutoMapper;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.RokDospeca;

namespace UgovorOZakupu.Profiles
{
    public class RokDospecaProfile : Profile
    {
        public RokDospecaProfile()
        {
            CreateMap<RokDospeca, RokDospecaDto>();

            CreateMap<CreateRokDospecaDto, RokDospeca>();

            CreateMap<UpdateRokDospecaDto, RokDospeca>()
                .ForMember(dest => dest.Rok, opt =>
                    opt.MapFrom((source, dest) => source.Rok ?? dest.Rok)
                )
                .ForMember(dest => dest.UgovorOZakupuId, opt =>
                    opt.MapFrom((source, dest) => source.UgovorOZakupuId ?? dest.UgovorOZakupuId)
                );
        }
    }
}