using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Models;

namespace ZalbaService.Profiles
{
    // <summary>
    /// Maper za klasu tip zalbe
    /// </summary>
    public class TipZalbeProfile : Profile
    {
        /// <summary>
        /// Konstruktor za mapiranje tipa zalbe
        /// </summary>
        public TipZalbeProfile()
        {
            CreateMap<TipZalbe, TipZalbeCreateDto>().ReverseMap();
            CreateMap<TipZalbeUpdateDto, TipZalbe>();
            CreateMap<TipZalbe, TipZalbe>();
            CreateMap<TipZalbe, TipZalbeDto>();
        }
    }
}
