using AutoMapper;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Models.Status;

namespace JavnoNadmetanjeService.Profiles
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<Status, StatusDto>();
            CreateMap<StatusUpdateDto, Status>();
            CreateMap<StatusCreationDto, Status>();
            CreateMap<Status, Status>(); //za update
        }
    }
}
