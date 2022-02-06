using AutoMapper;
using LicitacijaService.Entities;
using LicitacijaService.Entities.Confirmations;
using LicitacijaService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Profiles
{
    public class LicitacijaConfirmationProfile : Profile
    {
        public LicitacijaConfirmationProfile()
        {
            CreateMap<LicitacijaConfirmation, LicitacijaConfirmationDto>();
            CreateMap<Licitacija, LicitacijaConfirmation>();
        }
    }
}
