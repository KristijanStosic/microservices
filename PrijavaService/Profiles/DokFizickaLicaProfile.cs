using AutoMapper;
using PrijavaService.Entities;
using PrijavaService.Models.DokFizickoLice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Profiles
{
    public class DokFizickaLicaProfile : Profile
    {
        public DokFizickaLicaProfile()
        {
            CreateMap<DokFizickaLica, DokFizickaLicaDto>();
            CreateMap<DokFizickaLica, DokFizickaLica>();
            CreateMap<DokFizickaLicaUpdateDto, DokFizickaLica>();
            CreateMap<DokFizickaLicaCreateDto, DokFizickaLica>();

        }
    }
}
