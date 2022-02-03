using AutoMapper;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Models.DokFizickoLice;
using PrijavaService.Models.DokPravnoLice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Profiles
{
    public class DokFizickaLicaConfirmationProfile : Profile
    {
        public DokFizickaLicaConfirmationProfile()
        {
            CreateMap<DokFizickaLicaConfirmation, DokFizickaLicaConfirmationDto>();
            CreateMap<DokFizickaLica, DokFizickaLicaConfirmation>();
        }
    }
}
