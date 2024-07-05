using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.SupplierManagement
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public UpdateSupplierRequest UpdateSupplierRequest { get; set; } = new UpdateSupplierRequest();
        public void OnGet(int id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7269");
            var client = new SupplierIt.SupplierItClient(channel);

            var request = new ReadSupplierRequest
            {
                Id = id,
            };
            var supplier = client.ReadSupplier(request);
            UpdateSupplierRequest.Id = supplier.Id;
            UpdateSupplierRequest.Name = supplier.Name;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new SupplierIt.SupplierItClient(channel);

                client.UpdateSupplier(UpdateSupplierRequest);
                return RedirectToPage("/admin/suppliermanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Update failed");
                return Page();
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new SupplierIt.SupplierItClient(channel);

                var request = new DeleteSupplierRequest { Id = UpdateSupplierRequest.Id };
                client.DeleteSupplier(request);
                return RedirectToPage("/admin/suppliermanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Update failed");
                return Page();
            }
        }
    }
}
