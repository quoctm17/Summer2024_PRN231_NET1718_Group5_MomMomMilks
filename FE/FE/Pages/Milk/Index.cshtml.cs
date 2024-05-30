using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Milk
{
    public class IndexModel : PageModel
    {
        private readonly MilkService _milkService;

        public IndexModel(MilkService milkService)
        {
            _milkService = milkService;
        }

        public List<Models.Milk> Milks { get; set; }

        public async Task OnGetAsync()
        {
            Milks = await _milkService.GetMilksAsync();
        }
    }
}
