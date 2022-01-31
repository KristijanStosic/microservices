using AutoMapper;
using DocumentService.Entities;
using DocumentService.Models.TipDokumenta;

namespace DocumentService.Profiles
{
    public class TipDokumentaProfile : Profile
    {
        public TipDokumentaProfile()
        {
            CreateMap<TipDokumenta, TipDokumentaDto>()
                .ReverseMap();

            CreateMap<UpdateTipDokumentaDto, TipDokumenta>();
        }
    }
}