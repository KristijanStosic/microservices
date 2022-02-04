using AutoMapper;
using KupacService.Entities.ManyToMany;
using KupacService.Model.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Profiles
{
    public class KupacOvlascenoLiceProfile: Profile
    {
        public KupacOvlascenoLiceProfile()
        {
            CreateMap<KupacOvlascenoLice, KupacOvlascenoLiceDto>();
        }
    }
}
