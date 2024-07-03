using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FE.Pages.Admin.CategoryManagement
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateCategoryRequest CreateCategoryRequest { get; set; } = new CreateCategoryRequest();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(CreateCategoryRequest.Name))
                {
                    ModelState.AddModelError("Errors", "Fill in all fields");
                }
                var channel = GrpcChannel.ForAddress("https://localhost:7269");
                var client = new CategoryIt.CategoryItClient(channel);
                var response = client.CreateCategory(CreateCategoryRequest);
                return RedirectToPage("/admin/categorymanagement/index");
            }
            catch
            {
                ModelState.AddModelError("Errors", "Create failed");
                return Page();
            }
        }
    }
}
