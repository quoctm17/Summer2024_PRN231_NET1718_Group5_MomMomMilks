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

        //Paging
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 3;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));


        public async Task OnGetAsync()
        {
            Milks = (List<Models.Milk>)await _milkService.GetPaginatedMilks(CurrentPage,PageSize);
            Categories = await _categoryService.GetCategoriesAsync();
            var milks = await _milkService.GetMilksAsync();
            Count = milks.Count;
        }

        public async Task<IActionResult> OnPostIndex(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                Milks = await _milkService.GetMilksAsync();
            }
            else
            {
                Milks = await _milkService.GetMilksByNameAsync(title);
            }
            Count = Milks.Count;
            Categories = await _categoryService.GetCategoriesAsync();
            return Page();
        }
    }
}
