using AutoMapper;
using DocumentService.Entities;
using DocumentService.Models.Dokument;

namespace DocumentService.Profiles
{
    public class DokumentProfile : Profile
    {
        public DokumentProfile()
        {
            CreateMap<Dokument, DokumentDto>();

            CreateMap<CreateDokumentDto, Dokument>();

            CreateMap<UpdateDokumentDto, Dokument>();
        }
    }
}