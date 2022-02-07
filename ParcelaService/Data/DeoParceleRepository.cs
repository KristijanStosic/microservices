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
    public class DeoParceleRepository : IDeoParceleRepository
    {
        private readonly ParcelaContext _context;
        private readonly IMapper _mapper;

        public DeoParceleRepository(ParcelaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeoParceleConfirmation> CreateDeoParcele(DeoParcele deoParcele)
        {
            var kreiranDeoParcele = await _context.DeoParcele.AddAsync(deoParcele);

            return _mapper.Map<DeoParceleConfirmation>(kreiranDeoParcele.Entity);
        }

        public async Task DeleteDeoParcele(Guid parcelaId)
        {
            var deoParcele = await GetDeoParceleById(parcelaId);

            _context.DeoParcele.Remove(deoParcele);
        }

        public async Task<List<DeoParcele>> GetAllDeoParcele()
        {
            return await _context.DeoParcele
                .Include(k => k.Kultura)
                .Include(kl => kl.Klasa)
                .Include(o => o.Obradivost)
                .Include(p => p.Parcela)
                .Include(p => p.Parcela).ThenInclude(zs => zs.ZasticenaZona)
                .Include(p => p.Parcela).ThenInclude(ko => ko.KatastarskaOpstina)
                .Include(p => p.Parcela).ThenInclude(os => os.OblikSvojine)
                .Include(p => p.Parcela).ThenInclude(od => od.Odvodnjavanje)
                .ToListAsync();
        }

        public async Task<DeoParcele> GetDeoParceleById(Guid deoParceleId)
        {
            return await _context.DeoParcele
                .Include(k => k.Kultura)
                .Include(kl => kl.Klasa)
                .Include(o => o.Obradivost)
                .Include(p => p.Parcela)
                .Include(p => p.Parcela).ThenInclude(zs => zs.ZasticenaZona)
                .Include(p => p.Parcela).ThenInclude(ko => ko.KatastarskaOpstina)
                .Include(p => p.Parcela).ThenInclude(os => os.OblikSvojine)
                .Include(p => p.Parcela).ThenInclude(od => od.Odvodnjavanje).FirstOrDefaultAsync(dp => dp.DeoParceleId == deoParceleId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
