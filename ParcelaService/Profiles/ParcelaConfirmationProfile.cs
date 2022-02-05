using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Entities.Confirmations;
using ParcelaService.Models.Parcela;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class ParcelaConfirmationProfile : Profile
    {
        public ParcelaConfirmationProfile()
        {
            CreateMap<ParcelaConfirmation, ParcelaConfirmationDto>();
            CreateMap<Parcela, ParcelaConfirmation>();
        }
    }
}
