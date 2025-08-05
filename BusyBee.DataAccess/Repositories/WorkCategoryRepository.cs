using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusyBee.Domain.Models;
namespace BusyBee.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

public class WorkCategoryRepository
{
    private readonly BusyBeeDBContext _context;
    public WorkCategoryRepository(BusyBeeDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<WorkCategory>> GetAllCategoriesAsync()
    {
        return await _context.WorkCategories.ToListAsync();
    }

    public async Task<WorkCategory?> GetCategoryByIdAsync(int id)
    {
        return await _context.WorkCategories.FindAsync(id);
    }

    public async Task AddCategoryAsync(WorkCategory category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category), "Category cannot be null");
        }
        _context.WorkCategories.Add(category);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateCategoryAsync(WorkCategory category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category), "Category cannot be null");
        }
        _context.WorkCategories.Update(category);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteCategoryAsync(int id)
    {
        var category = await GetCategoryByIdAsync(id);
        if (category != null)
        {
            _context.WorkCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
