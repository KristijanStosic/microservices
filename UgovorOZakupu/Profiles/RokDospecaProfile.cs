using AutoMapper;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.RokDospeca;

namespace UgovorOZakupu.Profiles
{
    public class RokDospecaProfile : Profile
    {
        public RokDospecaProfile()
        {
            CreateMap<RokDospeca, RokDospecaDto>()
                .ReverseMap();

            CreateMap<CreateRokDospecaDto, RokDospeca>();

            CreateMap<UpdateRokDospecaDto, RokDospeca>();
        }
    }
}