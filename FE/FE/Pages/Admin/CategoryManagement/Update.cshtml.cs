using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.CategoryManagement
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public UpdateCategoryRequest UpdateCategoryRequest { get; set; } = new UpdateCategoryRequest();
        public void OnGet(int id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7269");
            var client = new CategoryIt.CategoryItClient(channel);

            var request = new ReadCategoryRequest
            {
                Id = id,
            };
            var category = client.ReadCategory(request);
            UpdateCategoryRequest.Id = category.Id;
            UpdateCategoryRequest.Name = category.Name;
            UpdateCategoryRequest.Description = category.Description;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new CategoryIt.CategoryItClient(channel);

                client.UpdateCategory(UpdateCategoryRequest);
                return RedirectToPage("/admin/categorymanagement/index");
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
                var client = new CategoryIt.CategoryItClient(channel);

                var request = new DeleteCategoryRequest { Id = UpdateCategoryRequest.Id };
                client.DeleteCategory(request);
                return RedirectToPage("/admin/categorymanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Update failed");
                return Page();
            }
        }
    }
}
