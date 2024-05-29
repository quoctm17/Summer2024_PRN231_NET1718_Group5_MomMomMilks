using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly CategoryService _categoryService;

        public CheckoutModel(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<Category> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _categoryService.GetCategoriesAsync();
        }

        public void OnPostAsync()
        {
 
        }
    }
}
