using AutoMapper;
using KupacService.Entities;
using KupacService.Model.Kupac.FizickoLice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Profiles
{
    public class FizickoLiceProfile : Profile
    {

        public FizickoLiceProfile()
        {
            CreateMap<FizickoLice, FizickoLiceDto>();
        }
    }
}
