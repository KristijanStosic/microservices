using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Profiles
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<Status, StatusDto>().ReverseMap(); //obostrano mapiranje
            CreateMap<StatusUpdateDto, Status>();
            CreateMap<Status, Status>(); //za update
        }
    }
}
