using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages
{
    public class shopping_cartModel : PageModel
    {
        private readonly CategoryService _categoryService;

        public shopping_cartModel(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<Category> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _categoryService.GetCategoriesAsync();
        }
    }
}
