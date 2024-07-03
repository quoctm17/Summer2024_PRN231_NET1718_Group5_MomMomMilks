using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.BrandManagement
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateBrandRequest CreateBrandRequest{ get; set; } = new CreateBrandRequest();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(CreateBrandRequest.Name)) 
                {
                    ModelState.AddModelError("Errors", "Fill in all fields");
                }
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new BrandIt.BrandItClient(channel);
                var response = client.CreateBrand(CreateBrandRequest);
                return RedirectToPage("/admin/brandmanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Create failed");
                return Page();
            }
        }
    }
}
