using BusinessObject.Entities;
using DataAccess.DAO.Interface;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ICategoryDAO _categoryDAO;

        public CategoryRepository(ICategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _categoryDAO.GetAllCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoryDAO.GetCategoryByIdAsync(categoryId);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryDAO.AddCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryDAO.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _categoryDAO.DeleteCategoryAsync(categoryId);
        }
    }
}
