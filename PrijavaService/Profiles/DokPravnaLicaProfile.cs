using AutoMapper;
using PrijavaService.Entities;
using PrijavaService.Models.DokPravnoLice;

namespace PrijavaService.Profiles
{
    public class DokPravnaLicaProfile : Profile
    {
        public DokPravnaLicaProfile()
        {
            CreateMap<DokPravnaLica, DokPravnaLicaDto>();
            CreateMap<DokPravnaLica, DokPravnaLica>();
            CreateMap<DokPravnaLicaUpdateDto, DokPravnaLica>();
            CreateMap<DokPravnaLicaCreateDto, DokPravnaLica>();

        }

    }
}
