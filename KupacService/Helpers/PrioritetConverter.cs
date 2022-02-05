using AutoMapper;
using KupacService.Data.Interfaces;
using KupacService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Helpers
{
    public class PrioritetConverter : ITypeConverter<Guid, Prioritet>
    {
        private readonly IPrioritetRepository _prioritetRepository;

        public PrioritetConverter(IPrioritetRepository prioritetRepository)
        {
            this._prioritetRepository = prioritetRepository;
        }

        public Prioritet Convert(Guid source, Prioritet destination, ResolutionContext context)
        {
            
            return _prioritetRepository.GetPrioritetById(source).Result;
        }
    }
}
