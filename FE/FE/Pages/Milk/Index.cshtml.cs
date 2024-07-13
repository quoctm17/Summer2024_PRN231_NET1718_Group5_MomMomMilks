using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        // Paging
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 8;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        // Sorting
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "Name";

        [BindProperty(SupportsGet = true)]
        public string IsAscendingString { get; set; } = "true";

        public bool IsAscending { get; private set; } = true;

        public async Task OnGetAsync()
        {
            // Manually parse the IsAscending query parameter
            string isAscendingQuery = Request.Query["IsAscending"];
            bool isAscendingParsed = bool.TryParse(isAscendingQuery, out bool isAscending);
            IsAscending = isAscendingParsed ? isAscending : IsAscending; // Use the parsed value if successful

            System.Diagnostics.Debug.WriteLine($"Raw IsAscending Query Parameter: {isAscendingQuery}");
            System.Diagnostics.Debug.WriteLine($"Initial IsAscendingString: {IsAscendingString}, Parsed IsAscending: {IsAscending}");
            System.Diagnostics.Debug.WriteLine($"SortBy: {SortBy}, IsAscending: {IsAscending}");
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostIndex(string title)
        {
            await LoadDataAsync(title);
            return Page();
        }

        public async Task<IActionResult> OnPostFilter(int categoryId)
        {
            await LoadDataAsync(categoryId: categoryId);
            return Page();
        }

        private async Task LoadDataAsync(string title = null, int? categoryId = null)
        {
            Categories = await _categoryService.GetCategoriesAsync();

            IEnumerable<Models.Milk> milks;

            if (categoryId.HasValue)
            {
                milks = categoryId == 0 ? await _milkService.GetMilksAsync() : await _milkService.GetMilkByCategoryAsync(categoryId.Value);
            }
            else if (!string.IsNullOrEmpty(title))
            {
                milks = await _milkService.GetMilksByNameAsync(title);
            }
            else
            {
                milks = await _milkService.GetMilksAsync();
            }

            Count = milks.Count();

            // Debugging output
            System.Diagnostics.Debug.WriteLine($"SortBy: {SortBy}, IsAscending: {IsAscending}");

            Milks = SortMilks(milks.ToList());
            Milks = PaginateMilks(Milks);
        }

        private List<Models.Milk> SortMilks(List<Models.Milk> milks)
        {
            return SortBy switch
            {
                "Price" => IsAscending ? milks.OrderBy(m => m.Price).ToList() : milks.OrderByDescending(m => m.Price).ToList(),
                "Name" => IsAscending ? milks.OrderBy(m => m.Name).ToList() : milks.OrderByDescending(m => m.Name).ToList(),
                _ => milks
            };
        }

        private List<Models.Milk> PaginateMilks(List<Models.Milk> milks)
        {
            return milks.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
