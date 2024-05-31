using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class IndexModel : PageModel
{

    private readonly CategoryService _categoryService;
    private readonly MilkService _milkService;

    public IndexModel(CategoryService categoryService, MilkService milkService)
    {
        _categoryService = categoryService;
        _milkService = milkService;
    }

    public List<Category> Categories { get; set; }
    public List<Milk> Milks { get; set; }

    public async Task OnGetAsync()
    {
        Categories = await _categoryService.GetCategoriesAsync();

        Milks = await _milkService.GetMilksAsync();
    }
}
