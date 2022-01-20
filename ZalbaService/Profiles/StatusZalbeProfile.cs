using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Models;
using ZalbaService.Models.StatusZalbe;

namespace ZalbaService.Profiles
{
    public class StatusZalbeProfile : Profile
    {
        public StatusZalbeProfile()
        {
            CreateMap<StatusZalbe, StatusZalbeCreateDto>().ReverseMap();
            CreateMap<StatusZalbeUpdateDto, StatusZalbe>();
            CreateMap<StatusZalbe, StatusZalbe>();
            CreateMap<StatusZalbe, StatusZalbeDto>();
        }
    }
}
