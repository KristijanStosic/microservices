using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrijavaService.Data.Interfaces;
using PrijavaService.Entities;
using PrijavaService.Entities.Confirmations;
using PrijavaService.Entities.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrijavaService.Data
{
    public class DokFizickaLicaRepository : IDokFizickaLicaRepository
    {
        private readonly PrijavaContext _context;
        private readonly IMapper _mapper;

        public DokFizickaLicaRepository(PrijavaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DokFizickaLicaConfirmation> CreateDokFizickaLica(DokFizickaLica dokFizickaLica)
        {
            var kreiraniDokFizickaLica = await _context.DokFizickaLica.AddAsync(dokFizickaLica);

            return _mapper.Map<DokFizickaLicaConfirmation>(kreiraniDokFizickaLica.Entity);
        }

        public async Task DeleteDokFizickaLica(Guid DokFizickaLicaId)
        {
            var dokFizickoLice = await GetDokFizickaLicaById(DokFizickaLicaId);

            _context.DokFizickaLica.Remove(dokFizickoLice);
        }

        public async Task<List<DokFizickaLica>> GetAllDokFizickaLica()
        {
            return await _context.DokFizickaLica.ToListAsync();
        }

        public async Task<DokFizickaLica> GetDokFizickaLicaById(Guid DokFizickaLicaId)
        {
            return await _context.DokFizickaLica.FirstOrDefaultAsync(x => x.DokFizickaLicaId == DokFizickaLicaId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
