using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.BatchManagement
{
    public class CreateModel : PageModel
    {
        private readonly MilkService _milkService;
        private readonly BatchService _batchService;

        public CreateModel(MilkService milkService, BatchService batchService)
        {
            _milkService = milkService;
            _batchService = batchService;
        }
        public List<FE.Models.Milk> Milks{ get; set; }
        [BindProperty]
        public Batch Batch { get; set; }
        public async Task OnGet()
        {
            Milks = await _milkService.GetMilksAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Milks = await _milkService.GetMilksAsync();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var result = await _batchService.CreateBatch(Batch);
                if(result)
                {
                    return RedirectToPage("/admin/batchmanagement/index");
                }
                ModelState.AddModelError("batch", "An error occured. No change is made");
                return Page();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("batch", "Error occured");
            }
            return Page();
        }
    }
}
