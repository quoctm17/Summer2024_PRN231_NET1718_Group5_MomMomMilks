using BusinessObject.Entities;
using Repository.Interface;

namespace Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await CategoryDAO.Instance.GetAllCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await CategoryDAO.Instance.GetCategoryByIdAsync(categoryId);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await CategoryDAO.Instance.AddCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await CategoryDAO.Instance.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await CategoryDAO.Instance.DeleteCategoryAsync(categoryId);
        }
    }
}
