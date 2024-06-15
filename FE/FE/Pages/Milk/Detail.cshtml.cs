using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Milk
{
    public class DetailModel : PageModel
    {
        private readonly MilkService _milkService;

        public DetailModel(MilkService milkService)
        {
            _milkService = milkService;
        }

        public Models.Milk Milk { get; set; }
        public int TotalQuantity { get; set; }

        public async Task<IActionResult> OnGetAsync(int milkId)
        {
            try
            {
                Milk = await _milkService.GetMilkByIdAsync(milkId);
                TotalQuantity = await _milkService.GetTotalQuantityByMilkIdAsync(milkId);
                return Page();
            }
            catch (HttpRequestException ex)
            {
               
                return NotFound();
            }
        }
    }
}
