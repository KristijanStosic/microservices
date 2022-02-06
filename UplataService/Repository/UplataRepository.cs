using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UplataService.Entities;
using UplataService.Entities.DataContext;

namespace UplataService.Repository
{
    public class UplataRepository : IUplataRepository
    {
        private readonly UplataContext _context;
        private readonly IMapper _mapper;

        public UplataRepository(UplataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Uplata>> GetAllUplate()
        {
            return await _context.Uplata.ToListAsync();
        }

        public async Task<Uplata> GetUplataById(Guid uplataId)
        {
            return await _context.Uplata.FirstOrDefaultAsync(u => u.UplataId == uplataId);
        }

        public async Task<UplataConfirmation> CreateUplata(Uplata uplata)
        {
            var kreiranaUplata = await _context.Uplata.AddAsync(uplata);

            await _context.SaveChangesAsync();

            return _mapper.Map<UplataConfirmation>(kreiranaUplata.Entity);
        }
        public async Task DeleteUplata(Guid uplataId)
        {
            var uplata = await GetUplataById(uplataId);

            _context.Uplata.Remove(uplata);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUplata(Uplata uplata)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsValidUplata(string brojRacuna)
        {
            var listaUplata = await _context.Uplata.Where(tz => tz.BrojRacuna == brojRacuna).ToListAsync();

            return listaUplata.Count == 0;
        }
    }
}
