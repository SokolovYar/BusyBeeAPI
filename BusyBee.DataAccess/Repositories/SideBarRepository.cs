using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusyBee.Domain.Models;



namespace BusyBee.DataAccess.Repositories
{
    public class SideBarRepository : ISideBarRepository
    {
        private readonly BusyBeeDBContext _context;
        public SideBarRepository(BusyBeeDBContext context)
        {
            _context = context;
        }

        public async Task<int> GetSpecialistAllAsync() =>
            await _context.Specialists.CountAsync();

        /* public async Task<int> GetSpecialistCurrentAsync() =>
             await _context.Specialists.CountAsync(s => s.IsOnline);*/

        public async Task<int> GetSpecialistCurrentAsync() 
        {
            var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-Specialist.TIME_TO_ONLINE);
            return await _context.Specialists
                         .CountAsync(s => s.LastActivityTime >= fiveMinutesAgo);
        }
       
        public async Task<List<string>> GetCategoryNamesAsync() =>
            await _context.WorkCategories.Select(c => c.Name).Distinct().ToListAsync();

        public async Task<List<string>> GetTagNamesAsync() =>
            await _context.Works.Select(w => w.Name).Distinct().ToListAsync();
    }
}
