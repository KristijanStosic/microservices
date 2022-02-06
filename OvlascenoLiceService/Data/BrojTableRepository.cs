using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OvlascenoLiceService.Data.Interfaces;
using OvlascenoLiceService.Entities;
using OvlascenoLiceService.Entities.Confirmations;
using OvlascenoLiceService.Entities.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OvlascenoLiceService.Data
{
    /// <summary>
    /// Repozitorijum za broj table
    /// </summary>
    public class BrojTableRepository : IBrojTableRepository
    {
        private readonly OvlascenoLiceContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Konstruktor repoa broj table
        /// </summary>
        /// <param name="context">Db context</param>
        /// <param name="mapper">AutoMapper</param>
        public BrojTableRepository(OvlascenoLiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Dobijanje podataka o svim brojevima table
        /// </summary>
        /// <returns></returns>
        public async Task<List<BrojTable>> GetAllBrojTable()
        {
            return await _context.BrojTable.ToListAsync();
        }
        /// <summary>
        /// Dobijanje broja table po id-u
        /// </summary>
        /// <param name="brojTableId">Id broja table</param>
        /// <returns></returns>
        public async Task<BrojTable> GetBrojTableById(Guid brojTableId)
        {
            return await _context.BrojTable.FirstOrDefaultAsync(t => t.BrojTableId == brojTableId);
        }
        /// <summary>
        /// Dobijanje brojeva table po obelezju oznaka table
        /// </summary>
        /// <param name="oznakaTable">Oznaka table</param>
        /// <returns></returns>
        public async Task<List<BrojTable>> GetBrojTableByOznakaTable(string oznakaTable)

        {
            return await _context.BrojTable.Where(t => t.OznakaTable == oznakaTable).ToListAsync();
        }
        /// <summary>
        /// Kreiranje broja table
        /// </summary>
        /// <param name="brojTable">Objekat broj table</param>
        /// <returns></returns>
        public async Task<BrojTableConfirmation> CreateBrojTable(BrojTable brojTable)
        {
            var kreiranBrojTable = await _context.BrojTable.AddAsync(brojTable);

            return _mapper.Map<BrojTableConfirmation>(kreiranBrojTable.Entity);
        }
        /// <summary>
        /// Brisanje broja table
        /// </summary>
        /// <param name="brojTableId">Id broja table</param>
        /// <returns></returns>
        public async Task DeleteBrojTable(Guid brojTableId)
        {
            var brojTable = await GetBrojTableById(brojTableId);

            _context.BrojTable.Remove(brojTable);
        }
        /// <summary>
        /// Cuvanje izmena u bazi podataka
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
