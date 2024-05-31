using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Milk
{
    public class IndexModel : PageModel
    {
        private readonly MilkService _milkService;
        private readonly CategoryService _categoryService;

        public IndexModel(MilkService milkService, CategoryService categoryService)
        {
            _milkService = milkService;
            _categoryService = categoryService;
        }

        public List<Models.Milk> Milks { get; set; }
        public List<Models.Category> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Milks = await _milkService.GetMilksAsync();
            Categories = await _categoryService.GetCategoriesAsync();
        }
    }
}
