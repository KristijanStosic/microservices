using AutoMapper;
using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using JavnoNadmetanjeService.Entities.Confirmations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data
{
    public class JavnoNadmetanjeRepository : IJavnoNadmetanjeRepository
    {
        private readonly JavnoNadmetanjeContext _context;
        private readonly IMapper _mapper;
        public JavnoNadmetanjeRepository(JavnoNadmetanjeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JavnoNadmetanje>> GetAllJavnoNadmetanje()
        {
            return await _context.JavnoNadmetanje
                .Include(s => s.Status)
                .Include(t => t.Tip)
                .Include(e => e.Etape)
                .ToListAsync();
        }

        public async Task<JavnoNadmetanje> GetJavnoNadmetanjeById(Guid javnoNadmetanjeId)
        {
            return await _context.JavnoNadmetanje
                .Include(s => s.Status)
                .Include(t => t.Tip)
                .Include(e => e.Etape)
                .FirstOrDefaultAsync(jn => jn.JavnoNadmetanjeId == javnoNadmetanjeId);
        }

        public async Task<JavnoNadmetanjeConfirmation> CreateJavnoNadmetanje(JavnoNadmetanje javnoNadmetanje)
        {
            if (javnoNadmetanje.IzlicitiranaCena > (javnoNadmetanje.PocetnaCenaHektar * 2))
            {
                javnoNadmetanje.VisinaDopuneDepozita = (int)(javnoNadmetanje.IzlicitiranaCena * 0.5);
            }
            else
            {
                javnoNadmetanje.VisinaDopuneDepozita = (int)(javnoNadmetanje.IzlicitiranaCena * 0.1);
            }

            var kreiranoNadmetanje = await _context.JavnoNadmetanje.AddAsync(javnoNadmetanje);

            return _mapper.Map<JavnoNadmetanjeConfirmation>(kreiranoNadmetanje.Entity);
        }

        public async Task DeleteJavnoNadmetanje(Guid javnoNadmetanjeId)
        {
            var javnoNadmetanje = await GetJavnoNadmetanjeById(javnoNadmetanjeId);

            _context.JavnoNadmetanje.Remove(javnoNadmetanje);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
