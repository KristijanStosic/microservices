using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Entities.Confirmations;
using ParcelaService.Entities.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data
{
    public class ParcelaRepository : IParcelaRepository
    {
        private readonly ParcelaContext _context;
        private readonly IMapper _mapper;

        public ParcelaRepository(ParcelaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ParcelaConfirmation> CreateParcela(Parcela parcela)
        {
            var kreiranaParcela = await _context.Parcela.AddAsync(parcela);

            return _mapper.Map<ParcelaConfirmation>(kreiranaParcela.Entity);
        }

        public async Task DeleteParcela(Guid parcelaId)
        {
            var parcela = await GetParcelaById(parcelaId);

            _context.Parcela.Remove(parcela);
        }

        public async Task<List<Parcela>> GetAllParcela()
        {
            return await _context.Parcela
                .Include(zz => zz.ZasticenaZona)
                .Include(os => os.OblikSvojine)
                .Include(o => o.Odvodnjavanje)
                .Include(ko => ko.KatastarskaOpstina)
                .Include(dp => dp.DeloviParcele).ToListAsync();
                
                
             
        }

        public async Task<Parcela> GetParcelaById(Guid parcelaId)
        {
            return await _context.Parcela
                .Include(zz => zz.ZasticenaZona)
                .Include(os => os.OblikSvojine)
                .Include(o => o.Odvodnjavanje)
                .Include(ko => ko.KatastarskaOpstina)
                .Include(dp => dp.DeloviParcele).FirstOrDefaultAsync(p => p.ParcelaId == parcelaId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
