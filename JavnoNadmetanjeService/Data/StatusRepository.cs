using JavnoNadmetanjeService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.Data
{
    public class StatusRepository : IStatusRepository
    {
        private readonly JavnoNadmetanjeContext _context;

        public StatusRepository(JavnoNadmetanjeContext context)
        {
            _context = context;
        }
        public async Task<List<Status>> GetAllStatus(string nazivStatusa = null)
        {
            return await _context.Status
                .Where(s => (nazivStatusa == null || s.NazivStatusa == nazivStatusa))
                .ToListAsync();
        }
        public async Task<Status> GetStatusById(Guid statusId)
        {
            return await _context.Status.FirstOrDefaultAsync(s => s.StatusId == statusId);
        }
        public async Task<Status> CreateStatus(Status status)
        {
            _context.Status.Add(status);
            await _context.SaveChangesAsync();

            return status;
        }

        public async Task UpdateStatus()
        {
            await _context.SaveChangesAsync();  //moze se zameniti i da bude metoda samo save changes async, za sada sam ostavio samo cuvanje promena ovde
        }

        public async Task DeleteStatus(Guid statusId)
        {
            var status = await GetStatusById(statusId);

            _context.Status.Remove(status);
            await _context.SaveChangesAsync();
        }

    }
}
