using AutoMapper;
using ParcelaService.Entities;
using ParcelaService.Models.OblikSvojine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Profiles
{
    public class OblikSvojineProfile : Profile
    {
        public OblikSvojineProfile()
        {
            CreateMap<OblikSvojine, OblikSvojineDto>();
            CreateMap<OblikSvojineUpdateDto, OblikSvojine>();
            CreateMap<OblikSvojineCreationDto, OblikSvojine>();
            CreateMap<OblikSvojine, OblikSvojine>();
        }
    }
}
