using System.Linq;
using AutoMapper;
using UgovorOZakupu.Entities;
using UgovorOZakupu.Models.Confirmations;
using UgovorOZakupu.Models.UgovorOZakupu;

namespace UgovorOZakupu.Profiles
{
    public class UgovorOZakupuProfile : Profile
    {
        public UgovorOZakupuProfile()
        {
            CreateMap<Entities.UgovorOZakupu, UgovorOZakupuDto>()
                .ForMember(u => u.TipGarancije, options =>
                    options.MapFrom(d => d.TipGarancije.NazivTipa)
                )
                .ForMember(u => u.RokoviDospeca, options =>
                    options.MapFrom(u => u.RokoviDospeca.Select(r => r.Rok))
                );

            CreateMap<CreateUgovorOZakupuDto, Entities.UgovorOZakupu>()
                .ForMember(u => u.RokoviDospeca, options =>
                    options.MapFrom(u =>
                        u.RokoviDospeca.Select(r =>
                            new RokDospeca
                            {
                                Rok = r
                            }
                        )
                    )
                );

            CreateMap<Entities.UgovorOZakupu, UgovorOZakupuConfirmation>();

            CreateMap<UpdateUgovorOZakupuDto, Entities.UgovorOZakupu>();
        }
    }
}