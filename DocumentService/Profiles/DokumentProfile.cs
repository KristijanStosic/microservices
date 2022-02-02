using AutoMapper;
using DocumentService.Entities;
using DocumentService.Models.Dokument;

namespace DocumentService.Profiles
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