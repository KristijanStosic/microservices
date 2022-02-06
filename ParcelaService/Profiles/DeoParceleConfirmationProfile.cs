using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Entities.Confirmations;
using ParcelaService.Models.DeoParcele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class DeoParceleConfirmationProfile : Profile
    {
        public DeoParceleConfirmationProfile()
        {
            CreateMap<DeoParceleConfirmation, DeoParceleDto>();
            CreateMap<DeoParcele, DeoParceleConfirmation>();
        }
    }
}
