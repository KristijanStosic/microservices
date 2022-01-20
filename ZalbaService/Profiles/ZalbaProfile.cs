using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Entities.Confirmations;
using ZalbaService.Models.Zalba;

namespace ZalbaService.Profiles
{
    public class ZalbaProfile : Profile
    {
        public ZalbaProfile()
        {
            CreateMap<Zalba, ZalbaCreateDto>().ReverseMap();
            CreateMap<ZalbaUpdateDto, Zalba>().ReverseMap();
            CreateMap<Zalba, Zalba>();
            CreateMap<Zalba, ZalbaDto>();
            CreateMap<ZalbaConfirmation, ZalbaConfirmationDto>();
            CreateMap<Zalba, ZalbaConfirmation>();
        }
    }
}
