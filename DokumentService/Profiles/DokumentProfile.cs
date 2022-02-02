using AutoMapper;
using DokumentService.Entities;
using DokumentService.Models.Dokument;

namespace DokumentService.Profiles
{
    public class DokumentProfile : Profile
    {
        public DokumentProfile()
        {
            CreateMap<Dokument, DokumentDto>()
                .ForMember(d => d.TipDokumenta, options =>
                    options.MapFrom(d => d.TipDokumenta.NazivTipa)
                );

            CreateMap<CreateDokumentDto, Dokument>();

            CreateMap<UpdateDokumentDto, Dokument>();
        }
    }
}