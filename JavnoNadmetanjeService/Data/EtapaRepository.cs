using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data
{
    public class EtapaRepository : IEtapaRepository
    {
        private readonly JavnoNadmetanjeContext _context;
        private readonly IMapper _mapper;

        public EtapaRepository(JavnoNadmetanjeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Etapa>> GetAllEtapa()
        {
            return await _context.Etapa.ToListAsync();
        }

        public async Task<Etapa> GetEtapaById(Guid etapaId)
        {
            return await _context.Etapa.FirstOrDefaultAsync(e => e.EtapaId == etapaId);
        }
        public async Task<EtapaConfirmation> CreateEtapa(Etapa etapa)
        {
            var kreiranaEtapa = await _context.Etapa.AddAsync(etapa);

            return _mapper.Map<EtapaConfirmation>(kreiranaEtapa.Entity);
        }

        public async Task DeleteEtapa(Guid etapaId)
        {
            var etapa = await GetEtapaById(etapaId);

            _context.Etapa.Remove(etapa);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
