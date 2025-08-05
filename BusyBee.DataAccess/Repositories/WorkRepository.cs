using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusyBee.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusyBee.DataAccess.Repositories
{
    public class WorkRepository : IWorkRepository
    {
        private readonly BusyBeeDBContext _context;

        public WorkRepository(BusyBeeDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Work>> GetAllWorks()
        {
            return await _context.Works.ToListAsync();
        }

        public async Task<Work> GetWorkById(int id)
        {
            return await _context.Works.FindAsync(id);
        }

        public async Task AddWork(Work work)
        {
            if (work == null) throw new ArgumentNullException(nameof(work));
            _context.Works.Add(work);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateWork(Work work)
        {
            if (work == null) throw new ArgumentNullException(nameof(work));
            _context.Works.Update(work);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteWork(int id)
        {
            var work = await GetWorkById(id);
            if (work == null) throw new KeyNotFoundException($"Work with ID {id} not found.");
            _context.Works.Remove(work);
            await _context.SaveChangesAsync();

        }
        public async Task<IEnumerable<Work>> GetWorksByCategory(int categoryId)
        {
            return await _context.Works
                .Where(w => w.WorkCategory.Id == categoryId)
                .ToListAsync();
        }

        public async Task AddWork(Work work, int categoryId)
        {
            if (work == null) throw new ArgumentNullException(nameof(work));
            var category = await _context.WorkCategories.FindAsync(categoryId);
            if (category == null) throw new KeyNotFoundException($"WorkCategory with ID {categoryId} not found.");
            work.WorkCategory = category;
            _context.Works.Add(work);
            await _context.SaveChangesAsync();
        }
    }
}
