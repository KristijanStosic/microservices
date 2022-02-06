using AutoMapper;
using DokumentService.Entities;
using DokumentService.Models.TipDokumenta;

namespace DokumentService.Profiles
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