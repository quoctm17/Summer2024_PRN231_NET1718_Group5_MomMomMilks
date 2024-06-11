using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.BatchManagement
{
    public class DetailsModel : PageModel
    {
        private readonly BatchService _batchService;
        private readonly MilkService _milkService;

        public DetailsModel(BatchService batchService, MilkService milkService)
        {
            _batchService = batchService;
            _milkService = milkService;
        }
        [BindProperty]
        public Batch Batch{ get; set; }
        public List<FE.Models.Milk> Milks { get; set; }

        public async Task OnGet(int id, string message = "")
        {
            if(!string.IsNullOrEmpty(message))
            {
                TempData["Message"] = "Update Successfully";
            }
            Milks = await _milkService.GetMilksAsync();
            Batch = await _batchService.GetSingleBatch(id);
        }

        public async Task<IActionResult> OnPost()
        {
            Milks = await _milkService.GetMilksAsync();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var result = await _batchService.UpdateBatch(Batch);
                if (result)
                {
                    return RedirectToPage("/admin/batchmanagement/details", new {
                        id = Batch.Id,
                        message = "updated"
                    });
                }
                ModelState.AddModelError("batch", "An error occured. No change is made");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("batch", "Error occured");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDelete()
        {
            Milks = await _milkService.GetMilksAsync();

            if (Batch.Id == 0)
            {
                ModelState.AddModelError("batch", "Delete Failed");
                return Page();
            }

            try
            {
                var result = await _batchService.DeleteBatch(Batch.Id);
                if(result)
                {
                    return RedirectToPage("/admin/batchmanagement/index", new
                    {
                        message = "deleted"
                    });
                }
                ModelState.AddModelError("batch", "Delete Failed");
                return Page();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("batch", "Delete Failed");
            }
            return Page();
        }
    }
}
