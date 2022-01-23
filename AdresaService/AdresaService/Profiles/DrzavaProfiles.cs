using AdresaService.Entities;
using AdresaService.Model.Drzava;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AdresaService.Profiles
{
    public class DrzavaProfiles : Profile
    {
        public DrzavaProfiles()
        {
            CreateMap<Drzava, DrzavaDto>();
            CreateMap<DrzavaDto, Drzava>();
            CreateMap<DrzavaUpdateDto, Drzava>();
            CreateMap<Drzava, Drzava>();
        }
    }
}
