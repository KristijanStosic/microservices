using AutoMapper;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Models.Prijava;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Profiles 
{
    public class PrijavaConfirmationProfile : Profile
    {
        public PrijavaConfirmationProfile()
        {
            CreateMap<PrijavaConfirmation, PrijavaConfirmationDto>();
            CreateMap<Prijava, PrijavaConfirmation>();
        }

    }
}
