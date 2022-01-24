using System;
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

            CreateMap<CreateDokumentDto, Dokument>()
                .ForMember(dest => dest.Datum, opt =>
                    opt.MapFrom(source => source.Datum ?? DateTime.Now)
                )
                .ForMember(dest => dest.DatumDonosenjaDokumenta, opt =>
                    opt.MapFrom(source => source.DatumDonosenjaDokumenta ?? DateTime.Now)
                );

            CreateMap<UpdateDokumentDto, Dokument>()
                .ForMember(dest => dest.ZavodniBroj, opt =>
                    opt.MapFrom((source, dest) => source.ZavodniBroj ?? dest.ZavodniBroj)
                )
                .ForMember(dest => dest.Datum, opt =>
                    opt.MapFrom((source, dest) => source.Datum ?? dest.Datum)
                )
                .ForMember(dest => dest.DatumDonosenjaDokumenta, opt =>
                    opt.MapFrom((source, dest) => source.DatumDonosenjaDokumenta ?? dest.DatumDonosenjaDokumenta)
                )
                .ForMember(dest => dest.TipDokumentaId, opt =>
                    opt.MapFrom((source, dest) => source.TipDokumentaId ?? dest.TipDokumentaId)
                );
        }
    }
}