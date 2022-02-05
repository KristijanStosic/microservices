using Microsoft.EntityFrameworkCore;
using ParcelaService.Data.Interfaces;
using ParcelaService.Entities;
using ParcelaService.Entities.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Data
{
    public class KatastarskaOpstinaRepository : IKatastarskaOpstinaRepository
    {
        private readonly ParcelaContext _context;

        public KatastarskaOpstinaRepository(ParcelaContext context)
        {
            _context = context;
        }

        public async Task<KatastarskaOpstina> CreateKatastarskaOpstina(KatastarskaOpstina katastarskaOpstina)
        {
            await _context.KatastarskaOpstina.AddAsync(katastarskaOpstina);

            return katastarskaOpstina;
        }

        public async Task DeleteKatastarskaOpstina(Guid katastarskaOpstinaId)
        {
            var katastarskaOpstina = await GetKatastarskaOpstinaById(katastarskaOpstinaId);

            _context.KatastarskaOpstina.Remove(katastarskaOpstina);
        }

        public async Task<List<KatastarskaOpstina>> GetAllKatastarskaOpstina(string nazivKatastarskeOpstine = null)
        {
            return await _context.KatastarskaOpstina
                .Where(ko => (nazivKatastarskeOpstine == null || ko.NazivKatastarskeOpstine == nazivKatastarskeOpstine))
                .ToListAsync();
        }

        public async Task<KatastarskaOpstina> GetKatastarskaOpstinaById(Guid katastarskaOpstinaId)
        {
            return await _context.KatastarskaOpstina.FirstOrDefaultAsync(ko => ko.KatastarskaOpstinaId == katastarskaOpstinaId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
