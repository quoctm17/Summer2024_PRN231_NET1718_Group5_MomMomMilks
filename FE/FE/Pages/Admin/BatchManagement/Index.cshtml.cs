using FE.Models;
using FE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.BatchManagement
{
    public class IndexModel : PageModel
    {
        private readonly BatchService _batchService;

        public IndexModel(BatchService batchService)
        {
            _batchService = batchService;
        }
        public List<Batch> Batches{ get; set; }
        public async Task OnGet(string message)
        {
            if (!string.IsNullOrEmpty(message) && message.Equals("deleted", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Message"] = "Deleted Successfully!";
            }
            Batches = await _batchService.GetAllBatches();
        }
    }
}
