using AutoMapper;
using KupacService.Entities;
using KupacService.Helpers;
using KupacService.Model.Prioritet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Profiles
{
    public class PrioritetProfile: Profile
    {
        public PrioritetProfile()
        {

            CreateMap<Prioritet, PrioritetDto>();
            CreateMap<PrioritetDto, Prioritet>();
            CreateMap<PrioritetUpdateDto, Prioritet>();
            CreateMap<Prioritet, Prioritet>();
            CreateMap<Guid, Prioritet>().ConvertUsing<PrioritetConverter>();

        }
    }
}
