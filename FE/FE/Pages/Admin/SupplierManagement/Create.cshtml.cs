using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.SupplierManagement
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateSupplierRequest CreateSupplierRequest { get; set; } = new CreateSupplierRequest();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(CreateSupplierRequest.Name))
                {
                    ModelState.AddModelError("Errors", "Fill in all fields");
                }
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new SupplierIt.SupplierItClient(channel);
                var response = client.CreateSupplier(CreateSupplierRequest);
                return RedirectToPage("/admin/suppliermanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Create failed");
                return Page();
            }
        }
    }
}
