using FE.Helpers;
using FE.Models;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.BrandManagement
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public UpdateBrandRequest UpdateBrandRequest { get; set; } = new UpdateBrandRequest();
        public void OnGet(int id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7269");
            var client = new BrandIt.BrandItClient(channel);

            var request = new ReadBrandRequest
            {
                Id = id,
            };
            var brand = client.ReadBrand(request);
            UpdateBrandRequest.Id = brand.Id;
            UpdateBrandRequest.Name = brand.Name;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new BrandIt.BrandItClient(channel);

                var user = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                var token = user.Token;

                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {token}" }
                };

                client.UpdateBrand(UpdateBrandRequest, headers);
                return RedirectToPage("/admin/brandmanagement/index");
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
                var client = new BrandIt.BrandItClient(channel);

                var user = SessionHelper.GetObjectFromJson<User>(HttpContext.Session, "user");
                var token = user.Token;

                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {token}" }
                };
                var request = new DeleteBrandRequest { Id = UpdateBrandRequest.Id };
                client.DeleteBrand(request, headers);
                return RedirectToPage("/admin/brandmanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Update failed");
                return Page();
            }
        }
    }
}
