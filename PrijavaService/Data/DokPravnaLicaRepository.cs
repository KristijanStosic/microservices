using AutoMapper;
using PrijavaService.Data.Interfaces;
using PrijavaService.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PrijavaService.Entities.DataContext;
using System.Linq;
using PrijavaService.Entities.Confirmations;

namespace PrijavaService.Data
{
    public class DokPravnaLicaRepository : IDokPravnaLicaRepository
    {
        private readonly PrijavaContext _context;
        private readonly IMapper _mapper;

        public DokPravnaLicaRepository(PrijavaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteDokPravnaLica(Guid DokPravnaLicaId)
        {
            var dokPravnoLice = await GetDokPravnaLicaById(DokPravnaLicaId);

            _context.DokPravnaLica.Remove(dokPravnoLice);
        }

        public async Task<List<DokPravnaLica>> GetAllDokPravnaLica()
        {
            return await _context.DokPravnaLica.ToListAsync();
        }

        public async Task<DokPravnaLica> GetDokPravnaLicaById(Guid DokPravnaLicaId)
        {
            return await _context.DokPravnaLica.FirstOrDefaultAsync(x => x.DokPravnaLicaId == DokPravnaLicaId);
        }

        public async Task<DokPravnaLicaConfirmation> CreateDokPravnaLica(DokPravnaLica dokPravnaLica)
        {
            var kreiraniDokPravnaLice = await _context.DokPravnaLica.AddAsync(dokPravnaLica);

            return _mapper.Map<DokPravnaLicaConfirmation>(kreiraniDokPravnaLice.Entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
