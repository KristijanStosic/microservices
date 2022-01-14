using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Models;

namespace ZalbaService.Profiles
{
    public class StatusZalbeProfile : Profile
    {
        public StatusZalbeProfile()
        {
            CreateMap<StatusZalbe, StatusZalbeCreateDto>().ReverseMap();
            CreateMap<StatusZalbeUpdateDto, StatusZalbe>();
            CreateMap<StatusZalbe, StatusZalbe>();
        }
    }
}
