using JavnoNadmetanjeService.Data.Interfaces;
using JavnoNadmetanjeService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data
{
    public class TipRepository : ITipRepository
    {
        private readonly JavnoNadmetanjeContext _context;
        public TipRepository(JavnoNadmetanjeContext context)
        {
            _context = context;
        }

        public async Task<List<Tip>> GetAllTip(string nazivTipa = null)
        {
            return await _context.Tip
                .Where(t => (nazivTipa == null || t.NazivTipa == nazivTipa))
                .ToListAsync();
        }

        public async Task<Tip> GetTipById(Guid tipId)
        {
            return await _context.Tip.FirstOrDefaultAsync(t => t.TipId == tipId);
        }

        public async Task<Tip> CreateTip(Tip tip)
        {
            await _context.Tip.AddAsync(tip);

            return tip;
        }

        public async Task DeleteTip(Guid tipId)
        {
            var tip = await GetTipById(tipId);

            _context.Tip.Remove(tip);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
