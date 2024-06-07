using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;

public class CategoryDAO
{
    private readonly AppDbContext _context;

    private static CategoryDAO instance;

    private CategoryDAO()
    {
        _context = new AppDbContext();
    }

    public static CategoryDAO Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CategoryDAO();
            }
            return instance;
        }
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        return await _context.Categories.FindAsync(categoryId);
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
